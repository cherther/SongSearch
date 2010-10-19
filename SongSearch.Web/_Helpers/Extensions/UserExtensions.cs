using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using SongSearch.Web.Services;
using SongSearch.Web.Data;

namespace SongSearch.Web {
	public static class UserExtensions {

		public static User User(this IPrincipal princ) {
			if (!princ.Identity.IsAuthenticated) {
				return null;
			}
			return SessionService.Session().User(princ.Identity.Name);
			
		}

		// **************************************
		// UserIsAnyAdmin
		// **************************************    
		public static bool UserIsAnyAdmin(this IPrincipal princ) {
			if (!princ.Identity.IsAuthenticated) {
				return false;
			}
			var user = SessionService.Session().User(princ.Identity.Name);
			return user != null && user.IsAnyAdmin();
		}

		// **************************************
		// UserIsSuperAdmin
		// **************************************    
		public static bool UserIsSuperAdmin(this IPrincipal princ) {
			if (!princ.Identity.IsAuthenticated) {
				return false;
			}
			var user = SessionService.Session().User(princ.Identity.Name);
			return user != null && user.IsSuperAdmin();
			
		}
		//public static bool UserIsSuperAdmin(this HttpContext ctx) {
		//    if (!ctx.User.Identity.IsAuthenticated) {
		//        return false;
		//    }
		//    var user = ctx.ActiveUserComplete();
		//    return user != null && user.IsSuperAdmin();

		//}
		// **************************************
		// UserIsInRole
		// **************************************    
		public static bool UserIsInRole(this IPrincipal princ, Roles role) {
			if (!princ.Identity.IsAuthenticated) {
				return false;
			}
			var user = SessionService.Session().User(princ.Identity.Name);
			return user != null && user.IsInRole(role);
		}

		// **************************************
		// UserIsAtLeastInRole
		// **************************************    
		public static bool UserIsAtLeastInRole(this IPrincipal princ, Roles role) {
			if (!princ.Identity.IsAuthenticated) {
				return false;
			}
			var user = SessionService.Session().User(princ.Identity.Name);
			return user != null && user.IsAtLeastInRole(role);
			
		}

		// **************************************
		// UserIsInAnyRole
		// **************************************    
		public static bool UserIsInAnyRole(this IPrincipal princ, Roles[] roles) {
			if (!princ.Identity.IsAuthenticated) {
				return false;
			}
			var user = SessionService.Session().User(princ.Identity.Name);
			return user != null && user.IsInAnyRole(roles);

		}

	}
}