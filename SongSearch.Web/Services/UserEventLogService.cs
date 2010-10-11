using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using SongSearch.Web.Data;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using Ninject;
using System.Runtime.Remoting.Messaging;
using SongSearch.Web;
using SongSearch.Web.Services;


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
	public class UserEventLogService : IUserEventLogService {

		// ----------------------------------------------------------------------------
		// (Properties)
		// ----------------------------------------------------------------------------
		//private IDataSession DataSession;
		private bool _disposed;
		//private string _activeUserIdentity;

		private User _user;

		public User ActiveUser {

			get {

				if (_user == null) {
					_user = Account.User();
				}
				return _user;
			}
			set {

				_user = value;
			}
		}


		public string SessionId { get; set; }
		
		public UserEventLogService(HttpContextBase httpContext) {
			if (httpContext != null) {
				SessionId = httpContext.Session.SessionID;
			}
		}


		private delegate void LogActionEventDelegate<T>(T logAction);
		//delegate void EndInvokeDelegate(IAsyncResult result);

		// **************************************
		// LogUserEvent: UserActionEvent
		// **************************************
		public void LogUserEvent(UserActions action) {
			
			if (SystemConfig.LogUserActions && ActiveUser != null) {
				var actionEvent = new UserActionEvent() {
					UserActionId = (int)action,
					UserId = ActiveUser.UserId,
					UserActionEventDate = DateTime.Now,
					SessionId = SessionId ?? String.Empty
				};

				LogActionEventDelegate<UserActionEvent> logActionEventDelegate = new LogActionEventDelegate<UserActionEvent>(LogEvent);

				logActionEventDelegate.BeginInvoke(actionEvent, null, null);
			}
		}

		// **************************************
		// LogUserEvent: ContentActionEvent
		// **************************************
		public void LogContentEvent(ContentActions action, int contentId) {
			
			if (SystemConfig.LogUserContentActions) {
				var actionEvent = new ContentActionEvent() {
					ContentActionId = (int)action,
					ContentId = contentId,
					UserId = ActiveUser.UserId,
					ContentActionEventDate = DateTime.Now,
					SessionId = SessionId ?? String.Empty
				};


				LogActionEventDelegate<ContentActionEvent> logActionEventDelegate = new LogActionEventDelegate<ContentActionEvent>(LogEvent);

				logActionEventDelegate.BeginInvoke(actionEvent, null, null);
			}
		}

		
		// **************************************
		// LogUserEvent: ContentActionEvent
		// **************************************
		public void LogSearchEvent(SearchActions action, string searchTerms, int resultsCount) {
			var actionEvent = new SearchEvent() {
				SearchActionId = (int)action,
				UserId = ActiveUser.UserId,
				SearchEventDate = DateTime.Now,
				SessionId = SessionId ?? String.Empty,
				QueryString = searchTerms,
				ResultCount = resultsCount
			};

			LogActionEventDelegate<SearchEvent> logActionEventDelegate = new LogActionEventDelegate<SearchEvent>(LogEvent);

			logActionEventDelegate.BeginInvoke(actionEvent, null, null);
		}


		// **************************************
		// ReportUserActions
		// **************************************
		public IList<UserActionEvent> ReportUserActions(DateTime startDate, DateTime endDate) {

			using (var session = App.DataSession) {

				var events = session.GetObjectQuery<UserActionEvent>().Include("User")
					.Where(e => e.UserActionEventDate >= startDate && e.UserActionEventDate <= endDate);

				return new PagedList<UserActionEvent>(events,0,0);

			}

		}


		

		// **************************************
		//LogUserActionEvent
		// **************************************
		private void LogEvent(UserActionEvent logEvent) {
			using (var session = App.DataSession) {

				try {
					session.QuickAdd<UserActionEvent>(logEvent);					
				}
				catch (Exception ex) { Log.Error(ex); }

			}
		}
		// **************************************
		//LogUserActionEvent
		// **************************************
		private void LogEvent(ContentActionEvent logEvent) {
			using (var session = App.DataSession) {

				try {
					session.QuickAdd<ContentActionEvent>(logEvent);					
				}
				catch (Exception ex) { Log.Error(ex); }
			}
		}
		// **************************************
		//LogUserActionEvent
		// **************************************
		private void LogEvent(SearchEvent logEvent) {
			using (var session = App.DataSession) {
				try {
					session.QuickAdd<SearchEvent>(logEvent);					
				}
				catch (Exception ex) { Log.Error(ex); }
			}
		}


		// ----------------------------------------------------------------------------
		// Dispose
		// ----------------------------------------------------------------------------

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}


		private void Dispose(bool disposing) {
			if (!_disposed) {
				{
									
				}

				_disposed = true;
			}
		}



	}
}