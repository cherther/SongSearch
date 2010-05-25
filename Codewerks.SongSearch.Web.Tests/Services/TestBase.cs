using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using SongSearch.Web;
using SongSearch.Web.Services;

namespace Codewerks.SongSearch.Web.Tests {

	public class TestBase {

		public TestBase() {

			_container = new StandardKernel(new TestModule());
		}

		// **************************************
		// DataSession
		// **************************************
		public static IDataSession DataSession {
			get {
				return Container.Get<IDataSession>();
			}
		}
		// **************************************
		// DataSession
		// **************************************
		public static IDataSessionReadOnly DataSessionReadOnly {
			get {
				return Container.Get<IDataSessionReadOnly>();
			}
		}

		// ----------------------------------------------------------------------------
		// NInject
		// ----------------------------------------------------------------------------
		private static IKernel _container;

		// **************************************
		// SiteModule
		// **************************************
		internal class TestModule : NinjectModule {
			public override void Load() {

				Bind<IDataSession>().To<SongSearchDataSession>();
				Bind<IDataSessionReadOnly>().To<SongSearchDataSessionReadOnly>();

				Bind<IAccountService>().To<AccountService>();
				Bind<ICartService>().To<CartService>();
				Bind<IContentAdminService>().To<ContentAdminService>();
				Bind<IUserManagementService>().To<UserManagementService>();
			}
		}

		// **************************************
		// Container
		// **************************************
		public static IKernel Container {
			get {
				if (_container == null) {
					_container = new StandardKernel(new TestModule());
				}
				return _container;
			}
		}
	}
}
