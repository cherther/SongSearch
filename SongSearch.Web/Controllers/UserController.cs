using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Services;
using System.Web.Routing;
using SongSearch.Web.Data;

namespace SongSearch.Web.Controllers
{
    [HandleError]
	[RequireAuthorization(MinAccessLevel=Roles.Admin)]
	public class UserController : Controller
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

		public UserController(
			IUserManagementService usrMgmtService
			) {
			_usrMgmtService = usrMgmtService;
        }

		//
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

		// **************************************
		// URL: /User/Invite
		// **************************************
		public ActionResult Invite() {
			var vm = new InviteViewModel() {
				NavigationLocation = "Admin"
			};

			vm.InviteId = Guid.Empty.ToString();
			
			return View(vm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateOnlyIncomingValues]
		public ActionResult Invite(InviteViewModel invites) {

			string[] recipients = invites.Recipients;
			string sender = String.Format("{0} <{1}>", _currentUser.FullName(), _currentUser.UserName); // Configuration.Get("AdminAddress");

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
			return View("InviteComplete", invites);
		}

		// **************************************
		// URL: /User/Manage
		// **************************************
		public ActionResult Manage() {

			var users = _usrMgmtService.GetMyUserHierarchy();
			var invites = _usrMgmtService.GetMyInvites(InvitationStatusCodes.Open);
			var vm = new UserViewModel();

			vm.MyUsers = users;
			vm.MyInvites = invites;
			vm.PageTitle = "User Management";
			vm.NavigationLocation = "Admin";

			return View(vm);
		}

		// **************************************
		// URL: /User/Manage
		// **************************************
		public ActionResult Detail(int id) {

			var user = _usrMgmtService.GetUserDetail(id);
			var vm = new UserViewModel();

			vm.MyUsers = new List<User>() { user };
			vm.Catalogs = CacheService.Catalogs();
			vm.Roles = ModelEnums.GetRoles().Where(r => r >= _currentUser.RoleId).ToArray();
			vm.IsThisUser = user.UserId == _currentUser.UserId;
			vm.NavigationLocation = "Admin";
			vm.AllowEdit = !vm.IsThisUser && !user.IsSuperAdmin();

			if (!_currentUser.IsSuperAdmin()) {
				//vm.LookupCatalogs = vm.LookupCatalogs.LimitToAdministeredBy(user);
			}
			if (Request.IsAjaxRequest()) {
				return View("ctrlDetail", vm);
			} else {
				return View(vm);
			}
		}
    }
}
