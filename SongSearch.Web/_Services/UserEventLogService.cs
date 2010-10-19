using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;


namespace SongSearch.Web.Services {

	public enum UserActions {
		Register = 1,
		Login,
		Logout,
		SessionStarted,
		UpdateProfile,
		ChangePassword,
		ResetPassword,
			
		SentInvite = 30,
		ViewUserDetail,
		UpdateUserRole,
		ToggleSystemAdminAccess,
		UpdateUserCatalogRole,
		UpdateAllCatalogs,
		UpdateAllUsers,
		DeleteUser,
		TakeOwnership,
		ViewCatalogDetail,
		DeleteCatalog,
		UploadCatalog,

		ViewCart = 50, 
		CompressCart,
		DownloadCart,
		DeleteCart,	
	}

	public enum ContentActions {

		ViewItemDetail = 10,
		PrintItemDetail,
		PrintItemList,
		AddToCart,
		RemoveFromCart,
		
		CreateNewContent = 20,
		UpdateContent,
		DeleteContent
	}
	public enum SearchActions {

		Search = 100,
		PrintList
	}
	// **************************************
	// UserEventLogService
	// **************************************
	public static class UserEventLogService {

		// ----------------------------------------------------------------------------
		// (Properties)
		// ----------------------------------------------------------------------------
		
		private delegate void LogActionEventDelegate<T>(T logAction);
		//delegate void EndInvokeDelegate(IAsyncResult result);

		// **************************************
		// LogUserEvent: UserActionEvent
		// **************************************
		public static void LogUserEvent(UserActions action) {
			var user = Account.User();
			if (SystemConfig.LogUserActions && user != null) {
				var actionEvent = new UserActionEvent() {
					UserActionId = (int)action,
					UserId = user.UserId,
					UserActionEventDate = DateTime.Now,
					SessionId = CurrentSessionId()
				};

				LogActionEventDelegate<UserActionEvent> logActionEventDelegate = new LogActionEventDelegate<UserActionEvent>(LogEvent);

				logActionEventDelegate.BeginInvoke(actionEvent, null, null);
			}
		}

		private static string CurrentSessionId() {
			return HttpContext.Current != null ?
									(HttpContext.Current.Session != null ? HttpContext.Current.Session.SessionID : String.Empty) : String.Empty;
		}

		// **************************************
		// LogUserEvent: ContentActionEvent
		// **************************************
		public static void LogContentEvent(ContentActions action, int contentId) {
			
			if (SystemConfig.LogUserContentActions) {
				var actionEvent = new ContentActionEvent() {
					ContentActionId = (int)action,
					ContentId = contentId,
					UserId = Account.User().UserId,
					ContentActionEventDate = DateTime.Now,
					SessionId = CurrentSessionId()
				};


				LogActionEventDelegate<ContentActionEvent> logActionEventDelegate = new LogActionEventDelegate<ContentActionEvent>(LogEvent);

				logActionEventDelegate.BeginInvoke(actionEvent, null, null);
			}
		}

		
		// **************************************
		// LogUserEvent: ContentActionEvent
		// **************************************
		public static void LogSearchEvent(SearchActions action, string searchTerms, int resultsCount) {
			var actionEvent = new SearchEvent() {
				SearchActionId = (int)action,
				UserId = Account.User().UserId,
				SearchEventDate = DateTime.Now,
				SessionId = CurrentSessionId(),
				QueryString = searchTerms,
				ResultCount = resultsCount
			};

			LogActionEventDelegate<SearchEvent> logActionEventDelegate = new LogActionEventDelegate<SearchEvent>(LogEvent);

			logActionEventDelegate.BeginInvoke(actionEvent, null, null);
		}


		// **************************************
		// ReportUserActions
		// **************************************
		public static IList<UserActionEvent> ReportUserActions(DateTime startDate, DateTime endDate) {

			using (var ctx = new SongSearchContext()) {

				var events = ctx.UserActionEvents.Include("User")
					.Where(e => e.UserActionEventDate >= startDate && e.UserActionEventDate <= endDate);

				return new PagedList<UserActionEvent>(events,0,0);

			}

		}


		

		// **************************************
		//LogUserActionEvent
		// **************************************
		private static void LogEvent(UserActionEvent logEvent) {
			using (var ctx = new SongSearchContext()) {
				ctx.ContextOptions.LazyLoadingEnabled = false;
				try {
					ctx.UserActionEvents.AddObject(logEvent);
					ctx.SaveChanges();
				}
				catch (Exception ex) { Log.Error(ex); }

			}
		}
		// **************************************
		//LogUserActionEvent
		// **************************************
		private static void LogEvent(ContentActionEvent logEvent) {
			using (var ctx = new SongSearchContext()) {
				ctx.ContextOptions.LazyLoadingEnabled = false;
				try {
					ctx.ContentActionEvents.AddObject(logEvent);
					ctx.SaveChanges();
				}
				catch (Exception ex) { Log.Error(ex); }
			}
		}
		// **************************************
		//LogUserActionEvent
		// **************************************
		private static void LogEvent(SearchEvent logEvent) {
			using (var ctx = new SongSearchContext()) {
				ctx.ContextOptions.LazyLoadingEnabled = false;
				try {
					ctx.SearchEvents.AddObject(logEvent);
					ctx.SaveChanges();
				}
				catch (Exception ex) { Log.Error(ex); }
			}
		}


		


	}
}