using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {
	
	// **************************************
	// IUserManagementService
	// **************************************
	public interface IUserManagementService : IDisposable {
		string ActiveUserName { get; set; }
		User ActiveUser { get; set; }

		IList<User> GetMyUserHierarchy();
		IList<Invitation> GetMyInvites(InvitationStatusCodes status);
		User GetUserDetail(int userId);
		User GetUserDetail(string userName);

		Guid CreateNewInvitation(string inviteEmailAddress);
		Invitation GetInvitation(string inviteId, string inviteEmailAddress);

		int GetNumberOfUsersByAccessLevel(Roles role);

		void DeleteUser(int userId, bool takeOwnerShip = true);
		void TakeOwnerShip(int userId);
		void UpdateUsersRole(int userId, int roleId);
		void ToggleSystemAdminAccess(int userId);

		void UpdateUserCatalogRole(int userId, int catalogId, int roleId);

		void UpdateAllCatalogs(int userId, int roleId);

		void UpdateAllUsers(int catalogId, int roleId);
	}
}