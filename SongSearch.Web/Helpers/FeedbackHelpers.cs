using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace System.Web.Mvc {

	public static class FeedbackHelpers {

		public static void FeedbackInfo(this Controller controller, string message) {
			
			var messages = controller.TempData["feedback"] as Dictionary<string, string> ?? new Dictionary<string, string>();;
			try {
				messages.Add(message, "info");
			}
			catch { }
			finally {
				controller.TempData["feedback"] = messages;
			}
		}
		public static void FeedbackWarning(this Controller controller, string message) {
			
			var messages = controller.TempData["feedback"] as Dictionary<string, string> ?? new Dictionary<string, string>(); ;
			try {
				messages.Add(message, "warning");
			}
			catch { }
			finally {
				controller.TempData["feedback"] = messages;
			}
		}
		public static void FeedbackError(this Controller controller, string message) {
			
			var messages = controller.TempData["feedback"] as Dictionary<string, string> ?? new Dictionary<string, string>(); ;
			try {
				messages.Add(message, "error");
			}
			catch { }
			finally {
				controller.TempData["feedback"] = messages;
			}
		}

		public static string Feedback(this HtmlHelper helper) {

			var messages = helper.ViewContext.TempData["feedback"] as Dictionary<string, string>;

			var sb = new StringBuilder();
			if (messages != null){//!String.IsNullOrEmpty(message)) {
				sb.AppendLine("<script>");
				sb.AppendLine("$(document).ready(function() {");
				foreach (var msg in messages) {
					sb.AppendFormat("feedback('{0}', '{1}');", msg.Value, msg.Key);
				}
				sb.AppendLine("});");
				sb.AppendLine("</script>");
			}
			return sb.ToString();
		}

	}
}
