using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using SongSearch.Web.Data;
using Ninject;
using System.Collections.Generic;

namespace SongSearch.Web.Services {

	// **************************************
	// AccountService
	// **************************************
	public static class AccountService {// : BaseService, IAccountService {

		// ----------------------------------------------------------------------------
		// (Properties)
		// ----------------------------------------------------------------------------
		private const int _minPasswordLength = 5;
		private const int _defaultUserId = 1;

		// ----------------------------------------------------------------------------
		// (Constructor)
		// ----------------------------------------------------------------------------
		//public AccountService(IDataSession dataSession, IDataSessionReadOnly readSession) : base(dataSession, readSession) { }
		//public AccountService(string activeUserIdentity) : base(activeUserIdentity) { }

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
		public static User RegisterUser(User user, Guid invitationCode) {


			using (var ctx = new SongSearchContext()) {
				var existing = ctx.GetUser(user);
				if (existing != null) {
					return existing;
				} else {
					var inv = ctx.Invitations.SingleOrDefault(i => i.InvitationId.Equals(invitationCode) && i.InvitationEmailAddress.Equals(user.UserName));
					var pricingPlan = ctx.PricingPlans.SingleOrDefault(x => x.PricingPlanId == user.PricingPlanId);

					if (inv == null) {
						throw new ArgumentOutOfRangeException(String.Format("Invalid invitation {0}", inv.InvitationId), innerException: null);
					} else {
						// ----------------------------------
						// CREATE USER
						// ----------------------------------
						var newUser = user.Create(inv, pricingPlan);

						// ----------------------------------
						// GET / CREATE PLAN SUBSCRIPTION
						// ----------------------------------
						if (!inv.IsPlanInvitation) {

							// ----------------------------------
							// GET / CREATE PLAN BALANCE
							// ----------------------------------
							var balance = newUser.SubscribeTo(pricingPlan);


						} else {

							newUser.PlanBalanceId = inv.InvitedByUser.PlanBalance.PlanBalanceId;
							newUser.PlanUserId = inv.InvitedByUser.UserId;
							ctx.AddToUserBalance(newUser);							
						}

						// ----------------------------------
						// CATALOG ACCESS
						// ----------------------------------

						// Get parent users catalog where parent user is at least a plugger and assign to new user in client role
						var catalogs = ctx.UserCatalogRoles.Where(x => x.UserId == inv.InvitedByUserId && x.RoleId <= (int)Roles.Admin);

						catalogs.ForEach(c =>
							newUser.UserCatalogRoles.Add(new UserCatalogRole() { CatalogId = c.CatalogId, RoleId = (int)Roles.Client })
						);

						inv.InvitationStatus = (int)InvitationStatusCodes.Registered;

						ctx.SaveChanges();
						inv = null;

						return newUser;
					}
				}


			}


		}

		// **************************************
		// CreateUser
		// **************************************  
		public static User Create(this User user, Invitation inv, PricingPlan pricingPlan) {

			using (var ctx = new SongSearchContext()) {

				var newUser = new User() {
					UserName = user.UserName,
					FirstName = user.FirstName,
					LastName = user.LastName,
					HasAgreedToPrivacyPolicy = user.HasAgreedToPrivacyPolicy,
					HasAllowedCommunication = user.HasAllowedCommunication,
					Password = user.Password.PasswordHashString(),
					ParentUserId = inv.InvitedByUserId > 0 ? inv.InvitedByUserId : _defaultUserId,
					PlanUserId = inv.InvitedByUser != null ? inv.InvitedByUser.UserId : _defaultUserId, //default placeholder;_defaultUserId; //default placeholder;
					PricingPlanId = pricingPlan.PricingPlanId,
					PlanBalanceId = inv.InvitedByUser != null ? inv.InvitedByUser.PlanBalanceId : _defaultUserId, //default placeholder;

					// Members are Clients until promoted, new plans are admins from the start:
					RoleId = inv.IsPlanInvitation ? (int)Roles.Client : (int)Roles.Admin,

					//user.PricingPlanId = (int)PricingPlans.Basic;
					SiteProfileId = inv.InvitedByUser.SiteProfileId,// int.Parse(SystemConfig.DefaultSiteProfileId);
					RegisteredOn = DateTime.Now,
					InvitationId = inv.InvitationId
				};
				//create user to get a userid
				ctx.Users.AddObject(newUser);
				ctx.SaveChanges();

				return newUser;
			}
		}

		// **************************************
		// Subscribe
		// **************************************  
		public static PlanBalance SubscribeTo(this User user, PricingPlan pricingPlan) {

			using (var ctx = new SongSearchContext()) {
				
				var balance = ctx.SubscribeUserTo(user, pricingPlan);
				ctx.SaveChanges();

				return balance;

			}
		}

		internal static PlanBalance SubscribeUserTo(this SongSearchContext ctx, User user, PricingPlan pricingPlan){
			
			if (user.EntityState == System.Data.EntityState.Detached) {

				//ctx.Detach(user);  
				ctx.Attach(user);
			}

			var oldSubs = user.Subscriptions;
			foreach (var sub in oldSubs) {
				if (sub.SubscriptionEndDate == null) {
					sub.SubscriptionEndDate = DateTime.Now;
				}
			}

			//Start a new Subscription
			var subscription = new Subscription() {
				SubscriptionStartDate = DateTime.Now,
				SubscriptionEndDate = null,
				PricingPlanId = pricingPlan.PricingPlanId,
				PlanCharge = pricingPlan.PlanCharge.GetValueOrDefault()
			};

			user.Subscriptions.Add(subscription);
			ctx.Subscriptions.AddObject(subscription);

			// Adjust current plan
			user.PricingPlanId = pricingPlan.PricingPlanId;

			// if user was already on a plan, switch the balance over; if not, open a new balance
			PlanBalance balance;

			if (user.IsPlanOwner) {

				balance = user.PlanBalance;
				balance.PricingPlanId = pricingPlan.PricingPlanId;
				balance.LastUpdatedByUserId = user.UserId;
				balance.LastUpdatedOn = DateTime.Now;

			} else {

				balance = new PlanBalance() {
					PricingPlanId = pricingPlan.PricingPlanId,
					NumberOfCatalogAdmins = 1,
					NumberOfInvitedUsers = 1,
					NumberOfSongs = 0,
					LastUpdatedByUserId = user.UserId,
					LastUpdatedOn = DateTime.Now
				};

				user.PlanUserId = user.UserId;

				balance.Users.Add(user);
				ctx.PlanBalances.AddObject(balance);
			}

			return balance;
			
		}


		// **************************************
		// UserIsValid
		// **************************************    
		public static bool UserIsValid(string userName, string password) {
			using (var ctx = new SongSearchContext()) {
				var user = ctx.Users.SingleOrDefault(x => x.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
				return user != null && PasswordHashMatches(user.Password, password);
			}
		}

		// **************************************
		// UserExists
		// **************************************    
		public static bool UserExists(string userName) {
			using (var ctx = new SongSearchContext()) {
				return ctx.GetUser(userName) != null ? true : false;
			}
		}

		// **************************************
		// UpdateProfile
		// **************************************    
		public static bool UpdateProfile(User user, IList<Contact> contacts) {

			
			using (var ctx = new SongSearchContext()) {

				var dbuser = ctx.GetUser(user);
				if (dbuser == null) {
					return false;
				}

				dbuser.FirstName = user.FirstName;
				dbuser.LastName = user.LastName;
				dbuser.Signature = user.Signature;
				dbuser.AppendSignatureToTitle = user.AppendSignatureToTitle;
				dbuser.HasAllowedCommunication = user.HasAllowedCommunication;

				foreach (var contact in contacts) {
					if (contact != null && (!String.IsNullOrWhiteSpace(contact.Phone1) || !String.IsNullOrWhiteSpace(contact.Email))) {
						var dbContact = dbuser.Contacts.SingleOrDefault(c => c.ContactId == contact.ContactId) ??
							new Contact() {
								ContactTypeId = contact.ContactTypeId > 0 ? contact.ContactTypeId : (int)ContactTypes.Main,
								//IsDefault = true,
								CreatedByUserId = dbuser.UserId,
								CreatedOn = DateTime.Now
							};

						dbContact.ContactName = contact.ContactName;
						dbContact.CompanyName = contact.CompanyName;
						dbContact.Address1 = contact.Address1;
						dbContact.Address2 = contact.Address2;
						dbContact.City = contact.City;
						dbContact.StateRegion = contact.StateRegion;
						dbContact.PostalCode = contact.PostalCode;
						dbContact.Country = contact.Country;
						dbContact.Phone1 = contact.Phone1;
						dbContact.Phone2 = contact.Phone2;
						dbContact.Fax = contact.Fax;
						dbContact.Email = contact.Email;
						dbContact.AdminEmail = contact.AdminEmail;

						if (dbContact.ContactId == 0) {
							ctx.Contacts.AddObject(dbContact);
							dbuser.Contacts.Add(dbContact);
						}
					}
				}

				ctx.SaveChanges();

				dbuser = null;
				return true;

			}

		}

		// **************************************
		// UpdateProfile
		// **************************************    
		public static bool ChangePassword(User user, string newPassword) {


			using (var ctx = new SongSearchContext()) {

				var dbuser = ctx.GetUser(user);
				if (dbuser == null) {
					return false;
				}

				if (!String.IsNullOrEmpty(newPassword)) {
					if (PasswordHashMatches(dbuser.Password, user.Password)) {
						dbuser.Password = newPassword.PasswordHashString();
					} else {
						throw new ArgumentException("Passwords do not match");
					}
				} else {
					throw new ArgumentNullException("New password cannot be blank");
				}

				ctx.SaveChanges();
				dbuser = null;
				return true;
			}

		}
		// **************************************
		// ResetPassword
		// **************************************    
		public static bool ResetPassword(string userName, string resetCode, string newPassword) {


			using (var ctx = new SongSearchContext()) {

				var user = ctx.GetUser(userName);
				if (user == null) {
					return false;
				}

				if (user.UserName.PasswordHashString().Equals(resetCode)) {
					user.Password = newPassword.PasswordHashString();
					ctx.SaveChanges();
					user = null;
					return true;
				} else {
					user = null;
					return false;
				}
			}
		}

		// ----------------------------------------------------------------------------
		// (Internal)
		// ----------------------------------------------------------------------------
		// **************************************
		// GetUser
		// **************************************    
		internal static User GetUser(this SongSearchContext ctx, User user) {
			return ctx.GetUser(user.UserName);
		}
		internal static User GetUser(this SongSearchContext ctx, string userName) {
			return ctx.Users.SingleOrDefault(x => x.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
		}
		internal static User GetUserGraph(this SongSearchContext ctx, string userName) {
			return ctx.Users
					.Include("ParentUser")
					.Include("Carts")
					.Include("Carts.Contents")
					.Include("UserCatalogRoles")
					.Include("Contacts")
					.Include("ParentUser.Contacts")
					.Include("PricingPlan")
					.Include("PlanBalance")
					.Include("PlanBalance.PricingPlan")
					.Include("ParentUser.PricingPlan")
					.SingleOrDefault(x => x.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
		}

		internal static User GetUserGraph(this SongSearchContext ctx, int userId) {
			return ctx.Users
					.Include("ParentUser")
					.Include("Carts")
					.Include("Carts.Contents")
					.Include("UserCatalogRoles")
					.Include("Contacts")
					.Include("ParentUser.Contacts")
					.Include("PricingPlan")
					.Include("PlanBalance")
					.Include("PlanBalance.PricingPlan")
					.Include("ParentUser.PricingPlan")
					.SingleOrDefault(u => u.UserId == userId);
		}
		// ----------------------------------------------------------------------------
		// (Private)
		// ----------------------------------------------------------------------------


		// **************************************
		// PasswordHashMatches
		// **************************************
		private static bool PasswordHashMatches(string hashed, string unhashed) {
			return hashed.Equals(unhashed.PasswordHashString());
		}

		

	}
}