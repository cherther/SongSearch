using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Web;
using SongSearch.Web.Data;
using Ninject;
using SongSearch.Web.Services;

namespace SongSearch.Web {
	public static class AccountData {

		// **************************************
		// User
		// **************************************
		public static User User(string userName) {
			using (var session = App.DataSessionReadOnly) {
				return session.Single<User>(u => u.UserName.ToUpper() == userName.ToUpper());
			}
		}

		public static User UserComplete(string userName) {

			using (var session = App.DataSessionReadOnly) {

				return session.GetObjectQuery<User>()
					.Include("Carts")
					.Include("UserCatalogRoles")
					.Where(u => u.UserName.ToUpper() == userName.ToUpper()).SingleOrDefault();

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

			return user != null && user.IsInAnyRole(levels);
		}

		// **************************************
		// IsSuperAdmin
		// **************************************    
		public static bool IsSuperAdmin(this User user) {
			return (user != null && user.IsInRole(Roles.SuperAdmin));
		}

		// **************************************
		// IsInRole
		// **************************************    
		public static bool IsInRole(this User user, Roles role) {

			return user != null && user.RoleId.Equals((int)role);
		}

		// **************************************
		// IsAtLeastInCatalogRole
		// **************************************    
		public static bool IsAtLeastInCatalogRole(this User user, Roles role, Catalog catalog) {
			return user != null &&  (user.IsSuperAdmin() || 
				(user.UserCatalogRoles != null ?
				user.UserCatalogRoles.Any(x => x.CatalogId == catalog.CatalogId && x.RoleId <= (int)role) :
				false));
		}

		public static bool IsAtLeastInCatalogRole(this User user, Roles role, int catalogId) {
			return user != null && (user.IsSuperAdmin() ||
				(user.UserCatalogRoles != null ?
				user.UserCatalogRoles.Any(x => x.CatalogId == catalogId && x.RoleId <= (int)role) :
				false));
		}

		// **************************************
		// IsAtLeastInRole
		// **************************************    
		public static bool IsAtLeastInRole(this User user, Roles role) {
			return user != null && user.RoleId <= (int)role;
		}

		// **************************************
		// IsInAnyRole
		// **************************************    
		public static bool IsInAnyRole(this User user, Roles[] roles) {
			return user != null && roles.Contains((Roles)user.RoleId);
		}

		// **************************************
		// FullName
		// **************************************    
		public static string FullName(this User user) {

			string name = user != null ? String.Format("{0} {1}", user.FirstName, user.LastName) : "";

			return String.IsNullOrEmpty(name.Trim()) ? user.UserName : name;

		}

		// **************************************
		// FullName
		// **************************************    
		public static string FullDisplayName(this User user) {

			string name = user != null ? String.Format("{0} <{1}>", user.FullName(), user.UserName) : "";

			return String.IsNullOrEmpty(name.Trim()) ? user.UserName : name;

		}
		// **************************************
		// ParentSignature
		// **************************************    
		public static string ParentSignature(this User user) {

			var parent = user != null ? user.ParentUser : null ; //rep.Single<User>(u => u.UserId == user.UserId).ParentUser;

			return parent == null ? "" : (parent.Signature ?? parent.UserName);
		}

		// **************************************
		// FileSignature
		// **************************************    
		public static string FileSignature(this User user) {
			return user != null ? (user.IsAnyAdmin() ? user.Signature : user.ParentSignature()) : "";
		}

		// **************************************
		// LoginMessage
		// **************************************    
		public static string LoginMessage(this User user) {

			string msg = null;
			var session = SessionService.Session();
			if (session.Session("LoginMessageShown") == null) {
				msg = string.Concat("Welcome ", user.FullName());
				session.SessionUpdate("1", "LoginMessageShown");

				if (session.Get("ActiveCartMessageShown") == null) {
					var cart = SessionService.Session().MyActiveCart(user.UserName);
					var activeItems = cart != null && cart.Contents != null ? cart.Contents.Count : 0;
					msg = activeItems > 0 ? String.Concat(msg, String.Format(". You have <strong>{0}</strong> {1} waiting in your song cart.", activeItems, activeItems > 1 ? "items" : "item")) : msg;

					session.SessionUpdate("1", "ActiveCartMessageShown");
				}
			}
			return msg;
		}

		// **************************************
		// DownloadCartMessage
		// **************************************    
		public static string DownloadCartMessage(this User user, IList<Cart> carts) {
			string msg = null;
			var session = SessionService.Session();
			if (session.Get("DownloadCartMessageShown") == null) {
				var compressedCarts = carts.Where(c => c.CartStatus == (int)CartStatusCodes.Compressed);
				var count = compressedCarts.Count();
				if (count > 0) {
					msg = String.Format("You have <strong>{0}</strong> {1} waiting to be downloaded.", count, count > 1 ? "carts" : "cart");

				}
				session.SessionUpdate("1", "DownloadCartMessageShown");
			}
			return msg;
		}

		// **************************************
		// ProcessingCartMessage
		// **************************************    
		//CacheService.SessionUpdate(cart.CartId, "ProcessingCartId");
		public static string ProcessingCartMessage(this User user, int cartId) {
			string msg = null;
			var session = SessionService.Session();
			var msgKey = String.Concat("NotifUserAboutProcessedCart_", cartId);
			if (session.Get(msgKey) == null) {

				msg = "Your requested zipped cart is now ready for downloading.";

				session.SessionUpdate("1", msgKey);
			}
			return msg;
		}
		
		// **************************************
		// MyUserHierarchy
		// **************************************    
		public static IList<User> MyUserHierarchy(this User user, bool withCatalogRoles = false) {

			using (var session = App.DataSessionReadOnly) {

				var set = session.GetObjectQuery<User>();
				var users = (withCatalogRoles ? set.Include("UserCatalogRoles") : set).Where(u => u.RoleId >= (int)user.RoleId).ToList();

				var topLevelUsers = (
						user.IsSuperAdmin() ?
						users.Where(u => !u.ParentUserId.HasValue) :
						users.Where(u => u.ParentUserId == user.UserId)
						).ToList();

				return topLevelUsers.AttachChildren(users);

			}
		}

		// **************************************
		// MyAdminCatalogs
		// **************************************    
		public static IList<Catalog> MyAdminCatalogs(this User user) {

			return CacheService.Catalogs().LimitToAdministeredBy(user);
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


	}
}