using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using SongSearch.Web.Data;
using Ninject;

namespace SongSearch.Web.Services {
	
	// **************************************
	// AccountService
	// **************************************
	public class AccountService : BaseService, IAccountService {

		// ----------------------------------------------------------------------------
		// (Properties)
		// ----------------------------------------------------------------------------
		private bool _disposed;
		private const int _minPasswordLength = 5;

		// ----------------------------------------------------------------------------
		// (Constructor)
		// ----------------------------------------------------------------------------
		public AccountService(IDataSession session) : base(session) {}
		public AccountService(string activeUserIdentity) : base(activeUserIdentity) { }
		
		// ----------------------------------------------------------------------------
		// (Public)
		// ----------------------------------------------------------------------------
		public static int MinPasswordLength {
			get {
				return _minPasswordLength; // _provider.MinRequiredPasswordLength;
			}
		}
		// **************************************
		// RegisterUser
		// **************************************    
		public User RegisterUser(User user, Guid invitationCode) {

			if (!UserExists(user.UserName)) {
				var inv = Session.Single<Invitation>(i => i.InvitationId.Equals(invitationCode) && i.InvitationEmailAddress.Equals(user.UserName));

				if (inv != null) {

					user.Password = user.Password.PasswordHashString();
					user.ParentUserId = inv.InvitedByUserId > 0 ? inv.InvitedByUserId : 1;
					user.RoleId = (int)Roles.Client;
					user.RegisteredOn = DateTime.Now;

					// Get parent users catalog where parent user is at least a plugger and assign to new user in client role
					var catalogs = Session.All<UserCatalogRole>().Where(x => x.UserId == inv.InvitedByUserId && x.RoleId <= (int)Roles.Plugger);
					catalogs.ForEach(c =>
						user.UserCatalogRoles.Add(new UserCatalogRole() { CatalogId = c.CatalogId, RoleId = (int)Roles.Client })
					);


					inv.InvitationStatus = (int)InvitationStatusCodes.Registered;

					Session.Add<User>(user);


					Session.CommitChanges();
					inv = null;
				}
			} else {
				user = GetUser(user);
			}

			return user;
		}

		// **************************************
		// UserIsValid
		// **************************************    
		public bool UserIsValid(string userName, string password) {
			var user = GetUser(userName);
			if (user == null) {
				return false;
			}
			bool isValid = PasswordHashMatches(user.Password, password);
			user = null;
			return isValid;
		}

		// **************************************
		// UserExists
		// **************************************    
		public bool UserExists(string userName) {
			var user = GetUser(userName); 
			bool exists = user != null ? true : false;
			user = null;

			return exists;
		}

		// **************************************
		// UpdateProfile
		// **************************************    
		public bool UpdateProfile(User user, string newPassword) {

			var dbuser = GetUser(user);
			if (dbuser == null) {
				return false;
			}
			if (!String.IsNullOrEmpty(user.FirstName))
				dbuser.FirstName = user.FirstName;
			if (!String.IsNullOrEmpty(user.LastName))
				dbuser.LastName = user.LastName;
			if (!String.IsNullOrEmpty(user.Signature) && dbuser.IsAtLeastInRole(Roles.Plugger))
				dbuser.Signature = user.Signature;

			if (!String.IsNullOrEmpty(newPassword) && PasswordHashMatches(dbuser.Password, user.Password)) {
				dbuser.Password = newPassword.PasswordHashString();
			}

			Session.Update<User>(dbuser);
			Session.CommitChanges();
			dbuser = null;
			return true;
			
		}

		
		// **************************************
		// ResetPassword
		// **************************************    
		public bool ResetPassword(string userName, string resetCode, string newPassword) {

			var user = GetUser(userName);

			if (user == null) { return false; }

			if (user.UserName.PasswordHashString().Equals(resetCode)) {
				user.Password = newPassword.PasswordHashString();
				Session.Update<User>(user);
				Session.CommitChanges();
				user = null;
				return true;
			}
			else {
				user = null;
				return false;
			}
		}

			
		

		// ----------------------------------------------------------------------------
		// (Private)
		// ----------------------------------------------------------------------------

		// **************************************
		// GetUser
		// **************************************    
		private User GetUser(User user) {
			return GetUser(user.UserName);
		}
		private User GetUser(string userName) {
			return Session.Single<User>(x => x.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
		}

		// **************************************
		// CreateUser
		// **************************************
		private void CreateUser(User user) {

			user.Password = user.Password.PasswordHashString();
			user.ParentUserId = user.ParentUserId.HasValue ? user.ParentUserId.Value : 1;
			user.RoleId = (int) Roles.Client;

			Session.Add<User>(user);

			user = null;
		}

		// **************************************
		// PasswordHashMatches
		// **************************************
		private static bool PasswordHashMatches(string hashed, string unhashed) {
			return hashed.Equals(unhashed.PasswordHashString());
		}

		// ----------------------------------------------------------------------------
		// Dispose
		// ----------------------------------------------------------------------------

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}


		private void Dispose(bool disposing) {
			if (!_disposed) {
				{
					if (Session != null) {
						Session.Dispose();
						Session = null;
					}
					if (SessionReadOnly != null) {
						SessionReadOnly.Dispose();
						SessionReadOnly = null;
					}
				}

				_disposed = true;
			}
		}

	}
}