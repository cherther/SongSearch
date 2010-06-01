using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace System.Web.Mvc {
	public static class FireHelpers {

		public static void FireInfo(this Controller controller, string message) {
			controller.TempData["info"] = message;
		}
		public static void FireWarning(this Controller controller, string message) {
			controller.TempData["warning"] = message;
		}
		public static void FireError(this Controller controller, string message) {
			controller.TempData["error"] = message;
		}

		public static string Fire(this HtmlHelper helper) {

			var message = "";
			var className = "";
			if (helper.ViewContext.TempData["info"] != null) {
				message = helper.ViewContext.TempData["info"].ToString();
				className = "info";
			} else if (helper.ViewContext.TempData["warning"] != null) {
				message = helper.ViewContext.TempData["warning"].ToString();
				className = "warning";
			} else if (helper.ViewContext.TempData["error"] != null) {
				message = helper.ViewContext.TempData["error"].ToString();
				className = "error";
			}
			var sb = new StringBuilder();
			if (!String.IsNullOrEmpty(message)) {
				sb.AppendLine("<script>");
				sb.AppendLine("$(document).ready(function() {");
				sb.AppendFormat("fire('{0}', '{1}');", className, message);
				sb.AppendLine("});");
				sb.AppendLine("</script>");
			}
			return sb.ToString();
		}

	}
}
