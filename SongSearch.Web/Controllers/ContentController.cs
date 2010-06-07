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
		private User _currentUser;
		IContentAdminService _cntAdmService;

		protected override void Initialize(RequestContext requestContext) {

			if (!String.IsNullOrWhiteSpace(requestContext.HttpContext.User.Identity.Name)) {
				_cntAdmService.ActiveUserName = requestContext.HttpContext.User.Identity.Name;
				_currentUser = _cntAdmService.ActiveUser;
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

				if (Request.IsAjaxRequest()) {
					model.ViewMode = ViewModes.Embedded;
					return View(Views.ctrlContentDetail, model);

				} else {
					model.ViewMode = ViewModes.Normal;
					return View(model);
				}
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
				model.EditMode = EditModes.Editing;

				if (Request.IsAjaxRequest()) {
					model.ViewMode = ViewModes.Embedded;
					return View("ctrlContentDetail", model);

				} else {
					model.ViewMode = ViewModes.Normal;
					return View("Detail", model);
				}
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
			IList<ContentRightViewModel> rights,
			bool returnData = true) {

			try {
				//if (ModelState.IsValid) {
					//do some saving
					_cntAdmService.Update(content, tags, rights);
				//}
				if (returnData) {
					var vm = GetEditModel(content.ContentId);
					

					if (Request.IsAjaxRequest()) {
						vm.ViewMode = ViewModes.Embedded;
						vm.EditMode = EditModes.Saving;
						return View("ctrlContentDetail", vm);

					} else {
						vm.ViewMode = ViewModes.Normal;
						vm.EditMode = EditModes.Viewing;
						return View("Detail", vm);
					}
				} else {
					return Content(content.ContentId.ToString());
				}
			}
			catch (Exception ex) {
				this.FeedbackError(ex.Message);
				return RedirectToAction(MVC.Error.Index(ex, ex.Message, this.ToString()));
				
			}
		}

		// **************************************
		// GetContentViewModel
		// **************************************
		private ContentViewModel GetContentViewModel() {

			return new ContentViewModel() {
				NavigationLocation = "Search",
				Tags = CacheService.Tags(),
				SectionsAllowed = new List<string> { "Overview", "Lyrics", "Tags" },
				SearchFields = CacheService.Session("SearchFields") as IList<SearchField> ?? new List<SearchField>()
			};

		}

		// **************************************
		// GetDetailModel
		// **************************************
		private ContentViewModel GetDetailModel(int id) {

			var content = SearchService.GetContentDetails(id, _currentUser);
			
			var model = GetContentViewModel();

			model.IsEdit = false;
			model.Content = content;
			if (_currentUser.IsAtLeastInCatalogRole(Roles.Plugger, content.Catalog)) {
				model.SectionsAllowed.Add("Notes");
				model.SectionsAllowed.Add("Rights");
			}

			model.UserCanEdit = _currentUser.IsAtLeastInCatalogRole(Roles.Admin, content.Catalog);
			return model;
		}

		// **************************************
		// GetEditModel
		// **************************************
		private ContentViewModel GetEditModel(int id) {
			//var user = AccountData.User(User.Identity.Name);
			var model = GetContentViewModel();


			var content = SearchService.GetContentDetails(id, _currentUser);

			if (_currentUser.IsAtLeastInCatalogRole(Roles.Admin, content.Catalog)) {

				model.UserCanEdit = true;
				model.Content = content;

				if (_currentUser.IsAtLeastInCatalogRole(Roles.Plugger, content.Catalog)) {
					model.SectionsAllowed.Add("Notes");
					model.SectionsAllowed.Add("Rights");
					model.Territories = CacheService.Territories();
				}
			}

			return model;
		}



    }
}
