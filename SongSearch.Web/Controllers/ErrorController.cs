using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Logging;

namespace SongSearch.Web.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

		
		ILogger _logger;

		public ErrorController(ILogger logger) {
			_logger = logger;
		}

		public ActionResult Index(Exception exc, string message = null, string controllerName = "unknown controller", string actionName = "unknown action") {
			Exception lastError = Server.GetLastError();
			lastError = lastError ?? new Exception(message ?? "Unknown Error");
			var vm = new ErrorViewModel() { Error = new HandleErrorInfo(lastError, controllerName, actionName) };
			return View(vm);
		}
		/// <summary>
		/// This is fired when the site hits a 500
		/// </summary>
		/// <returns></returns>
		public ActionResult Problem() {
			//no logging here - let the app do it 
			//we don't get reliable error traps here
			return View();
		}
		/// <summary>
		/// This is fired when the site gets a bad URL
		/// </summary>
		/// <returns></returns>
		public ActionResult NotFound() {
			//you should probably log this - if you're getting 
			//bad links you'll want to know...
			_logger.Warn(string.Format("404 - {0}", Request.UrlReferrer));
			return View();
		}
    }
}
