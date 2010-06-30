using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongSearch.Web;
using SongSearch.Web.Controllers;
using SongSearch.Web.Services;
using SongSearch.Web.Data;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;

namespace Codewerks.SongSearch.Web.Tests.Services {
	[TestClass]
	public class AccountServiceTests : TestBase {

		public AccountServiceTests() {

			_acc = Container.Get<AccountService>();

			_usr = Container.Get<UserManagementService>();
			_usr.ActiveUserName = _admin;
			
		}

		/*
		 void RegisterUser(User user);
		bool UserIsValid(string userName, string password);
		bool UserExists(string userName);
		bool UpdateProfile(User user, string newPassword);
		bool ResetPassword(string userName, string oldPassword, string newPassword);

		void UpdateSignature(string userName, string signature);
		 * */
		[TestMethod]
		public void Can_Register_User() {
			// Arrange
			var user = new User() {
				UserName = _dummyuser,
				FirstName = "New",
				LastName = "User",
				ParentUserId = _adminId,
				Password = _dummypw
			};
			

			// Act
			user = _acc.RegisterUser(user, _invitationCode);
			_dummyId = user.UserId;

			var isValid = _acc.UserIsValid(_dummyuser, _dummypw);
			// Assert
			Assert.IsTrue(isValid);
			user = null;
		}

		[TestMethod]
		public void Registered_User_Has_Invitation_Parent_User() {
			// Arrange
			if (_dummyId == 0) { Can_Register_User(); }
			var user = _usr.GetUserDetail(_dummyId);
			// Act
			var parentId = user.ParentUser.UserId;
			Assert.AreEqual(parentId, _adminId);
			user = null;
		}

		[TestMethod]
		public void Can_Validate_User() {
			// Arrange
			//var username = "claus_herther@yahoo.com";
			//var password = "go1974";
			if (_dummyId == 0) { Can_Register_User(); }

			// Act
			var isValid = _acc.UserIsValid(_dummyuser, _dummypw);
			
			// Assert
			Assert.IsTrue(isValid);
		}

		[TestMethod]
		public void Can_Check_If_User_Exists() {
			// Arrange
			if (_dummyId == 0) { Can_Register_User(); }

			// Act
			var exists = _acc.UserExists(_dummyuser);

			// Assert
			Assert.IsTrue(exists);
		}


		[TestMethod]
		public void User_Can_Update_Password() {
			// Arrange
			if (_dummyId == 0) { Can_Register_User(); }

			var user = new User() {
				UserName = _dummyuser,
				Password = _dummypw			
			};
			var newpassword = "1234567890";
			// Act
			_acc.ChangePassword(user, newpassword);

			var isvalid = _acc.UserIsValid(_dummyuser, newpassword);
			// Assert
			Assert.IsTrue(isvalid);

			//reset
			user.Password = newpassword;
			_acc.ChangePassword(user, _dummypw);
			user = null;

		}

		[TestMethod]
		public void User_Can_Update_Profile() {
			// Arrange
			if (_dummyId == 0) { Can_Register_User(); }

			var name = "Client " + DateTime.Now.ToLongTimeString();

			var user = new User() {
				UserName = _dummyuser,
				FirstName = "Test",
				LastName = name
			};
			// Act
			_acc.UpdateProfile(user);

			var dbuser = _usr.GetUserDetail(_dummyuser);
			// Assert
			Assert.AreEqual(name, dbuser.LastName);
			user = null;
			dbuser = null;
		}

		[TestMethod]
		public void Plugger_Can_Update_User_Signature() {
			// Arrange
			var signature = "Contact Me!" + DateTime.Now.ToLongTimeString();
			var user = new User() {
				UserName = _plugger,
				Signature = signature
			};
			// Act
			_acc.UpdateProfile(user);

			var usersig = _usr.GetUserDetail(_plugger).Signature;
			// Assert
			Assert.AreEqual(signature, usersig);
			user = null;
		}

		[TestMethod]
		public void Client_Cannot_Update_User_Signature() {
			// Arrange
			if (_dummyId == 0) { Can_Register_User(); }

			var signature = "Contact Me!" + DateTime.Now.ToLongTimeString();
			var user = new User() {
				UserName = _dummyuser,
				Signature = signature
			};
			// Act
			_acc.UpdateProfile(user);

			var usersig = _usr.GetUserDetail(_dummyuser).Signature;
			// Assert
			Assert.AreNotEqual(signature, usersig);
			user = null;
		}

		private static IAccountService _acc;
		private static IUserManagementService _usr;
		private static string _admin = "claus_herther@yahoo.com";
		private static int _adminId = 2;

		private static string _plugger = "seller@yahoo.com";
		private static string _dummyuser = "newuser@yahoo.com";
		private static string _dummypw = "testing";
		private static int _dummyId;
		private static Guid _invitationCode = new Guid("0F9C1057-25BE-4CD0-AE60-4AF35700698A");

		[ClassInitialize()]
		public static void Initialize(TestContext testContext) {

			
			ResetInvitation();
		}

		[ClassCleanup()]
		public static void Cleanup() {

			DeleteUser(_dummyId);
			

			//_acc.Dispose();
			//_usr.Dispose();
		}

		private static void DeleteUser(int id) {
			//_usr.DeleteUser(id, false);

		}

		private static void ResetInvitation() {

			using (var session = new SongSearchDataSession()) {

				var inv = session.Single<Invitation>(x => x.InvitationId == _invitationCode);
				if (inv != null)
					inv.InvitationStatus = (int)InvitationStatusCodes.Open;
				session.CommitChanges();
			}
		}
	}
}
