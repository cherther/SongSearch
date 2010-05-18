using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Services;
using System.Web.Routing;

namespace SongSearch.Web
{
    [HandleError]
	[RequireAuthorization(MinAccessLevel=Roles.Admin)]
	public class UserController : Controller
    {
		IUserManagementService _ums;

		protected override void Initialize(RequestContext requestContext) {
			if (_ums == null) { _ums = new UserManagementService(requestContext.HttpContext.User.Identity.Name); }

			base.Initialize(requestContext);

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
			var user = AccountData.User(User.Identity.Name);
			string sender = String.Format("{0} <{1}>", user.FullName(), user.UserName); // Configuration.Get("AdminAddress");

			foreach (string recipient in recipients) {
				string address = recipient.ToLower().Trim();

				if (address != "sample@sample.com") {
					var invite = new InviteViewModel();

					var inviteId = _ums.CreateNewInvitation(address);

					invite.NavigationLocation = "Admin";

					if ((inviteId != null) && (!inviteId.Equals(Guid.Empty))) {
						invite.InviteId = inviteId.ToString();
						invite.Sender = sender;
						invite.Recipient = address;
						invite.BaseUrl = Settings.BaseUrl.Text();
						invite.InviteUrl = String.Format("{0}/{1}", invite.BaseUrl, "Account/Register");

						//string inviteLink = String.Format("{0}", baseUrl);
						string subject = Messages.InvitationSubjectLine.Text();
						string message = this.RenderViewToString("InviteMessage", invite);

						Mail.SendMail(sender, address, subject, message);
					}

				}
			}
			return View("InviteComplete", invites);
		}

    }
}
