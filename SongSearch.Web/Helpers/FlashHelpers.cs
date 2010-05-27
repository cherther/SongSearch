using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace System.Web.Mvc {
	public static class FlashHelpers {

		public static void FlashInfo(this Controller controller, string message) {
			controller.TempData["info"] = message;
		}
		public static void FlashWarning(this Controller controller, string message) {
			controller.TempData["warning"] = message;
		}
		public static void FlashError(this Controller controller, string message) {
			controller.TempData["error"] = message;
		}

		public static string Flash(this HtmlHelper helper) {

			var message = "";
			//var fade = true;
			var className = "";
			if (helper.ViewContext.TempData["info"] != null) {
				message = helper.ViewContext.TempData["info"].ToString();
				className = "info";
			} else if (helper.ViewContext.TempData["warning"] != null) {
				message = helper.ViewContext.TempData["warning"].ToString();
				className = "warning";
				//fade = false;
			} else if (helper.ViewContext.TempData["error"] != null) {
				message = helper.ViewContext.TempData["error"].ToString();
				className = "error";
				//fade = false;
			}
			var sb = new StringBuilder();
			if (!String.IsNullOrEmpty(message)) {
				sb.AppendLine("<script>");
				sb.AppendLine("$(document).ready(function() {");
				sb.AppendFormat("flash('{0}', '{1}');", className, message);
				//sb.AppendLine("var flash = $('#flash');");
				//sb.AppendFormat("flash.html('{0}');", message);
				//sb.AppendLine("flash.removeClass('');");
				//sb.AppendFormat("flash.addClass('{0}');", className);
				//sb.AppendLine("flash.slideDown('slow')");
				//sb.Append(fade ? ".delay('1200').fadeOut('slow');" : ";");
				//sb.AppendLine("flash.click(function(){$('#flash').toggle('highlight')});");
				sb.AppendLine("});");
				sb.AppendLine("</script>");
			}
			return sb.ToString();
		}

	}
}
