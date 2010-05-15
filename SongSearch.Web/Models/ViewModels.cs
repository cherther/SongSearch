using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web {

	// **************************************
	// ViewModel
	// **************************************
	public class ViewModel {
		public string PageTitle { get; set; }
		public string NavigationLocation { get; set; }

		public ViewModel() {
			PageTitle = "";
			NavigationLocation = "";
		}
	}

	public class SearchViewModel : ViewModel {

		public IList<SearchProperty> SearchMenuProperties { get; set; }
		public IList<SearchField> SearchFields { get; set; }
		public IList<Tag> SearchTags { get; set; }
		public IList<Content> SearchResults { get; set; }


	}
}