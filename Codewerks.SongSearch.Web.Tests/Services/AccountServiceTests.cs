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

namespace Codewerks.SongSearch.Web.Tests.Services {
	[TestClass]
	public class AccountServiceTests {

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
			var password = "go1974";
			var user = new User() {
				UserName = _dummyuser,
				FirstName = "New",
				LastName = "User",
				ParentUserId = 3,
				Password = password
			};
			

			// Act
			_acc.RegisterUser(user, _invitationCode);
			_dummyId = user.UserId;

			var isValid = _acc.UserIsValid(_dummyuser, password);
			// Assert
			Assert.IsTrue(isValid);
		}

		[TestMethod]
		public void Can_Validate_User() {
			// Arrange
			var username = "claus_herther@yahoo.com";
			var password = "go1974";

			// Act
			var isValid = _acc.UserIsValid(username, password);
			
			// Assert
			Assert.IsTrue(isValid);
		}

		[TestMethod]
		public void Can_Check_If_User_Exists() {
			// Arrange
			
			// Act
			var exists = _acc.UserExists(_client);

			// Assert
			Assert.IsTrue(exists);
		}


		[TestMethod]
		public void User_Can_Update_Password() {
			// Arrange
			var user = new User() {
				UserName = _client,
				Password = "rogers1"			
			};
			// Act
			var good = _acc.UpdateProfile(user, "go1974");

			// Assert
			Assert.IsTrue(good);
		}

		[TestMethod]
		public void User_Can_Update_Profile() {
			// Arrange
			var user = new User() {
				UserName = _client,
				FirstName = "Client",
				LastName = "#1"
			};
			// Act
			var good = _acc.UpdateProfile(user, "");

			// Assert
			Assert.IsTrue(good);
		}

		[TestMethod]
		public void Plugger_Can_Update_User_Signature() {
			// Arrange
			var user = new User() {
				UserName = _plugger,
				Signature = "Contact Me!"
			};
			// Act
			var good = _acc.UpdateProfile(user, "");

			// Assert
			Assert.IsTrue(good);
		}

		[TestMethod]
		public void Plugger_Cannot_Update_User_Signature() {
			// Arrange
			var user = new User() {
				UserName = _client,
				Signature = "Contact Me!"
			};
			// Act
			var good = _acc.UpdateProfile(user, "");

			// Assert
			Assert.IsTrue(good);
		}

		private static IAccountService _acc;
		private static IUserManagementService _usr;
		private static string _admin = "claus_herther@yahoo.com";
		private static string _client = "client@yahoo.com";
		private static string _plugger = "seller@yahoo.com";
		private static string _dummyuser = "newuser@yahoo.com";
		private static int _dummyId;
		private static Guid _invitationCode = new Guid("0F9C1057-25BE-4CD0-AE60-4AF35700698A");

		[ClassInitialize()]
		public static void Initialize(TestContext testContext) {

			_acc = new AccountService();
			_usr = new UserManagementService(_admin);
			DeleteUser(11);
			DeleteUser(12);
			DeleteUser(13);
			DeleteUser(14);
			ResetInvitation();
		}

		[ClassCleanup()]
		public static void Cleanup() {

			DeleteUser(_dummyId);
			

			_acc.Dispose();
			_usr.Dispose();
		}

		private static void DeleteUser(int id) {
			_usr.DeleteUser(id, false);

		}

		private static void ResetInvitation() {

			using (ISession session = new EFSession()) {

				var inv = session.Single<Invitation>(x => x.InvitationId == _invitationCode);
				if (inv != null)
					inv.InvitationStatus = (int)InvitationStatusCodes.Open;
				session.CommitChanges();
			}
		}
	}
}
