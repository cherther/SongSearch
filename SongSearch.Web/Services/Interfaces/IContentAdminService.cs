using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {
	// **************************************
	// IContentAdminService
	// **************************************
	public interface IContentAdminService : IDisposable {
		string ActiveUserName { get; set; }
		User ActiveUser { get; set; }
		void Update(Content content);
		void Delete(int contentId);
	}
}