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
	public class SearchServiceTests {

		public SearchServiceTests() {

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
		public void Search_Love_Should_Return_448_Results() {

			//Arrange
			//var testActiveUserId = 3;
			IList<SearchField> searchFields = new List<SearchField>();
			searchFields.Add(new SearchField() { P = 1, T = SearchTypes.Contains, V = new string[] { "Love"} });
			int expected = 448;
			//Act
			var results = SearchService.GetContentSearchResults(searchFields, _admin);

			//Assert
			Assert.AreEqual(expected, results.TotalCount);
		}

		[TestMethod]
		public void Search_Results_Can_Be_User_Sorted() {

			//Arrange
			//var testActiveUserId = 3;
			IList<SearchField> searchFields = new List<SearchField>();
			searchFields.Add(new SearchField() { P = 1, T = SearchTypes.Contains, V = new string[] { "Love" } });
			
			//Act
			var results = SearchService.GetContentSearchResults(searchFields, _admin);
			var resultsSort = SearchService.GetContentSearchResults(searchFields, _admin, 1, (int)SortType.Descending);

			//Assert
			Assert.AreNotEqual(results, resultsSort);
			Assert.AreEqual(results.TotalCount, resultsSort.TotalCount);
		}

		[TestMethod]
		public void Content_Details_Should_Contain_Releated_Entities() {
			
			//Arrange
			int contentId = 8080;
			//Act

			var content = SearchService.GetContentDetails(contentId, _admin);
			//Assert
			Assert.IsNotNull(content.Catalog, "Catalog is null");
			Assert.IsTrue(content.Catalog.CatalogName.Equals("Paradise Artists", StringComparison.InvariantCultureIgnoreCase));
			Assert.IsTrue(content.Tags.Count > 0, "Tags are null");
			Assert.IsTrue(content.ContentRights.Count > 0, "ContentRights are null");
			Assert.IsTrue(content.ContentRights.First().Territories.Count > 0, "Territories are null");
		}

		[TestMethod]
		public void Repeated_Calls_To_Content_Details_Should_Not_Block() {

			//Arrange
			//var testActiveUserId = 3;
			IList<SearchField> searchFields = new List<SearchField>();
			searchFields.Add(new SearchField() { P = 1, T = SearchTypes.Contains, V = new string[] { "Love" } });
			var limit = 10;
			var results = SearchService.GetContentSearchResults(searchFields, _admin, null, limit, 0);

			foreach (var result in results) {
				//Act
				var content = SearchService.GetContentDetails(result.ContentId, _admin);
				//Assert
				Assert.IsNotNull(content, "Content is null");
				Assert.IsNotNull(content.Catalog, "Catalog is null");
				//Assert.IsTrue(content.Catalog.CatalogName.Equals("Paradise Artists", StringComparison.InvariantCultureIgnoreCase));
				Assert.IsNotNull(content.Tags != null, "Tags are null");
				Assert.IsNotNull(content.ContentRights, "ContentRights are null");
				Assert.IsNotNull(content.ContentRights.First().Territories, "Territories are null");
			}
		}

		[TestMethod]
		public void Lookup_List_Artist_Should_Return_1637_Items() {

			//Arrange
			int expected = 1637;
			//Act
			var list = SearchService.GetLookupListContent("Artist");
			//var label = SearchService.GetLookupListContent("RecordLabel");
			//var catalogs = SearchService.GetLookupListContent("Catalog.CatalogName");
			//var list = SearchService.GetLookupListContent("Artist");
			//Assert
			Assert.IsNotNull(list, "list is null");
			Assert.AreEqual(expected, list.Count, "Incorrect number of artists");
		}

		[TestMethod]
		public void Lookup_List_Catalog_Should_Return_All_Items() {

			//Arrange
			int expected = 180;
			//Act
			var list = SearchService.GetLookupList<Catalog>();
			//var label = SearchService.GetLookupListContent("RecordLabel");
			//var catalogs = SearchService.GetLookupListContent("Catalog.CatalogName");
			//var list = SearchService.GetLookupListContent("Artist");
			//Assert
			Assert.IsNotNull(list, "list is null");
			Assert.AreEqual(expected, list.Count, "Incorrect number of catalogs");
		}

		[TestMethod]
		public void Lookup_List_Territories_Should_Return_All_Items() {

			//Arrange
			int expected = 7;
			//Act
			var list = SearchService.GetLookupList<Territory>();
			//var label = SearchService.GetLookupListContent("RecordLabel");
			//var catalogs = SearchService.GetLookupListContent("Catalog.CatalogName");
			//var list = SearchService.GetLookupListContent("Artist");
			//Assert
			Assert.IsNotNull(list, "list is null");
			Assert.AreEqual(expected, list.Count, "Incorrect number of catalogs");
		}


		[TestMethod]
		public void TopStyles_Should_Return_OrderedByMostTagged() {

			//Arrange
			//Act
			var styles = SearchService.GetTopTags(TagType.Styles);
			//var label = SearchService.GetLookupListContent("RecordLabel");
			//var catalogs = SearchService.GetLookupListContent("Catalog.CatalogName");
			//var list = SearchService.GetLookupListContent("Artist");
			//Assert
			Assert.IsNotNull(styles, "tags is null");
			Assert.IsTrue(styles.First().TagName.Equals("Pop"),"Pop should be most tagged style");
			//Assert.AreEqual(expected, artists.Count, "Incorrect number of artists");
		}

		[TestMethod]
		public void TopStyles_Should_Return_TopLimitOnly() {

			//Arrange
			int topLimit = 5;
			//Act
			var styles = SearchService.GetTopTags(TagType.Styles, topLimit);
			//var label = SearchService.GetLookupListContent("RecordLabel");
			//var catalogs = SearchService.GetLookupListContent("Catalog.CatalogName");
			//var list = SearchService.GetLookupListContent("Artist");
			//Assert
			Assert.IsNotNull(styles, "tags is null");
			Assert.AreEqual(topLimit, styles.Count(), "Incorrect number of top tags");
			//Assert.AreEqual(expected, artists.Count, "Incorrect number of artists");
		}

		private static User _admin;
		//private static int _adminId = 3;


		[ClassInitialize()]
		public static void Initialize(TestContext testContext) {

			_admin = AccountData.User("claus_herther@yahoo.com");
//			_admin = AccountData.User("admin@junestreet.com");
		}

		[ClassCleanup()]
		public static void Cleanup() {
		}

	}
}
