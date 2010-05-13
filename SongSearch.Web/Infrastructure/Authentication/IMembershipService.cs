using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using SongSearch.Web.Models;

namespace SongSearch.Web.Services {

	public interface IMembershipService {
		int MinPasswordLength { get; }

		bool ValidateUser(string userName, string password);
		bool RegisterUser(RegisterModel model);
		bool UpdateProfile(UpdateProfileModel model);
		bool ResetPasswordProcessRequest(ResetPasswordModel model);
		bool ResetPassword(ResetPasswordModel model);
	}
}