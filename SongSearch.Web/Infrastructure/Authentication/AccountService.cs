using System;
using System.Security.Principal;
using System.Web;

namespace SongSearch.Web.Services {
	// **************************************
	// IAccountService
	// **************************************
	public interface IAccountService : IDisposable {

		void RegisterUser(DisplayUser displayUser);
		bool UserIsValid(string userName, string password);
		bool UserExists(string userName);
		bool UpdateProfile(DisplayUser displayUser, string newPassword);
		bool ResetPassword(DisplayUser displayUser, string newPassword);

		void UpdateSignature(DisplayUser displayUser);
	}


	// **************************************
	// AccountService
	// **************************************
	public class AccountService : IAccountService {
		// ----------------------------------------------------------------------------
		// (Properties)
		// ----------------------------------------------------------------------------
		private ISession _session;
		private IPrincipal _princ;
		private bool _disposed;

		

		// ----------------------------------------------------------------------------
		// (Constructor)
		// ----------------------------------------------------------------------------
		public AccountService() : this(HttpContext.Current.User) {}

		public AccountService(IPrincipal princ) : this(new SqlSession(), princ) {}
		public AccountService(ISession repository) : this(repository, HttpContext.Current.User) {}

		public AccountService(ISession repository, IPrincipal princ) {
			if (_session == null) {
				_session = new SqlSession();
			}
			if (_princ == null) {
				_princ = princ;
			}
		}

		// ----------------------------------------------------------------------------
		// (Public)
		// ----------------------------------------------------------------------------


		// **************************************
		// RegisterUser
		// **************************************    
		public void RegisterUser(DisplayUser displayUser) {

			var inv = _session.Single<Invitation>(i => i.InvitationId.ToString() == displayUser.Invitation.InvitationId.ToString());

			if (inv != null) {
				inv.InvitationStatus = (int) InvitationStatusCodes.Registered;
				CreateUser(displayUser);
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
			var user = _session.Single<User>(u => u.UserName.ToUpper().Equals(userName.ToUpper()));
			bool exists = user != null ? true : false;
			user = null;

			return exists;
		}

		// **************************************
		// UpdateProfile
		// **************************************    
		public bool UpdateProfile(DisplayUser displayUser, string newPassword) {
			var user = GetDbUser(displayUser);
			if (user == null) {
				return false;
			}

			if (!String.IsNullOrEmpty(displayUser.FirstName))
				user.FirstName = displayUser.FirstName;
			if (!String.IsNullOrEmpty(displayUser.LastName))
				user.LastName = displayUser.LastName;
			if (!String.IsNullOrEmpty(displayUser.Signature))
				user.Signature = displayUser.Signature;

			if (!String.IsNullOrEmpty(newPassword) && PasswordHashMatches(user.Password, displayUser.Password)) {
				user.Password = newPassword.PasswordHashString();
			}

			_session.CommitChanges();

			user = null;

			return true;
		}

		
		// **************************************
		// ResetPassword
		// **************************************    
		public bool ResetPassword(DisplayUser displayUser, string newPassword) {
			var user = GetDbUser(displayUser.UserName);
			if (user == null) {
				return false;
			}

			if (displayUser.Password.Equals(user.UserName.PasswordHashString())) {
				user.Password = newPassword.PasswordHashString();
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
		public void UpdateSignature(DisplayUser displayUser) {
			var user = GetDbUser(displayUser.UserName);
			if (user == null) {
				throw new ArgumentException();
			}
			user.Signature = displayUser.Signature;
			_session.CommitChanges();
			user = null;
		}

		
		

		// ----------------------------------------------------------------------------
		// (Private)
		// ----------------------------------------------------------------------------

		// **************************************
		// GetDbUser
		// **************************************    
		private User GetDbUser(DisplayUser displayUser) {
			return GetDbUser(displayUser.UserName);
		}
		private User GetDbUser(string userName) {
			return _session.Single<User>(u => u.UserName.ToUpper().Equals(userName.ToUpper()));
		}

		// **************************************
		// CreateUser
		// **************************************
		private void CreateUser(DisplayUser displayUser) {
			var user = new User
			           	{
			           		UserName = displayUser.UserName,
			           		Password = displayUser.Password.PasswordHashString(),
			           		FirstName = displayUser.FirstName ?? "",
			           		LastName = displayUser.LastName ?? "",
			           		ParentUserId = displayUser.ParentUserId.HasValue ? displayUser.ParentUserId.Value : 1,
			           		InvitationId = displayUser.Invitation.InvitationId
			           	};

			var ur = new UsersRole
			         	{
			         		UserId = user.UserId,
			         		RoleId = (int) AccessLevels.Client
			         	};

			user.UsersRoles.Add(ur);

			_session.Add<User>(user);

			user = null;
			ur = null;
			//return user.UserId;
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
				// If disposing equals true, dispose all managed
				// and unmanaged resources.if (disposing)
				{
					if (_session != null) {
						_session.Dispose();
						_session = null;
					}
					if (_princ != null) {
						_princ = null;
					}
				}

				// Call the appropriate methods to clean up
				// unmanaged resources here.
				//CloseHandle(handle);
				//handle = IntPtr.Zero;

				_disposed = true;
			}
		}

	}
}