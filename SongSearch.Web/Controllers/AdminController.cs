﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Services;
using System.Web.Routing;
using SongSearch.Web.Data;

namespace SongSearch.Web.Controllers
{
	[RequireAuthorization(MinAccessLevel = Roles.SuperAdmin)]
	public partial class AdminController : Controller
	{
		protected override void Initialize(RequestContext requestContext) {

			base.Initialize(requestContext);
		}

		public virtual ActionResult UpdateCache() {
			return View(new ViewModel() { NavigationLocation = new string[] { "Admin" } });

		}

		[HttpPost]
		public virtual ActionResult UpdateCache(FormCollection form) {
			SessionService.Session().InitializeSession(true);
			CacheService.InitializeApp(true);
			return View(new ViewModel() { NavigationLocation = new string[] { "Admin" } });

		}

		public virtual ActionResult MySession() {
			return View(new ViewModel() { NavigationLocation = new string[] { "Admin" } });

		}

		public virtual ActionResult ReportUserActions(string start, string end) {

			var startDate = start != null ? DateTime.Parse(start) : DateTime.Now.AddDays(-7);
			var endDate = end != null ? DateTime.Parse(end): DateTime.Now;

			var events = UserEventLogService.ReportUserActions(startDate, endDate) as PagedList<UserActionEvent>;
//			events = events.OrderByDescending(e => e.UserActionEventDate).ThenByDescending(e => e.UserActionEventId) as PagedList<UserActionEvent>;

			return View(new ReportEventViewModel() { UserActionEvents = events });

		}

	}
}
