using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Data;
using System.Web.Routing;
using SongSearch.Web.Services;

namespace SongSearch.Web.Controllers
{
	[RequireAuthorization]
	[HandleError]
	public partial class ContentController : Controller
	{
		IContentAdminService _cntAdmService;

		protected override void Initialize(RequestContext requestContext) {

			if (!String.IsNullOrWhiteSpace(requestContext.HttpContext.User.Identity.Name)) {
				_cntAdmService.ActiveUserName = requestContext.HttpContext.User.Identity.Name;
				
			}


			base.Initialize(requestContext);
		}

		public ContentController(IContentAdminService cntAdmService) {
			_cntAdmService = cntAdmService;
		}

		// **************************************
		// Detail/5
		// **************************************
		public virtual ActionResult Detail(int id) {

			try {
				var model = GetDetailModel(id);
				model.EditMode = EditModes.Viewing;

				//if (Request.IsAjaxRequest()) {
					model.ViewMode = ViewModes.Embedded;
					return View(Views.ctrlContentDetail, model);

				//} else {
				//    model.ViewMode = ViewModes.Normal;
				//    return View(model);
				//}
			}
			catch (Exception ex) {
				this.FeedbackError(ex.Message);
				return RedirectToAction(MVC.Error.Index(ex, ex.Message, this.ToString()));
				
			}
		}

		// **************************************
		// Print/5
		// **************************************
		public virtual ActionResult Print(int id) {
			try {
				var model = GetDetailModel(id);
				model.ViewMode = ViewModes.Print;
				model.EditMode = EditModes.Viewing;

				return View(model);
			}
			catch (Exception ex) {
				this.FeedbackError(ex.Message);
				return RedirectToAction(MVC.Error.Index(ex, ex.Message, this.ToString()));
				
			}
		}

		
		// **************************************
		// Edit/5
		// **************************************
		[RequireAuthorization(MinAccessLevel=Roles.Admin)]
		public virtual ActionResult Edit(int id) {

			try {
				var model = GetEditModel(id);
				model.ActiveUserId = Account.User().UserId;

				model.EditMode = EditModes.Editing;

				//if (Request.IsAjaxRequest()) {
					model.ViewMode = ViewModes.Embedded;
					return View("ctrlContentDetail", model);

				//} else {
				//    model.ViewMode = ViewModes.Normal;
				//    return View("Detail", model);
				//}
			}
			catch (Exception ex) {
				this.FeedbackError(ex.Message);
				return RedirectToAction(MVC.Error.Index(ex, ex.Message, this.ToString()));
			}
		}

		// **************************************
		// Save/5
		// **************************************
		[RequireAuthorization(MinAccessLevel = Roles.Admin)]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public virtual ActionResult Save(Content content, 
			IList<int> tags,
			IDictionary<TagType, string> newTags,
			IList<ContentRightViewModel> rights,
			bool returnData = true) {

			try {
				//if (ModelState.IsValid) {
					//do some saving
				if (Account.User().HasAccessToContentWithRole(content, Roles.Admin)) {
					_cntAdmService.Update(content, tags, newTags, rights);
				}
				//}
				if (returnData) {
					var vm = GetEditModel(content.ContentId);
					

					//if (Request.IsAjaxRequest()) {
						vm.ViewMode = ViewModes.Embedded;
						vm.EditMode = EditModes.Saving;
						return View("ctrlContentDetail", vm);

					//} else {
					//    vm.ViewMode = ViewModes.Normal;
					//    vm.EditMode = EditModes.Viewing;
					//    return View("Detail", vm);
					//}
				} else {
					return Content(content.ContentId.ToString());
				}
			}
			catch (Exception ex) {
				this.FeedbackError(ex.Message);
				return RedirectToAction(MVC.Error.Index(ex, ex.Message, this.ToString()));
				
			}
		}
		[RequireAuthorization(MinAccessLevel = Roles.Admin)]
		[HttpPost]
		public virtual ActionResult SaveMetaDataToFile(int id) {

			_cntAdmService.SaveMetaDataToFile(id);
	
			if (Request.IsAjaxRequest()) {
				return Json(id);

			} else {
				return View();
			}
		}

		[RequireAuthorization(MinAccessLevel = Roles.Admin)]
		[HttpPost]
		public virtual ActionResult SaveMediaFiles(int contentId, IList<UploadFile> uploadFiles) {

			_cntAdmService.UpdateContentMedia(contentId, uploadFiles);
			
			if (Request.IsAjaxRequest()) {
				return Json(contentId);

			} else {
				return View();
			}
		}

		[RequireAuthorization(MinAccessLevel = Roles.Admin)]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public virtual ActionResult Delete(int id) {

			return View();
		}

		[RequireAuthorization(MinAccessLevel = Roles.Admin)]
		[HttpPost]
		public virtual ActionResult DeleteMultiple() {

			try {
				//var itemsPosted = Request.Form["deleteContentItems"];
				var itemsPosted = Request.Form["items[]"];
				var items = itemsPosted != null ? itemsPosted.Split(',') : new string[] { };
				var count = items != null ? items.Count() : 0;

				var contentIds = items.Select(i => int.Parse(i)).ToArray();
				_cntAdmService.Delete(contentIds);

				CacheService.InitializeApp(true);
				SessionService.Session().RefreshMyActiveCart(this.UserName());

				if (Request.IsAjaxRequest()) {
					return Json(count, JsonRequestBehavior.AllowGet);
				} else {
					this.FeedbackInfo("Item(s) deleted");
					return RedirectToAction(MVC.CatalogManagement.Index());
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

		[RequireAuthorization(MinAccessLevel = Roles.Admin)]
		[HttpPost]
		public virtual ActionResult DeleteTag(int id) {

			_cntAdmService.DeleteTag(id);

			if (Request.IsAjaxRequest()) {
				return Json(id, JsonRequestBehavior.AllowGet);
			} else {
				this.FeedbackInfo("Tag deleted");
				return View();
			}
		}

		// **************************************
		// GetContentViewModel
		// **************************************
		private ContentViewModel GetContentViewModel() {

			return new ContentViewModel() {
				NavigationLocation = new string[] { "Search" },
				Tags = CacheService.Tags(),
				SectionsAllowed = new List<string> { "Overview", "Lyrics", "Tags" },
				SearchFields = SessionService.Session().Session("SearchFields") as IList<SearchField> ?? new List<SearchField>()
			};

		}

		// **************************************
		// GetDetailModel
		// **************************************
		private ContentViewModel GetDetailModel(int id) {

			var user = Account.User();
			var content = SearchService.GetContentDetails(id, user);
			
			var model = GetContentViewModel();

			model.IsEdit = false;
			model.Content = content;

			if (user.IsSuperAdmin()) {
				model.SectionsAllowed.Add("Notes");
			}

			if (user.IsAtLeastInCatalogRole(Roles.Plugger, content.Catalog)) {
				model.SectionsAllowed.Add("Rights");
			}

			if (user.IsAtLeastInCatalogRole(Roles.Admin, content.Catalog)) {
				model.SectionsAllowed.Add("Media");
				//if (user.ShowDebugInfo.GetValueOrDefault(false)) {
				//    model.SectionsAllowed.Add("MediaExtended");
				//}
				model.UserCanEdit = true;
			} else {
				model.UserCanEdit = false;
			}
			return model;
		}

		// **************************************
		// GetEditModel
		// **************************************
		private ContentViewModel GetEditModel(int id) {
			//var user = AccountData.User(User.Identity.Name);
			var model = GetContentViewModel();
			model.Territories = CacheService.Territories();

			var user = Account.User();
			var content = SearchService.GetContentDetails(id, user);

			if (user.IsSuperAdmin()) {
				model.SectionsAllowed.Add("Notes");
			}

			if (user.IsAtLeastInCatalogRole(Roles.Plugger, content.Catalog)) {
				model.SectionsAllowed.Add("Rights");
			}

			if (user.IsAtLeastInCatalogRole(Roles.Admin, content.Catalog)) {

				model.UserCanEdit = true;
				model.Content = content;
				model.SectionsAllowed.Add("Media");
				if (user.ShowDebugInfo.GetValueOrDefault(false)) {
					model.SectionsAllowed.Add("MediaExtended");
				}
			}

			return model;
		}



	}
}
