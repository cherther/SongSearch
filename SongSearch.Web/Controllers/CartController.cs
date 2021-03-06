﻿using System;
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
	public partial class CartController : Controller
	{

		protected override void Initialize(RequestContext requestContext) {
			
			base.Initialize(requestContext);

		}

		// **************************************
		// URL: /Cart/
		// **************************************
		public virtual ActionResult Index()
		{
			try {

				var vm = GetCartViewModel();
				vm.MyCarts = CartService.MyCarts();
				vm.CartToHighlight = vm.MyCarts.GetLastProcessedCartId();
				
				vm.CartContentHeaders = new string[] { "Title", "Artist", "Year", "File Name", "Download", "Remove" };
				//Reset the searchFields to stop any previous highlighting preferences
				SessionService.Session().SessionUpdate(null, "SearchFields");

				var user = Account.User();
				var msg = user != null ? 
					(
						vm.CartToHighlight != 0 ? this.HttpContext.Session.ProcessingCartMessage(vm.CartToHighlight) : null ??
						HttpContext.Session.DownloadCartMessage(vm.MyCarts)
					): "";

				if (msg != null) {
					this.FeedbackInfo(msg);
				}

				UserEventLogService.LogUserEvent(UserActions.ViewCart);

				return View(vm);
			}
			catch (Exception ex) {
				Log.Error(ex);
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
				count = SessionService.Session().MyActiveCartCount(this.UserName());
			}
			catch (Exception ex) {
				Log.Error(ex);
			}

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

				CartService.AddToMyActiveCart(id);
				UserEventLogService.LogContentEvent(ContentActions.AddToCart, id);

				SessionService.Session().RefreshMyActiveCart(this.UserName());

				if (Request.IsAjaxRequest()) {
					return Json(id, JsonRequestBehavior.AllowGet);
				} else {
					this.FeedbackInfo("1 item added to your song cart");
					return RedirectToAction(MVC.Cart.Index());
				}
			}
			catch (Exception ex) {
				Log.Error(ex);
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					this.FeedbackError("There was an error adding this item");
					return RedirectToAction(MVC.Error.Problem());
				
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
					CartService.AddToMyActiveCart(contentIds);

					contentIds.ForEach(c => UserEventLogService.LogContentEvent(ContentActions.AddToCart, c));

					SessionService.Session().RefreshMyActiveCart(this.UserName());
				}
				if (Request.IsAjaxRequest()) {
					return Json(count, JsonRequestBehavior.AllowGet);
				} else {
					this.FeedbackInfo(String.Format("{0} {1} added to your song cart", items.Count(), "item".Pluralize(items.Count())));
					return RedirectToAction(MVC.Cart.Index());
				}
			}
			catch (Exception ex) {
				Log.Error(ex);
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					this.FeedbackError("There was an error adding the items");
					return RedirectToAction(MVC.Error.Problem());

				}
			}
		}
		// **************************************
		// URL: /Cart/Remove/5
		// **************************************
		//[HttpPost]
		public virtual ActionResult Remove(int id) {
			try {

				CartService.RemoveFromMyActiveCart(id);
				UserEventLogService.LogContentEvent(ContentActions.RemoveFromCart, id);
				SessionService.Session().RefreshMyActiveCart(this.UserName());

				if (Request.IsAjaxRequest()) {
					return Json(id, JsonRequestBehavior.AllowGet);
				} else {
					this.FeedbackInfo("1 item removed from your song cart");
					return RedirectToAction(MVC.Cart.Index());
				}
			}
			catch (Exception ex) {
				Log.Error(ex);
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					this.FeedbackError("There was an error removing this item");
					return RedirectToAction(MVC.Error.Problem());
				
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
				CartService.RemoveFromMyActiveCart(contentIds);
				contentIds.ForEach(c => UserEventLogService.LogContentEvent(ContentActions.RemoveFromCart, c));
				SessionService.Session().RefreshMyActiveCart(this.UserName());

				if (Request.IsAjaxRequest()) {
					return Json(count, JsonRequestBehavior.AllowGet);
				} else {
					this.FeedbackInfo(String.Format("{0} {1} removed from your your song cart", items.Count(), "item".Pluralize(items.Count())));
					return RedirectToAction(MVC.Cart.Index());
				}
			}
			catch (Exception ex) {
				Log.Error(ex);
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					this.FeedbackError("There was an error removing the item(s)");
					return RedirectToAction(MVC.Error.Problem());
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

				CartService.DeleteCart(id);
				//CacheService.RefreshMyActiveCart(_currentUser.UserName);
				//UserEventLogService.LogUserEvent(ContentActions.DeletedCart, id);
				UserEventLogService.LogUserEvent(UserActions.DeleteCart);
				this.FeedbackInfo("Cart deleted");
			}
			catch (Exception ex) {
				Log.Error(ex);
				this.FeedbackError("There was an error deleting this cart");				
			}
			return RedirectToAction(MVC.Cart.Index());

		}

		// **************************************
		// URL: /Cart/Zip/3
		// **************************************
		[HttpPost]
		[ValidateAntiForgeryToken]
		public virtual ActionResult Zip(int id, string userArchiveName, IList<ContentUserDownloadable> contentNames) {

			try {
				//if (contentNames.Count() > 10 || SystemConfig.UseRemoteMedia) {
				CartService.CompressMyActiveCartOffline(userArchiveName, contentNames);
					UserEventLogService.LogUserEvent(UserActions.CompressCart);

			}
			catch (Exception ex) {
				Log.Error(ex);
				this.FeedbackError("There was an error zipping this cart. Please try again in a bit.");
				return RedirectToAction(MVC.Cart.Index());

			}
			
			SessionService.Session().RefreshMyActiveCart(this.UserName());
			var msgKey = String.Concat("ShowProcessedCart_", id);
			SessionService.Session().SessionUpdate("1", msgKey);
			this.FeedbackInfo("Your cart is currently being zipped up and will be available for download shortly. Please check back on this page in a few minutes.");

				//} else {
				//    _cartService.CompressMyActiveCart(userArchiveName, contentNames);
				//    SessionService.Session().RefreshMyActiveCart(this.UserName());
				//    this.FeedbackInfo("Your cart is ready for download");
				//}
			
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
				var cart = CartService.DownloadCompressedCart(id);
				UserEventLogService.LogUserEvent(UserActions.DownloadCart);

				//CacheService.RefreshMyActiveCart(_currentUser.UserName);

				if (cart == null) { throw new ArgumentException(); }

				Response.ContentType = "application/zip";
				Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", cart.ArchiveName));

				return new FileStreamResult(new System.IO.FileStream(cart.ArchivePath(), System.IO.FileMode.Open), "application/zip");
			}
			catch (Exception ex) {
				Log.Error(ex);
				this.FeedbackError("There was an error downloading this cart. Please try again in a bit.");
				return RedirectToAction(MVC.Cart.Index());
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
			return new CartViewModel() {
				PageTitle = "My Song Cart",
				NavigationLocation = new string[] { "Cart" },
				
			};
			
		}
	}
}
