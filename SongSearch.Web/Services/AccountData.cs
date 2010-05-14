using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
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

		// ----------------------------------------------------------------------------
		// Extensions
		// ----------------------------------------------------------------------------
		// **************************************
		// IsAnyAdmin
		// **************************************    
		public static bool IsAnyAdmin(this User user) {
			var levels = new[] { Roles.SuperAdmin, Roles.Admin };

			return user.IsInAnyRole(levels);
			//            return (princ.IsInRole(SUPERADMIN) || princ.IsInRole(CLIENTADMIN));
		}

		// **************************************
		// IsSuperAdmin
		// **************************************    
		public static bool IsSuperAdmin(this User user) {
			return (user.IsInRole(Roles.SuperAdmin));
		}

		// **************************************
		// IsInRole
		// **************************************    
		public static bool IsInRole(this User user, Roles role) {

			return user.RoleId.Equals((int)role);
		}

		// **************************************
		// IsAtLeastInDbRole
		// **************************************    
		public static bool IsAtLeastInRole(this User user, Roles role) {
			return user.RoleId <= (int)role;
		}

		// **************************************
		// IsInAnyDbRole
		// **************************************    
		public static bool IsInAnyRole(this User user, Roles[] roles) {
			return roles.Contains((Roles)user.RoleId);
		}

		// **************************************
		// UserAccessLevels
		// **************************************    
		public static string FullName(this User user) {


			string name = user != null ? String.Format("{0} {1}", user.FirstName, user.LastName) : "";

			name = string.IsNullOrEmpty(name.Trim()) ? user.UserName : name;

			user = null;

			return name;
		}

		// **************************************
		// ParentSignature
		// **************************************    
		public static string ParentSignature(this User user) {

			var parent = user.ParentUser; //rep.Single<User>(u => u.UserId == user.UserId).ParentUser;

			string sig = parent == null ? "" : (parent.Signature ?? parent.UserName);
			parent = null;
			return sig;
		}

		// **************************************
		// FileSignature
		// **************************************    
		public static string FileSignature(this User user) {

			if (user.IsAnyAdmin()) {
				return user.Signature;
			} else {
				return user.ParentSignature();
			}
		}

		// **************************************
		// GetUserHierarchy
		// **************************************    
		public static IList<User> GetUserHierarchy(this User user, bool withCatalogRoles = false) {

			using (var ctx = new SongSearchContext(Connections.ConnectionString(ConnectionStrings.SongSearchContext))) {
				//
				if (withCatalogRoles)
					ctx.ContextOptions.LazyLoadingEnabled = false;

				//		DataLoadOptions dlo = new DataLoadOptions();

				//using (ISession session = new EFSession(ctx)) {
					// Only users with same or lesser access rights
					//var rep = fullyLoaded ? _usrMgmtSvc.GetUserRepositoryFullyLoaded() : SqlSession;
				var set = ctx.CreateObjectSet<User>();
				var users = (withCatalogRoles ? set.Include("UserCatalogRoles") : set).Where(u => u.RoleId >= (int)user.RoleId).ToList();
					//				users = !roleId.HasValue ? users : users.Where(u => u.RoleId == roleId);

					var topLevelUsers = (
						user.IsSuperAdmin() ?
						users.Where(u => !u.ParentUserId.HasValue) :
						users.Where(u => u.ParentUserId == user.UserId)
						).ToList();

					var userHierarchy = topLevelUsers.AttachChildren(users);

					return userHierarchy;//SqlSession.GetUsers().Where(x => x.ParentUserId == User.UserId).Flatten();
				//}
			}
		}

		// **************************************
		// AttachChildren
		// **************************************
		private static IList<User> AttachChildren(this IList<User> parents, IList<User> users) {
			foreach (var p in parents) {
				var parent = p;
				var children = users.Where(u => u.ParentUserId == parent.UserId).ToList();
				parent.ChildUsers = AttachChildren(children, users);
			}
			return parents;
		}


		// **************************************
		// ActiveCartContentsCount
		// **************************************    
		//public static int ActiveCartContentsCount(this DisplayUser user) {

		//    using (ICartService cart = new CartService(user.UserName)) {
		//        return cart == null ? 0 : cart.MyActiveCartContentsCount();
		//    }
		//}

	}
}