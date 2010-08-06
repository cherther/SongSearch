using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using SongSearch.Web.Logging;
using SongSearch.Web.Services;

namespace SongSearch.Web {
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	// **************************************
	// AppEnvironment
	// **************************************
	public enum AppEnvironment {
		Development,
		Staging,
		Production
	}

	public enum AppVersion {
		SongSearch_2_0,
		SongSearch_2_1
	}

	// **************************************
	// App
	// **************************************
	public class App : NinjectHttpApplication {

		// ----------------------------------------------------------------------------
		// App-Level Variables
		// ----------------------------------------------------------------------------
		// **************************************
		// Environment
		// **************************************
		public static AppEnvironment Environment {
			get {
#if DEBUG
				return AppEnvironment.Development;
#else

				return AppEnvironment.Production;
#endif
			}
		}

		public static AppVersion Version {
			get {
				return AppVersion.SongSearch_2_1;				
			}
		}

		public static AppVersion BaseVersion {
			get {
				return AppVersion.SongSearch_2_0;
			}
		}
		public static AppVersion LicensedVersion {
			get {
				return AppVersion.SongSearch_2_1;
			}
		}
		public static bool IsBaseVersion {
			get {
				return Version <= BaseVersion;
			}
		}

		public static bool IsLicensedVersion {
			get {
				return Version >= LicensedVersion;
			}
		}
		// **************************************
		// DataSession
		// **************************************
		public static IDataSession DataSession {
			get {
				return _container.Get<IDataSession>();
			}
		}
		// **************************************
		// DataSession
		// **************************************
		public static IDataSessionReadOnly DataSessionReadOnly {
			get {
				return _container.Get<IDataSessionReadOnly>();
			}
		}

		// ----------------------------------------------------------------------------
		// ASP.NET MVC Routes
		// ----------------------------------------------------------------------------
		// **************************************
		// RegisterRoutes
		// **************************************
		public static void RegisterRoutes(RouteCollection routes) {
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			var controllers = new List<string>();
			controllers.Add("Cart");
			controllers.Add("CatalogManagement");
			controllers.Add("CatalogUpload");
			controllers.Add("UserManagement");
			controllers.Add("Error");
			controllers.Add("Home");
			controllers.Add("Search");


			routes.MapRoute(
				"Contact",
				"Contact",
				new { controller = "Home", action = "Contact" }
				);
			routes.MapRoute(
				"Privacy",
				"Privacy",
				new { controller = "Home", action = "PrivacyPolicy" }
				);

			routes.MapRoute(
				"Legal",
				"Legal",
				new { controller = "Home", action = "TermsOfUse" }
				);

			routes.MapRoute(
				"Login",
				"Login",
				new { controller = "Account", action = "Login" }
			);
			routes.MapRoute(
				"Logout",
				"Logout",
				new { controller = "Account", action = "Logout" }
			);

			routes.MapRoute(
				"Register",
				"Register",
				new { controller = "Account", action = "Register" }
			);

			foreach (var ctrl in controllers) {

				routes.MapRoute(
					ctrl,
					String.Concat(ctrl, "/{action}/{id}"),
					new { controller = ctrl, action = "Index", id = UrlParameter.Optional }
				);

			}

			//routes.MapRoute(
			//    "Search",
			//    "Search/{action}/{id}",
			//    new { controller = "Search", action = "Index", id = UrlParameter.Optional }
			//);

			//routes.MapRoute(
			//    "UserManagement",
			//    "UserManagement/{action}/{id}",
			//    new { controller = "UserManagement", action = "Index", id = UrlParameter.Optional }
			//);

			//routes.MapRoute(
			//    "CatalogManagement",
			//    "CatalogManagement/{action}/{id}",
			//    new { controller = "CatalogManagement", action = "Index", id = UrlParameter.Optional }
			//);

			//routes.MapRoute(
			//    "CatalogUpload",
			//    "CatalogUpload/{action}/{id}",
			//    new { controller = "CatalogUpload", action = "Index", id = UrlParameter.Optional }
			//);

			//routes.MapRoute(
			//    "Error",
			//    "Error/{action}/{id}",
			//    new { controller = "Error", action = "Index", id = UrlParameter.Optional }
			//);
			
			routes.MapRoute(
				"Profile",
				"{profileName}",
				new { controller = "Home", action = "Profile" }//,
				//new { profileName = @"\w\s"	}
			);
			

			//routes.MapRoute(
			//    "Profile", // Route name
			//    "{id}", // URL with parameters
			//    new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			//);
			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);

		}

		// ----------------------------------------------------------------------------
		// ASP.NET Session
		// ----------------------------------------------------------------------------
		// **************************************
		// Session_Start
		// **************************************
		protected void Session_Start() {
			try {
				SessionService.Session().InitializeSession();
			//DataSession["UserName"] = User.Identity.Name;
			}
			catch { }
		}


		// ----------------------------------------------------------------------------
		// ASP.NET App
		// ----------------------------------------------------------------------------
		// **************************************
		// OnApplicationStarted
		// **************************************
		protected override void OnApplicationStarted() {
			AreaRegistration.RegisterAllAreas();

			RegisterRoutes(RouteTable.Routes);
			RegisterAllControllersIn(Assembly.GetExecutingAssembly());

			//if (Environment == AppEnvironment.Development) {
			//    HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
			//}
			try {
				CacheService.Initialize();
			}
			catch { }
			//Logger.Info("App is starting up");
		}

		// **************************************
		// Application_End
		// **************************************
		protected void Application_End() {
			Logger.Info("App is shutting down");
		}

		// **************************************
		// Application_Error
		// **************************************
		protected void Application_Error() {
			Exception lastException = Server.GetLastError();
			Logger.Fatal(lastException);
		}

		// ----------------------------------------------------------------------------
		// NLog
		// ----------------------------------------------------------------------------
		// **************************************
		// Logger
		// **************************************
		public static ILogger Logger {
			get {
				return Container.Get<ILogger>();
			}
		}

		// ----------------------------------------------------------------------------
		// NInject
		// ----------------------------------------------------------------------------
		private static IKernel _container;

		// **************************************
		// CreateKernel
		// **************************************
		protected override IKernel CreateKernel() {
			return Container;
		}

		// **************************************
		// SiteModule
		// **************************************
		internal class SiteModule : NinjectModule {
			public override void Load() {

				Bind<ILogger>().To<NLogLogger>();

				Bind<IDataSession>().To<SongSearchDataSession>();
				Bind<IDataSessionReadOnly>().To<SongSearchDataSessionReadOnly>();

				Bind<IAccountService>().To<AccountService>();
				Bind<ICartService>().To<CartService>();
				Bind<IContentAdminService>().To<ContentAdminService>();
				Bind<IUserManagementService>().To<UserManagementService>();
				Bind<ICatalogManagementService>().To<CatalogManagementService>();
				Bind<ICatalogUploadService>().To<CatalogUploadService>();

				Bind<IMediaService>().To<MediaService>();
				Bind<IMediaCloudService>().To<AmazonCloudService>();
				
				Bind<IFormsAuthenticationService>().To<FormsAuthenticationService>();
			}
		}

		// **************************************
		// Container
		// **************************************
		public static IKernel Container {
			get {
				if (_container == null) {
					_container = new StandardKernel(new SiteModule());
				}
				return _container;
			}
		}
	}
}