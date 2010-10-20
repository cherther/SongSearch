﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Security.Application;

namespace System.Web.Mvc {
	public static class XSSHelpers {
		public static string h(this HtmlHelper helper, string input) {
			return Encoder.HtmlEncode(input);
		}
		public static string h(string input) {			
			return Encoder.HtmlEncode(input);
		}
		public static string Sanitize(this HtmlHelper helper, string input) {
			return Sanitizer.GetSafeHtml(input);
		}
		
		/// <summary>
		/// Encodes Javascript
		/// </summary>
		public static string hscript(this HtmlHelper helper, string input) {
			return Encoder.JavaScriptEncode(input);
		}

		public static string hscript(string input) {
			return Encoder.JavaScriptEncode(input);
		}
	}
}