using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using SongSearch.Web.Services;
using SongSearch.Web.Data;

// see if this shows up, yup it does!
namespace SongSearch.Web.Controllers {

	[HandleError]
	public partial class AccountController : Controller {

		IFormsAuthenticationService _authService;
		IUserManagementService _usrMgmtService;
		IAccountService _acctService;

		protected override void Initialize(RequestContext requestContext) {
			try {
				if (!String.IsNullOrWhiteSpace(requestContext.HttpContext.User.Identity.Name)) {
					_usrMgmtService.ActiveUserName = requestContext.HttpContext.User.Identity.Name;					
				}
			}
			catch { }
			base.Initialize(requestContext);

		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext) {
			ViewData["PasswordLength"] = AccountService.MinPasswordLength;

			base.OnActionExecuting(filterContext);
		}

		public AccountController(
			IFormsAuthenticationService authService,
			IUserManagementService usrMgmtService,
			IAccountService acctService
			) {
			_authService = authService;
			_usrMgmtService = usrMgmtService;
			_acctService = acctService;
		}

		// **************************************
		// URL: /Account/LogIn
		// **************************************
		[ValidateOnlyIncomingValues]
		public virtual ActionResult LogIn(LogOnModel model) {
			model.NavigationLocation = new string[] { "Home", "Login" };
			model.RememberMe = true;
			
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public virtual ActionResult LogIn(LogOnModel model, string returnUrl) {

			model.Email = model.Email.Trim();

			if (ModelState.IsValid) {
				if (_acctService.UserIsValid(model.Email, model.Password))
				//if (_ms.UserIsValid(contentModel.Email, contentModel.Password))
		{

					_authService.SignIn(model.Email, model.RememberMe);
					//_usrMgmtService.ActiveUserName = contentModel.Email;
					
					SessionService.Session().InitializeSession(model.Email, true);

					var user = Account.User(model.Email);

					SetFriendlyNameCookie(user.FullName());

					var msg = user.LoginMessage();// string.Concat("Welcome ", user.FullName());

					//if (CacheService.Session("ActiveCartMessageShown") == null) {
					//    var cart = CacheService.MyActiveCart(user.UserName);
					//    var activeItems = cart != null && cart.Contents != null ? cart.Contents.Count : 0;
					//    msg = activeItems > 0 ? String.Concat(msg, String.Format(". You have <strong>{0}</strong> {1} waiting in your song cart.", activeItems, activeItems > 1 ? "items" : "item")) : msg;
						
					//    CacheService.SessionUpdate("1", "ActiveCartMessageShown");
					//}
					this.FeedbackInfo(msg);

					if (!String.IsNullOrEmpty(returnUrl)) {
						return Redirect(returnUrl);
					} else {
						return RedirectToAction(MVC.Home.Index());
					}
				} else {
					ModelState.AddModelError("", SystemErrors.LoginFailed);
					this.FeedbackError("There was an error logging you in...");
				}

			}

			//something failed
			ModelState.AddModelError("", SystemErrors.LoginFailed);

			model.NavigationLocation = new string[] { "Home", "Login" };

			ViewData["PasswordLength"] = AccountService.MinPasswordLength;

			return View(model);
		}

		
		private void SetFriendlyNameCookie(string friendly) {
			Response.Cookies["friendly"].Value = friendly;
			Response.Cookies["friendly"].Expires = DateTime.Now.AddDays(30);
			Response.Cookies["friendly"].HttpOnly = true;
		}



		// **************************************
		// URL: /Account/LogOut
		// **************************************
		[RequireAuthorization]
		public virtual ActionResult LogOut() {

			_authService.SignOut();

			Session.Abandon();
			this.FeedbackInfo("Goodbye!");
			return RedirectToAction(Actions.LogIn());
		}


		// **************************************
		// URL: /Account/Register
		// **************************************        
		public virtual ActionResult Register(string id, string em) {

			ViewData["PasswordLength"] = AccountService.MinPasswordLength;
			ViewData["inviteId"] = id;
			ViewData["email"] = em;


			_authService.SignOut();

			Session.Abandon();

			Invitation inv = null;
			if (!String.IsNullOrWhiteSpace(id) && !String.IsNullOrWhiteSpace(em)) {
				try {
					inv = _usrMgmtService.GetInvitation(id, em);
					if (inv == null) {
						ModelState.AddModelError("InviteId", SystemErrors.InviteCodeNoMatch);
					}
				}
				catch {

					ModelState.AddModelError("InviteId", SystemErrors.InviteCodeNoMatch);
				}
			}
			return View(new RegisterModel() {

				NavigationLocation = new string[] { "Register" },
				InviteId = id,
				Email = em,
				SelectedPricingPlan = PricingPlans.Introductory,
				Invitation = inv
			});
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public virtual ActionResult Register(RegisterModel model) {

			//set username to email address
			//contentModel.UserName = contentModel.Email;

			if (ModelState.IsValid) {
				if (_acctService.UserExists(model.Email)) {
					ModelState.AddModelError("Email", SystemErrors.UserAlreadyRegistered);
				} else if (!model.HasAgreedToPrivacyPolicy){
					ModelState.AddModelError("HasAgreedToPrivacyPolicy", "We''re sorry, we can only register you if you accept our Terms of Use.");
				} else {
					// Check invitation code
					Invitation inv = null;
					try {
						inv = _usrMgmtService.GetInvitation(model.InviteId, model.Email);
					}
					catch {

						ModelState.AddModelError("InviteId", SystemErrors.InviteCodeNoMatch);

					}

					if (inv != null) {
						switch (inv.InvitationStatus) {
							case (int)InvitationStatusCodes.Open: {
									model.Invitation = inv;

									// Attempt to register the myUser
									//MembershipCreateStatus createStatus = _ms.CreateUser(contentModel.Email, contentModel.Password, contentModel.Email);
									if (String.IsNullOrEmpty(model.Email)) throw new ArgumentException("Value cannot be null or empty.", "Email");
									if (String.IsNullOrEmpty(model.Password)) throw new ArgumentException("Value cannot be null or empty.", "Password");
									if (String.IsNullOrEmpty(model.InviteId)) throw new ArgumentException("Value cannot be null or empty.", "InviteId");

									var selectedPricingPlanId = (int)model.SelectedPricingPlan;

								User user = new User() {
										UserName = model.Email,
										Password = model.Password,
										FirstName = model.FirstName,
										LastName = model.LastName,
										ParentUserId = model.Invitation.InvitedByUserId,
										PricingPlanId = selectedPricingPlanId > 0 ? selectedPricingPlanId : (int)PricingPlans.Member,
										HasAgreedToPrivacyPolicy = model.HasAgreedToPrivacyPolicy,
										HasAllowedCommunication = model.HasAllowedCommunication
									};

									try {
										user = _acctService.RegisterUser(user, inv.InvitationId);

										SetFriendlyNameCookie(user.FullName());
										_authService.SignIn(user.UserName, true /* createPersistentCookie */);

										return RedirectToAction(MVC.Home.Index());
									}
									catch (Exception ex){
										App.Logger.Error(ex);
										ModelState.AddModelError("Email", SystemErrors.UserCreationFailed);//AccountValidation.ErrorCodeToString(createStatus));
									}

									break;
								}
							case (int)InvitationStatusCodes.Registered: {
								ModelState.AddModelError("InviteId", SystemErrors.InviteCodeAlreadyUsed);
									break;
								}

							case (int)InvitationStatusCodes.Expired: {
								ModelState.AddModelError("InviteId", SystemErrors.InviteCodeExpired);
									break;
								}
						}
					} else {
						ModelState.AddModelError("InviteId", SystemErrors.InviteCodeNoMatch);
					}
				}

			}

			// If we got this far, something failed, redisplay form
			model.NavigationLocation = new string[] { "Register" };
			this.FeedbackError("There was an error registering you...");
			ViewData["PasswordLength"] = AccountService.MinPasswordLength;

			return View(model);
		}
		// **************************************
		// URL: /Account/ChangePassword
		// **************************************
		[RequireAuthorization]
		public virtual ActionResult ChangePassword() {
			ViewData["PasswordLength"] = AccountService.MinPasswordLength;

			return View(new UpdateProfileModel() { NavigationLocation = new string[] { "Home", "ChangePassword" } });
		}

		[RequireAuthorization]
		[HttpPost]
		[ValidateOnlyIncomingValues]
		[ValidateAntiForgeryToken]
		public virtual ActionResult ChangePassword(UpdateProfileModel model) {

			if (ModelState.IsValid) {
				var userName = this.UserName();
				var user = new User() { UserName = userName, Password = model.OldPassword };
				if (_acctService.ChangePassword(user, model.NewPassword)) {
					//_acctService.UpdateCurrentUserInSession();
					return RedirectToAction(Actions.ChangePasswordSuccess());
				} else {
					ModelState.AddModelError("", SystemErrors.PasswordChangeFailed);
				}
			}

			// If we got this far, something failed, redisplay form
			ViewData["PasswordLength"] = AccountService.MinPasswordLength;
			this.FeedbackError("There was an error changing your password...");

			model.NavigationLocation = new string[] { "Home", "ChangePassword" };
			return View(model);
		}

		// **************************************
		// URL: /Account/ChangePasswordSuccess
		// **************************************        
		[RequireAuthorization]
		public virtual ActionResult ChangePasswordSuccess() {
			string email = this.UserName();

			try {
				Mail.SendMail(
					SiteProfileData.SiteProfile().AdminEmail,
					email,
					SystemMessages.PasswordChangeSuccessSubjectLine,
					SystemMessages.PasswordChangeSuccess
					);
			}
			catch { }
			return View(new UpdateProfileModel() { NavigationLocation = new string[] { "Home", "ChangePassword" } });
		}


		// **************************************
		// URL: /Account/UpdateProfile
		// **************************************
		[RequireAuthorization]
		public virtual ActionResult UpdateProfile() {

			try {
				ViewData["PasswordLength"] = AccountService.MinPasswordLength;

				var user = Account.User();// SessionService.Session().User(User.Identity.Name);
				var vm = new UpdateProfileModel() {
					NavigationLocation = new string[] { "Home", "Profile" },
					Email = this.UserName(),
					FirstName = user.FirstName,
					LastName = user.LastName,
					ShowSignatureField = user.IsAtLeastInCatalogRole(Roles.Plugger),
					ShowContactInfo = user.IsAtLeastInCatalogRole(Roles.Admin),
					HasAllowedCommunication = user.HasAllowedCommunication,
					PageTitle = "Update Profile",
					PageMessage= "My User Profile"

				};
				if (vm.ShowSignatureField){
					vm.AppendSignatureToTitle = user.AppendSignatureToTitle;
					vm.Signature = user.Signature;
				}
				if (vm.ShowContactInfo){
					vm.Contact = user.GetContactInfo(false);
				}

				return View(vm);
			}
			catch {
				this.FeedbackError("There was an error loading the Update Profile page. Please try again in a bit.");
				return RedirectToAction(MVC.Home.Index());
			}
		}

		[RequireAuthorization]
		[HttpPost]
		[ValidateOnlyIncomingValues]
		[ValidateAntiForgeryToken]
		public virtual ActionResult UpdateProfile(UpdateProfileModel model) {

			var userModel = new User() {
				UserName = this.UserName(),
				FirstName = model.FirstName,
				LastName = model.LastName,
				Signature = model.Signature,
				AppendSignatureToTitle = model.AppendSignatureToTitle,
				HasAllowedCommunication = model.HasAllowedCommunication
			};

			var contact = model.Contact;

			//var session = SessionService.Session();
			var currentUser = Account.User();//session.User(User.Identity.Name);
			model.ShowSignatureField = currentUser.IsAtLeastInCatalogRole(Roles.Plugger);
			model.ShowContactInfo = currentUser.IsAtLeastInCatalogRole(Roles.Admin);
	
			//update the user's profile in the database
			if (_acctService.UpdateProfile(userModel, contact)) {
					
				// UpdateModelWith the user dataSession cached in dataSession
				SessionService.Session().InitializeSession(true);
				CacheService.CacheUpdate(CacheService.CacheKeys.SiteProfile);

				var friendly = userModel.FullName();
				SetFriendlyNameCookie(friendly);

				
				this.FeedbackInfo("Successfully updated your profile");

			} else {

				//model.ShowSignatureField = user.IsAtLeastInCatalogRole(Roles.Plugger);

				ModelState.AddModelError("",
					SystemErrors.PasswordChangeFailed);
				this.FeedbackError("There was an error updating your profile");

			}
		
			// If we got this far, something failed, redisplay form
			ViewData["PasswordLength"] =
				AccountService.MinPasswordLength;
			model.NavigationLocation = new string[] { "Home", "Profile" };
			model.PageTitle = "Update Profile";
			model.PageMessage = "My User Profile";
			return View(model);
		}

		//// **************************************
		//// URL: /Account/UpdateProfileSuccess
		//// **************************************        
		//[RequireAuthorization]
		//public ActionResult UpdateProfileSuccess() {
		//    //string email = User.Identity.Name;

		//    //try
		//    //{
		//    //    _cas.SendMail(
		//    //        Settings.AdminEmailAddress.Text(), 
		//    //        email, 
		//    //        Messages.PasswordChangeSuccessSubjectLine.Text(), 
		//    //        Messages.PasswordChangeSuccess.Text()
		//    //        );
		//    //}
		//    //catch { }
		//    UpdateProfileModel vm = new UpdateProfileModel() { NavigationLocation = "Account" };
		//    return View(vm);
		//}


		// **************************************
		// URL: /Account/ResetPassword
		// **************************************        
		public virtual ActionResult ResetPassword() {
			ViewData["PasswordLength"] = AccountService.MinPasswordLength;

			return View(new ResetPasswordModel() {
				NavigationLocation = new string[] { "Home", "ResetPassword" }
			});
		}

		[HttpPost]
		[ValidateOnlyIncomingValues]
		[ValidateAntiForgeryToken]
		public virtual ActionResult ResetPassword(ResetPasswordModel model) {

			if (ModelState.IsValid) {
				model.ResetCode = model.Email.PasswordHashString();

				//Send email

				string link = String.Format(@"visit our <a href='{0}/Account/ResetPasswordRespond/{1}?rc={2}'>Password Reset page</a>",
					SystemConfig.BaseUrl,
					model.Email,
					model.ResetCode);
				string msg = String.Format(SystemMessages.PasswordResetRequestLink, link);

				Mail.SendMail(
					SiteProfileData.SiteProfile().AdminEmail,
					model.Email,
					SystemMessages.PasswordResetRequestSubjectLine,
					String.Format("{0} {1}", String.Format(SystemMessages.PasswordResetRequest, SiteProfileData.SiteProfile().CompanyName), msg)

					);
				return RedirectToAction(Actions.ResetPasswordSuccess());
			}
			ViewData["PasswordLength"] = AccountService.MinPasswordLength;
			model.NavigationLocation = new string[] { "Home", "ResetPassword" };
			this.FeedbackError("There was an error processing your password reset request...");

			return View(model);
		}

		// **************************************
		// URL: /Account/ResetPasswordSuccess
		// **************************************        
		public virtual ActionResult ResetPasswordSuccess() {
			return View(new ResetPasswordModel() { NavigationLocation = new string[] { "Home", "ResetPassword" } });
		}

		// **************************************
		// URL: /Account/ResetPasswordRespond
		// **************************************        
		public virtual ActionResult ResetPasswordRespond(string id, string rc) {
			var model = new ResetPasswordModel() {
				Email = id,
				ResetCode = rc
			};

			ViewData["PasswordLength"] = AccountService.MinPasswordLength;
			model.NavigationLocation = new string[] { "Home", "ResetPassword" };

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public virtual ActionResult ResetPasswordRespond(ResetPasswordModel model) {
			if (
				ModelState.IsValid &&
				_acctService.ResetPassword(model.Email, model.ResetCode, model.NewPassword)
				) {

				return RedirectToAction(Actions.LogIn());

			} else {
				ModelState.AddModelError("", SystemErrors.PasswordResetFailed);

				ViewData["PasswordLength"] = AccountService.MinPasswordLength;
				model.NavigationLocation = new string[] { "Home", "ResetPassword" };
				this.FeedbackError("There was an error processing your password reset request...");

				return View(model);
			}

		}

		// **************************************
		// URL: /Account/UpdateProfile
		// **************************************
		[RequireAuthorization]
		[RequiresVersion(MinAppVersion=AppVersion.SongSearch_2_1)]
		public virtual ActionResult Plan() {
			var vm = new PricingPlansViewModel() {
				NavigationLocation = new string[] { "Home", "Plan" },
				PageTitle = "My Plan",
				MyPricingPlan = Account.User().PricingPlan,				
				MyUserQuotas = Account.User().MyQuotas()

			};
			vm.PricingPlans = User.UserIsSuperAdmin() ? vm.PricingPlans.Where(p => p.PricingPlanId > (int)PricingPlans.Member).ToList() :
				vm.PricingPlans.Where(p => p.IsEnabled == true && p.PlanCharge >= vm.MyPricingPlan.PlanCharge).OrderByDescending(p => p.IsPromo).ToList();

			vm.SelectedPricingPlan = (PricingPlans)vm.MyPricingPlan.PricingPlanId;

			return View(vm);
		}

	}
}
