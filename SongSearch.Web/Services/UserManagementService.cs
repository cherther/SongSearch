using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {

	public class UserManagementService : IUserManagementService {

		// ----------------------------------------------------------------------------
		// (Properties)
		// ----------------------------------------------------------------------------
		private ISession _session;
		private bool _disposed;
		private string _activeUserIdentity;

		public User ActiveUser { get; set; }

		public UserManagementService(string activeUserIdentity) {
			if (_session == null) {
				_session = new EFSession();
			}

			_activeUserIdentity = activeUserIdentity;
			ActiveUser = AccountData.User(activeUserIdentity);
		}

		// ----------------------------------------------------------------------------
		// (Public)
		// ----------------------------------------------------------------------------

		// **************************************
		// GetUserHierarchy
		// **************************************
		public IList<User> GetMyUserHierarchy() {

			return ActiveUser.GetUserHierarchy();

		}

		// **************************************
		// GetUserCatalogHierarchy
		// **************************************
		public IList<UserCatalogRole> GetUserCatalogHierarchy(int? parentUserId) {
			var query = GetUsersCatalog();
			var roleId = ActiveUser.RoleId;

			// Only users with same or lesser access rights
			query = query.Where(x => x.Role.RoleId >= roleId);

			// For anyone other than SuperAdmin, get child users only
			parentUserId = !ActiveUser.IsSuperAdmin() ? ActiveUser.UserId : parentUserId;
			query = !parentUserId.HasValue ? query : query.Where(x => x.User.ParentUser.UserId == parentUserId.Value);

			var users = query.ToList();

			var userHierarchy = ActiveUser.IsSuperAdmin()
												? users.Where(u => !u.User.ParentUserId.HasValue).ToList()
												: users;

			//userHierarchy = AttachChildren(userHierarchy, users);

			//var parents = _princ.IsSuperAdmin()
			//            ? query.Where(u => !u.ParentUserId.HasValue)
			//            : query;

			//var children = query.Where(u => u.ParentUserId.HasValue);

			//IQueryable<DisplayUser> results = MakeUserTree(parents, children);
			//var userTree = results.ToList();

			return userHierarchy;
		}


		public IList<Invitation> GetMyInvites(InvitationStatusCodes status) {
			return _session.All<Invitation>()
				.Where(i => i.InvitedByUserId == ActiveUser.UserId
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
				_session.Single<User>(u => u.UserId == user.ParentUserId) :
				null;

			return user;
		}
		// **************************************
		// DeleteUser
		// **************************************
		public void DeleteUser(int userId, bool takeOwnerShip = true) {
			var user = GetUser(userId);

			if (user != null) {
				if (user.UserId == ActiveUser.UserId) {
					throw new ArgumentException("You cannot delete yourself");
				}
				if (user.IsSuperAdmin() && GetNumberOfUsersByAccessLevel(Roles.SuperAdmin) <= 1) {
					throw new ArgumentException("You cannot delete the last SuperAdmin");
				} else {
					if (takeOwnerShip) {
						TakeOwnerShip(user);
					}

					if (user.Invitation != null) {
						_session.Delete<Invitation>(user.Invitation);
					}
					//handled via cascade in db
					//user.Carts.ToList().ForEach(x => user.Carts.Remove(x));
					//user.UserCatalogRoles.ToList().ForEach(x => user.UserCatalogRoles.Remove(x));

					_session.Delete<User>(user);
					_session.CommitChanges();
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
			user.ParentUserId = ActiveUser.UserId;

			// take over the child users
			var childUsers = _session.All<User>().Where(u => u.ParentUserId == user.UserId);
			foreach (var child in childUsers) {
				child.ParentUserId = ActiveUser.UserId;
			}

			//Re-assign invites from this myUser to active myUser
			var invites = _session.All<Invitation>().Where(i => i.InvitedByUserId == user.UserId);
			foreach (var invite in invites) {
				invite.InvitedByUserId = ActiveUser.UserId;
			}

			_session.CommitChanges();
		}

		// **************************************
		// CreateNewInvitation
		// **************************************
		public Guid CreateNewInvitation(string inviteEmailAddress) {

			inviteEmailAddress = inviteEmailAddress.ToLower();

			var inv = _session.Single<Invitation>(
				i => i.InvitationEmailAddress.ToLower() == inviteEmailAddress
				&& i.InvitationStatus == (int)InvitationStatusCodes.Open
				&& i.InvitedByUserId == ActiveUser.UserId);

			if (inv == null) {
				inv = new Invitation {
					InvitationEmailAddress = inviteEmailAddress,
					ExpirationDate = DateTime.Now.AddDays(30).Date,
					InvitedByUserId = ActiveUser.UserId,
					InvitationStatus = (int)InvitationStatusCodes.Open
				};

				_session.Add<Invitation>(inv);
			} else //extend expiration date
			{
				//inv.InvitationStatus = (int)InvitationStatusCodes.Open;
				inv.ExpirationDate = DateTime.Now.AddDays(30).Date;

			}
			_session.CommitChanges();

			var inviteId = inv.InvitationId;

			return inviteId;
		}

		// **************************************
		// GetInvitation
		// **************************************
		public Invitation GetInvitation(string inviteId, string inviteEmailAddress) {
			var inviteGuid = new Guid(inviteId);

			var inv = _session.Single<Invitation>(i => i.InvitationId == inviteGuid && i.InvitationEmailAddress.ToLower() == inviteEmailAddress);

			return inv;
		}

		// **************************************
		// GetNumberOfUsersByAccessLevel
		// **************************************
		public int GetNumberOfUsersByAccessLevel(Roles role) {
			var users = _session.All<User>().Where(u => u.RoleId == (int)role);
			return (users.Count());
		}

		// **************************************
		// UpdateUsersRole
		// **************************************
		public void UpdateUsersRole(int userId, int roleId) {
			var user = GetUser(userId);

			if (user != null && ModelEnums.GetRoles().Contains(roleId)) {
				user.RoleId = roleId;
				_session.Update<User>(user);
				_session.CommitChanges();
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
						_session.Add<UserCatalogRole>(usrCatalogRole);
					}
				} else {
				
					if (roleId > 0) {
						// update role
						usrCatalogRole.RoleId = roleId;
					
					} else {

						//also remove these catalogs from any child users, really?
						var childUsers = user.GetUserHierarchy(withCatalogRoles: true);
						foreach (var child in childUsers) {
							var cat = _session.Single<UserCatalogRole>(x => x.CatalogId == catalogId && x.UserId == child.UserId);
							if (cat != null) { _session.Delete<UserCatalogRole>(cat); }
						}

						// revoke parent access
						_session.Delete<UserCatalogRole>(usrCatalogRole);


					}
				}

				_session.CommitChanges();

			}
		}


		// **************************************
		// UpdateUserRoleAllCatalogs
		// **************************************
		public void UpdateUserRoleAllCatalogs(int userId, int roleId) {
			var usr = GetUser(userId);
			var parentUsr = GetUser(usr.ParentUserId.GetValueOrDefault());

			if (usr != null) {
				var catalogs = _session.All<Catalog>();
				// limit to catalogs with myUser admin access if not superadmin
				if (!ActiveUser.IsSuperAdmin() && parentUsr != null) {
					catalogs = from uc in parentUsr.UserCatalogRoles.AsQueryable()
							   join c in catalogs on uc.CatalogId equals c.CatalogId
							   select c;
				}

				foreach (var catalog in catalogs) {
					var usrCatalog = usr.UserCatalogRoles.Where(uc => uc.CatalogId == catalog.CatalogId).SingleOrDefault();
					
					if (usrCatalog == null && roleId > 0) {
					
						// new catalog role
						// check role against enum
						roleId = ModelEnums.GetRoles().GetBestMatchForRole(roleId);

						usrCatalog = new UserCatalogRole {
							UserId = userId,
							CatalogId = catalog.CatalogId,
							RoleId = roleId
						};
						_session.Add<UserCatalogRole>(usrCatalog);
					} else {
						if (roleId > 0) {
							usrCatalog.RoleId = roleId;

						} else {
							// revoke access
							_session.Delete<UserCatalogRole>(usrCatalog);
						}
					}
				}

				_session.CommitChanges();
			}
		}

		// **************************************
		// UpdateCatalogRoleAllUsers
		// **************************************
		public void UpdateCatalogRoleAllUsers(int catalogId, int roleId) {

			var cat = _session.Single<Catalog>(x => x.CatalogId == catalogId);

			if (cat != null) {
				var allUsers = _session.All<User>();
				var myUsers = ActiveUser.GetUserHierarchy();

				SetCatalogRole(catalogId, roleId, allUsers, myUsers);
			}
		}

		// **************************************
		// SetCatalogRole
		// **************************************
		private void SetCatalogRole(int catalogId, int roleId, IQueryable<User> allUsers, IList<User> myUsers) {
			foreach (var myUser in myUsers) {

				var user = allUsers.Where(x => x.UserId == myUser.UserId).SingleOrDefault();

				if (user != null) {
					var usrCatalog = user.UserCatalogRoles.Where(uc => uc.CatalogId == catalogId).SingleOrDefault();

					roleId = ModelEnums.GetRoles().GetBestMatchForRole(roleId);

					if (usrCatalog == null && roleId > 0) {
						usrCatalog = new UserCatalogRole {
							UserId = myUser.UserId,
							CatalogId = catalogId,
							RoleId = roleId
						};
						_session.Add<UserCatalogRole>(usrCatalog);
					} else {
						if (roleId > 0) {
							usrCatalog.RoleId = roleId;
						} else {
							_session.Delete<UserCatalogRole>(usrCatalog);
						}
					}

					_session.CommitChanges();

					if (myUser.ChildUsers.Count > 0) {

						SetCatalogRole(catalogId, roleId, allUsers.AsQueryable(), myUser.GetUserHierarchy());

					}
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
			var user = _session.Single<User>(u => u.UserId == userId);
			return user;
		}
		private User GetUser(string userName) {
			var user = _session.Single<User>(u => u.UserName == userName);
			return user;
		}
		private IQueryable<User> GetUsers() {
			var users = _session.All<User>();
			return users;
		}

		private IQueryable<UserCatalogRole> GetUsersCatalog() {
			var users = _session.All<UserCatalogRole>();
			return users;
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
				// If disposing equals true, dispose all managed
				// and unmanaged resources.if (disposing)
				{
					if (_session != null) {
						_session.Dispose();
						_session = null;
					}
					//if (UserRepository != null) {
					//    UserRepository.Dispose();
					//    UserRepository = null;
					//}
					//if (_princ != null) {
					//    _princ = null;
					//}
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