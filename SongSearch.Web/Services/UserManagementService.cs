using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
namespace SongSearch.Web.Services {

	public class UserManagementService : BaseService, IUserManagementService {

		// ----------------------------------------------------------------------------
		// (Properties)
		// ----------------------------------------------------------------------------
		private bool _disposed;
				
		public UserManagementService(IDataSession session) : base(session) {}
		public UserManagementService(string activeUserIdentity) : base(activeUserIdentity) { }


		// ----------------------------------------------------------------------------
		// (Public)
		// ----------------------------------------------------------------------------

		// **************************************
		// MyUserHierarchy
		// **************************************
		public IList<User> GetMyUserHierarchy() {

			return Account.User().MyUserHierarchy();

		}

		// **************************************
		// GetUserCatalogHierarchy
		// **************************************
		public IList<UserCatalogRole> GetUserCatalogHierarchy(int? parentUserId) {
			var query = GetUsersCatalog();
			var user = Account.User();
			var roleId = user.RoleId;

			// Only users with same or lesser access rightsModel
			query = query.Where(x => x.Role.RoleId >= roleId);

			// For anyone other than SuperAdmin, get child users only
			parentUserId = !user.IsSuperAdmin() ? user.UserId : parentUserId;
			query = !parentUserId.HasValue ? query : query.Where(x => x.User.ParentUser.UserId == parentUserId.Value);

			var users = query.ToList();

			return user.IsSuperAdmin() ? users.Where(u => !u.User.ParentUserId.HasValue).ToList()
												: users;

		}
		
		// **************************************
		// GetUserCatalogHierarchy
		// **************************************
		public IList<Invitation> GetMyInvites(InvitationStatusCodes status) {
			var userId = Account.User().UserId;
			return DataSession.All<Invitation>()
				.Where(i => i.InvitedByUserId == userId
							&& i.InvitationStatus == (int)status)
				.ToList();				
		}

		// **************************************
		// GetUserDetail
		// **************************************
		public User GetUserDetail(int userId) {
			return GetUserDetail(GetUser(userId));
		}
		public User GetUserDetail(string userName) {
			return GetUserDetail(GetUser(userName));
		}
		private User GetUserDetail(User user) {

			//TODO: is this not handled via self-join?
			user.ParentUser = user.ParentUserId.HasValue ?
				DataSession.Single<User>(u => u.UserId == user.ParentUserId) :
				null;

			return user;
		}

		
		// **************************************
		// DeleteUser
		// **************************************
		public void DeleteUser(int userId, bool takeOwnerShip = true) {
			var user = GetUser(userId);

			if (user != null) {
				if (user.UserId == Account.User().UserId) {
					throw new ArgumentException("You cannot delete yourself");
				}
				if (user.IsSuperAdmin() && GetNumberOfUsersByAccessLevel(Roles.SuperAdmin) <= 1) {
					throw new ArgumentException("You cannot delete the last SuperAdmin");
				} else {

					if (takeOwnerShip) {
						TakeOwnerShip(user);
					}
					
					if (user.CreatedCatalogs.Count() > 0) {

						var userCats = user.CreatedCatalogs.ToList();
						var catSvc = App.Container.Get<ICatalogManagementService>();
		
						foreach (var cat in userCats) {
							catSvc.DeleteCatalog(cat.CatalogId);						
						}
					}
					
					if (user.Invitation != null) {
						DataSession.Delete<Invitation>(user.Invitation);
					}
					//handled via cascade in db
					//user.ActiveCartContents.ToList().ForEach(x => user.ActiveCartContents.Remove(x));
					//user.UserCatalogRoles.ToList().ForEach(x => user.UserCatalogRoles.Remove(x));

					DataSession.Delete<User>(user);
					DataSession.CommitChanges();
				}
			}
		}

		// **************************************
		// TakeOwnerShip
		// **************************************
		public void TakeOwnerShip(int userId) {
			var user = GetUser(userId);

			TakeOwnerShip(user);

			user = null;
		}

		private void TakeOwnerShip(User user) {
			
			//become the parent users
			var userId = Account.User().UserId;
			user.ParentUserId = userId;

			// take over the child users
			var childUsers = DataSession.All<User>().Where(u => u.ParentUserId == user.UserId);
			foreach (var child in childUsers) {
				child.ParentUserId = userId;
			}

			//Re-assign contentModel from this myUser to active myUser
			var invites = DataSession.All<Invitation>().Where(i => i.InvitedByUserId == user.UserId);
			foreach (var invite in invites) {
				invite.InvitedByUserId = userId;
			}

			DataSession.CommitChanges();
		}

		// **************************************
		// CreateNewInvitation
		// **************************************
		public Guid CreateNewInvitation(string inviteEmailAddress) {

			inviteEmailAddress = inviteEmailAddress.ToLower();
			var userId = Account.User().UserId;
			var inv = DataSession.Single<Invitation>(
				i => i.InvitationEmailAddress.ToLower() == inviteEmailAddress.ToLower()
				&& i.InvitationStatus == (int)InvitationStatusCodes.Open
				&& i.InvitedByUserId == userId);

			if (inv == null) {
				inv = new Invitation {
					InvitationId = Guid.NewGuid(),
					InvitationEmailAddress = inviteEmailAddress,
					ExpirationDate = DateTime.Now.AddDays(30).Date,
					InvitedByUserId = userId,
					InvitationStatus = (int)InvitationStatusCodes.Open,
					IsPlanInvitation = !Account.User().IsSuperAdmin() //if it's a member admin, count against their user quota
				};

				DataSession.Add<Invitation>(inv);
			} else //extend expiration date
			{
				//inv.InvitationStatus = (int)InvitationStatusCodes.Open;
				inv.ExpirationDate = DateTime.Now.AddDays(30).Date;

			}
			DataSession.CommitChanges();

			return inv.InvitationId;

		}

		// **************************************
		// GetInvitation
		// **************************************
		public Invitation GetInvitation(string inviteId, string inviteEmailAddress) {
			var inviteGuid = new Guid(inviteId);

			return DataSession.GetObjectQuery<Invitation>()
                    .Include("InvitedByUser").SingleOrDefault(i => i.InvitationId == inviteGuid && i.InvitationEmailAddress.ToLower() == inviteEmailAddress);
		}

		// **************************************
		// GetNumberOfUsersByAccessLevel
		// **************************************
		public int GetNumberOfUsersByAccessLevel(Roles role) {
			return DataSession.All<User>().Where(u => u.RoleId == (int)role).Count();
		}

		// **************************************
		// UpdateUsersRole
		// **************************************
		public void UpdateUsersRole(int userId, int roleId) {
			var user = GetUser(userId);

			if (user != null && ModelEnums.GetRoles().Contains(roleId)) {
				user.RoleId = roleId;
				DataSession.Update<User>(user);
				DataSession.CommitChanges();
			}
		}

		// **************************************
		// UpdateUsersRole
		// **************************************
		public void ToggleSystemAdminAccess(int userId) {

			var admin = (int)Roles.Admin;
			var user = GetUser(userId);

			if (user != null && !user.IsSuperAdmin()) {

				user.RoleId = user.RoleId != admin ? admin : (int)Roles.Client;
				DataSession.Update<User>(user);
				DataSession.CommitChanges();
			}
		}


		// **************************************
		// UpdateUserCatalogRole
		// **************************************
		public void UpdateUserCatalogRole(int userId, int catalogId, int roleId) {
			var user = GetUserDetail(userId);
			if (user != null && catalogId > 0) {
				
				var usrCatalogRole = user.UserCatalogRoles.Where(c => c.CatalogId == catalogId).SingleOrDefault();

				if (usrCatalogRole == null) {
					if (roleId > 0) {
						// new catalog role

						// check role against enum
						roleId = ModelEnums.GetRoles().GetBestMatchForRole(roleId);

						usrCatalogRole =
							new UserCatalogRole {
								UserId = userId,
								CatalogId = catalogId,
								RoleId = roleId
							};
						DataSession.Add<UserCatalogRole>(usrCatalogRole);
					}
				} else {
				
					if (roleId > 0) {
						// update role
						usrCatalogRole.RoleId = roleId;
					
					} else {

						//also remove these catalogs from any child users, really?
						var childUsers = user.MyUserHierarchy(withCatalogRoles: true);
						foreach (var child in childUsers) {
							var cat = DataSession.Single<UserCatalogRole>(x => x.CatalogId == catalogId && x.UserId == child.UserId);
							if (cat != null) { DataSession.Delete<UserCatalogRole>(cat); }
						}

						// revoke parent access
						DataSession.Delete<UserCatalogRole>(usrCatalogRole);


					}
				}

				// check if it's an admin role; if so, elevate the system role to Admin if user is not already there
				if (roleId == (int)Roles.Admin & user.RoleId > roleId) {
					user.RoleId = roleId;
				}
				DataSession.CommitChanges();

			}
		}


		// **************************************
		// UpdateUserRoleAllCatalogs
		// **************************************
		public void UpdateAllCatalogs(int userId, int roleId) {

			var catalogs = DataSession.All<Catalog>();
			var usr = DataSession.Single<User>(u => u.UserId == userId);
			
			if (!Account.User().IsSuperAdmin()){

				var parentUsr = DataSession.GetObjectQuery<User>()
					.Include("UserCatalogRoles.Catalog")
					.SingleOrDefault(u => u.UserId == usr.ParentUserId); 
				
				// limit to catalogs with myUser admin access if not superadmin
				catalogs = parentUsr != null ? catalogs.LimitToAdministeredBy(parentUsr) : catalogs;
			}

			foreach (var catalog in catalogs) {

				var usrCatalog = usr.UserCatalogRoles.SingleOrDefault(uc => uc.CatalogId == catalog.CatalogId);
					
				if (usrCatalog == null && roleId > 0) {
					
					// new catalog role
					// check role against enum
					roleId = ModelEnums.GetRoles().GetBestMatchForRole(roleId);

					usrCatalog = new UserCatalogRole {
						UserId = userId,
						CatalogId = catalog.CatalogId,
						RoleId = roleId
					};
					DataSession.Add<UserCatalogRole>(usrCatalog);
				} else {
					if (roleId > 0) {
						usrCatalog.RoleId = roleId;

					} else {
						// revoke access
						DataSession.Delete<UserCatalogRole>(usrCatalog);
					}
				}
			}

			DataSession.CommitChanges();
			
		}

		// **************************************
		// UpdateCatalogRoleAllUsers
		// **************************************
		public void UpdateAllUsers(int catalogId, int roleId) {

			var cat = DataSession.Single<Catalog>(x => x.CatalogId == catalogId);

			if (cat != null) {
				//var users = DataSession.GetObjectQuery<User>().Include("UserCatalogRoles").ToList();
				var users = Account.User().MyUserHierarchy(true);

				SetCatalogRole(catalogId, roleId, users);
				DataSession.CommitChanges();
			}
		}

		// **************************************
		// SetCatalogRole
		// **************************************
		private void SetCatalogRole(int catalogId, int roleId, IList<User> users) {
			foreach (var user in users) {

				var usrCatalog = DataSession.Single<UserCatalogRole>(uc => uc.CatalogId == catalogId && uc.UserId == user.UserId);
					
				roleId = ModelEnums.GetRoles().GetBestMatchForRole(roleId);

				if (usrCatalog == null && roleId > 0) {
					usrCatalog = new UserCatalogRole {
						UserId = user.UserId,
						CatalogId = catalogId,
						RoleId = roleId
					};
					DataSession.Add<UserCatalogRole>(usrCatalog);
				} else {
					if (roleId > 0) {
						usrCatalog.RoleId = roleId;
					} else {
						DataSession.Delete<UserCatalogRole>(usrCatalog);
					}
				}

				if (user.ChildUsers.Count > 0) {

					SetCatalogRole(catalogId, roleId, user.ChildUsers);

				}
			}
		}

		public User GetUserDetailWithCatalogsRoles(int userId) {
			User user = GetUser(userId);
			user.ParentUser = user.ParentUserId.HasValue ? GetUser(user.ParentUserId.Value) : null;

			return user;
		}


		// ----------------------------------------------------------------------------
		// (Private)
		// ----------------------------------------------------------------------------
		//private User GetUser(string userName) {
		//    var myUser = _mongo.Single<User>(u => u.UserId == userId).MakeDisplay();
		//    return myUser;
		//}
		private User GetUser(int userId) {
			return DataSession.Single<User>(u => u.UserId == userId);			
		}
		private User GetUser(string userName) {
			return DataSession.Single<User>(u => u.UserName == userName);			
		}
		private IQueryable<User> GetUsers() {
			return DataSession.All<User>();
		}

		private IQueryable<UserCatalogRole> GetUsersCatalog() {
			return DataSession.All<UserCatalogRole>();
		}

		private IList<UserCatalogRole> AttachChildren(IList<UserCatalogRole> parents, IList<UserCatalogRole> users) {
			foreach (var p in parents) {
				var parent = p;
				var children = GetUsersCatalog().Where(x => x.User.ParentUser.UserId == parent.User.UserId);
				//parent.User.ChildCatalogUsers = AttachChildren(children, users);
			}
			return parents;
		}

		// ----------------------------------------------------------------------------
		// (Dispose)
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