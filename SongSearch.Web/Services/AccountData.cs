using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Web;
using SongSearch.Web.Data;
using Ninject;
using SongSearch.Web.Services;
using System.IO;
using System.Data.Objects;

namespace SongSearch.Web {
	public static class Account {

		// **************************************
		// User
		// **************************************
		public static User User(bool cached = true) {
			return User(HttpContext.Current.User.Identity.Name, cached);
		}

		public static User User(int userId) {
			
			using (var session = App.DataSessionReadOnly) {
				var user = session.GetUserQuery()
					.Where(u => u.UserId == userId).SingleOrDefault();

				return user;	
			}
		}
		public static User User(string userName, bool cached = true) {
			if (cached) {
				return SessionService.Session().User(userName);
			} else {
				using (var session = App.DataSessionReadOnly) {
					var user = session.GetUserQuery()
						.Where(u => u.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();

					return user;
				}
			}
		}

		
		private static int GetNumberOfSongs() {

			using (var session = App.DataSessionReadOnly) {
				var adminCats = User().MyAdminCatalogs().Select(c => c.CatalogId);
				var songCount = session.All<Content>().Where(x => adminCats.Contains(x.CatalogId)).Count();

				return songCount;
			}
		}
		private static int GetNumberOfUsers() {
			return User().MyUserHierarchy().CountWithChildren();
		}
		private static int GetNumberOfCatalogAdmins() {
			//var myUsers = User().MyUserHierarchy();
			var myAdmins = User().MyAdminUserHierarchy();// Where(x => x.UserCatalogRoles.Any(r => r.RoleId >= (int)Roles.Admin)).ToList();
			var adminCount = myAdmins.CountWithChildren();
			if (!User().IsSuperAdmin() && User().IsAtLeastInCatalogRole(Roles.Admin)) {
				adminCount++;
			}
			return adminCount;
		}

		private static int GetDefaultNumberOfSongs() {
			return CacheService.PricingPlans().Where(p => p.IsEnabled == true && p.NumberOfSongs.HasValue).Min(p => p.NumberOfSongs.Value);
		}
		// **************************************
		// Cart
		// **************************************
		public static Cart Cart() {
			return Cart(true);
		}
		public static Cart Cart(bool cached = true) {
			return Cart(HttpContext.Current.User.Identity.Name, cached);
		}
		public static Cart Cart(string userName, bool cached = true) {
			if (cached) {
				return SessionService.Session().MyActiveCart(userName);
			} else {
				return User(userName, cached).Carts.SingleOrDefault(c => c.CartStatus == (int)CartStatusCodes.Active);
			}
		}


		// **************************************
		// CartContents
		// **************************************
		public static IList<int> CartContents() {
			var contents = CartContents(true);
			return contents;
		}
		public static IList<int> CartContents(bool cached = true) {
			return CartContents(HttpContext.Current.User.Identity.Name, cached);
		}
		public static IList<int> CartContents(string userName, bool cached = true) {
			var cart = Cart(userName, cached);
			return cart != null && cart.Contents != null ?
				cart.Contents.Select(c => c.ContentId).ToList()
				: new List<int> { };
		}

		// **************************************
		// UserHasAccessToContent
		// **************************************
		public static bool UserHasAccessToContent(int userId, int contentId) {

			using (var session = App.DataSessionReadOnly) {

				var content = session.All<Content>().Where(c => c.ContentId == contentId
						&& c.Catalog.UserCatalogRoles.Any(u => u.UserId == userId));
				
				return content.Count() > 0;

			}

		}
		// **************************************
		// GetUserQuery
		// **************************************
		private static ObjectQuery<User> GetUserQuery(this IDataSessionReadOnly session) {

			return session.GetObjectQuery<User>()
					.Include("ParentUser")
					.Include("Carts")
					.Include("Carts.Contents")
					.Include("UserCatalogRoles")
					.Include("Contacts")
					.Include("ParentUser.Contacts")
					.Include("PricingPlan")
					.Include("ParentUser.PricingPlan");
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
		public static bool IsAtLeastInCatalogRole(this User user, Roles role) {
			return user != null && (user.IsSuperAdmin() || user.RoleId <=(int)role ||
				(user.UserCatalogRoles != null ?
				user.UserCatalogRoles.Any(x => x.RoleId <= (int)role) :
				false));
		}
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

		public static bool HasAccessToContent(this User user, Content content) {
			if (user.IsSuperAdmin()) {
				return true;
			} else {
				return user.UserCatalogRoles.Any(c => c.CatalogId == content.CatalogId);
			}
		}
		public static bool HasAccessToContentWithRole(this User user, Content content, Roles role) {
			if (user.IsSuperAdmin()) {
				return true;
			} else {
				return user.UserCatalogRoles.Any(c => c.CatalogId == content.CatalogId && c.RoleId <= (int)role);
			}
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

			return parent == null ? "" : (parent.Signature.AsEmptyIfNull());
		}

		// **************************************
		// FileSignature
		// **************************************    
		public static string FileSignature(this User user, Content content) {
			return user != null ? (
				user.HasAccessToContentWithRole(content, Roles.Plugger) ? 
					user.Signature 
					: user.ParentSignature()) 
				: "";
		}

		// **************************************
		// AppendToTitleData
		// **************************************    
		public static string AppendToTitleData(this User user, Content content) {
			
			string append = null;
			
			if (user != null){
				if (user.HasAccessToContentWithRole(content, Roles.Plugger)) {
					append = user.AppendSignatureToTitle ? user.Signature : null;
				} else if (user.ParentUser != null && user.ParentUser.AppendSignatureToTitle) {
					append = user.ParentSignature();
				}
			}

			return append;
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
					msg = activeItems > 0 ? 
						String.Concat(msg,
						String.Format(@". You have <strong>{0}</strong> {1} waiting in your <a href=""/Cart/"">song cart</a>.", activeItems, "item".Pluralize(activeItems))) : msg;

					session.SessionUpdate("1", "ActiveCartMessageShown");
				}
			}
			return msg;
		}

		

		// **************************************
		// UploadFolder
		// **************************************    
		public static string UploadFolder(this User user, string mediaVersion = "", bool create = true) {

			string uploadPath = SystemConfig.UploadPath; 
			
			string userFolder = Path.Combine(uploadPath, user.UserId.ToString(), mediaVersion);
			if (create) { FileSystem.CreateFolder(userFolder); }  

			return userFolder;
		}

		// **************************************
		// UploadFile
		// **************************************    
		public static string UploadFile(this User user, string fileName = "", string mediaVersion = "") {

			return Path.Combine(user.UploadFolder(mediaVersion), fileName);
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
		// MyUserHierarchy
		// **************************************    
		public static IList<User> MyAdminUserHierarchy(this User user, bool withCatalogRoles = false) {

			using (var session = App.DataSessionReadOnly) {

				var set = session.GetObjectQuery<User>();
				var users = (withCatalogRoles ? set.Include("UserCatalogRoles") : set).Where(u => u.RoleId <= (int)Roles.Admin).ToList();

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

		public static UserQuotas MyQuotas(this User user) {

			var quotas = new UserQuotas();
			var plan = user.PricingPlan;
			quotas.NumberOfSongs.Default = GetDefaultNumberOfSongs();
			quotas.NumberOfSongs.Allowed = plan.NumberOfSongs; 
			quotas.NumberOfSongs.Used = GetNumberOfSongs();

			quotas.NumberOfInvitedUsers.Allowed = plan.NumberOfInvitedUsers;
			quotas.NumberOfInvitedUsers.Used = GetNumberOfUsers();

			quotas.NumberOfCatalogAdmins.Allowed = plan.NumberOfCatalogAdmins;
			quotas.NumberOfCatalogAdmins.Used = GetNumberOfCatalogAdmins();

			return quotas;
		}

		public static int[] MyAssignableRoles(this User user) {

			if (!user.IsSuperAdmin() && App.IsLicensedVersion && user.MyQuotas().NumberOfCatalogAdmins.Remaining.GetValueOrDefault() == 0) {
				return ModelEnums.GetPublicRoles().Where(r => r > (int)Roles.Admin).ToArray();
			} else {
				return ModelEnums.GetPublicRoles().Where(r => r >= user.RoleId).ToArray();
			}
		}
		public static bool CanAssignAdmin(this User user) {
			return user.MyAssignableRoles().Contains((int)Roles.Admin);
		}
		// **************************************
		// GetContactInfo
		// **************************************
		public static Contact GetContactInfo(this SiteProfile profile, User user) {

			return (user != null ? user.GetContactInfo() : null) ?? profile.Contacts.FirstOrDefault(c => c.IsDefault);

		}

		public static Contact GetContactInfo(this User user, bool checkParent = true) {

			if (user == null) { return null; }

			Contact contact = null;
			// Pluggers and above can set up their own contact info, everyone else inherits down
			if (user.IsPlanUser && 
				user.PricingPlan != null && 
				user.PricingPlan.CustomContactUs && 
				user.IsAtLeastInCatalogRole(Roles.Admin)) {
				
				contact = user.Contacts.FirstOrDefault(c => c.IsDefault);
			
			}
			return contact ??
				(checkParent && user.ParentUser != null ?
					user.ParentUser.GetContactInfo() :
					null);
		}

		public static int GetSiteProfileId(this User user) {
			int siteProfileId = int.Parse(SystemConfig.DefaultSiteProfileId);
			if (user == null){
				return siteProfileId;
			}

			if (user.IsSuperAdmin() || user.PricingPlanId >= (int)PricingPlans.Business) {
				siteProfileId = user.SiteProfileId;
			} else if (user.ParentUser != null) {
				siteProfileId = user.ParentUser.GetSiteProfileId();
			}

			return siteProfileId;
		
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