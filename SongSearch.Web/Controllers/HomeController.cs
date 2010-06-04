using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Services;

namespace SongSearch.Web.Controllers {
	[HandleError]
	public class HomeController : Controller {
		// **************************************
		// URL: /
		// **************************************
		public ActionResult Index() {

			try {
				if (User.Identity.IsAuthenticated) {

					var msg = CacheService.User(User.Identity.Name).LoginMessage();

					this.FireInfo(msg);
				}
			}
			catch { }
			return View(new ViewModel() { NavigationLocation = "Home" });
			
		}

		// **************************************
		// URL: /Home/About/
		// **************************************
		public ActionResult About() {
			//var vm = new ViewModel() { NavigationLocation = "About" };
			return View();
		}
	}
}
