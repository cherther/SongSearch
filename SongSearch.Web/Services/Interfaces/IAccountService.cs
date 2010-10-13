using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {

	// **************************************
	// IAccountService
	// **************************************
	public interface IAccountService : IDisposable {

		User RegisterUser(User user, Guid invitationCode);
		bool UserIsValid(string userName, string password);
		bool UserExists(string userName);
		bool UpdateProfile(User user, IList<Contact> contacts);
		bool ChangePassword(User user, string newPassword);
		bool ResetPassword(string userName, string resetCode, string newPassword);
	}
}