using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using SongSearch.Web.Services;
using System.Threading;
using System.Web;

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
				return SystemConfig.Environment;
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
		// DataSessionReadOnly
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
			controllers.Add("Admin");
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

			foreach (var ctrl in controllers) {

				routes.MapRoute(
					string.Concat(ctrl, "-profile"),
					String.Concat("{profileName}/", ctrl, "/{action}/{id}"),
					new { controller = ctrl, action = "Index", id = UrlParameter.Optional }
				);

			}

			
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
				using (var log = Container.Get<IUserEventLogService>()) {
					log.SessionId = HttpContext.Current.Session.SessionID;
					log.LogUserEvent(UserActions.SessionStarted);
				}
			//DataSession["UserName"] = User.Identity.Name;
			}
			catch (Exception ex) {
				Log.Error(ex);
			}
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
			catch (Exception ex) {
				Log.Error(ex);
			}
			//Logger.Info("App is starting up");

//#if !DEBUG
			if (App.Environment == AppEnvironment.Production) {


				//ThreadStart amazonSync = new ThreadStart(AmazonSyncLoopPreview);
				//Thread t1 = new Thread(amazonSync);
				//t1.Start();
			
				ThreadStart amazonSyncFull = new ThreadStart(AmazonSyncLoopFull);
				Thread amz = new Thread(amazonSyncFull);
				amz.Start();

			}

			//ThreadStart amazonRecyleFull = new ThreadStart(AmazonRecyleFull);
			//Thread t3 = new Thread(amazonRecyleFull);
			//t3.Start();

			//ThreadStart amazonRecylePreview = new ThreadStart(AmazonRecylePreview);
			//Thread t4 = new Thread(amazonRecylePreview);
			//t4.Start();
//#endif

			ThreadStart cartCleanup = new ThreadStart(CartCleanupLoop);
			Thread cartLoop = new Thread(cartCleanup);
			cartLoop.Start();

			//ThreadStart ping = new ThreadStart(PingLoop);
			//Thread pingLoop = new Thread(ping);
			//pingLoop.Start();

		}

		//// **************************************
		//// Application_End
		//// **************************************
		//protected void Application_End() {
		//    Logger.Info("App is shutting down");
		//}

		//// **************************************
		//// Application_Error
		//// **************************************
		//protected void Application_Error() {
		//    Exception lastException = Server.GetLastError();
		//    Logger.Fatal(lastException);
		//}

		// **************************************
		// AmazonSyncLoop
		// **************************************
		static void AmazonSyncLoopPreview() {
			// In this example, task will repeat in infinite loop
			// You can additional parameter if you want to have an option 
			// to stop the task from some page
			while (true) {
				// Execute scheduled task
				//ScheduledTask();
				using (var amz = new SongSearch.Web.Tasks.AmazonRemoteMedia(new SongSearchDataSession(), new SongSearchDataSessionReadOnly())) {

					//amz.UploadToRemote(checkSize: false, onlyNewContent: true);
					try {
						amz.UploadToRemote(checkSize: SystemConfig.RemoteMediaCheckSize,
						onlyNewContent: SystemConfig.RemoteMediaUploadNewOnly, mediaVersions: new MediaVersion[] { MediaVersion.Preview });
					}
					catch (Exception ex) {
						Log.Error(ex);
						

					}
				}
				// Wait for certain time interval
				System.Threading.Thread.Sleep(TimeSpan.FromDays(1));
			}
		}
		static void AmazonSyncLoopFull() {
			// In this example, task will repeat in infinite loop
			// You can additional parameter if you want to have an option 
			// to stop the task from some page
			while (true) {
				// Execute scheduled task
				//ScheduledTask();
				using (var amz = new SongSearch.Web.Tasks.AmazonRemoteMedia(new SongSearchDataSession(), new SongSearchDataSessionReadOnly())) {

					//amz.UploadToRemote(checkSize: false, onlyNewContent: true);
					try {
						amz.UploadToRemote(
							checkSize: SystemConfig.RemoteMediaCheckSize
							, onlyNewContent: SystemConfig.RemoteMediaUploadNewOnly
							, mediaVersions: new MediaVersion[] { MediaVersion.Full }
							);
					}
					catch (Exception ex) {
						Log.Error(ex);
					}
				}
				// Wait for certain time interval
				System.Threading.Thread.Sleep(TimeSpan.FromDays(1));
			}
		}
		static void AmazonRecylePreview() {
			// In this example, task will repeat in infinite loop
			// You can additional parameter if you want to have an option 
			// to stop the task from some page
			using (var amz = new SongSearch.Web.Tasks.AmazonRemoteMedia(new SongSearchDataSession(), new SongSearchDataSessionReadOnly())) {

				//amz.UploadToRemote(checkSize: false, onlyNewContent: true);
				try {
					amz.UploadToRemote(
							checkSize: SystemConfig.RemoteMediaCheckSize
							, onlyNewContent: SystemConfig.RemoteMediaUploadNewOnly
							, mediaVersions: new MediaVersion[] { MediaVersion.Preview });
				}
				catch (Exception ex) {
					Log.Error(ex);
				}
			}
		}
		static void AmazonRecyleFull() {
			using (var amz = new SongSearch.Web.Tasks.AmazonRemoteMedia(new SongSearchDataSession(), new SongSearchDataSessionReadOnly())) {

				//amz.UploadToRemote(checkSize: false, onlyNewContent: true);
				try {
					amz.UploadToRemote(checkSize: true, onlyNewContent: false, mediaVersions: new MediaVersion[] { MediaVersion.Full });
				}
				catch (Exception ex) {
					Log.Error(ex);
				}
			}
		}
		// **************************************
		// CartLoop
		// **************************************
		static void CartCleanupLoop() {
			// In this example, task will repeat in infinite loop
			// You can additional parameter if you want to have an option 
			// to stop the task from some page
			while (true) {
				// Execute scheduled task

				CartService.ArchiveExpiredCarts();
				CartService.DeletedExpiredArchivedCarts();

				// Wait for certain time interval
				System.Threading.Thread.Sleep(TimeSpan.FromDays(1));
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

				Bind<IDataSession>().To<SongSearchDataSession>();
				Bind<IDataSessionReadOnly>().To<SongSearchDataSessionReadOnly>();

				//Bind<IAccountService>().To<AccountService>();
				Bind<ICartService>().To<CartService>();
				Bind<IContentAdminService>().To<ContentAdminService>();
				Bind<ICatalogManagementService>().To<CatalogManagementService>();
				Bind<ICatalogUploadService>().To<CatalogUploadService>();

				Bind<IMediaService>().To<MediaService>();
				Bind<IMediaCloudService>().To<AmazonCloudService>();

				Bind<IUserEventLogService>().To<UserEventLogService>();

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