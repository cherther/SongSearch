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
			vm.CartContentHeaders = new string[] { "Title", "Artist", "ReleaseYear", "File name"};
			

			return View(vm);
        }

		// **************************************
		// URL: /Cart/CartCount
		// **************************************
		public ActionResult CartCount() {
			var cart = CacheService.MyActiveCart(_currentUser.UserName);
			int count = cart != null && cart.Contents != null ? cart.Contents.Count() : 0;// _crts.MyActiveCartContentsCount();
			cart = null;

			if (Request.IsAjaxRequest()) {
				return Json(count, JsonRequestBehavior.AllowGet);
			} else {
				ViewData["MyActiveCartContentsCount"] = count;
				return View();
			}
		}

		// **************************************
		// URL: /Cart/Add/5
		// **************************************
		[HttpPost]
		public ActionResult Add(int id) {
			try {

				_cartService.AddToMyActiveCart(id);
				CacheService.RefreshMyActiveCart(_currentUser.UserName);

				if (Request.IsAjaxRequest()) {
					return Json(id);
				} else {
					return RedirectToAction("Index");
				}
			}
			catch (Exception ex) {
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					return RedirectToAction("Index", "Error", ex);
				}
			}
		}

		// **************************************
		// URL: /Cart/Remove/5
		// **************************************
		[HttpPost]
		public ActionResult Remove(int id) {
			try {

				_cartService.RemoveFromMyActiveCart(id);
				CacheService.RefreshMyActiveCart(_currentUser.UserName);

				if (Request.IsAjaxRequest()) {
					return Json(id);
				} else {
					return RedirectToAction("Index");
				}
			}
			catch (Exception ex) {
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					return RedirectToAction("Index", "Error", ex);
				}
			}
		}

		// **************************************
		// URL: /Cart/Delete/3
		// **************************************
		[HttpPost]
		public ActionResult Delete(int id) {
			try {

				//itmTag = new ItemsTag() { ContentID = id, TagID = tid };
				//rep.Insert(itmTag);
				_cartService.DeleteCart(id);
				CacheService.RefreshMyActiveCart(_currentUser.UserName);

				if (Request.IsAjaxRequest()) {
					return Json(id);
				} else {
					return RedirectToAction("Index");
				}
			}
			catch (Exception ex) {
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					return RedirectToAction("Index", "Error", ex);
				}
			}
		}

		// **************************************
		// URL: /Cart/Zip/3
		// **************************************
		[HttpPost]
		public ActionResult Zip(string userArchiveName, IList<ContentUserDownloadable> contentNames) {
			_cartService.CompressMyActiveCart(userArchiveName, contentNames);
			CacheService.RefreshMyActiveCart(_currentUser.UserName);
			return RedirectToAction("Index");

		}
		public ActionResult ZipCompleted() {
			return RedirectToAction("Index");

		}
		// **************************************
		// URL: /MyCarts/Download/5
		// **************************************
		public ActionResult Download(int id) {

			var cart = _cartService.DownloadCompressedCart(id);
			CacheService.RefreshMyActiveCart(_currentUser.UserName);

			if (cart == null) { throw new ArgumentException(); }

			Response.ContentType = "application/zip";
			Response.AddHeader("content-disposition", String.Format("filename={0}", cart.ArchiveName));
			return new FileStreamResult(new System.IO.FileStream(cart.ArchivePath(), System.IO.FileMode.Open), "application/zip");
		}

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