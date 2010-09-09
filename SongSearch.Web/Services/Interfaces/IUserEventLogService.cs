using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {
	// **************************************
	// ICartService
	// **************************************
	public interface IUserEventLogService : IDisposable {
		string SessionId { get; set; }
		void LogUserEvent(UserActions actionEvent);
		void LogContentEvent(ContentActions actionEvent, int contentId);
		void LogSearchEvent(string searchTerms);

		IList<UserActionEvent> ReportUserActions(DateTime startDate, DateTime endDate);

	}
}