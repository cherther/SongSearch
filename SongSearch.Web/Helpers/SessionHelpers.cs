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
					msg = String.Format("You have <strong>{0}</strong> {1} waiting to be downloaded.", count, count > 1 ? "carts" : "cart");

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
			var msgKey = String.Concat("NotifUserAboutProcessedCart_", cartId);
			if (session[msgKey] == null) {

				msg = "Your requested zipped cart is now ready for downloading.";

				session[msgKey] = "1";
			}
			return msg;
		}

	}
}