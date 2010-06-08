using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Services;
using SongSearch.Web.Data;
using System.Web.Routing;

namespace SongSearch.Web.Controllers
{
	[HandleError]
	[RequireAuthorization(MinAccessLevel = Roles.Admin)]
	public partial class CatalogManagementController : Controller {
		private ICatalogManagementService _catMgmtService;
		private User _currentUser;

		protected override void Initialize(RequestContext requestContext) {

			if (!String.IsNullOrWhiteSpace(requestContext.HttpContext.User.Identity.Name)) {
				_catMgmtService.ActiveUserName = requestContext.HttpContext.User.Identity.Name;
				_currentUser = _catMgmtService.ActiveUser;
			}
			base.Initialize(requestContext);

		}

		public CatalogManagementController(ICatalogManagementService catMgmtService) {
			_catMgmtService = catMgmtService;
		}


		// **************************************
		// URL: /CatalogManagement/
		// **************************************
		public virtual ActionResult Index()
        {
			try {
				var catalogs = _catMgmtService.GetMyCatalogs();
				var vm = new CatalogViewModel();

				vm.MyCatalogs = catalogs;
				vm.PageTitle = "CatalogManagement";
				vm.NavigationLocation = "Admin";

				return View(vm);
			}
			catch {
				this.FeedbackError("There was an error loading the Catalog Management page. Please try again in a bit.");
				return RedirectToAction(MVC.Home.Index());
			}
        }

		// **************************************
		// URL: /CatalogManagement/Detail/5
		// **************************************
		public virtual ActionResult Detail(int id) {
			try {
				var catalog = _catMgmtService.GetCatalogDetail(id);
				var vm = new CatalogViewModel();

				vm.MyCatalogs = new List<Catalog>() { catalog };
				vm.Users = _currentUser.GetUserHierarchy();
				vm.Roles = ModelEnums.GetRoles().Where(r => r >= _currentUser.RoleId).ToArray();
				vm.CatalogRoles = ModelEnums.GetPublicRoles().Where(r => r >= _currentUser.RoleId).ToArray();
				vm.NavigationLocation = "Admin";
				vm.AllowEdit = _currentUser.IsSuperAdmin() || _currentUser.IsAtLeastInCatalogRole(Roles.Admin, id);

				if (!_currentUser.IsSuperAdmin()) {
					//vm.LookupCatalogs = vm.LookupCatalogs.LimitToAdministeredBy(user);
				}
				if (Request.IsAjaxRequest()) {
					return View(Views.ctrlDetail, vm);
				} else {
					return RedirectToAction(Actions.Index()); //return View(vm);
				}
			}
			catch (Exception ex) {
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					this.FeedbackError("There was an error processing your request");
					return RedirectToAction(Actions.Index());
				}
			}
		}

		// **************************************
		// URL: /CatalogManagement/Delete/5
		// **************************************
		[HttpPost]
		public virtual ActionResult Delete(int id) {
			return View();
		}

    }
}
