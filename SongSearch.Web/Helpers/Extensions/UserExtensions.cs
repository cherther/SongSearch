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

			return CacheService.User(princ.Identity.Name).IsAnyAdmin();
		}

		// **************************************
		// UserIsSuperAdmin
		// **************************************    
		public static bool UserIsSuperAdmin(this IPrincipal princ) {
			return princ.Identity.IsAuthenticated ?
				CacheService.User(princ.Identity.Name).IsSuperAdmin()
				: false;
			
		}

		// **************************************
		// UserIsInRole
		// **************************************    
		public static bool UserIsInRole(this IPrincipal princ, Roles role) {
			return princ.Identity.IsAuthenticated ?
				CacheService.User(princ.Identity.Name).IsInRole(role)
				: false;
		}

		// **************************************
		// UserIsAtLeastInRole
		// **************************************    
		public static bool UserIsAtLeastInRole(this IPrincipal princ, Roles role) {
			return princ.Identity.IsAuthenticated ?
				CacheService.User(princ.Identity.Name).IsAtLeastInRole(role)
				: false;
		}

		// **************************************
		// UserIsInAnyRole
		// **************************************    
		public static bool UserIsInAnyRole(this IPrincipal princ, Roles[] roles) {
			return princ.Identity.IsAuthenticated ?
				CacheService.User(princ.Identity.Name).IsInAnyRole(roles)
				: false;
		}

	}
}