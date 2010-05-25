using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using SongSearch.Web.Data;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using Ninject;
using NLog;

namespace SongSearch.Web.Services {
	
	// **************************************
	// AccountService
	// **************************************
	public class CartService : BaseService, ICartService {

		// ----------------------------------------------------------------------------
		// (Properties)
		// ----------------------------------------------------------------------------
		//private IDataSession DataSession;
		private bool _disposed;
		//private string _activeUserIdentity;

		private const int _daysToExpire = 30;
		private const int _daysToDelete = 7;

		public CartService(IDataSession dataSession, IDataSessionReadOnly readSession) : base(dataSession, readSession) { }

		public CartService(string activeUserIdentity) : base(activeUserIdentity) { }
		// **************************************
		// MyCarts
		// **************************************
		public IList<Cart> MyCarts() {

			var carts = ReadSession.GetObjectQuery<Cart>()
				.Include("Contents")
				.Where(c => c.UserId == ActiveUser.UserId).ToList();

			carts = AddUserDownloadableNames(carts);
			return carts;
		}

		private List<Cart> AddUserDownloadableNames(List<Cart> carts) {

			var signature = ActiveUser.Signature;
			foreach (var cart in carts) {

				if (cart.CartStatus == (int)CartStatusCodes.Active) {
					var contents = cart.Contents;

					foreach (var content in contents) {
						content.UserDownloadableName = content.DownloadableName(signature);
					}
				}
			}

			return carts;
		}

		// **************************************
		// MyActiveCart
		// **************************************
		public Cart MyActiveCart() {

			return DataSession.Single<Cart>(c => c.UserId == ActiveUser.UserId && c.CartStatus == (int)CartStatusCodes.Active);
		}

		// **************************************
		// MyActiveCartContents
		// **************************************
		public Cart MyActiveCartContents() {

			var cart = ReadSession.GetObjectQuery<Cart>()
				.Include("Contents")
				.Where(c => c.UserId == ActiveUser.UserId && c.CartStatus == (int)CartStatusCodes.Active).SingleOrDefault();

			return cart;
			
		}
		
		// **************************************
		// MyCartContents
		// **************************************
		public int[] MyCartContents() {
			var query = MyActiveCart();
			var cartContents = new[] { 0 };

			if (query != null) {
				cartContents = query.Contents.Select(q => q.ContentId).AsParallel().ToArray();
			}
			query = null;
			return cartContents;
		}

		// **************************************
		// IsInMyActiveCart
		// **************************************
		public bool IsInMyActiveCart(int contentId) {
			var query = MyActiveCart();

			if (query != null) {
				bool isInCart = query.Contents != null && query.Contents.Any(i => i.ContentId == contentId);
				query = null;

				return isInCart;
			} else {
				return false;
			}
		}

		// **************************************
		// AddToMyActiveCart
		// **************************************
		public void AddToMyActiveCart(int contentId) {

			var content = DataSession.Single<Content>(c => c.ContentId == contentId);
			if (content == null) {
				throw new ArgumentException(Errors.ItemDoesNotExist.Text());
			}

			// Check if open cart exists and if needed create new cart
			var cart = MyActiveCart();

			if (cart != null) {
				if (cart.Contents.Any(i => i.ContentId == contentId)) {
					return;
				}
			} else {
				cart = new Cart {
					CreatedOn = DateTime.Now,
					LastUpdatedOn = DateTime.Now,
					UserId = ActiveUser.UserId,
					CartStatus = (int)CartStatusCodes.Active					
				};

				DataSession.Add<Cart>(cart);
			}

			cart.Contents.Add(content);
			DataSession.CommitChanges();
		}

		// **************************************
		// RemoveFromMyActiveCart
		// **************************************
		public void RemoveFromMyActiveCart(int contentId) {

			var cart = MyActiveCart();

			if (cart != null) {
				var content = cart.Contents.Where(c => c.ContentId == contentId).SingleOrDefault();
				if (content != null) {
					cart.Contents.Remove(content);

					DataSession.CommitChanges();
				}
			}

			cart = null;
		}

		// **************************************
		// CompressMyActiveCart
		// **************************************
		public void CompressMyActiveCart(string userArchiveName = null, IList<ContentUserDownloadable> contentNames = null) {
			var cart = MyActiveCart();

			if (cart != null && cart.Contents.Count() > 0) {
				string zipName = cart.ArchiveDownloadName(userArchiveName);
				cart.ArchiveName = zipName;
				string zipPath = CompressCart(cart, contentNames);
				cart.MarkAsCompressed(zipPath);
				DataSession.CommitChanges();
				cart = null;
			}
		}

		// **************************************
		// DownloadPackagedCart
		// **************************************
		public Cart DownloadCompressedCart(int cartId) {

			var cart = DataSession.Single<Cart>(c => c.UserId == ActiveUser.UserId && c.CartId == cartId);
			if (cart != null) {
				throw new ArgumentOutOfRangeException();
			}
			cart.MarkAsDownloaded();
			return cart;
		}

		// **************************************
		// DeleteCart
		// **************************************
		public void DeleteCart(int cartId) {

			var cart = DataSession.Single<Cart>(c => c.CartId == cartId);
			if (cart != null && cart.UserId != ActiveUser.UserId) {
				throw new ArgumentOutOfRangeException();
			}
			
			string path = cart.ArchivePath();

			DataSession.Delete<Cart>(cart);
			DataSession.CommitChanges();

			cart = null;

			Files.Delete(path);

		}

		

		// **************************************
		// ArchiveExpiredCarts
		// **************************************
		public void ArchiveExpiredCarts() {
			var expiredCarts = DataSession.All<Cart>()
									.Where(c => c.CartStatus == (int)CartStatusCodes.Compressed &&
										c.LastUpdatedOn < DateTime.Now.AddDays(-_daysToExpire));

			foreach (var cart in expiredCarts) {
				cart.CartStatus = (int)CartStatusCodes.Downloaded;
				cart.LastUpdatedOn = DateTime.Now;
			}
			DataSession.CommitChanges();

			expiredCarts = null;
		}

		// **************************************
		// DeletedExpiredArchivedCarts
		// **************************************
		public void DeletedExpiredArchivedCarts() {
			var expiredCarts = DataSession.All<Cart>()
									.Where(c => c.CartStatus == (int)CartStatusCodes.Downloaded &&
										 c.LastUpdatedOn < DateTime.Now.AddDays(-_daysToDelete));

			foreach (Cart cart in expiredCarts) {
				DeleteCart(cart.CartId);
			}
			expiredCarts = null;
		}


		// ----------------------------------------------------------------------------
		// (Private(
		// ----------------------------------------------------------------------------

		// **************************************
		// Zip
		// **************************************
		private string CompressCart(Cart cart, IList<ContentUserDownloadable> contentNames) {

				string zipPath = cart.ArchivePath(); 
				string signature = ActiveUser.IsAnyAdmin() ? ActiveUser.Signature : ActiveUser.ParentSignature();
				
				var contents = cart.Contents.ToList();

				using (var zip = new ZipFile()) {
					foreach (var content in contents) {
						
						if (content.HasMediaFullVersion) {
						
							var assetFileName = String.Concat(content.ContentId, ".mp3");
							var asset = new FileInfo(Path.Combine(Settings.AssetPathFullSong.Text(), assetFileName));

							if (asset.Exists) {

								var nameUserOverride = contentNames != null && contentNames.Any(x => x.ContentId == content.ContentId) ? 
									contentNames.Where(x => x.ContentId == content.ContentId).Single().DownloadableName : null;
								var downloadName = nameUserOverride ?? (content.DownloadableName() ?? asset.Name);

								try {
									zip.AddEntry(String.Format("{0}\\{1}", cart.ArchiveName.Replace(".zip", ""), downloadName, asset.Extension),
												File.ReadAllBytes(asset.FullName));
								}
								catch {

									Application.Logger.Info(String.Concat(asset.FullName, " is missing."));
								}
							}
						}
					}
					zip.Save(zipPath);
				}
			
			return zipPath;
		}
		

		
		// ----------------------------------------------------------------------------
		// Dispose
		// ----------------------------------------------------------------------------

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}


		private void Dispose(bool disposing) {
			if (!_disposed) {
				{
					if (DataSession != null) {
						DataSession.Dispose();
						DataSession = null;
					}					
				}

				_disposed = true;
			}
		}



	}
}