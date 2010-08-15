using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;
using SongSearch.Web.Infrastructure.Storage.EF;

namespace SongSearch.Web.Services {
	// **************************************
	// ICartService
	// **************************************
	public interface IUserEventLogService : IDisposable {
		string SessionId { get; set; }
		void Log(UserActions actionEvent);
		void Log(ContentActions actionEvent, int contentId);
		void Log(string searchTerms);

	}
}