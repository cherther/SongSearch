using System;
using System.Security.Principal;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {
	// **************************************
	// IAccountService
	// **************************************
	public interface IAccountService : IDisposable {

		void RegisterUser(User user);
		bool UserIsValid(string userName, string password);
		bool UserExists(string userName);
		bool UpdateProfile(User user, string newPassword);
		bool ResetPassword(string userName, string oldPassword, string newPassword);

		void UpdateSignature(string userName, string signature);
	}


	// **************************************
	// AccountService
	// **************************************
	public class AccountService : IAccountService {

		// ----------------------------------------------------------------------------
		// (Properties)
		// ----------------------------------------------------------------------------
		private ISession _session;
		private bool _disposed;
		//private string _activeUserIdentity;
		

		// ----------------------------------------------------------------------------
		// (Constructor)
		// ----------------------------------------------------------------------------
		public AccountService() {
			if (_session == null) {
				_session = new EFSession();
			}

//			_activeUserIdentity = activeUserIdentity;
//			ActiveUser = UserData.User(activeUserIdentity);
		}
		

		// ----------------------------------------------------------------------------
		// (Public)
		// ----------------------------------------------------------------------------


		// **************************************
		// RegisterUser
		// **************************************    
		public void RegisterUser(User user) {

			var inv = _session.Single<Invitation>(i => i.InvitationId.ToString().Equals(user.Invitation.InvitationId.ToString()));

			if (inv != null) {
				inv.InvitationStatus = (int) InvitationStatusCodes.Registered;
				CreateUser(user);
				_session.CommitChanges();
				inv = null;
			}
		}

		// **************************************
		// UserIsValid
		// **************************************    
		public bool UserIsValid(string userName, string password) {
			var user = GetDbUser(userName);
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
			var user = GetDbUser(userName); 
			bool exists = user != null ? true : false;
			user = null;

			return exists;
		}

		// **************************************
		// UpdateProfile
		// **************************************    
		public bool UpdateProfile(User user, string newPassword) {

			var dbuser = GetDbUser(user); 
			
			if (!String.IsNullOrEmpty(user.FirstName))
				dbuser.FirstName = user.FirstName;
			if (!String.IsNullOrEmpty(user.LastName))
				dbuser.LastName = user.LastName;
			if (!String.IsNullOrEmpty(user.Signature))
				dbuser.Signature = user.Signature;

			if (!String.IsNullOrEmpty(newPassword) && PasswordHashMatches(user.Password, user.Password)) {
				dbuser.Password = newPassword.PasswordHashString();
			}

			_session.Update<User>(dbuser);
			_session.CommitChanges();

			dbuser = null;

			return true;
		}

		
		// **************************************
		// ResetPassword
		// **************************************    
		public bool ResetPassword(string userName, string oldPassword, string newPassword) {

			var user = GetDbUser(userName); 
			
			if (user.Password.Equals(oldPassword.PasswordHashString())) {
				user.Password = newPassword.PasswordHashString();
				_session.Update<User>(user);
				_session.CommitChanges();
				user = null;
				return true;
			}
			else {
				user = null;
				return false;
			}
		}


		// **************************************
		// UpdateSignature
		// **************************************    
		public void UpdateSignature(string userName, string signature) {

			var user = GetDbUser(userName); 
			user.Signature = signature;
			_session.Update<User>(user);
			_session.CommitChanges();
			user = null;
		}

		
		

		// ----------------------------------------------------------------------------
		// (Private)
		// ----------------------------------------------------------------------------

		// **************************************
		// GetDbUser
		// **************************************    
		private User GetDbUser(User user) {
			return GetDbUser(user.UserName);
		}
		private User GetDbUser(string userName) {
			return _session.Single<User>(x => x.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
		}

		// **************************************
		// CreateUser
		// **************************************
		private void CreateUser(User user) {

			user.Password = user.Password.PasswordHashString();
			user.ParentUserId = user.ParentUserId.HasValue ? user.ParentUserId.Value : 1;
			user.RoleId = (int) Roles.Client;

			_session.Add<User>(user);

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
					if (_session != null) {
						_session.Dispose();
						_session = null;
					}					
				}

				_disposed = true;
			}
		}

	}
}