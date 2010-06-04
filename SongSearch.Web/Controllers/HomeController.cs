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

			if (User.Identity.IsAuthenticated){
				
				var msg = CacheService.User(User.Identity.Name).LoginMessage();
				
				this.FireInfo(msg);
			}
			return View(new ViewModel() { NavigationLocation = "Home" });
			//return RedirectToAction("Index", "Search");
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
