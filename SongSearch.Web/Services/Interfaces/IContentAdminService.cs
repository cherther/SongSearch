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
		void Update(Content content, IList<int> tags, IList<ContentRightViewModel> rights);
		void Delete(int contentId);
	}
}