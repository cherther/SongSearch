using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Logging;

namespace SongSearch.Web.Controllers
{
	public partial class ErrorController : Controller
    {
        //
        // GET: /Error/

		
		ILogger _logger;

		public ErrorController(ILogger logger) {
			_logger = logger;
		}

		public virtual ActionResult Index(Exception exc, string message = null, string controllerName = "unknown controller", string actionName = "unknown action") {
			Exception lastError = Server.GetLastError();
			lastError = lastError ?? new Exception(message ?? "Unknown Error");
			this.FireError("Oops! We're sorry, something bad happened while processing your request...");
			return View(new ErrorViewModel() { Error = new HandleErrorInfo(lastError, controllerName, actionName) } );
		}
		/// <summary>
		/// This is fired when the site hits a 500
		/// </summary>
		/// <returns></returns>
		public virtual ActionResult Problem() {
			//no logging here - let the app do it 
			//we don't get reliable error traps here
			return View(Views.Index);
		}
		/// <summary>
		/// This is fired when the site gets a bad URL
		/// </summary>
		/// <returns></returns>
		public virtual ActionResult NotFound() {
			//you should probably log this - if you're getting 
			//bad links you'll want to know...
			_logger.Warn(string.Format("404 - {0}", Request.UrlReferrer));
			return View(Views.Index);
		}
    }
}
