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
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class CartServiceTests : TestBase {

		public CartServiceTests() {

			_crt = Container.Get<CartService>();			
			_crt.ActiveUserName = _admin;
			
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

		User ActiveUser { get; set; }

		IList<Cart> MyCarts();
		Cart MyActiveCart();

		int[] MyCartContents();
		bool IsInMyActiveCart(int contentId);

		void AddToMyActiveCart(int contentId);
		void RemoveFromMyActiveCart(int contentId);

		void PackageMyActiveCart(string archiveName);//, IList<Content> items);

		void DeleteCart(int cartId);
		Cart DownloadPackagedCart(int cartId);

		void ArchiveExpiredCarts();
		void DeletedExpiredArchivedCarts();
		 * 
		 */
		[TestMethod]
		public void User_Can_Add_Item_To_Active_Cart() {

			//Arrange
			//var contentId = 8080;

			//Act
			_crt.AddToMyActiveCart(_contentId);

			//Assert
			var isInCart = _crt.IsInMyActiveCart(_contentId);
			Assert.IsTrue(isInCart);
		}

		[TestMethod]
		public void User_Can_Add_Item_To_Active_Cart_Twice_Without_Error() {

			//Arrange
			//var contentId = 8080;

			//Act
			_crt.AddToMyActiveCart(_contentId);
			_crt.AddToMyActiveCart(_contentId);
			//Assert
			var isInCart = _crt.IsInMyActiveCart(_contentId);
			Assert.IsTrue(isInCart);
		}
		[TestMethod]
		public void User_Can_Remove_Item_From_Active_Cart() {

			//Arrange
			//var contentId = 8080;

			//Act
			_crt.RemoveFromMyActiveCart(_contentId);
			

			//Assert
			var isInCart = _crt.IsInMyActiveCart(_contentId);
			Assert.IsFalse(isInCart);
		}
		[TestMethod]
		public void User_Can_Remove_Item_From_Active_Cart_Twice_Without_Error() {

			//Arrange
			//var contentId = 8080;

			//Act
			_crt.RemoveFromMyActiveCart(_contentId);
			_crt.RemoveFromMyActiveCart(_contentId);
			//Assert
			var isInCart = _crt.IsInMyActiveCart(_contentId);
			Assert.IsFalse(isInCart);
		}

		private static string _admin = "admin@junestreet.com";
		private static int _adminId = 3;
		private static int _contentId = 8080;

		private static ICartService _crt;

		[ClassInitialize()]
		public static void Initialize(TestContext testContext) {
			HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();

			
		}

		[ClassCleanup()]
		public static void Cleanup() {
			_crt.RemoveFromMyActiveCart(_contentId);
			_crt.Dispose();
		}

	}
}
