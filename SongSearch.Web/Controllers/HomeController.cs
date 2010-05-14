using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SongSearch.Web.Controllers {
	[HandleError]
	public class HomeController : Controller {
		// **************************************
		// URL: /
		// **************************************
		public ActionResult Index() {
			var vm = new ViewModel() { NavigationLocation = "Home" };
			return View(vm);
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
