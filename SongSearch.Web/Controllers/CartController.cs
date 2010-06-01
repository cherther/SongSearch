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
	public class CartController : AsyncController
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
		public ActionResult Index()
        {
			var vm = GetCartViewModel();
			vm.MyCarts = _cartService.MyCarts();
			vm.CartContentHeaders = new string[] { "Title", "Artist", "Year", "File Name", "Download", "Remove"};

			//Reset the searchFields to stop any previous highlighting preferences
			CacheService.SessionUpdate(null, "SearchFields");

			var msg = _currentUser.DownloadCartMessage(vm.MyCarts);
			if (msg != null) {
				this.FireInfo(msg);
			}
			return View(vm);
        }

		// **************************************
		// URL: /Cart/CartCount
		// **************************************
		public ActionResult CartCount() {
			
			var count = CacheService.MyActiveCartCount(_currentUser.UserName);
			
			if (Request.IsAjaxRequest()) {
				return Json(count, JsonRequestBehavior.AllowGet);
			} else {
				//ViewData["MyActiveCartContentsCount"] = count;
				var vm = new ViewModel() { MyActiveCartCount = count };
				return View("ctrlCartCount", vm);
			}
		}

		// **************************************
		// URL: /Cart/Add/5
		// **************************************
		//[HttpPost]
		public ActionResult Add(int id) {
			try {

				_cartService.AddToMyActiveCart(id);
				CacheService.RefreshMyActiveCart(_currentUser.UserName);

				if (Request.IsAjaxRequest()) {
					return Json(id, JsonRequestBehavior.AllowGet);
				} else {
					this.FireInfo("Item added to cart");
					return RedirectToAction("Index");
				}
			}
			catch (Exception ex) {
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					this.FireError("There was an error adding this item");
					return RedirectToAction("Index", "Error", ex);
				}
			}
		}

		// **************************************
		// URL: /Cart/Remove/5
		// **************************************
		//[HttpPost]
		public ActionResult Remove(int id) {
			try {

				_cartService.RemoveFromMyActiveCart(id);
				CacheService.RefreshMyActiveCart(_currentUser.UserName);

				if (Request.IsAjaxRequest()) {
					return Json(id, JsonRequestBehavior.AllowGet);
				} else {
					this.FireInfo("Item removed from cart");
					return RedirectToAction("Index");
				}
			}
			catch (Exception ex) {
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					this.FireError("There was an error removing this item");
					return RedirectToAction("Index", "Error", ex);
				}
			}
		}

		// **************************************
		// URL: /Cart/Delete/3
		// **************************************
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id) {
			try {

				_cartService.DeleteCart(id);
				//CacheService.RefreshMyActiveCart(_currentUser.UserName);

				this.FireInfo("Cart Deleted");
			}
			catch {
				this.FireError("There was an error deleting this cart");				
			}
			return RedirectToAction("Index");

		}

		// **************************************
		// URL: /Cart/Zip/3
		// **************************************
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Zip(string userArchiveName, IList<ContentUserDownloadable> contentNames) {

			try {
				_cartService.CompressMyActiveCart(userArchiveName, contentNames);
				CacheService.RefreshMyActiveCart(_currentUser.UserName);
				this.FireInfo("Your cart is ready for download");

			}
			catch {
				this.FireError("There was an error zipping this cart. Please try again in a bit.");

			}
			return RedirectToAction("Index");
		}
		//[HttpPost]
		//public ActionResult ZipCompleted() {
		//    return RedirectToAction("Index");

		//}
		// **************************************
		// URL: /MyCarts/Download/5
		// **************************************
		public ActionResult Download(int id) {

			try {
				var cart = _cartService.DownloadCompressedCart(id);
				//CacheService.RefreshMyActiveCart(_currentUser.UserName);

				if (cart == null) { throw new ArgumentException(); }

				Response.ContentType = "application/zip";
				Response.AddHeader("content-disposition", String.Format("filename={0}", cart.ArchiveName));
				return new FileStreamResult(new System.IO.FileStream(cart.ArchivePath(), System.IO.FileMode.Open), "application/zip");
			}
			catch {
				this.FireError("There was an error downloading this cart. Please try again in a bit.");
				return RedirectToAction("Index");
			}
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

			var role = (Roles)_currentUser.RoleId;
			var model = new CartViewModel() {
				PageTitle = "My Song Cart",
				NavigationLocation = "Cart",
				//SearchMenuProperties = CacheService.SearchProperties(role)
				
			};
			return model;

		}
    }
}
