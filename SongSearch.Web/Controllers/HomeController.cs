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
				var h = new HtmlHelper(new ViewContext(), new ViewPage());

				var msg = CacheService.User(User.Identity.Name).LoginMessage();
				
				this.FireInfo(msg);
			}
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
