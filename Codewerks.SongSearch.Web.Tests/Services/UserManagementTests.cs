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
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class UserManagementTests {

		public UserManagementTests() {

		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		/*

		IList<DisplayInvitation> GetMyInvites(InvitationStatusCodes status);

		Guid CreateNewInvitation(string inviteEmailAddress);
		DisplayInvitation GetInvitation(string inviteId, string inviteEmailAddress);
		
		void DeleteUser(int userId, bool takeOwnerShip = true);
		void TakeOwnerShip(int userId);
		void TakeOwnerShip(User user);
		void UpdateUsersRole(int userId, int roleId);
		void UpdateUserCatalogRole(int userId, int catalogId, int roleId);

		void UpdateUserRoleAllCatalogs(int userId, int roleId);
		void UpdateCatalogRoleAllUsers(int catalogId, int roleId);
		 * 
		 */
		[TestMethod]
		public void User_Detail_Returns_Correct_User() {

			//Arrange
			//var testActiveUserId = 3;

			//Act
			var user = _usr.GetUserDetail(_adminId);

			//Assert
			Assert.IsTrue(_admin.Equals(user.UserName, StringComparison.InvariantCultureIgnoreCase));
		}

		[TestMethod]
		public void Can_Return_Users_In_Hierarchy() {

			
				//Act
				var hierarchy = _usr.GetMyUserHierarchy();

				//Assert

				Assert.IsNotNull(hierarchy);
			
		}

		[TestMethod]
		public void Super_Admin_Sees_All_Users_In_Hierarchy() {

			//Arrange
			int numberOfTotalUsers;

			using (IDataSession rep = new SongSearchDataSession()) {
				numberOfTotalUsers = rep.All<User>().Count();
			}
			//Act
			var hierarchy = _usr.GetMyUserHierarchy();
			int hierarchyCount = hierarchy.CountWithChildren();

			//Assert

			Assert.AreEqual(hierarchyCount, numberOfTotalUsers);
			
		}


		[TestMethod]
		public void Admin_User_Sees_Only_Sub_Users_In_Hierarchy() {

			//Arrange
			var testActiveUserIdentity = "seller@yahoo.com";
			var testActiveUserId = 6;
			int numberOfTotalUsers;
			using (IDataSession rep = new SongSearchDataSession()) {
				numberOfTotalUsers = rep.All<User>().Count();
			}
			using (IUserManagementService svc = new UserManagementService(testActiveUserIdentity)) {

				var user = svc.GetUserDetail(testActiveUserId);

				//Act
				var hierarchy = svc.GetMyUserHierarchy();
				int hierarchyCount = hierarchy.CountWithChildren();
				

				//Assert
				Assert.IsTrue(testActiveUserIdentity.Equals(user.UserName, StringComparison.InvariantCultureIgnoreCase));

				Assert.AreNotEqual(hierarchyCount, numberOfTotalUsers);
			}

		}

		[TestMethod]
		public void Non_Admin_User_Sees_No_Sub_Users_In_Hierarchy() {

			//Arrange
			var testActiveUserIdentity = "client3@yahoo.com";

			using (IUserManagementService svc = new UserManagementService(testActiveUserIdentity)) {


				//Act
				var hierarchy = svc.GetMyUserHierarchy();
				int hierarchyCount = hierarchy.CountWithChildren();
				//int subUserCount = user.ChildUsers(null, false).Count();

				//Assert
				Assert.AreEqual(hierarchyCount, 0);
			}

		}

		//Invitations?

		[TestMethod]
		public void There_Is_At_Least_One_SuperAdmin() {

			//Arrange
			var testActiveUserIdentity = "claus_herther@yahoo.com";

			using (IUserManagementService svc = new UserManagementService(testActiveUserIdentity)) {


				//Act
				var admins = svc.GetNumberOfUsersByAccessLevel(Roles.SuperAdmin);

				//int subUserCount = user.ChildUsers(null, false).Count();

				//Assert
				Assert.AreNotEqual(admins, 0);
			}
		}

		[TestMethod]
		public void Can_Delete_Users_And_Take_Ownership() {


		}

		[TestMethod]
		public void Can_Delete_Users_Without_Taking_Ownership() {


		}
		[TestMethod]
		public void Admin_User_Can_Assign_Child_User_to_Admin_Role() {

			//Arrange
			//var testActiveUserIdentity = "claus_herther@yahoo.com";
			
			using (IUserManagementService svc = new UserManagementService(_admin)) {

				var testUserId = 7;
				var testRoleId = (int)Roles.Admin;

				//Act
				svc.UpdateUsersRole(testUserId, testRoleId);

				//Assert
				var usr = svc.GetUserDetail(testUserId);
				
				Assert.IsNotNull(usr.RoleId == testRoleId);

				//reset 
				svc.UpdateUsersRole(testUserId, (int)Roles.Client);
			}

		}

		[TestMethod]
		public void Admin_User_Can_Revoke_Catalog_Access_To_User_And_ChildUsers() {

			//Arrange
			
			using (IUserManagementService svc = new UserManagementService(_admin)) {

				var testUserId = 6;
				//var testRoleId = (int)Roles.Admin;
				var testCatalogId = 8;

				//Assert
				var preusr = svc.GetUserDetail(testUserId);
				var prechildren = preusr.GetUserHierarchy(true);
				var prehasAccess = preusr.UserCatalogRoles.Any(x => x.CatalogId == testCatalogId);
				var prechildrenHaveAccess = prechildren.Any(x => x.UserCatalogRoles.Any(r => r.CatalogId == testCatalogId));

				Assert.IsTrue(prechildrenHaveAccess, "User has access");
				Assert.IsTrue(prechildrenHaveAccess, "Child users have access");


				//Act
				svc.UpdateUserCatalogRole(testUserId, testCatalogId, 0);

				//Assert
				var usr = svc.GetUserDetail(testUserId);
				var children = usr.GetUserHierarchy(true);
				var hasAccess = usr.UserCatalogRoles.Any(x => x.CatalogId == testCatalogId);
				var childrenHaveAccess = children.Any(x => x.UserCatalogRoles.Any(r => r.CatalogId == testCatalogId));

				Assert.IsFalse(hasAccess, "User does not have access");
				Assert.IsFalse(childrenHaveAccess, "Child users do not have access");


				//reset
				svc.UpdateUserCatalogRole(testUserId, testCatalogId, (int)Roles.Client);
				children.ForEach(c => svc.UpdateUserCatalogRole(c.UserId, testCatalogId, (int)Roles.Client));
	
			}

		}


		private static string _admin = "admin@junestreet.com";
		private static int _adminId = 3;

		private static IUserManagementService _usr;

		[ClassInitialize()]
		public static void Initialize(TestContext testContext) {

			_usr = new UserManagementService(_admin);
		}

		[ClassCleanup()]
		public static void Cleanup() {
			_usr.Dispose();
		}

	}
}
