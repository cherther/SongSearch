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
		public AccountService(IDataSession dataSession, IDataSessionReadOnly readSession) : base(dataSession, readSession) { }
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
				var inv = DataSession.Single<Invitation>(i => i.InvitationId.Equals(invitationCode) && i.InvitationEmailAddress.Equals(user.UserName));

				if (inv != null) {

					user.Password = user.Password.PasswordHashString();
					user.ParentUserId = inv.InvitedByUserId > 0 ? inv.InvitedByUserId : 1;
					user.RoleId = (int)Roles.Client;
					user.RegisteredOn = DateTime.Now;

					// Get parent users catalog where parent user is at least a plugger and assign to new user in client role
					var catalogs = DataSession.All<UserCatalogRole>().Where(x => x.UserId == inv.InvitedByUserId && x.RoleId <= (int)Roles.Plugger);
					catalogs.ForEach(c =>
						user.UserCatalogRoles.Add(new UserCatalogRole() { CatalogId = c.CatalogId, RoleId = (int)Roles.Client })
					);


					inv.InvitationStatus = (int)InvitationStatusCodes.Registered;

					DataSession.Add<User>(user);


					DataSession.CommitChanges();
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
			var user = ReadSession.Single<User>(x => x.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
			return user != null && PasswordHashMatches(user.Password, password);
		}

		// **************************************
		// UserExists
		// **************************************    
		public bool UserExists(string userName) {
			return GetUser(userName) != null ? true : false;
		}

		// **************************************
		// UpdateProfile
		// **************************************    
		public bool UpdateProfile(User user) {

			var dbuser = GetUser(user);
			if (dbuser == null) {
				return false;
			}
			dbuser.FirstName = user.FirstName;
			dbuser.LastName = user.LastName;
			dbuser.Signature = user.Signature;
			dbuser.AppendSignatureToTitle = user.AppendSignatureToTitle;

			DataSession.CommitChanges();
			dbuser = null;
			return true;
			
		}

		// **************************************
		// UpdateProfile
		// **************************************    
		public bool ChangePassword(User user, string newPassword) {

			var dbuser = GetUser(user);
			if (dbuser == null) {
				return false;
			}

			if (!String.IsNullOrEmpty(newPassword) && PasswordHashMatches(dbuser.Password, user.Password)) {
				dbuser.Password = newPassword.PasswordHashString();
			}

			DataSession.CommitChanges();
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
				DataSession.Update<User>(user);
				DataSession.CommitChanges();
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
			return DataSession.Single<User>(x => x.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
		}

		// **************************************
		// CreateUser
		// **************************************
		private void CreateUser(User user) {

			user.Password = user.Password.PasswordHashString();
			user.ParentUserId = user.ParentUserId.HasValue ? user.ParentUserId.Value : 1;
			user.RoleId = (int) Roles.Client;
			user.SiteProfileId = int.Parse(Settings.SiteProfile.Value());
			user.ShowDebugInfo = false;
			user.AppendSignatureToTitle = false;

			DataSession.Add<User>(user);

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
					if (DataSession != null) {
						DataSession.Dispose();
						DataSession = null;
					}
					if (ReadSession != null) {
						ReadSession.Dispose();
						ReadSession = null;
					}
				}

				_disposed = true;
			}
		}

	}
}