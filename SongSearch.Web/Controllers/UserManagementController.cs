using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Services;
using System.Web.Routing;
using SongSearch.Web.Data;
using System.Text.RegularExpressions;
using System.Runtime.Remoting.Messaging;

namespace SongSearch.Web
{
	[HandleError]
	[RequireAuthorization(MinAccessLevel=Roles.Admin)]
	public partial class UserManagementController : Controller
	{
		private IUserManagementService _usrMgmtService;
		
		protected override void Initialize(RequestContext requestContext) {
			
			if (!String.IsNullOrWhiteSpace(requestContext.HttpContext.User.Identity.Name)) {
				_usrMgmtService.ActiveUserName = requestContext.HttpContext.User.Identity.Name;
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
				vm.NavigationLocation = new string[] { "Admin" };

				return View(vm);
			}
			catch {
				this.FeedbackError("There was an error loading the User Management page. Please try again in a bit.");
				return RedirectToAction(MVC.Home.Index());
			}
		}

		// **************************************
		// URL: /UserManagement/Invite
		// **************************************
		public virtual ActionResult Invite() {

			var vm = GetInviteModel(new InviteViewModel());

			return View(vm);

		}

		private static InviteViewModel GetInviteModel(InviteViewModel model) {
			
			var user = Account.User();
			var quotas = user.MyQuotas();

			model.NavigationLocation = new string[] { "Admin" };
			model.InviteId = model.InviteId ?? Guid.Empty.ToString();
			model.IsPlanInvitation = !user.IsSuperAdmin();
			model.ShowInviteForm = !quotas.NumberOfInvitedUsers.IsAtTheLimit;
			model.ShowPlanMessage = user.IsSuperAdmin() ||
									(quotas.NumberOfCatalogAdmins.Allowed.HasValue && 
									quotas.NumberOfCatalogAdmins.Remaining > 0);
			model.ShowQuotaWidget = user.IsPlanUser;

			return model;
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateOnlyIncomingValues]
		public virtual ActionResult Invite(InviteViewModel model) {

			model = GetInviteModel(model);
			if (ModelState.IsValid) {
				string[] recipients = model.Recipients;
				if (recipients.Count() == 0) {
					ModelState.AddModelError("Recipient", "Please enter a valid email address");
					this.FeedbackError("There was an error processing your invites...");
					return View(model);
				
				}
				string sender = String.Format("{0} <{1}>", this.Friendly(), this.UserName()); // Configuration.Get("AdminAddress");

				foreach (string recipient in recipients) {
					string address = recipient.ToLower().Trim();

					if (!Regex.Match(address, @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}").Success) {
						ModelState.AddModelError("Recipient", "'" + address + "' is not a valid email address");
						this.FeedbackError("There was an error processing your invites...");
						return View(model);
					}
				}

				SendInvites(recipients, sender);

				return View(Views.InviteComplete, model);
			} else {
				ModelState.AddModelError("", "There was an error processing your invites");
				this.FeedbackError("There was an error processing your invites...");
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
				var activeUser = Account.User();

				vm.MyUsers = new List<User>() { user };
				vm.Catalogs = activeUser.IsSuperAdmin() ? CacheService.Catalogs() : activeUser.MyAdminCatalogs();
				vm.Roles = ModelEnums.GetRoles().Where(r => r >= activeUser.RoleId).ToArray();

				vm.CatalogRoles = activeUser.MyAssignableRoles();
				vm.IsThisUser = user.UserId == activeUser.UserId;
				vm.NavigationLocation = new string[] { "Admin" };
				vm.AllowEdit = !vm.IsThisUser && !user.IsSuperAdmin();

				if (Request.IsAjaxRequest()) {
					return View(Views.ctrlDetail, vm);
				} else {
					return View(Views.ctrlDetail, vm); //RedirectToAction(Actions.Index()); //return View(vm);
				}
			}
			catch (Exception ex){
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					this.FeedbackError("There was an error processing your request");
					return RedirectToAction(Actions.Index());
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
					this.FeedbackError("There was an error updating this user's role...");
				}
			}

			if (Request.IsAjaxRequest()) {
				return Json(roleId);
			} else {
				this.FeedbackInfo("Succesfully updated user's role");
				return RedirectToAction(MVC.UserManagement.Index());
			}
		}

		// **************************************
		// URL: /UserManagement/ToggleSystemAdminAccess?user=5
		// **************************************
		[HttpPost]
		public virtual ActionResult ToggleSystemAdminAccess(int userId) {
			try {

				_usrMgmtService.ToggleSystemAdminAccess(userId);

			}
			catch (Exception ex) {
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					this.FeedbackError("There was an error updating this user's role...");
				}
			}

			if (Request.IsAjaxRequest()) {
				return Json(userId);
			} else {
				this.FeedbackInfo("Succesfully updated user's role");
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
					this.FeedbackError("There was an error updating this user's catalog access...");
				}
			}

			if (Request.IsAjaxRequest()) {
				return Json(roleId);
			} else {
				this.FeedbackInfo("Succesfully updated user's catalog access");
				return RedirectToAction(Actions.Index());
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
					this.FeedbackError("There was an error updating this user's catalog access...");
				}
			}

			if (Request.IsAjaxRequest()) {
				return Json(roleId);
			} else {
				this.FeedbackInfo("Succesfully updated user's catalog access");
				return RedirectToAction(Actions.Index());
			}
		}

		// **************************************
		// URL: /UserManagement/UpdateCatalog?user=5&roleId=3
		// **************************************
		[HttpPost]
		public virtual ActionResult UpdateAllUsers(int catalogId, int roleId) {
			try {

				_usrMgmtService.UpdateAllUsers(catalogId, roleId);

			}
			catch (Exception ex) {
				if (Request.IsAjaxRequest()) {
					throw ex;
				} else {
					this.FeedbackError("There was an error updating all users' catalog access...");
				}
			}

			if (Request.IsAjaxRequest()) {
				return Json(roleId);
			} else {
				this.FeedbackInfo("Succesfully updated all users' catalog access");
				return RedirectToAction(Actions.Index());
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
					this.FeedbackError("There was an error deleting the user...");
				}
			}

			if (Request.IsAjaxRequest()) {
				return Json(id);
			} else {
				this.FeedbackInfo("User Deleted");
				return RedirectToAction(Actions.Index());
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
					this.FeedbackError("There was an error processing your request...");
				}
			}

			if (Request.IsAjaxRequest()) {
				return Json(id);
			} else {
				this.FeedbackInfo("Sucessfully transferred user ownership");
				return RedirectToAction(Actions.Index());
			}
		}


		private delegate void SendInvitesDelegate(string[] recipients, string sender);
		delegate void EndInvokeDelegate(IAsyncResult result);

		private void SendInvitesAsync(string[] recipients, string sender) {
			
			//hand off compression work
				SendInvitesDelegate inviteDelegate = new SendInvitesDelegate(SendInvites);
				// Define the AsyncCallback delegate.
				AsyncCallback callBack = new AsyncCallback(this.SendInvitesCallback);

				inviteDelegate.BeginInvoke(recipients, sender,
					callBack,
					new object()
					);

		}

		private void SendInvitesCallback(IAsyncResult asyncResult) {
			// Extract the delegate from the 
			// System.Runtime.Remoting.Messaging.AsyncResult.
			SendInvitesDelegate inviteDelegate = (SendInvitesDelegate)((AsyncResult)asyncResult).AsyncDelegate;
			int cartId = (int)asyncResult.AsyncState;

			inviteDelegate.EndInvoke(asyncResult);
		}

		private void SendInvites(string[] recipients, string sender) {
			foreach (string recipient in recipients) {
				string address = recipient.ToLower().Trim();

				if (address != "sample@sample.com") {

					var inviteId = _usrMgmtService.CreateNewInvitation(address);


					if ((inviteId != null) && (!inviteId.Equals(Guid.Empty))) {
						var inviteMsg = new InviteViewModel();
						inviteMsg.NavigationLocation = new string[] { "Admin" };
						inviteMsg.InviteId = inviteId.ToString();
						inviteMsg.Sender = sender;
						inviteMsg.Recipient = address;
						inviteMsg.BaseUrl = SystemConfig.BaseUrl;
						inviteMsg.InviteUrl = String.Format("{0}/{1}", inviteMsg.BaseUrl, "Account/Register");

						//string inviteLink = String.Format("{0}", baseUrl);
						string subject = String.Format("{0} {1}", SiteProfileData.SiteProfile().CompanyName,
							SystemMessages.InvitationSubjectLine);
						string message = this.RenderViewToString("InviteMessage", inviteMsg);

						Mail.SendMail(sender, address, subject, message);
					}

				}
			}
		}
	}
}
