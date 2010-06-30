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
using System.Runtime.Remoting.Messaging;

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
			var userId = Account.User().UserId;
			var carts = ReadSession.GetObjectQuery<Cart>()
				.Include("Contents")
				.Where(c => c.UserId == userId).ToList();

			return AddUserDownloadableNames(carts);
		}

		private List<Cart> AddUserDownloadableNames(List<Cart> carts) {

			var signature = Account.User().Signature;
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
			var userId = Account.User().UserId;
			return DataSession.Single<Cart>(c => c.UserId == userId && c.CartStatus == (int)CartStatusCodes.Active);
		}

		// **************************************
		// MyActiveCartContents
		// **************************************
		public Cart MyActiveCartContents() {
			var userId = Account.User().UserId;
			return ReadSession.GetObjectQuery<Cart>()
				.Include("Contents")
				.Where(c => c.UserId == userId && c.CartStatus == (int)CartStatusCodes.Active).SingleOrDefault();			
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

			return query != null && query.Contents != null && query.Contents.Any(i => i.ContentId == contentId);
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
				cart = EmptyCart();

				DataSession.Add<Cart>(cart);
			}
			if (cart.Contents.Count() < 100) {
				cart.Contents.Add(content);
			} else {
				throw new ArgumentOutOfRangeException("You cart already contains the maximum number of items (100)");
			}
			DataSession.CommitChanges();

		}

		

		public void AddToMyActiveCart(int[] contentIds) {

			// Check if open cart exists and if needed create new cart
			var cart = MyActiveCart() ?? EmptyCart();

			if (cart.CartId == 0) { DataSession.Add<Cart>(cart); }
			
			if (cart.Contents.Count() + contentIds.Count() > 100){
				throw new ArgumentOutOfRangeException("You cart already contains the maximum number of items (100)");
			}

			var contents = DataSession.All<Content>().Where(c => contentIds.Contains(c.ContentId)).ToList();
			

			foreach (var contentId in contentIds){
				if (!cart.Contents.Any(i => i.ContentId == contentId)){
					var content = contents.SingleOrDefault(c => c.ContentId == contentId);
					if (content == null) {
						throw new ArgumentException(Errors.ItemDoesNotExist.Text());
					}
					cart.Contents.Add(content);
				}
			}

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

		public void RemoveFromMyActiveCart(int[] contentIds) {

			var cart = MyActiveCart();

			if (cart != null) {
				foreach (var contentId in contentIds) {
					var content = cart.Contents.Where(c => c.ContentId == contentId).SingleOrDefault();
					if (content != null) {
						cart.Contents.Remove(content);
					}
				}

				DataSession.CommitChanges();

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
				CompressCart(cart, contentNames);
				cart.MarkAsCompressed();
				DataSession.CommitChanges();
				SessionService.Session().SessionUpdate(cart.CartId, "ProcessingCartId");
				cart = null;
			}
		}

		private delegate void CompressCartDelegate(Cart cart, IList<ContentUserDownloadable> contentNames);
		delegate void EndInvokeDelegate(IAsyncResult result);

		//
		// ATTENTION: This seems to require the Application Pool to run as an admin or some account with elevated privileges, have to check...
		//
		public void CompressMyActiveCartOffline(string userArchiveName = null, IList<ContentUserDownloadable> contentNames = null) {
			//mark as processing
			var cart = MyActiveCart();

			if (cart != null && cart.Contents.Count() > 0) {

				cart.ArchiveName = cart.ArchiveDownloadName(userArchiveName);
	
				//hand off compression work
				CompressCartDelegate compressDelegate = new CompressCartDelegate(CompressCart);
				// Define the AsyncCallback delegate.
				AsyncCallback callBack = new AsyncCallback(this.CompressCartCallback);

				compressDelegate.BeginInvoke(cart, contentNames,
					callBack, 
					cart.CartId
					);

				cart.CartStatus = (int)CartStatusCodes.Processing;
				DataSession.CommitChanges();

				
				
				cart = null;
				
			}

		}

		public void CompressCartCallback(IAsyncResult asyncResult) {
			// Extract the delegate from the 
			// System.Runtime.Remoting.Messaging.AsyncResult.
			CompressCartDelegate compressDelegate = (CompressCartDelegate)((AsyncResult)asyncResult).AsyncDelegate;
			int cartId = (int)asyncResult.AsyncState;
			
			compressDelegate.EndInvoke(asyncResult);

			var cart = DataSession.Single<Cart>(c => c.CartId == cartId);

			if (cart != null && cart.Contents.Count() > 0) {

				cart.MarkAsCompressed();
				DataSession.CommitChanges();
				
				cart = null;
			}
			//object[] parameters = asyncResult.AsyncState as object[];
			//if (parameters != null && parameters.Length > 0) {
			//    EndInvokeDelegate endInvokeDelegate = parameters[0] as EndInvokeDelegate;
			//    if (endInvokeDelegate != null) {
			//        endInvokeDelegate.Invoke(asyncResult);
			//    }

		}

		// **************************************
		// DownloadPackagedCart
		// **************************************
		public Cart DownloadCompressedCart(int cartId) {
			var userId = Account.User().UserId;
			var cart = DataSession.Single<Cart>(c => c.UserId == userId && c.CartId == cartId);
			if (cart == null) {
				throw new ArgumentOutOfRangeException();
			}
			cart.MarkAsDownloaded();
			DataSession.CommitChanges();
			return cart;
		}

		// **************************************
		// DeleteCart
		// **************************************
		public void DeleteCart(int cartId) {
			var userId = Account.User().UserId;
			var cart = DataSession.Single<Cart>(c => c.UserId == userId && c.CartId == cartId);
			if (cart == null) {
				throw new ArgumentOutOfRangeException();
			}
			
			string path = cart.ArchivePath();

			DataSession.Delete<Cart>(cart);
			DataSession.CommitChanges();

			cart = null;

			FileSystem.SafeDelete(path, false);

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
		// EmptyCart
		// **************************************
		private Cart EmptyCart() {
			return new Cart {
				CreatedOn = DateTime.Now,
				LastUpdatedOn = DateTime.Now,
				UserId = Account.User().UserId,
				IsLastProcessed = false,
				CartStatus = (int)CartStatusCodes.Active
			};
		}

		// **************************************
		// Zip
		// **************************************
		private void CompressCart(Cart cart, IList<ContentUserDownloadable> contentNames) {

			var user = Account.User();
			string zipPath = cart.ArchivePath();
			string signature = user.IsAnyAdmin() ? user.Signature : user.ParentSignature();
				
			var contents = cart.Contents.ToList();

			using (var zip = new ZipFile()) {
					
				zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestSpeed;
					
				foreach (var content in contents) {
						
					if (content.HasMediaFullVersion) {

						var nameUserOverride = contentNames != null && contentNames.Any(x => x.ContentId == content.ContentId) ?
													contentNames.Where(x => x.ContentId == content.ContentId).Single().DownloadableName : null;
						var downloadName = nameUserOverride ?? (content.UserDownloadableName ?? MediaService.GetContentMediaFileName(content.ContentId));
								
						try {
							var asset = MediaService.GetContentMedia(content.ContentId, MediaVersion.FullSong);

							zip.AddEntry(String.Format("{0}\\{1}{2}", cart.ArchiveName.Replace(".zip", ""), downloadName, MediaService.ContentMediaExtension),
										asset);
						}
						catch {

							App.Logger.Info(String.Concat(content.ContentId, " is missing."));
						}
					}
				}
				zip.Save(zipPath);
			}
			
			
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