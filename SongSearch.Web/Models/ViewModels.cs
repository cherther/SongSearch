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

		public int? SortPropertyId { get; set; }
		public SortType SortType { get; set; }

		public IList<SearchProperty> SearchMenuProperties { get; set; }
		public IList<SearchField> SearchFields { get; set; }
		public IList<Tag> SearchTags { get; set; }
		public PagedList<Content> SearchResults { get; set; }


	}
}