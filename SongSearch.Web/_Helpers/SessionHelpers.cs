using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using SongSearch.Web;
using Microsoft.Security.Application;
using System.Security.Principal;
using SongSearch.Web.Data;

namespace System.Web.Mvc {
	public static class SessionHelpers {

		// **************************************
		// DownloadCartMessage
		// **************************************    
		public static string DownloadCartMessage(this HttpSessionStateBase session, IList<Cart> carts) {
			string msg = null;
			//var session = SessionService.Session();
			if (session["DownloadCartMessageShown"] == null) {
				var compressedCarts = carts.Where(c => c.CartStatus == (int)CartStatusCodes.Compressed);
				var count = compressedCarts.Count();
				if (count > 0) {
					msg = String.Format(
						@"You have <strong>{0}</strong> <a href=""/Cart/"">{1}</a> waiting to be downloaded.", 
						count, "carts".Pluralize(count));

				}
				session["DownloadCartMessageShown"] = "1";
			}
			return msg;
		}

		// **************************************
		// ProcessingCartMessage
		// **************************************    
		//CacheService.SessionUpdate(cart.CartId, "ProcessingCartId");
		public static string ProcessingCartMessage(this HttpSessionStateBase session, int cartId) {
			string msg = null;
			
			var msgKey = String.Concat("ShowProcessedCart_", cartId);
			var doneKey = String.Concat("Done", msgKey);
			if (session[msgKey] as string == "1" && session[doneKey] == null) {

				msg = @"Your requested <a href=""/Cart/"">zipped cart</a> is now ready for downloading.";

				session.Remove(msgKey);
				session.Add(doneKey, "1");
			}
			return msg;
		}

	}
}