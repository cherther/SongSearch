using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Services;
using System.Web.Routing;
using SongSearch.Web.Data;
using System.IO;

namespace SongSearch.Web.Controllers
{
	[RequireAuthorization]
	[HandleError]
	public partial class CartController : AsyncController
	{
		private User _currentUser;

		ICartService _cartService;

		protected override void Initialize(RequestContext requestContext) {
			
			if (!String.IsNullOrWhiteSpace(requestContext.HttpContext.User.Identity.Name)) {
				_cartService.ActiveUserName = requestContext.HttpContext.User.Identity.Name;
				_currentUser = _cartService.ActiveUser;
			}
			base.Initialize(requestContext);

		}

		public CartController(
			ICartService cartService
			) {
			_cartService = cartService;
		}

		// **************************************
		// URL: /Cart/
		// **************************************
		public virtual ActionResult Index()
		{
			try {

				var vm = GetCartViewModel();
				vm.MyCarts = _cartService.MyCarts();
				vm.CartToHighlight = GetLastProcessedCartId();

				vm.CartContentHeaders = new string[] { "Title", "Artist", "Year", "File Name", "Download", "Remove" };
				//Reset the searchFields to stop any previous highlighting preferences
				CacheService.SessionUpdate(null, "SearchFields");

				var msg = _currentUser != null ? 
					(
					_currentUser.ProcessingCartMessage() ?? 
					_currentUser.DownloadCartMessage(vm.MyCarts)
					): "";

				if (msg != null) {
					this.FeedbackInfo(msg);
				}

				return View(vm);
			}
			catch {
				this.FeedbackError("There was an error loading the Song Cart page. Please try again in a bit.");
				return RedirectToAction(MVC.Home.Index());
			}
			
		}

		// **************************************
		// URL: /Cart/CartCount
		// **************************************
		public virtual ActionResult CartCount() {
			var count = 0;
			try {
				count = CacheService.MyActiveCartCount(_currentUser.UserName);
			}
			catch { }

			if (Request.IsAjaxRequest()) {
				return Json(count, JsonRequestBehavior.AllowGet);
			} else {
				return View(Views.ctrlCartCount, new ViewModel() { MyActiveCartCount = count });
			}
		}

		// **************************************
		// URL: /Cart/Add/5
		// **************************************
		//[HttpPost]
		public virtual ActionResult Add(int id) {
			try {

				_cartService.AddToMyActiveCart(id);
				CacheService.RefreshMyActiveCart(_currentUser.UserName);

				if (Request.IsAjaxRequest()) {
					return Json(id, JsonRequestBehavior.AllowGet);
				} else {
					this.FeedbackInfo("Item added to cart");
					return RedirectToAction(MVC.Cart.Index());
				}
			}
			catch (Exception ex) {
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					this.FeedbackError("There was an error adding this item");
					return RedirectToAction(MVC.Error.Index(ex, ex.Message, this.ToString()));
				
				}
			}
		}
		[HttpPost]
		public virtual ActionResult AddMultiple() {
			try {
				var itemsPosted = Request.Form["items[]"];
				var items = itemsPosted != null ? itemsPosted.Split(',') : new string[] {};
				var count = items != null ? items.Count() : 0;
				if (items != null) {
					var contentIds = items.Select(i => int.Parse(i)).ToArray();
					_cartService.AddToMyActiveCart(contentIds);
					CacheService.RefreshMyActiveCart(_currentUser.UserName);
				}
				if (Request.IsAjaxRequest()) {
					return Json(count, JsonRequestBehavior.AllowGet);
				} else {
					this.FeedbackInfo("Items added to cart");
					return RedirectToAction(MVC.Cart.Index());
				}
			}
			catch (Exception ex) {
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					this.FeedbackError("There was an error adding the items");
					return RedirectToAction(MVC.Error.Index(ex, ex.Message, this.ToString()));

				}
			}
		}
		// **************************************
		// URL: /Cart/Remove/5
		// **************************************
		//[HttpPost]
		public virtual ActionResult Remove(int id) {
			try {

				_cartService.RemoveFromMyActiveCart(id);
				CacheService.RefreshMyActiveCart(_currentUser.UserName);

				if (Request.IsAjaxRequest()) {
					return Json(id, JsonRequestBehavior.AllowGet);
				} else {
					this.FeedbackInfo("Item removed from cart");
					return RedirectToAction(MVC.Cart.Index());
				}
			}
			catch (Exception ex) {
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					this.FeedbackError("There was an error removing this item");
					return RedirectToAction(MVC.Error.Index(ex, ex.Message, this.ToString()));
				
				}
			}
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public virtual ActionResult RemoveMultiple() {
			try {
				var itemsPosted = Request.Form["removeFromCartItems"];
				if (itemsPosted == null) {
					return RedirectToAction(MVC.Cart.Index());
				}
				
				var items = itemsPosted != null ? itemsPosted.Split(',') : new string[] { };
				var count = items != null ? items.Count() : 0;

				var contentIds = items.Select(i => int.Parse(i)).ToArray();
				_cartService.RemoveFromMyActiveCart(contentIds);
				CacheService.RefreshMyActiveCart(_currentUser.UserName);

				if (Request.IsAjaxRequest()) {
					return Json(count, JsonRequestBehavior.AllowGet);
				} else {
					this.FeedbackInfo("Item(s) removed from cart");
					return RedirectToAction(MVC.Cart.Index());
				}
			}
			catch (Exception ex) {
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					this.FeedbackError("There was an error removing the item(s)");
					return RedirectToAction(MVC.Error.Index(ex, ex.Message, this.ToString()));

				}
			}
		}
		// **************************************
		// URL: /Cart/Delete/3
		// **************************************
		[HttpPost]
		[ValidateAntiForgeryToken]
		public virtual ActionResult Delete(int id) {
			try {

				_cartService.DeleteCart(id);
				//CacheService.RefreshMyActiveCart(_currentUser.UserName);

				this.FeedbackInfo("Cart Deleted");
			}
			catch {
				this.FeedbackError("There was an error deleting this cart");				
			}
			return RedirectToAction(MVC.Cart.Index());

		}

		// **************************************
		// URL: /Cart/Zip/3
		// **************************************
		[HttpPost]
		[ValidateAntiForgeryToken]
		public virtual ActionResult Zip(string userArchiveName, IList<ContentUserDownloadable> contentNames) {

			try {
				if (contentNames.Count() > 10) {
					_cartService.CompressMyActiveCartOffline(userArchiveName, contentNames);
					CacheService.RefreshMyActiveCart(_currentUser.UserName);
					this.FeedbackInfo("Your cart is currently being zipped up and will be available for download shortly. Please check back on this page in a few minutes.");

				} else {
					_cartService.CompressMyActiveCart(userArchiveName, contentNames);
					CacheService.RefreshMyActiveCart(_currentUser.UserName);
					this.FeedbackInfo("Your cart is ready for download");
				}
			}
			catch {
				this.FeedbackError("There was an error zipping this cart. Please try again in a bit.");

			}
			return RedirectToAction(MVC.Cart.Index());
		}

		
		
		//[HttpPost]
		//public ActionResult ZipCompleted() {
		//    return RedirectToAction("Index");

		//}
		// **************************************
		// URL: /MyCarts/Download/5
		// **************************************
		public virtual ActionResult Download(int id) {

			try {
				var cart = _cartService.DownloadCompressedCart(id);
				if (cart.CartId == GetLastProcessedCartId()) {
					CacheService.SessionUpdate(null, "ProcessingCartId");
				}

				//CacheService.RefreshMyActiveCart(_currentUser.UserName);

				if (cart == null) { throw new ArgumentException(); }

				Response.ContentType = "application/zip";
				Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", cart.ArchiveName));

				return new FileStreamResult(new System.IO.FileStream(cart.ArchivePath(), System.IO.FileMode.Open), "application/zip");
			}
			catch {
				this.FeedbackError("There was an error downloading this cart. Please try again in a bit.");
				return RedirectToAction(MVC.Cart.Index());
			}
		}

		// **************************************
		// GetLastProcessedCartId
		// **************************************
		private int GetLastProcessedCartId() {
			var lastProcessedCart = CacheService.Session("ProcessingCartId");
			var lastProcessedCartId = lastProcessedCart != null ? (int)lastProcessedCart : 0;
			return lastProcessedCartId;
		}
		//public ActionResult DownloadDone() {

		//    var vm = GetCartViewModel();
		//    vm.MyCarts = _cartService.MyCarts();
		//    vm.CartContentHeaders = new string[] { "Title", "Artist", "ReleaseYear", "File Name", "Remove" };


		//    return View(vm);
		//}

		// **************************************
		// GetSearchViewModel
		// **************************************
		private CartViewModel GetCartViewModel() {
			return new CartViewModel() {
				PageTitle = "My Song Cart",
				NavigationLocation = "Cart",
				
			};
			
		}
	}
}
