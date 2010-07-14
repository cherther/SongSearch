using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Services;
using System.Text;

namespace SongSearch.Web.Controllers {
	[HandleError]
	public partial class HomeController : Controller {
		// **************************************
		// URL: /
		// **************************************
		public virtual ActionResult Index() {

			try {
				if (User.Identity.IsAuthenticated) {

					var msg = SessionService.Session().User(User.Identity.Name).LoginMessage();

					this.FeedbackInfo(msg);
				}
			}
			catch { }
			return View(new ViewModel() { NavigationLocation = new string[] { "Home" } });
			
		}

		// **************************************
		// URL: /Home/Contact/
		// **************************************
		public virtual ActionResult Contact() {
			var model = new ContactModel() { 
				Email = User.Identity.IsAuthenticated ? User.Identity.Name : "",
				Name = User.Identity.IsAuthenticated ? this.Friendly() : "",
				NavigationLocation = new string[] { "Contact" } 
			
			};
			model.PageTitle = "Send us a question or comment:";
			return View(model);
		}

		[HttpPost]
		[ValidateOnlyIncomingValues]
		[ValidateAntiForgeryToken]
		public virtual ActionResult Contact(ContactModel model) {

			if (ModelState.IsValid) {
				var vm = new ContactModel() { NavigationLocation = new string[] { "Contact" } };
				vm.PageTitle = "Thanks for e-mailing us!";
				vm.PageMessage = "Your e-mail has been successfully sent to our team, and we will review your message and respond as quickly as possible.";

				string sender = String.Format("{0} <{1}>", model.Name, model.Email);
				string subject = String.Format("[{0} Contact Us] {1}", vm.SiteProfile.CompanyName, model.Subject);
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("<p>Name: {0}</p>", model.Name);
				sb.AppendFormat("<p>Email: {0}</p>", model.Email);
				sb.AppendFormat("<p>Company: {0}</p>", model.Company);
				sb.AppendFormat("<p>Regarding: {0}</p>", model.Subject);
				sb.AppendFormat("<p>Comments:</p><p><pre>{0}</pre></p>", model.Body);

				string msg = sb.ToString();
				sb = null;


				Mail.SendMail(
					sender,
					vm.SiteProfile.ContactEmail,//SiteProfileData.SiteProfile().ContactEmail,//Settings.ContactEmailAddress.Text(),
					subject,
					msg
					);

				return View(vm);

			} else {
				model.NavigationLocation = new string[] { "Contact" };
				model.PageTitle = "Send us a question or comment:";
				this.FeedbackError("There was an error with the contact request you sent...");

				return View(model);
			}
		}
	}
}
