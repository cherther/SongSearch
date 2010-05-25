using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using SongSearch.Web.Data;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using Ninject;

namespace SongSearch.Web.Services {
	
	// **************************************
	// AccountService
	// **************************************
	public class CartService : BaseService, ICartService {

		// ----------------------------------------------------------------------------
		// (Properties)
		// ----------------------------------------------------------------------------
		//private IDataSession Session;
		private bool _disposed;
		//private string _activeUserIdentity;

		private const int _daysToExpire = 30;
		private const int _daysToDelete = 7;

		public CartService(IDataSession session) : base(session) {}

		public CartService(string activeUserIdentity) : base(activeUserIdentity) { }
		// **************************************
		// MyCarts
		// **************************************
		public IList<Cart> MyCarts() {

			var carts = Session.All<Cart>().Where(c => c.UserId == ActiveUser.UserId).ToList();

			return carts;
		}

		// **************************************
		// MyActiveCart
		// **************************************
		public Cart MyActiveCart() {
			return Session.Single<Cart>(c => c.UserId == ActiveUser.UserId && c.CartStatus == (int)CartStatusCodes.Active);
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
				bool isInCart = query.Contents.Any(i => i.ContentId == contentId);
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

			var content = Session.Single<Content>(c => c.ContentId == contentId);
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

				Session.Add<Cart>(cart);
			}

			cart.Contents.Add(content);
			Session.CommitChanges();
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

					Session.CommitChanges();
				}
			}

			cart = null;
		}

		// **************************************
		// PackageMyActiveCart
		// **************************************
		public void CompressMyActiveCart(string userArchiveName = null) {
			var cart = MyActiveCart();

			if (cart != null && cart.Contents.Count() > 0) {
				string zipName = cart.ArchiveDownloadName(userArchiveName);
				string zipPath = CompressCart(cart);
				cart.MarkAsCompressed(zipPath);
				
				cart = null;
			}
		}

		// **************************************
		// DownloadPackagedCart
		// **************************************
		public Cart DownloadCompressedCart(int cartId) {
			throw new NotImplementedException();
		}

		// **************************************
		// DeleteCart
		// **************************************
		public void DeleteCart(int cartId) {

			var cart = Session.Single<Cart>(c => c.CartId == cartId);
			if (cart != null && cart.UserId != ActiveUser.UserId) {
				throw new ArgumentOutOfRangeException();
			}
			
			string path = cart.ArchivePath();

			Session.Delete<Cart>(cart);
			Session.CommitChanges();

			cart = null;

			Files.Delete(path);

		}

		

		// **************************************
		// ArchiveExpiredCarts
		// **************************************
		public void ArchiveExpiredCarts() {
			var expiredCarts = Session.All<Cart>()
									.Where(c => c.CartStatus == (int)CartStatusCodes.Compressed &&
										c.LastUpdatedOn < DateTime.Now.AddDays(-_daysToExpire));

			foreach (var cart in expiredCarts) {
				cart.CartStatus = (int)CartStatusCodes.Downloaded;
				cart.LastUpdatedOn = DateTime.Now;
			}
			Session.CommitChanges();

			expiredCarts = null;
		}

		// **************************************
		// DeletedExpiredArchivedCarts
		// **************************************
		public void DeletedExpiredArchivedCarts() {
			var expiredCarts = Session.All<Cart>()
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
		private string CompressCart(Cart cart) {

				string zipPath = cart.ArchivePath(); 
				string signature = ActiveUser.IsAnyAdmin() ? ActiveUser.Signature : ActiveUser.ParentSignature();
				
				var contents = cart.Contents.ToList();

				using (var zip = new ZipFile()) {
					foreach (var content in contents) {
						
						if (content.HasMediaFullVersion) {
						
							var asset = new FileInfo(Path.Combine(Settings.AssetPathFullSong.Text(), content.ContentId.ToString(), ".mp3"));

							if (asset.Exists) {

								var downloadName = content.DownloadableName() ?? asset.Name;
								
								zip.AddEntry(String.Format("{0}\\{1}", cart.ArchiveName.Replace(".zip", ""), downloadName, asset.Extension),
											File.ReadAllBytes(asset.FullName));
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
					if (Session != null) {
						Session.Dispose();
						Session = null;
					}					
				}

				_disposed = true;
			}
		}



	}
}