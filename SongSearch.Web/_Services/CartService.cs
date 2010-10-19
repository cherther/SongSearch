using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using SongSearch.Web.Data;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using Ninject;
using System.Runtime.Remoting.Messaging;

namespace SongSearch.Web.Services {
	
	// **************************************
	// AccountService
	// **************************************
	public static class CartService {

		// ----------------------------------------------------------------------------
		// (Properties)
		// ----------------------------------------------------------------------------
		// **************************************
		// MyCarts
		// **************************************
		public static IList<Cart> MyCarts() {
			var userId = Account.User().UserId;

			using (var ctx = new SongSearchContext()) {
				ctx.ContextOptions.LazyLoadingEnabled = false;
				var carts = ctx.Carts
					.Include("Contents")
					.Include("Contents.ContentMedia")
					.Where(c => c.UserId == userId).ToList();

				return AddUserDownloadableNames(carts);
			}
		}

		private static List<Cart> AddUserDownloadableNames(List<Cart> carts) {

			foreach (var cart in carts) {

				if (cart.CartStatus == (int)CartStatusCodes.Active) {
					var contents = cart.Contents;

					foreach (var content in contents) {
						var signature = Account.User().FileSignature(content);// Signature;
						content.UserDownloadableName = content.DownloadableName(signature);
					}
				}
			}

			return carts;
		}

		// **************************************
		// MyActiveCart
		// **************************************
		public static Cart MyActiveCart() {
			var userId = Account.User().UserId;
			using (var ctx = new SongSearchContext()) {
				return ctx.GetActiveCart();
			}
		}
		private static Cart GetActiveCart(this SongSearchContext ctx) {
			var userId = Account.User().UserId;
			return ctx.Carts.SingleOrDefault(c => c.UserId == userId && c.CartStatus == (int)CartStatusCodes.Active);
		}
		// **************************************
		// MyActiveCartContents
		// **************************************
		public static Cart MyActiveCartContents() {
			var userId = Account.User().UserId;
			using (var ctx = new SongSearchContext()) {
				ctx.ContextOptions.LazyLoadingEnabled = false;
				return ctx.Carts
				.Include("Contents").Include("Contents.ContentMedia")
				.SingleOrDefault(c => c.UserId == userId && c.CartStatus == (int)CartStatusCodes.Active);
			}
		}
		
		// **************************************
		// MyCartContents
		// **************************************
		public static int[] MyCartContents() {
			var query = MyActiveCart();
			var cartContents = new[] { 0 };

			if (query != null) {
				cartContents = query.Contents.Select(q => q.ContentId).ToArray();
			}
			query = null;
			return cartContents;
		}

		// **************************************
		// IsInMyActiveCart
		// **************************************
		public static bool IsInMyActiveCart(int contentId) {
			var query = MyActiveCart();

			return query != null && query.Contents != null && query.Contents.Any(i => i.ContentId == contentId);
		}

		// **************************************
		// AddToMyActiveCart
		// **************************************
		public static void AddToMyActiveCart(int contentId) {

			using (var ctx = new SongSearchContext()) {
				var content = ctx.Contents.SingleOrDefault(c => c.ContentId == contentId);
				if (content == null) {
					throw new ArgumentException(SystemErrors.ItemDoesNotExist);
				}

				// Check if open cart exists and if needed create new cart
				var cart = ctx.GetActiveCart() ?? EmptyCart();

				if (cart != null) {
					if (cart.Contents.Any(i => i.ContentId == contentId)) {
						return;
					}
				} else {
					cart = EmptyCart();
					ctx.Carts.AddObject(cart);
				}
				if (cart.Contents.Count() < 100) {
					cart.Contents.Add(content);
				} else {
					throw new ArgumentOutOfRangeException("You cart already contains the maximum number of items (100)");
				}
				ctx.SaveChanges();
			}
		}



		public static void AddToMyActiveCart(int[] contentIds) {

			// Check if open cart exists and if needed create new cart
			using (var ctx = new SongSearchContext()) {
				var cart = ctx.GetActiveCart() ?? EmptyCart();
			
				if (cart.CartId == 0) { ctx.Carts.AddObject(cart); }

				if (cart.Contents.Count() + contentIds.Count() > 100) {
					throw new ArgumentOutOfRangeException("You cart already contains the maximum number of items (100)");
				}

				var contents = ctx.Contents.Where(c => contentIds.Contains(c.ContentId)).ToList();

				foreach (var contentId in contentIds) {
					if (!cart.Contents.Any(i => i.ContentId == contentId)) {
						var content = contents.SingleOrDefault(c => c.ContentId == contentId);
						if (content == null) {
							throw new ArgumentException(SystemErrors.ItemDoesNotExist);
						}
						cart.Contents.Add(content);
					}
				}

				ctx.SaveChanges();
			}
		}
		// **************************************
		// RemoveFromMyActiveCart
		// **************************************
		public static void RemoveFromMyActiveCart(int contentId) {

			using (var ctx = new SongSearchContext()) {
				var cart = ctx.GetActiveCart();

				if (cart != null) {
				
					var content = cart.Contents.Where(c => c.ContentId == contentId).SingleOrDefault();
					if (content != null) {
						cart.Contents.Remove(content);

						ctx.SaveChanges();
					}
				}

				cart = null;
			}
		}

		public static void RemoveFromMyActiveCart(int[] contentIds) {

			using (var ctx = new SongSearchContext()) {
				var cart = ctx.GetActiveCart();

				if (cart != null) {
					foreach (var contentId in contentIds) {
						var content = cart.Contents.Where(c => c.ContentId == contentId).SingleOrDefault();
						if (content != null) {
							cart.Contents.Remove(content);
						}
					}

					ctx.SaveChanges();

				}

				cart = null;
			}
		}

		// **************************************
		// CompressMyActiveCart
		// **************************************
		public static void CompressMyActiveCart(string userArchiveName = null, IList<ContentUserDownloadable> contentNames = null) {
			using (var ctx = new SongSearchContext()) {
				var cart = ctx.GetActiveCart();

				if (cart != null && cart.Contents.Count() > 0) {
					string zipName = cart.ArchiveDownloadName(userArchiveName);
					cart.ArchiveName = zipName;
					
					CompressCart(cart.CartId, contentNames);
					
					cart.MarkAsCompressed();

					ctx.SaveChanges();

					SessionService.Session().SessionUpdate(cart.CartId, "ProcessingCartId");
					cart = null;
				}
			}
		}

		private delegate void CompressCartDelegate(int cartId, IList<ContentUserDownloadable> contentNames);
		delegate void EndInvokeDelegate(IAsyncResult result);

		//
		// ATTENTION: This seems to require the Application Pool to run as an admin or some account with elevated privileges, have to check...
		//
		public static void CompressMyActiveCartOffline(string userArchiveName = null, IList<ContentUserDownloadable> contentNames = null) {
			//mark as processing

			try {

				using (var ctx = new SongSearchContext()) {
					var userId = Account.User().UserId;

					var cart = ctx.Carts
								//.Include("Contents").Include("Contents.ContentMedia")
								.SingleOrDefault(c => c.UserId == userId && c.CartStatus == (int)CartStatusCodes.Active);

					if (cart != null && cart.Contents.Count() > 0) {

						
						cart.ArchiveName = cart.ArchiveDownloadName(userArchiveName);
						cart.CartStatus = (int)CartStatusCodes.Processing;

						ctx.SaveChanges();

						//hand off compression work
						CompressCartDelegate compressDelegate = new CompressCartDelegate(CompressCart);
						// Define the AsyncCallback delegate.
						AsyncCallback callBack = new AsyncCallback(CompressCartCallback);

						compressDelegate.BeginInvoke(cart.CartId, contentNames,
							callBack,
							cart.CartId
							);

					}
				
					cart = null;

				}
			}
			catch (Exception ex) {
				Log.Error(ex);
				throw ex;
			}

		}

		public static void CompressCartCallback(IAsyncResult asyncResult) {
			// Extract the delegate from the 
			// System.Runtime.Remoting.Messaging.AsyncResult.

			using (var ctx = new SongSearchContext()) {

				try {

					CompressCartDelegate compressDelegate = (CompressCartDelegate)((AsyncResult)asyncResult).AsyncDelegate;
					int cartId = (int)asyncResult.AsyncState;

					compressDelegate.EndInvoke(asyncResult);

					var cart = ctx
								.Carts
								.Include("Contents")//.Include("Contents.ContentMedia")
								.SingleOrDefault(c => c.CartId == cartId);

					if (cart != null && cart.Contents.Count() > 0) {

						cart.MarkAsCompressed();
						ctx.SaveChanges();

						cart = null;
					}
				}
				catch (Exception ex) {
					Log.Error(ex);
				}

			}

		}

		// **************************************
		// DownloadPackagedCart
		// **************************************
		public static Cart DownloadCompressedCart(int cartId) {
			var userId = Account.User().UserId;
			using (var ctx = new SongSearchContext()) {

				var cart = ctx.Carts.SingleOrDefault(c => c.UserId == userId && c.CartId == cartId);
				if (cart == null) {
					throw new ArgumentOutOfRangeException();
				}
				cart.MarkAsDownloaded();
				ctx.SaveChanges();
				return cart;
			}
		}

		// **************************************
		// DeleteCart
		// **************************************
		public static void DeleteCart(int cartId) {

			var userId = Account.User().UserId;

			using (var ctx = new SongSearchContext()) { 

				var cart = ctx.Carts.SingleOrDefault(c => c.UserId == userId && c.CartId == cartId);

				if (cart == null) {
					throw new ArgumentOutOfRangeException();
				}
			
				string path = cart.ArchivePath();
				ctx.Carts.DeleteObject(cart);
				ctx.SaveChanges();

				cart = null;
				
				FileSystem.SafeDelete(path, false);
			}
		}

		
		// ----------------------------------------------------------------------------
		// (Private(
		// ----------------------------------------------------------------------------
		// **************************************
		// EmptyCart
		// **************************************
		private static Cart EmptyCart() {
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
		private static void CompressCart(int cartId, IList<ContentUserDownloadable> contentNames) {

			using (var ctx = new SongSearchContext()) {
				ctx.ContextOptions.LazyLoadingEnabled = false;

				try {

					var cart = ctx.Carts
								.Include("User")
								.Include("Contents")
								.Include("Contents.ContentMedia")
								.SingleOrDefault(c => c.CartId == cartId);

					if (cart != null) {

						var user = cart.User;// Account.User(cart.UserId);
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
										byte[] asset = MediaService.GetContentMedia(content.ContentMedia.FullVersion());

										asset = MediaService.WriteMediaSignature(asset, content, user);

										zip.AddEntry(String.Format("{0}\\{1}{2}", cart.ArchiveName.Replace(".zip", ""), downloadName, SystemConfig.MediaDefaultExtension),
													asset);
									}
									catch (Exception ex) {
										Log.Error(ex);
										Log.Debug(String.Concat(content.ContentId, " has an error/is missing."));
									}
								}
							}
							zip.Save(zipPath);
						}
					}
				}
				catch (Exception ex) {
					Log.Error(ex);
					throw ex;
				}
			}
			
		}
		

		private const int DAYS_TO_EXPIRE = 3;
		private const int DAYS_TO_DELETE = 7;
		
		// **************************************
		// ArchiveExpiredCarts
		// **************************************
		public static void ArchiveExpiredCarts() {

			using (var ctx = new SongSearchContext()) {

				var cutOffDate = DateTime.Now.AddDays(-DAYS_TO_EXPIRE);

				var expiredCarts = ctx.Carts
										.Where(c => c.CartStatus == (int)CartStatusCodes.Compressed &&
											c.LastUpdatedOn < cutOffDate);

				foreach (var cart in expiredCarts) {
					cart.CartStatus = (int)CartStatusCodes.Downloaded;
					cart.LastUpdatedOn = DateTime.Now;
				}
				ctx.SaveChanges();

				expiredCarts = null;

			}
		}

		// **************************************
		// DeletedExpiredArchivedCarts
		// **************************************
		public static void DeletedExpiredArchivedCarts() {

			using (var ctx = new SongSearchContext()) {

				var cutOffDate = DateTime.Now.AddDays(-DAYS_TO_DELETE);
				var expiredCarts = ctx.Carts
										.Where(c => c.CartStatus == (int)CartStatusCodes.Downloaded &&
											 c.LastUpdatedOn < cutOffDate)
											 .ToList();

				foreach (var cart in expiredCarts) {

					string path = cart.ArchivePath();

					ctx.Carts.DeleteObject(cart);
					ctx.SaveChanges();

					FileSystem.SafeDelete(path, false);

				}
				expiredCarts = null;
			}
		}

	}
}