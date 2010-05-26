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
		IUserManagementService _usrMgmtService;
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
		// URL: /Admin/Invite
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
			string sender = String.Format("{0} <{1}>", _usrMgmtService.ActiveUser.FullName(), _usrMgmtService.ActiveUser.UserName); // Configuration.Get("AdminAddress");

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

    }
}
