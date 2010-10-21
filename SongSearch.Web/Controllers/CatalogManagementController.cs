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
		
		protected override void Initialize(RequestContext requestContext) {

			base.Initialize(requestContext);

		}


		// **************************************
		// URL: /CatalogManagement/
		// **************************************
		public virtual ActionResult Index(int? id = null)
		{
			try {
				var vm = new CatalogViewModel();
				
				vm.MyCatalogs = Account.User(false).MyAdminCatalogs();;
				vm.PageTitle = "Catalog Management";
				vm.NavigationLocation = new string[] { "Admin" };
				vm.ActiveUserId = Account.User().UserId;
				vm.CatalogContents = SearchService.GetLatestContent(Account.User(), id);

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
				var user = Account.User();
				var catalog = CatalogManagementService.GetCatalogDetail(id);

				var vm = new CatalogViewModel();
				vm.PageTitle = "Catalog Management";
				vm.NavigationLocation = new string[] { "Admin" };
				vm.AllowEdit = user.IsSuperAdmin() || user.IsAtLeastInCatalogRole(Roles.Admin, id);
				vm.ActiveUserId = Account.User().UserId;

				vm.MyCatalogs = Account.User(false).MyAdminCatalogs(); ;
				vm.MyUsers = user.MyUserHierarchy();
				//vm.MyUsers = vm.MyUsers != null ? vm.MyUsers.OrderBy(x => x.FullName()).ToList() : null;

				vm.Roles = ModelEnums.GetRoles().Where(r => r >= user.RoleId).ToArray();
				vm.CatalogRoles = user.MyAssignableRoles();

				vm.Catalog = catalog;
				vm.CatalogContents = catalog.Contents.OrderBy(c => c.Artist).ThenBy(c => c.Title).ToList();
		
	
				UserEventLogService.LogUserEvent(UserActions.ViewCatalogDetail);

				if (!user.IsSuperAdmin()) {
					//vm.LookupCatalogs = vm.LookupCatalogs.LimitToAdministeredBy(user);
				}
				if (Request.IsAjaxRequest()) {
					return View(Views.ctrlDetail, vm);
				} else {
					return View(Views.Detail, vm); //return View(vm);
				}
			}
			catch (Exception ex) {
				Log.Error(ex);

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
				CatalogManagementService.DeleteCatalog(id);
				UserEventLogService.LogUserEvent(UserActions.DeleteCatalog);

				CacheService.InitializeApp(true);
				SessionService.Session().InitializeSession(true);

			}
			catch (Exception ex) {
				Log.Error(ex);

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
