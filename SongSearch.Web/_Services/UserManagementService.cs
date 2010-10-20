using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;
using SongSearch.Web.Services;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
namespace SongSearch.Web.Services {

	public static class UserManagementService { //: BaseService, IUserManagementService {


		// ----------------------------------------------------------------------------
		// (Public)
		// ----------------------------------------------------------------------------

		// **************************************
		// MyUserHierarchy
		// **************************************
		//public static IList<User> GetMyUserHierarchy() {

		//    return Account.User().MyUserHierarchy();

		//}

		// **************************************
		// GetUserCatalogHierarchy
		// **************************************
		public static IList<UserCatalogRole> GetUserCatalogHierarchy(int? parentUserId) {

			using (var ctx = new SongSearchContext()) {

				var user = Account.User();
				var roleId = user.RoleId;

				// Only users with same or lesser access rightsModel
				var query = ctx.UserCatalogRoles.Where(x => x.Role.RoleId >= roleId);

				// For anyone other than SuperAdmin, get child users only
				parentUserId = !user.IsSuperAdmin() ? user.UserId : parentUserId;
				query = !parentUserId.HasValue ? query : query.Where(x => x.User.ParentUser.UserId == parentUserId.Value);

				var users = query.ToList();

				return user.IsSuperAdmin() ? users.Where(u => !u.User.ParentUserId.HasValue).ToList()
													: users;
			}
		}

		// **************************************
		// GetUserCatalogHierarchy
		// **************************************
		public static IList<Invitation> GetMyInvites(InvitationStatusCodes status) {
			using (var ctx = new SongSearchContext()) {

				var userId = Account.User().UserId;
				return ctx.Invitations
					.Where(i => i.InvitedByUserId == userId
								&& i.InvitationStatus == (int)status)
					.ToList();
			}
		}

		// **************************************
		// GetUserDetail
		// **************************************
		public static User GetUserDetail(int userId) {
			using (var ctx = new SongSearchContext()) {
				ctx.ContextOptions.LazyLoadingEnabled = false;
				return ctx.GetUserGraph(userId);
			}
		}
		public static User GetUserDetail(string userName) {
			using (var ctx = new SongSearchContext()) {
				ctx.ContextOptions.LazyLoadingEnabled = false;
				return ctx.GetUserGraph(userName);
			}
		}
		


		// **************************************
		// DeleteUser
		// **************************************
		public static void DeleteUser(int userId, bool takeOwnerShip = false) {

			using (var ctx = new SongSearchContext()) {
				var user = ctx.Users.SingleOrDefault(x => x.UserId == userId);

				if (user != null) {
					if (user.UserId == Account.User().UserId) {
						throw new ArgumentException("You cannot delete yourself");
					}
					if (user.IsSuperAdmin() && GetNumberOfUsersByAccessLevel(Roles.SuperAdmin) <= 1) {
						throw new ArgumentException("You cannot delete the last SuperAdmin");
					} else {

						//if (takeOwnerShip) {
						//    ctx.TakeOwnerShip(user);
						//}
						ctx.ReParentOrphans(user, user.ParentUser);

						if (user.CreatedCatalogs.Count() > 0) {

							var userCats = user.CreatedCatalogs.ToList();
							
							foreach (var cat in userCats) {
								ctx.DeleteCatalog(cat.CatalogId);
							}
						}

						if (user.Invitation != null) {
							ctx.Invitations.DeleteObject(user.Invitation);
						}
						//handled via cascade in db
						//user.ActiveCartContents.ToList().ForEach(x => user.ActiveCartContents.Remove(x));
						//user.UserCatalogRoles.ToList().ForEach(x => user.UserCatalogRoles.Remove(x));
						var balance = user.PlanBalance; // ctx.PlanBalances.SingleOrDefault(x => x.PlanBalanceId == user.PlanBalanceId);
						if (balance != null) {
							balance.Users.Remove(user);
							if (balance.Users.Count() == 0) {
								ctx.Delete(balance);
							} else {
								ctx.RemoveFromUserBalance(user);
								if (user.IsAnyAdmin()) {
									ctx.RemoveFromAdminBalance(user);
								}
							}
						}
						ctx.Users.DeleteObject(user);
						ctx.SaveChanges();

					}
				}
			}
		}

		// **************************************
		// TakeOwnerShip
		// **************************************
		public static void TakeOwnerShip(int userId) {
			using (var ctx = new SongSearchContext()) {
				var user = ctx.GetUser(userId);
				if (user != null) {
					ctx.TakeOwnerShip(user);
					ctx.SaveChanges();
				}
				user = null;
			}
		}

		private static void TakeOwnerShip(this SongSearchContext ctx, User user) {

			//become the parent user
			var userId = Account.User().UserId;
			user.ParentUserId = userId;

			ctx.RemoveFromUserBalance(user);

			//put user on a plan
			if (!user.IsPlanOwner) {

				user.PricingPlanId = (int)PricingPlans.Introductory;
				
				ctx.SubscribeUserTo(user, new PricingPlan() { PricingPlanId = (int)PricingPlans.Introductory });
			}
			// take over the child users
			var childUsers = ctx.Users.Where(u => u.ParentUserId == user.UserId);
			foreach (var child in childUsers) {
				//child.ParentUserId = userId;
				child.PlanUserId = user.UserId;
			}

			//Re-assign invite from this user to active user
			var invites = ctx.Invitations.Where(i => i.InvitedByUserId == user.UserId);
			foreach (var invite in invites) {
				invite.InvitedByUserId = userId;
			}

			
		}
		private static void ReParentOrphans(this SongSearchContext ctx, User user, User newParent) {

			// take over the child users
			var childUsers = ctx.Users.Where(u => u.ParentUserId == user.UserId);
			foreach (var child in childUsers) {
				child.ParentUserId = newParent.UserId;
				child.PlanUserId = newParent.PlanUserId;
				child.PlanBalanceId = newParent.PlanBalanceId;
			}

			//Re-assign invite from this user to active user
			var invites = ctx.Invitations.Where(i => i.InvitedByUserId == user.UserId);
			foreach (var invite in invites) {
				invite.InvitedByUserId = newParent.UserId;
			}


		}

		// **************************************
		// CreateNewInvitation
		// **************************************
		public static Guid CreateNewInvitation(string inviteEmailAddress) {

			using (var ctx = new SongSearchContext()) {
				inviteEmailAddress = inviteEmailAddress.ToLower();
				var userId = Account.User().UserId;
				var inv = ctx.Invitations.SingleOrDefault(
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

					ctx.Invitations.AddObject(inv);
				} else {//extend expiration date

					//inv.InvitationStatus = (int)InvitationStatusCodes.Open;
					inv.ExpirationDate = DateTime.Now.AddDays(30).Date;

				}

				ctx.SaveChanges();

				return inv.InvitationId;
			}
		}

		// **************************************
		// GetInvitation
		// **************************************
		public static Invitation GetInvitation(string inviteId, string inviteEmailAddress) {
			var inviteGuid = new Guid(inviteId);
			using (var ctx = new SongSearchContext()) {
				return ctx.Invitations
						.Include("InvitedByUser")
						.SingleOrDefault(i => i.InvitationId == inviteGuid && i.InvitationEmailAddress.ToLower() == inviteEmailAddress);
			}
		}

		// **************************************
		// GetNumberOfUsersByAccessLevel
		// **************************************
		public static int GetNumberOfUsersByAccessLevel(Roles role) {
			using (var ctx = new SongSearchContext()) {
				return ctx.Users.Count(u => u.RoleId == (int)role);
			}
		}

		// **************************************
		// UpdateUsersRole
		// **************************************
		public static void UpdateUsersRole(int userId, int roleId) {
			using (var ctx = new SongSearchContext()) {
				var user = ctx.GetUser(userId);

				if (user != null && ModelEnums.GetRoles().Contains(roleId)) {
					user.RoleId = roleId;
					if (roleId == (int)Roles.Admin) {
						user.AddToAdminBalance();
					} else if (user.RoleId == (int)Roles.Admin) {
						user.RemoveFromAdminBalance();
					}
					user.RoleId = roleId;
					ctx.SaveChanges();
				}
			}
		}

		// **************************************
		// UpdateUsersRole
		// **************************************
		public static void ToggleSystemAdminAccess(int userId) {
			using (var ctx = new SongSearchContext()) {

				var user = ctx.GetUser(userId);

				var admin = (int)Roles.Admin;

				if (user != null && !user.IsSuperAdmin()) {

					if (user.RoleId != admin) {
						user.RoleId = admin;
						ctx.AddToAdminBalance(user);
					} else {
						user.RoleId = (int)Roles.Client;
						ctx.RemoveFromAdminBalance(user);
					}

					ctx.SaveChanges();
					
					
				}
			}
		}


		// **************************************
		// UpdateUserCatalogRole
		// **************************************
		public static void UpdateUserCatalogRole(int userId, int catalogId, int roleId) {

			using (var ctx = new SongSearchContext()) {

				var user = ctx.GetUserDetail(userId);

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

							ctx.UserCatalogRoles.AddObject(usrCatalogRole);
						}
					} else {

						if (roleId > 0) {
							// update role
							usrCatalogRole.RoleId = roleId;

						} else {

							//also remove these catalogs from any child users, really?
							var childUsers = user.MyUserHierarchy(withCatalogRoles: true);
							foreach (var child in childUsers) {
								var cat = ctx.UserCatalogRoles.SingleOrDefault(x => x.CatalogId == catalogId && x.UserId == child.UserId);
								if (cat != null) { ctx.UserCatalogRoles.DeleteObject(cat); }
							}

							// revoke parent access
							ctx.UserCatalogRoles.DeleteObject(usrCatalogRole);


						}
					}

					// check if it's an admin role; if so, elevate the system role to Admin if user is not already there
					if (roleId == (int)Roles.Admin && user.RoleId > roleId) {
						user.RoleId = roleId;
						ctx.AddToAdminBalance(user);
					}
					ctx.SaveChanges();


				}
			}
		}


		// **************************************
		// UpdateUserRoleAllCatalogs
		// **************************************
		public static void UpdateAllCatalogs(int userId, int roleId) {
			using (var ctx = new SongSearchContext()) {

				var catalogs = ctx.Catalogs.AsQueryable();
				var user = ctx.GetUser(userId);

				if (!Account.User().IsSuperAdmin()) {

					var parentUser = ctx.Users
						.Include("UserCatalogRoles.Catalog")
						.SingleOrDefault(u => u.UserId == user.ParentUserId);

					// limit to catalogs with myUser admin access if not superadmin
					catalogs = parentUser != null ? catalogs.LimitToAdministeredBy(parentUser) : catalogs;
				}

				foreach (var catalog in catalogs) {

					var usrCatalog = user.UserCatalogRoles.SingleOrDefault(uc => uc.CatalogId == catalog.CatalogId);

					if (usrCatalog == null && roleId > 0) {

						// new catalog role
						// check role against enum
						roleId = ModelEnums.GetRoles().GetBestMatchForRole(roleId);

						usrCatalog = new UserCatalogRole {
							UserId = userId,
							CatalogId = catalog.CatalogId,
							RoleId = roleId
						};
						ctx.UserCatalogRoles.AddObject(usrCatalog);
					} else {
						if (roleId > 0) {
							usrCatalog.RoleId = roleId;

						} else {
							// revoke access
							ctx.UserCatalogRoles.DeleteObject(usrCatalog);
						}
					}
				}

				if (user.RoleId >= (int)Roles.Admin && roleId == (int)Roles.Admin) {
					// newly minted admin?
					ctx.AddToAdminBalance(user);
				}
				ctx.SaveChanges();

			}
		}

		// **************************************
		// UpdateCatalogRoleAllUsers
		// **************************************
		public static void UpdateAllUsers(int catalogId, int roleId) {
			using (var ctx = new SongSearchContext()) {
				var cat = ctx.Catalogs.SingleOrDefault(x => x.CatalogId == catalogId);

				if (cat != null) {
					//var users = DataSession.GetObjectQuery<User>().Include("UserCatalogRoles").ToList();
					var users = Account.User().MyUserHierarchy(true);

					ctx.SetCatalogRole(catalogId, roleId, users);
					ctx.SaveChanges();
				}
			}
		}

		
		// **************************************
		// SetCatalogRole
		// **************************************
		private static void SetCatalogRole(this SongSearchContext ctx, int catalogId, int roleId, IList<User> users) {
			foreach (var user in users) {

				var usrCatalog = ctx.UserCatalogRoles.SingleOrDefault(uc => uc.CatalogId == catalogId && uc.UserId == user.UserId);

				roleId = ModelEnums.GetRoles().GetBestMatchForRole(roleId);

				if (usrCatalog == null && roleId > 0) {
					usrCatalog = new UserCatalogRole {
						UserId = user.UserId,
						CatalogId = catalogId,
						RoleId = roleId
					};
					ctx.UserCatalogRoles.AddObject(usrCatalog);
				} else {
					if (roleId > 0) {
						usrCatalog.RoleId = roleId;
						if (user.RoleId >= (int)Roles.Admin && roleId == (int)Roles.Admin) {
							// newly minted admin?
							ctx.AddToAdminBalance(user);
						}
					} else {
						ctx.UserCatalogRoles.DeleteObject(usrCatalog);
					}
				}

				if (user.ChildUsers.Count > 0) {

					ctx.SetCatalogRole(catalogId, roleId, user.ChildUsers);

				}
			}
		}
		// **************************************
		// GetUserDetailWithCatalogsRoles
		// **************************************
		public static User GetUserDetailWithCatalogsRoles(int userId) {
			using (var ctx = new SongSearchContext()) {
				User user = ctx.GetUser(userId);
				user.ParentUser = user.ParentUserId.HasValue ? ctx.GetUser(user.ParentUserId.Value) : null;

				return user;
			}
		}


		// ----------------------------------------------------------------------------
		// (Private)
		// ----------------------------------------------------------------------------
		//private User GetUser(string userName) {
		//    var myUser = _mongo.Single<User>(u => u.UserId == userId).MakeDisplay();
		//    return myUser;
		//}
		private static User GetUser(this SongSearchContext ctx, int userId) {
			return ctx.Users.SingleOrDefault(u => u.UserId == userId);
		}
		private static User GetUser(this SongSearchContext ctx, string userName) {
			return ctx.Users.SingleOrDefault(x => x.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
		}
		private static User GetUserDetail(this SongSearchContext ctx, int userId) {
			return ctx.GetUserGraph(userId);
		}
		private static User GetUserDetail(this SongSearchContext ctx, User user) {
			//TODO: is this not handled via self-join?
			user = ctx.GetUserGraph(user.UserId);

			user.ParentUser = user.ParentUserId.HasValue ?
				ctx.GetUserGraph(user.ParentUserId.Value) :
				null;

			return user;
		}
		private static IQueryable<UserCatalogRole> GetUserCatalogs() {
			using (var ctx = new SongSearchContext()) {
				return ctx.UserCatalogRoles;
			}
		}

		private static IList<UserCatalogRole> AttachChildren(IList<UserCatalogRole> parents, IList<UserCatalogRole> users) {
			foreach (var p in parents) {
				var parent = p;
				var children = GetUserCatalogs().Where(x => x.User.ParentUser.UserId == parent.User.UserId);
				//parent.User.ChildCatalogUsers = AttachChildren(children, users);
			}
			return parents;
		}


	}
}