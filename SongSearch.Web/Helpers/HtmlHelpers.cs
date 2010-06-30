using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using SongSearch.Web;
using Microsoft.Security.Application;
using System.Security.Principal;

namespace System.Web.Mvc {
	public static class HtmlHelpers {
		const string pubDir = "/public";
		const string cssDir = "css";
		const string imageDir = "images";
		const string scriptDir = "javascript";

		public static string DatePickerEnable(this HtmlHelper helper) {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("<script type='text/javascript'>$(document).ready(function() {$('.date-selector').datepicker();});</script>\n");
			return sb.ToString();
		}

		public static string Friendly(this HtmlHelper helper) {
			if (helper.ViewContext.HttpContext.Request.Cookies["friendly"] != null) {
				return helper.h(helper.ViewContext.HttpContext.Request.Cookies["friendly"].Value);
			} else {
				return "";
			}
		}

		public static string Friendly(this Controller controller ) {
			if (controller.ControllerContext.HttpContext.Request.Cookies["friendly"] != null) {
				return controller.ControllerContext.HttpContext.Request.Cookies["friendly"].Value;
			} else {
				return "";
			}
		}
		public static string UserName(this Controller controller) {
			return controller.ControllerContext.HttpContext.User.Identity.Name;
		}

		public static string Script(this HtmlHelper helper, string fileName) {
			if (!fileName.EndsWith(".js"))
				fileName += ".js";
			return string.Format("<script src='{0}/{1}/{2}' ></script>\n", pubDir, scriptDir, helper.AttributeEncode(fileName));
			
		}
		public static string CSS(this HtmlHelper helper, string fileName) {
			return CSS(helper, fileName, "screen");
		}
		public static string CSS(this HtmlHelper helper, string fileName, string media) {
			if (!fileName.EndsWith(".css"))
				fileName += ".css";
			return string.Format("<link rel='stylesheet' type='text/css' href='{0}/{1}/{2}'  media='" + media + "'/>\n", pubDir, cssDir, helper.AttributeEncode(fileName));
			
		}
		public static string Image(this HtmlHelper helper, string fileName) {
			return Image(helper, fileName, "");
		}
		public static string Image(this HtmlHelper helper, string fileName, string attributes) {
			fileName = string.Format("{0}/{1}/{2}", pubDir, imageDir, fileName);
			return string.Format("<img src='{0}' {1}' />", helper.AttributeEncode(fileName), helper.AttributeEncode(attributes));
		}


		public static string HighlightSearchTerm(this HtmlHelper helper, string fieldValue, SearchField searchField) {
			var value = fieldValue != null ? AntiXss.HtmlEncode(fieldValue.ToUpper()) : null;
			
			if (searchField != null && value != null) {
				var searchVal = searchField.V.FirstOrDefault() ?? "";
				value = value.Replace(searchVal.ToUpper(), String.Format(@"<span class='cw-highlight'>{0}</span>", searchVal.ToUpper()));
			}

			return value;
		}
	}
}

namespace System.Web {
	public static class HtmlHelpers {

		public static string Friendly(this HttpContext ctx) {
			if (ctx.Request.Cookies["friendly"] != null) {
				return ctx.Request.Cookies["friendly"].Value;
			} else {
				return "";
			}
		}
	}
	
}