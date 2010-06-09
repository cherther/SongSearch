using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Services;

namespace SongSearch.Web.Controllers
{
	[RequireAuthorization(MinAccessLevel = Roles.Admin)]
	public partial class AdminController : Controller
	{
		public virtual ActionResult UpdateCache() {
			return View(new ViewModel() { NavigationLocation = "Admin" });

		}

		[HttpPost]
		public virtual ActionResult UpdateCache(FormCollection form) {
			CacheService.InitializeSession(true);
			CacheService.InitializeApp(true);
			return View(new ViewModel() { NavigationLocation = "Admin" });

		}

	}
}
