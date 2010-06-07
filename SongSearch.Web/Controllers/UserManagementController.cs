using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Services;
using System.Web.Routing;
using SongSearch.Web.Data;
using System.Text.RegularExpressions;

namespace SongSearch.Web
{
    [HandleError]
	[RequireAuthorization(MinAccessLevel=Roles.Admin)]
	public partial class UserManagementController : Controller
    {
		private IUserManagementService _usrMgmtService;
		private User _currentUser;
		
		protected override void Initialize(RequestContext requestContext) {
			
			if (!String.IsNullOrWhiteSpace(requestContext.HttpContext.User.Identity.Name)) {
				_usrMgmtService.ActiveUserName = requestContext.HttpContext.User.Identity.Name;
				_currentUser = _usrMgmtService.ActiveUser;
			}
			base.Initialize(requestContext);

		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext) {
			ViewData["PasswordLength"] = AccountService.MinPasswordLength;

			base.OnActionExecuting(filterContext);
		}

		public UserManagementController(IUserManagementService usrMgmtService) {
			_usrMgmtService = usrMgmtService;
        }


		// **************************************
		// URL: /UserManagement/
		// **************************************
		public virtual ActionResult Index() {

			try {
				var users = _usrMgmtService.GetMyUserHierarchy();
				var invites = _usrMgmtService.GetMyInvites(InvitationStatusCodes.Open);
				var vm = new UserViewModel();

				vm.MyUsers = users;
				vm.MyInvites = invites;
				vm.PageTitle = "User Management";
				vm.NavigationLocation = "Admin";

				return View(vm);
			}
			catch {
				this.FireError("There was an error loading the User Management page. Please try again in a bit.");
				return RedirectToAction(MVC.Home.Index());
			}
		}

		// **************************************
		// URL: /UserManagement/Invite
		// **************************************
		public virtual ActionResult Invite() {
			return View(
				new InviteViewModel() {
					NavigationLocation = "Admin",
					InviteId = Guid.Empty.ToString()
				});
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateOnlyIncomingValues]
		public virtual ActionResult Invite(InviteViewModel model) {

			if (ModelState.IsValid) {
				string[] recipients = model.Recipients;
				if (recipients.Count() == 0) {
					ModelState.AddModelError("Recipient", "Please enter a valid email address");
					this.FireError("There was an error processing your invites...");
					return View(model);
				
				}
				string sender = String.Format("{0} <{1}>", _currentUser.FullName(), _currentUser.UserName); // Configuration.Get("AdminAddress");

				foreach (string recipient in recipients) {
					string address = recipient.ToLower().Trim();

					if (!Regex.Match(address, @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}").Success) {
						ModelState.AddModelError("Recipient", "'" + address + "' is not a valid email address");
						this.FireError("There was an error processing your invites...");
						return View(model);
					}
				}

				foreach (string recipient in recipients) {
					string address = recipient.ToLower().Trim();

					if (address != "sample@sample.com") {

						var inviteId = _usrMgmtService.CreateNewInvitation(address);


						if ((inviteId != null) && (!inviteId.Equals(Guid.Empty))) {
							var inviteMsg = new InviteViewModel();
							inviteMsg.NavigationLocation = "Admin";
							inviteMsg.InviteId = inviteId.ToString();
							inviteMsg.Sender = sender;
							inviteMsg.Recipient = address;
							inviteMsg.BaseUrl = Settings.BaseUrl.Text();
							inviteMsg.InviteUrl = String.Format("{0}/{1}", inviteMsg.BaseUrl, "Account/Register");

							//string inviteLink = String.Format("{0}", baseUrl);
							string subject = Messages.InvitationSubjectLine.Text();
							string message = this.RenderViewToString("InviteMessage", inviteMsg);

							Mail.SendMail(sender, address, subject, message);
						}

					}
				}
				return View(Views.InviteComplete, model);
			} else {
				ModelState.AddModelError("", "There was an error processing your invites");
				this.FireError("There was an error processing your invites...");
				return View(model);
			}
		}
		
		// **************************************
		// URL: /UserManagement/User/5
		// **************************************
		public virtual ActionResult Detail(int id) {

			try {
				var user = _usrMgmtService.GetUserDetail(id);
				var vm = new UserViewModel();

				vm.MyUsers = new List<User>() { user };
				vm.Catalogs = CacheService.Catalogs();
				vm.Roles = ModelEnums.GetRoles().Where(r => r >= _currentUser.RoleId).ToArray();
				vm.CatalogRoles = ModelEnums.GetPublicRoles().Where(r => r >= _currentUser.RoleId).ToArray();
				vm.IsThisUser = user.UserId == _currentUser.UserId;
				vm.NavigationLocation = "Admin";
				vm.AllowEdit = !vm.IsThisUser && !user.IsSuperAdmin();

				if (!_currentUser.IsSuperAdmin()) {
					//vm.LookupCatalogs = vm.LookupCatalogs.LimitToAdministeredBy(user);
				}
				if (Request.IsAjaxRequest()) {
					return View(Views.ctrlDetail, vm);
				} else {
					return RedirectToAction(MVC.UserManagement.Index()); //return View(vm);
				}
			}
			catch (Exception ex){
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					this.FireError("There was an error processing your request");
					return RedirectToAction(MVC.UserManagement.Index());
				}
			}
		}

		// **************************************
		// URL: /UserManagement/UpdateRole?user=5&roleId=3
		// **************************************
		[HttpPost]
		public virtual ActionResult UpdateRole(int userId, int roleId) {
			try {

				_usrMgmtService.UpdateUsersRole(userId, roleId);

			}
			catch (Exception ex) {
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					this.FireError("There was an error updating this user's role...");
				}
			}

			if (Request.IsAjaxRequest()) {
				return Json(roleId);
			} else {
				this.FireInfo("Succesfully updated user's role");
				return RedirectToAction(MVC.UserManagement.Index());
			}
		}

		// **************************************
		// URL: /UserManagement/UpdateCatalog?user=5&roleId=3
		// **************************************
		[HttpPost]
		public virtual ActionResult UpdateCatalog(int userId, int catalogId, int roleId) {
			try {

				_usrMgmtService.UpdateUserCatalogRole(userId, catalogId, roleId);

			}
			catch (Exception ex) {
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					this.FireError("There was an error updating this user's catalog access...");
				}
			}

			if (Request.IsAjaxRequest()) {
				return Json(roleId);
			} else {
				this.FireInfo("Succesfully updated user's catalog access");
				return RedirectToAction(MVC.UserManagement.Index());
			}
		}

		// **************************************
		// URL: /UserManagement/UpdateCatalog?user=5&roleId=3
		// **************************************
		[HttpPost]
		public virtual ActionResult UpdateAllCatalogs(int userId, int roleId) {
			try {

				_usrMgmtService.UpdateAllCatalogs(userId, roleId);

			}
			catch (Exception ex) {
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					this.FireError("There was an error updating this user's catalog access...");
				}
			}

			if (Request.IsAjaxRequest()) {
				return Json(roleId);
			} else {
				this.FireInfo("Succesfully updated user's catalog access");
				return RedirectToAction(MVC.UserManagement.Index());
			}
		}
		// **************************************
		// URL: /UserManagement/Delete/5
		// **************************************
		[HttpPost]
		[ValidateAntiForgeryToken]
		public virtual ActionResult Delete(int id) {

			try {
				_usrMgmtService.DeleteUser(id, true);

			}
			catch (Exception ex) {
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					this.FireError("There was an error deleting the user...");
				}
			}

			if (Request.IsAjaxRequest()) {
				return Json(id);
			} else {
				this.FireInfo("User Deleted");
				return RedirectToAction(MVC.UserManagement.Index());
			}
		}

		// **************************************
		// URL: /UserManagement/TakeOwnership/5
		// **************************************
		[HttpPost]
		[ValidateAntiForgeryToken]
		public virtual ActionResult TakeOwnership(int id) {
			try {
				_usrMgmtService.TakeOwnerShip(id);
			}
			catch (Exception ex) {
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					this.FireError("There was an error processing your request...");
				}
			}

			if (Request.IsAjaxRequest()) {
				return Json(id);
			} else {
				this.FireInfo("Sucessfully transferred user ownership");
				return RedirectToAction(MVC.UserManagement.Index());
			}
		}
    }
}
