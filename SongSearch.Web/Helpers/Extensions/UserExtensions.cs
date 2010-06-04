using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using SongSearch.Web.Services;

namespace SongSearch.Web {
	public static class UserExtensions {


		// **************************************
		// UserIsAnyAdmin
		// **************************************    
		public static bool UserIsAnyAdmin(this IPrincipal princ) {
			if (!princ.Identity.IsAuthenticated) {
				return false;
			}
			var user = CacheService.User(princ.Identity.Name);
			return user != null && user.IsAnyAdmin();
		}

		// **************************************
		// UserIsSuperAdmin
		// **************************************    
		public static bool UserIsSuperAdmin(this IPrincipal princ) {
			if (!princ.Identity.IsAuthenticated) {
				return false;
			}
			var user = CacheService.User(princ.Identity.Name);
			return user != null && user.IsSuperAdmin();
			
		}

		// **************************************
		// UserIsInRole
		// **************************************    
		public static bool UserIsInRole(this IPrincipal princ, Roles role) {
			if (!princ.Identity.IsAuthenticated) {
				return false;
			}
			var user = CacheService.User(princ.Identity.Name);
			return user != null && user.IsInRole(role);
		}

		// **************************************
		// UserIsAtLeastInRole
		// **************************************    
		public static bool UserIsAtLeastInRole(this IPrincipal princ, Roles role) {
			if (!princ.Identity.IsAuthenticated) {
				return false;
			}
			var user = CacheService.User(princ.Identity.Name);
			return user != null && user.IsAtLeastInRole(role);
			
		}

		// **************************************
		// UserIsInAnyRole
		// **************************************    
		public static bool UserIsInAnyRole(this IPrincipal princ, Roles[] roles) {
			if (!princ.Identity.IsAuthenticated) {
				return false;
			}
			var user = CacheService.User(princ.Identity.Name);
			return user != null && user.IsInAnyRole(roles);

		}

	}
}