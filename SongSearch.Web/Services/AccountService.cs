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
	public class AccountService : BaseService, IAccountService {

		// ----------------------------------------------------------------------------
		// (Properties)
		// ----------------------------------------------------------------------------
		private bool _disposed;
		private const int _minPasswordLength = 5;
		private const int _defaultUserId = 1;

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
				var invUser = DataSession.Single<User>(u => u.UserId == inv.InvitedByUserId);
				var pricingPlan = DataSession.Single<PricingPlan>(x => x.PricingPlanId == user.PricingPlanId);

				if (inv != null) {

					user.Password = user.Password.PasswordHashString();
					user.ParentUserId = inv.InvitedByUserId > 0 ? inv.InvitedByUserId : _defaultUserId;
					user.PlanUserId = _defaultUserId; //default placeholder;
					user.PricingPlanId = pricingPlan.PricingPlanId;

					// Members are Clients until promoted, new plans are admins from the start:
					user.RoleId = inv.IsPlanInvitation ? (int)Roles.Client : (int)Roles.Admin;

					//user.PricingPlanId = (int)PricingPlans.Basic;
					user.SiteProfileId = int.Parse(SystemConfig.DefaultSiteProfileId);
					user.RegisteredOn = DateTime.Now;
					user.InvitationId = inv.InvitationId;

					// Get parent users catalog where parent user is at least a plugger and assign to new user in client role
					var catalogs = DataSession.All<UserCatalogRole>()
						.Where(x => x.UserId == inv.InvitedByUserId && x.RoleId <= (int)Roles.Admin);
					catalogs.ForEach(c =>
						user.UserCatalogRoles.Add(new UserCatalogRole() { CatalogId = c.CatalogId, RoleId = (int)Roles.Client })
					);

					if (!inv.IsPlanInvitation) {
						//Start a new Subscription
						var subscription = new Subscription() {
							SubscriptionStartDate = DateTime.Now,
							SubscriptionEndDate = null,
							PricingPlanId = pricingPlan.PricingPlanId,
							PlanCharge = pricingPlan.PlanCharge.GetValueOrDefault()
						};

						user.Subscriptions.Add(subscription);
					}

					inv.InvitationStatus = (int)InvitationStatusCodes.Registered;

					DataSession.Add<User>(user);
					DataSession.CommitChanges();

					//move under inviter's plan, or under user's own new plan
					user.PlanUserId = inv.IsPlanInvitation ? invUser.PlanUserId : user.UserId;
					
					DataSession.CommitChanges();

					//user.PlanUserId = inv.IsPlanInvitation ? invUser.PlanUserId : user.UserId;
					//DataSession.CommitChanges();

		
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
		public bool UpdateProfile(User user, IList<Contact> contacts) {

			var dbuser = GetUser(user);
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
						DataSession.Add<Contact>(dbContact);
						dbuser.Contacts.Add(dbContact);
					}
				}
			}
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

			if (!String.IsNullOrEmpty(newPassword)) {
				if (PasswordHashMatches(dbuser.Password, user.Password)) {
					dbuser.Password = newPassword.PasswordHashString();
				} else {
					throw new ArgumentException("Passwords do not match");
				}
			} else {
				throw new ArgumentNullException("New password cannot be blank");
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
		//private void CreateUser(User user) {

		//    user.Password = user.Password.PasswordHashString();
		//    user.ParentUserId = user.ParentUserId.HasValue ? user.ParentUserId.Value : 1;
		//    user.RoleId = (int) Roles.Client;
		//    user.SiteProfileId = int.Parse(Settings.DefaultSiteProfileId.Value());
		//    user.ShowDebugInfo = false;
		//    user.AppendSignatureToTitle = false;

		//    DataSession.Add<User>(user);

		//    user = null;
		//}

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