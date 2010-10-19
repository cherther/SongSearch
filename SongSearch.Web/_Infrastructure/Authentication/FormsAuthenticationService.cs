using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SongSearch.Web.Services {

	public static class FormsAuthenticationService {
		public static void SignIn(string userName, bool createPersistentCookie) {
			if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

			FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);

		}

		public static void SignOut() {
			FormsAuthentication.SignOut();
		}
	}
	
}