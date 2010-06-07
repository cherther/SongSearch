using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Services;

namespace SongSearch.Web.Controllers {
	[HandleError]
	public partial class HomeController : Controller {
		// **************************************
		// URL: /
		// **************************************
		public virtual ActionResult Index() {

			try {
				if (User.Identity.IsAuthenticated) {

					var msg = CacheService.User(User.Identity.Name).LoginMessage();

					this.FeedbackInfo(msg);
				}
			}
			catch { }
			return View(new ViewModel() { NavigationLocation = "Home" });
			
		}

		// **************************************
		// URL: /Home/About/
		// **************************************
		public virtual ActionResult About() {
			//var vm = new ViewModel() { NavigationLocation = "About" };
			return View();
		}
	}
}
