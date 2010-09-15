using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SongSearch.Web.Logging {
	public class LogUtility {

		public static string BuildExceptionMessage(Exception x) {
			
			Exception logException = x.InnerException ?? x;

			string msg = String.Empty;

			if (logException != null) {

			
				//msg += Environment.NewLine + "Error in Path :" + System.Web.HttpContext.Current != null ? System.Web.HttpContext.Current.Request.Path : "(no httpContext)";

				//// Get the QueryString along with the Virtual Path
				//msg += Environment.NewLine + "Raw Url :" + System.Web.HttpContext.Current != null ? System.Web.HttpContext.Current.Request.RawUrl : "(no httpContext)";

			
				// Get the error message
				msg += Environment.NewLine + "Message :" + logException.Message;

				// Source of the message
				msg += Environment.NewLine + "Source :" + logException.Source;

				// Stack Trace of the error

				msg += Environment.NewLine + "Stack Trace :" + logException.StackTrace;

				// Method where the error occurred
				msg += Environment.NewLine + "TargetSite :" + logException.TargetSite;
			}

			return msg;
		}
	}
}
