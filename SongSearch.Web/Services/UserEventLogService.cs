using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using SongSearch.Web.Data;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using Ninject;
using NLog;
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
		SentInvite
	}

	public enum ContentActions {

		ViewItemDetail = 10,
		PrintItemDetail,
		PrintItemList,
		AddToCart,
		RemoveFromCart,
		CompressCart,
		DownloadCart,
		DeletedCart,	
		CreatedNewContent = 20,
		UpdatedContent,
		DeletedContent
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
		// Log: UserActionEvent
		// **************************************
		public void Log(UserActions action) {
			
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
		// Log: ContentActionEvent
		// **************************************
		public void Log(ContentActions action, int contentId) {
			
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
		// Log: ContentActionEvent
		// **************************************
		public void Log(string searchTerms) {
			var actionEvent = new SearchEvent() {
				UserId = ActiveUser.UserId,
				SearchEventDate = DateTime.Now,
				SessionId = SessionId ?? String.Empty,
				QueryString = searchTerms
			};

			LogActionEventDelegate<SearchEvent> logActionEventDelegate = new LogActionEventDelegate<SearchEvent>(LogEvent);

			logActionEventDelegate.BeginInvoke(actionEvent, null, null);
		}




		

		// **************************************
		//LogUserActionEvent
		// **************************************
		private void LogEvent(UserActionEvent logEvent) {
			using (var session = App.DataSession) {

				try {
					session.QuickAdd<UserActionEvent>(logEvent);					
				}
				catch (Exception ex) { App.Logger.Error(ex); }

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
				catch (Exception ex) { App.Logger.Error(ex); }
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
				catch (Exception ex) { App.Logger.Error(ex); }
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