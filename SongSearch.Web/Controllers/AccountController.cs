﻿using System;
using System.Web.Mvc;
using System.Web.Routing;
using SongSearch.Web.Services;
using SongSearch.Web.Models;

// see if this shows up, yup it does!
namespace SongSearch.Web.Controllers {

	/// <summary>
	/// 
	/// </summary>
	[HandleError]
	public class AccountController : Controller {

		IFormsAuthenticationService _fs;
		IMembershipService _ms;
		IUserManagementService _ums;
		IAccountService _accs;

		protected override void Initialize(RequestContext requestContext) {
			if (_fs == null) { _fs = new FormsAuthenticationService(); }
			if (_ms == null) { _ms = new MembershipService(); }
			if (_ums == null) { _ums = new UserManagementService(requestContext.HttpContext.User.Identity.Name); }
			if (_accs == null) { _accs = new AccountService(); }

			base.Initialize(requestContext);

		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext) {
			ViewData["PasswordLength"] = _ms.MinPasswordLength;

			base.OnActionExecuting(filterContext);
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
		public ActionResult LogIn(LogOnModel model, string returnUrl) {

			if (ModelState.IsValid) {
				if (_accs.UserIsValid(model.Email, model.Password))
				//if (_ms.UserIsValid(model.Email, model.Password))
				{

					_fs.SignIn(model.Email, model.RememberMe);


					if (!String.IsNullOrEmpty(returnUrl)) {
						return Redirect(returnUrl);
					} else {
						return RedirectToAction("Index", "Home");
					}
				} else {
					ModelState.AddModelError("", Errors.LoginFailed.Text());
				}

			}

			//something failed
			ModelState.AddModelError("", Errors.LoginFailed.Text());

			model.NavigationLocation = "Account";

			ViewData["PasswordLength"] = _ms.MinPasswordLength;

			return View(model);
		}



		// **************************************
		// URL: /Account/LogOut
		// **************************************
		public ActionResult LogOut() {

			_fs.SignOut();
			
			Session.Abandon();

			return RedirectToAction("LogIn", "Account");
		}


		// **************************************
		// URL: /Account/Register
		// **************************************        
		public ActionResult Register(string id, string em) {
			ViewData["PasswordLength"] = _ms.MinPasswordLength;
			ViewData["inviteId"] = id;
			ViewData["email"] = em;


			_fs.SignOut();

			Session.Abandon();

			var vm = new RegisterModel() {

				NavigationLocation = "Register",
				InviteId = id,
				Email = em

			};
			return View(vm);
		}

		[HttpPost]
		public ActionResult Register(RegisterModel model) {

			//set username to email address
			//model.UserName = model.Email;

			if (ModelState.IsValid) {
				if (_accs.UserExists(model.Email)) {
					ModelState.AddModelError("Email", Errors.UserAlreadyRegistered.Text());
				} else {

					// Check invitation code
					var inv = _ums.GetInvitation(model.InviteId, model.Email);

					if (inv != null) {
						switch (inv.InvitationStatus) {
							case (int)InvitationStatusCodes.Open: {
									model.Invitation = inv;

									// Attempt to register the myUser
									//MembershipCreateStatus createStatus = _ms.CreateUser(model.Email, model.Password, model.Email);

									if (_ms.RegisterUser(model))//MembershipCreateStatus.Success)
									{
										_fs.SignIn(model.Email, false /* createPersistentCookie */);
										
										return RedirectToAction("Index", "Home");
									} else {   //TODO: createStatus enums
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

			ViewData["PasswordLength"] = _ms.MinPasswordLength;

			return View(model);
		}
		// **************************************
		// URL: /Account/ChangePassword
		// **************************************
		[RequireMinRole]
		public ActionResult ChangePassword() {
			ViewData["PasswordLength"] = _ms.MinPasswordLength;

			var vm = new UpdateProfileModel() { NavigationLocation = "Account" };

			return View(vm);
		}

		[RequireMinRole]
		[HttpPost]
		[ValidateOnlyIncomingValues]
		public ActionResult ChangePassword(UpdateProfileModel model) {

			if (ModelState.IsValid) {
				if (_ms.UpdateProfile(model)) {
					//_accs.UpdateCurrentUserInSession();
					return RedirectToAction("ChangePasswordSuccess");
				} else {
					ModelState.AddModelError("", Errors.PasswordChangeFailed.Text());
				}
			}

			// If we got this far, something failed, redisplay form
			ViewData["PasswordLength"] = _ms.MinPasswordLength;
			model.NavigationLocation = "Account";
			return View(model);
		}

		// **************************************
		// URL: /Account/ChangePasswordSuccess
		// **************************************        
		[RequireMinRole]
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
			var vm = new UpdateProfileModel() { NavigationLocation = "Account" };
			return View(vm);
		}


		// **************************************
		// URL: /Account/UpdateProfile
		// **************************************
		[RequireMinRole]
		public ActionResult UpdateProfile() {
			ViewData["PasswordLength"] = _ms.MinPasswordLength;

			var vm = new UpdateProfileModel() { NavigationLocation = "Account" };

			return View(vm);
		}

		[RequireMinRole]
		[HttpPost]
		[ValidateOnlyIncomingValues]
		public ActionResult UpdateProfile(UpdateProfileModel model) {
			if (ModelState.IsValid) {
				if (_ms.UpdateProfile(model)) {
					//UserData.UpdateSession();
					return RedirectToAction("UpdateProfileSuccess");
				} else {
					ModelState.AddModelError("", Errors.PasswordChangeFailed.Text());
				}
			}

			// If we got this far, something failed, redisplay form
			ViewData["PasswordLength"] = _ms.MinPasswordLength;
			model.NavigationLocation = "Account";
			return View(model);
		}

		// **************************************
		// URL: /Account/UpdateProfileSuccess
		// **************************************        
		[RequireMinRole]
		public ActionResult UpdateProfileSuccess() {
			//string email = User.Identity.Name;

			//try
			//{
			//    _cas.SendMail(
			//        Settings.AdminEmailAddress.Text(), 
			//        email, 
			//        Messages.PasswordChangeSuccessSubjectLine.Text(), 
			//        Messages.PasswordChangeSuccess.Text()
			//        );
			//}
			//catch { }
			UpdateProfileModel vm = new UpdateProfileModel() { NavigationLocation = "Account" };
			return View(vm);
		}


		// **************************************
		// URL: /Account/ResetPassword
		// **************************************        
		public ActionResult ResetPassword() {
			var model = new ResetPasswordModel() {
				NavigationLocation = "Account"
			};

			ViewData["PasswordLength"] = _ms.MinPasswordLength;

			return View(model);
		}

		[HttpPost]
		[ValidateOnlyIncomingValues]
		public ActionResult ResetPassword(ResetPasswordModel model) {

			if (ModelState.IsValid) {
				model.ResetCode = model.Email.PasswordHashString();


				//Send email
				if (_ms.ResetPasswordProcessRequest(model)) {
					try {
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
					}
					catch { }

					return RedirectToAction("ResetPasswordSuccess");
				} else {
					ModelState.AddModelError("", Errors.PasswordResetFailed.Text());
				}
			}
			ViewData["PasswordLength"] = _ms.MinPasswordLength;
			model.NavigationLocation = "Account";

			return View(model);
		}

		// **************************************
		// URL: /Account/ResetPasswordSuccess
		// **************************************        
		public ActionResult ResetPasswordSuccess() {
			return View();
		}

		public ActionResult ResetPasswordRespond(string id, string rc) {
			var model = new ResetPasswordModel() {
				NavigationLocation = "Account",
				Email = id,
				ResetCode = rc
			};

			ViewData["PasswordLength"] = _ms.MinPasswordLength;
			model.NavigationLocation = "Account";

			return View(model);
		}

		[HttpPost]
		public ActionResult ResetPasswordRespond(ResetPasswordModel model) {
			if (_ms.ResetPassword(model)) {
				return RedirectToAction("LogIn", "Account");
			} else {
				ModelState.AddModelError("", Errors.PasswordResetFailed.Text());
			}

			ViewData["PasswordLength"] = _ms.MinPasswordLength;
			model.NavigationLocation = "Account";

			return View(model);

		}



	}
}
