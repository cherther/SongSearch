using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {
	public static class AccountData {

		// **************************************
		// User
		// **************************************
		public static User User(string userName) {

			using (ISession rep = new EFSession()) {
				return rep.Single<User>(
						u => u.UserName.ToUpper() == userName.ToUpper()
						);
			}
		}

	}
}