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
		
		protected override void Initialize(RequestContext requestContext) {

			if (!String.IsNullOrWhiteSpace(requestContext.HttpContext.User.Identity.Name)) {
				_catMgmtService.ActiveUserName = requestContext.HttpContext.User.Identity.Name;
				
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
				var vm = new CatalogViewModel();
				
				vm.MyCatalogs = Account.User(false).MyAdminCatalogs();;
				vm.PageTitle = "Catalog Management";
				vm.NavigationLocation = new string[] { "Admin" };
				vm.ActiveUserId = Account.User().UserId;

				return View(vm);
			}
			catch (Exception ex) {
				Log.Error(ex);

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
				var user = Account.User();

				vm.MyCatalogs = new List<Catalog>() { catalog };
				vm.Users = user.MyUserHierarchy();
				vm.Roles = ModelEnums.GetRoles().Where(r => r >= user.RoleId).ToArray();
				vm.CatalogRoles = user.MyAssignableRoles();
				vm.NavigationLocation = new string[] { "Admin" };
				vm.AllowEdit = user.IsSuperAdmin() || user.IsAtLeastInCatalogRole(Roles.Admin, id);

				if (!user.IsSuperAdmin()) {
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

			try {
				_catMgmtService.DeleteCatalog(id);
				CacheService.InitializeApp(true);
				SessionService.Session().InitializeSession(true);

			}
			catch (Exception ex) {
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					this.FeedbackError("There was an error deleting this Catalog...");
				}
			}

			if (Request.IsAjaxRequest()) {
				return Json(id);
			} else {
				this.FeedbackInfo("Catalog deleted");//Catalog Deleted");
				return RedirectToAction(Actions.Index());
			}
		}

		
	}
}
