using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Services;
using System.Text;
using SongSearch.Web.Data;

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

			var model = new ContactUsModel() {
				
				Email = User.Identity.IsAuthenticated ? User.Identity.Name : "",
				Name = User.Identity.IsAuthenticated ? this.Friendly() : "",
				NavigationLocation = new string[] { "Contact" }, 
				PageTitle = "Send us a question or comment:"
				
			};

			model.ContactInfo = this.SiteProfile().GetContactInfo(Account.User());
			return View(model);
		}

		[HttpPost]
		[ValidateOnlyIncomingValues]
		[ValidateAntiForgeryToken]
		public virtual ActionResult Contact(ContactUsModel model) {

			if (ModelState.IsValid) {
				var vm = new ContactUsModel() { 
					NavigationLocation = new string[] { "Contact" } 
				};
				vm.PageTitle = "Thanks for e-mailing us!";
				vm.PageMessage = "Your e-mail has been successfully sent to our team, and we will review your message and respond as quickly as possible.";
				vm.ContactInfo = this.SiteProfile().GetContactInfo(Account.User());

				string sender = String.Format("{0} <{1}>", this.SiteProfile().CompanyName, SystemConfig.AdminEmailAddress);//
				string subject = String.Format("[{0} Contact Us] {1}", this.SiteProfile().CompanyName, model.Subject);
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("<p>Name: {0}</p>", model.Name);
				sb.AppendFormat("<p>Email: {0}</p>", model.Email);
				sb.AppendFormat("<p>Company: {0}</p>", model.Company);
				sb.AppendFormat("<p>Regarding: {0}</p>", model.Subject);
				sb.AppendFormat("<p>Comments:</p><p><pre>{0}</pre></p>", model.Body);

				string msg = sb.ToString();
				sb = null;

				try {
					Mail.SendMail(
						sender,
						String.Concat(vm.ContactInfo.Email, ",", vm.ContactInfo.AdminEmail),//SiteProfileData.SiteProfile().ContactEmail,//Settings.ContactEmailAddress.Text(),
						subject,
						msg
						);
				}
				catch (Exception ex) {
					Log.Error(ex);
				}
				return View(vm);

			} else {
				model.NavigationLocation = new string[] { "Contact" };
				model.PageTitle = "Send us a question or comment:";
				model.ContactInfo = this.SiteProfile().GetContactInfo(Account.User());

				this.FeedbackError("There was an error with the contact request you sent...");

				return View(model);
			}
		}

		public virtual ActionResult Profile(string profileName) {
			// Check if Profile exists

			// Set up SiteProfile per the request
			System.Diagnostics.Debug.Write(profileName);

			return View("Index", new ViewModel() { NavigationLocation = new string[] { "Home" } });
		}
		public virtual ActionResult Help() {
			return View(new ViewModel() { PageTitle = "Help", NavigationLocation = new string[] { "Help" } });
		}
		public virtual ActionResult PrivacyPolicy() {
			return View(new ViewModel() { PageTitle = "Privacy Policy", NavigationLocation = new string[] { "Home" } });
		}
		public virtual ActionResult TermsOfUse() {
			return View(new ViewModel() { PageTitle = "Terms of Use", NavigationLocation = new string[] { "Home" } });
		}
		public virtual ActionResult Maintenance() {
			return View(new ViewModel() { PageTitle = "We'll be right back", NavigationLocation = new string[] { "Home" } });
		}
	}
}
