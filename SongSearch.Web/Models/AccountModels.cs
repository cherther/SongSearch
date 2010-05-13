using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SongSearch.Web.Data;
using SongSearch.Web.Services;

namespace SongSearch.Web.Models {

	#region Models
	// **************************************
	// ResetPasswordModel
	// **************************************
	[PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage = "The new password and confirmation password do not match.")]
	public class ResetPasswordModel : ViewModel {
		[Required]
		[DataType(DataType.EmailAddress)]
		[DisplayName("Email address")]
		public string Email { get; set; }

		[Required]
		[DisplayName("Reset Code")]
		public string ResetCode { get; set; }

		[Required]
		[ValidatePasswordLength]
		[DataType(DataType.Password)]
		[DisplayName("New password")]
		public string NewPassword { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[DisplayName("Confirm new password")]
		public string ConfirmPassword { get; set; }


	}
	// **************************************
	// LogOnModel
	// **************************************
	public class LogOnModel : ViewModel {
		//[Required]
		//[DisplayName("User name")]
		//public string UserName { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[DisplayName("Email address")]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[DisplayName("Password")]
		public string Password { get; set; }

		[DisplayName("Remember me?")]
		public bool RememberMe { get; set; }

		public string ReturnUrl { get; set; }


	}

	// **************************************
	// RegisterModel
	// **************************************
	[PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "The password and confirmation password do not match.")]
	public class RegisterModel : ViewModel {
		//[Required]
		//[DisplayName("User name")]
		//public string UserName { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[DisplayName("Email address")]
		public string Email { get; set; }

		[Required]
		[DisplayName("Invitation Code")]
		public string InviteId { get; set; }

		[DisplayName("First Name")]
		public string FirstName { get; set; }

		[DisplayName("Last Name")]
		public string LastName { get; set; }

		[Required]
		[ValidatePasswordLength]
		[DataType(DataType.Password)]
		[DisplayName("Password")]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[DisplayName("Confirm password")]
		public string ConfirmPassword { get; set; }

		public Invitation Invitation { get; set; }
	}

	// **************************************
	// UpdateProfileModel
	// **************************************
	[PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage = "The new password and confirmation password do not match.")]
	public class UpdateProfileModel : ViewModel {
		//private IPrincipal _princ;

		//public IPrincipal Principal {
		//    get {
		//        return _princ;
		//    }
		//    set {
		//        _princ = value;
		//        SetUserFields(_princ);
		//    }
		//}

		[DisplayName("First Name")]
		public string FirstName { get; set; }

		[DisplayName("Last Name")]
		public string LastName { get; set; }

		[DisplayName("Download Signature (default text appended to the name of your song files)")]
		[RegularExpression(@"[^\\/:*?""<>|]*", ErrorMessage = @"Signatures may not contain the characters \ /:*?""<>|")]
		public string Signature { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[DisplayName("Current password")]
		public string OldPassword { get; set; }

		[Required]
		[ValidatePasswordLength]
		[DataType(DataType.Password)]
		[DisplayName("New password")]
		public string NewPassword { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[DisplayName("Confirm new password")]
		public string ConfirmPassword { get; set; }

		public string Email { get; set; }

	}

	#endregion

	#region Validation

	// **************************************
	// ValidateOnlyIncomingValuesAttribute
	// **************************************
	public class ValidateOnlyIncomingValuesAttribute : ActionFilterAttribute {
		public override void OnActionExecuting(ActionExecutingContext filterContext) {
			var modelState = filterContext.Controller.ViewData.ModelState;
			var valueProvider = filterContext.Controller.ValueProvider;

			var keysWithNoIncomingValue = modelState.Keys.Where(x => !valueProvider.ContainsPrefix(x));
			foreach (var key in keysWithNoIncomingValue)
				modelState[key].Errors.Clear();
		}
	}

	// **************************************
	// AccountValidation
	// **************************************
	public static class AccountValidation {
		public static string ErrorCodeToString(MembershipCreateStatus createStatus) {
			// See http://go.microsoft.com/fwlink/?LinkID=177550 for
			// a full list of status codes.
			switch (createStatus) {
				case MembershipCreateStatus.DuplicateUserName:
					return "Username already exists. Please enter a different myUser name.";

				case MembershipCreateStatus.DuplicateEmail:
					return "A username for that e-mail address already exists. Please enter a different e-mail address.";

				case MembershipCreateStatus.InvalidPassword:
					return "The password provided is invalid. Please enter a valid password value.";

				case MembershipCreateStatus.InvalidEmail:
					return "The e-mail address provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidAnswer:
					return "The password retrieval answer provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidQuestion:
					return "The password retrieval question provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidUserName:
					return "The myUser name provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.ProviderError:
					return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				case MembershipCreateStatus.UserRejected:
					return "The myUser creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				default:
					return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
			}
		}
	}

	// **************************************
	// PropertiesMustMatchAttribute
	// **************************************
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public sealed class PropertiesMustMatchAttribute : ValidationAttribute {
		private const string _defaultErrorMessage = "'{0}' and '{1}' do not match.";
		private readonly object _typeId = new object();

		public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
			: base(_defaultErrorMessage) {
			OriginalProperty = originalProperty;
			ConfirmProperty = confirmProperty;
		}

		public string ConfirmProperty { get; private set; }
		public string OriginalProperty { get; private set; }

		public override object TypeId {
			get {
				return _typeId;
			}
		}

		public override string FormatErrorMessage(string name) {
			return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
				OriginalProperty, ConfirmProperty);
		}

		public override bool IsValid(object value) {
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
			object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
			object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
			return Object.Equals(originalValue, confirmValue);
		}
	}

	// **************************************
	// ValidatePasswordLengthAttribute
	// **************************************
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class ValidatePasswordLengthAttribute : ValidationAttribute {
		private const string _defaultErrorMessage = "'{0}' must be at least {1} characters long.";
		private readonly int _minCharacters = AccountService.MinPasswordLength;

		public ValidatePasswordLengthAttribute()
			: base(_defaultErrorMessage) {
		}

		public override string FormatErrorMessage(string name) {
			return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
				name, _minCharacters);
		}

		public override bool IsValid(object value) {
			string valueAsString = value as string;
			return (valueAsString != null && valueAsString.Length >= _minCharacters);
		}
	}

	#endregion
}