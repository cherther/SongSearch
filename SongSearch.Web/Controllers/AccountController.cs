using System;
using System.Web.Mvc;
using System.Web.Routing;
using SongSearch.Web.Services;
using SongSearch.Web.Data;

// see if this shows up, yup it does!
namespace SongSearch.Web.Controllers {

	[HandleError]
	public class AccountController : Controller {

		IFormsAuthenticationService _authService;
		IUserManagementService _usrMgmtService;
		IAccountService _acctService;

		protected override void Initialize(RequestContext requestContext) {
			
			if (!String.IsNullOrWhiteSpace(requestContext.HttpContext.User.Identity.Name)) {
				_usrMgmtService.ActiveUserName = requestContext.HttpContext.User.Identity.Name;
			}
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
		public ActionResult LogIn(LogOnModel model) {
			model.NavigationLocation = "Account";
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogIn(LogOnModel model, string returnUrl) {

			if (ModelState.IsValid) {
				if (_acctService.UserIsValid(model.Email, model.Password))
				//if (_ms.UserIsValid(contentModel.Email, contentModel.Password))
		{

					_authService.SignIn(model.Email, model.RememberMe);
					//_usrMgmtService.ActiveUserName = contentModel.Email;
					CacheService.InitializeSession(model.Email, true);
					var user = CacheService.User(model.Email);

					SetFriendlyNameCookie(user.FullName());

					var msg = user.LoginMessage();// string.Concat("Welcome ", user.FullName());

					//if (CacheService.Session("ActiveCartMessageShown") == null) {
					//    var cart = CacheService.MyActiveCart(user.UserName);
					//    var activeItems = cart != null && cart.Contents != null ? cart.Contents.Count : 0;
					//    msg = activeItems > 0 ? String.Concat(msg, String.Format(". You have <strong>{0}</strong> {1} waiting in your song cart.", activeItems, activeItems > 1 ? "items" : "item")) : msg;
						
					//    CacheService.SessionUpdate("1", "ActiveCartMessageShown");
					//}
					this.FireInfo(msg);

					if (!String.IsNullOrEmpty(returnUrl)) {
						return Redirect(returnUrl);
					} else {
						return RedirectToAction("Index", "Home");
					}
				} else {
					ModelState.AddModelError("", Errors.LoginFailed.Text());
					this.FireError("There was an error logging you in...");
				}

			}

			//something failed
			ModelState.AddModelError("", Errors.LoginFailed.Text());

			model.NavigationLocation = "Account";

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
		public ActionResult LogOut() {

			_authService.SignOut();

			Session.Abandon();

			return RedirectToAction("LogIn", "Account");
		}


		// **************************************
		// URL: /Account/Register
		// **************************************        
		public ActionResult Register(string id, string em) {
			ViewData["PasswordLength"] = AccountService.MinPasswordLength;
			ViewData["inviteId"] = id;
			ViewData["email"] = em;


			_authService.SignOut();

			Session.Abandon();

			return View(new RegisterModel() {

				NavigationLocation = "Register",
				InviteId = id,
				Email = em

			});
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Register(RegisterModel model) {

			//set username to email address
			//contentModel.UserName = contentModel.Email;

			if (ModelState.IsValid) {
				if (_acctService.UserExists(model.Email)) {
					ModelState.AddModelError("Email", Errors.UserAlreadyRegistered.Text());
				} else {

					// Check invitation code
					var inv = _usrMgmtService.GetInvitation(model.InviteId, model.Email);

					if (inv != null) {
						switch (inv.InvitationStatus) {
							case (int)InvitationStatusCodes.Open: {
									model.Invitation = inv;

									// Attempt to register the myUser
									//MembershipCreateStatus createStatus = _ms.CreateUser(contentModel.Email, contentModel.Password, contentModel.Email);
									if (String.IsNullOrEmpty(model.Email)) throw new ArgumentException("Value cannot be null or empty.", "Email");
									if (String.IsNullOrEmpty(model.Password)) throw new ArgumentException("Value cannot be null or empty.", "Password");
									if (String.IsNullOrEmpty(model.InviteId)) throw new ArgumentException("Value cannot be null or empty.", "InviteId");

									User user = new User() {
										UserName = model.Email,
										Password = model.Password,
										FirstName = model.FirstName,
										LastName = model.LastName,
										ParentUserId = model.Invitation.InvitedByUserId
									};

									try {
										user = _acctService.RegisterUser(user, inv.InvitationId);

										_authService.SignIn(user.UserName, false /* createPersistentCookie */);

										return RedirectToAction("Index", "Home");
									}
									catch {
										ModelState.AddModelError("Email", Errors.UserCreationFailed.Text());//AccountValidation.ErrorCodeToString(createStatus));
									}

									break;
								}
							case (int)InvitationStatusCodes.Registered: {
									ModelState.AddModelError("InviteId", Errors.InviteCodeAlreadyUsed.Text());
									break;
								}

							case (int)InvitationStatusCodes.Expired: {
									ModelState.AddModelError("InviteId", Errors.InviteCodeExpired.Text());
									break;
								}
						}
					} else {
						ModelState.AddModelError("InviteId", Errors.InviteCodeNoMatch.Text());
					}
				}

			}

			// If we got this far, something failed, redisplay form
			model.NavigationLocation = "Register";
			this.FireError("There was an error registering you...");
			ViewData["PasswordLength"] = AccountService.MinPasswordLength;

			return View(model);
		}
		// **************************************
		// URL: /Account/ChangePassword
		// **************************************
		[RequireAuthorization]
		public ActionResult ChangePassword() {
			ViewData["PasswordLength"] = AccountService.MinPasswordLength;

			return View(new UpdateProfileModel() { NavigationLocation = "Account" });
		}

		[RequireAuthorization]
		[HttpPost]
		[ValidateOnlyIncomingValues]
		[ValidateAntiForgeryToken]
		public ActionResult ChangePassword(UpdateProfileModel model) {

			if (ModelState.IsValid) {
				var userName = User.Identity.Name;
				var user = new User() { UserName = userName, Password = model.OldPassword };
				if (_acctService.UpdateProfile(user, model.NewPassword)) {
					//_acctService.UpdateCurrentUserInSession();
					return RedirectToAction("ChangePasswordSuccess");
				} else {
					ModelState.AddModelError("", Errors.PasswordChangeFailed.Text());
				}
			}

			// If we got this far, something failed, redisplay form
			ViewData["PasswordLength"] = AccountService.MinPasswordLength;
			this.FireError("There was an error changing your password...");

			model.NavigationLocation = "Account";
			return View(model);
		}

		// **************************************
		// URL: /Account/ChangePasswordSuccess
		// **************************************        
		[RequireAuthorization]
		public ActionResult ChangePasswordSuccess() {
			string email = User.Identity.Name;

			try {
				Mail.SendMail(
					Settings.AdminEmailAddress.Text(),
					email,
					Messages.PasswordChangeSuccessSubjectLine.Text(),
					Messages.PasswordChangeSuccess.Text()
					);
			}
			catch { }
			return View(new UpdateProfileModel() { NavigationLocation = "Account" });
		}


		// **************************************
		// URL: /Account/UpdateProfile
		// **************************************
		[RequireAuthorization]
		public ActionResult UpdateProfile() {
			ViewData["PasswordLength"] = AccountService.MinPasswordLength;

			var user = CacheService.User(User.Identity.Name);

			var vm = new UpdateProfileModel() {
				NavigationLocation = "Account",
				Email = User.Identity.Name,
				FirstName = user.FirstName,
				LastName = user.LastName,
				ShowSignatureField = user.IsAnyAdmin(),
				Signature = user.Signature

			};

			return View(vm);
		}

		[RequireAuthorization]
		[HttpPost]
		[ValidateOnlyIncomingValues]
		[ValidateAntiForgeryToken]
		public ActionResult UpdateProfile(UpdateProfileModel model) {
			if (ModelState.IsValid) {
				var userName = User.Identity.Name;
				User user = new User() {
					UserName = userName,
					FirstName = model.FirstName,
					LastName = model.LastName,
					Signature = model.Signature
				};
				//update the user's profile in the database
				if (_acctService.UpdateProfile(user, null)) {
					
					// UpdateModelWith the user dataSession cached in dataSession
					CacheService.InitializeSession(userName, true);
					user = CacheService.User(User.Identity.Name);
					var friendly = user.FullName();
					SetFriendlyNameCookie(friendly);

					model.ShowSignatureField = user.IsAnyAdmin();
					this.FireInfo("Successfully updated your profile...");

					return View(model);

				} else {
					ModelState.AddModelError("",
						Errors.PasswordChangeFailed.Text());
				}
			}

			// If we got this far, something failed, redisplay form
			ViewData["PasswordLength"] =
				AccountService.MinPasswordLength;
			model.NavigationLocation = "Account";
			this.FireError("There was an error updating your profile...");

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
		public ActionResult ResetPassword() {
			ViewData["PasswordLength"] = AccountService.MinPasswordLength;

			return View(new ResetPasswordModel() {
				NavigationLocation = "Account"
			});
		}

		[HttpPost]
		[ValidateOnlyIncomingValues]
		[ValidateAntiForgeryToken]
		public ActionResult ResetPassword(ResetPasswordModel model) {

			if (ModelState.IsValid) {
				model.ResetCode = model.Email.PasswordHashString();

				//Send email

				string link = String.Format(@"<a href='{0}/Account/ResetPasswordRespond/{1}?rc={2}'>visit our Password Reset page</a>",
					Settings.BaseUrl.Text(),
					model.Email,
					model.ResetCode);
				string msg = String.Format(Messages.PasswordResetRequestLink.Text(), link);

				Mail.SendMail(
					Settings.AdminEmailAddress.Text(),
					model.Email,
					Messages.PasswordResetRequestSubjectLine.Text(),
					String.Format("{0} {1}", Messages.PasswordResetRequest.Text(), msg)

					);
				return RedirectToAction("ResetPasswordSuccess");
			}
			ViewData["PasswordLength"] = AccountService.MinPasswordLength;
			model.NavigationLocation = "Account";
			this.FireError("There was an error processing your password reset request...");

			return View(model);
		}

		// **************************************
		// URL: /Account/ResetPasswordSuccess
		// **************************************        
		public ActionResult ResetPasswordSuccess() {
			return View();
		}

		// **************************************
		// URL: /Account/ResetPasswordRespond
		// **************************************        
		public ActionResult ResetPasswordRespond(string id, string rc) {
			var model = new ResetPasswordModel() {
				NavigationLocation = "Account",
				Email = id,
				ResetCode = rc
			};

			ViewData["PasswordLength"] = AccountService.MinPasswordLength;
			model.NavigationLocation = "Account";

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ResetPasswordRespond(ResetPasswordModel model) {
			if (
				ModelState.IsValid &&
				_acctService.ResetPassword(model.Email, model.ResetCode, model.NewPassword)
				) {

				return RedirectToAction("LogIn", "Account");

			} else {
				ModelState.AddModelError("", Errors.PasswordResetFailed.Text());

				ViewData["PasswordLength"] = AccountService.MinPasswordLength;
				model.NavigationLocation = "Account";
				this.FireError("There was an error processing your password reset request...");

				return View(model);
			}

		}

	}
}
