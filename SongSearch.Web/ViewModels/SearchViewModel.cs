using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web {
	public class SearchViewModel : ViewModel {

		public int? SortPropertyId { get; set; }
		public SortType SortType { get; set; }

		public IList<SearchProperty> SearchMenuProperties { get; set; }
		public IList<SearchField> SearchFields { get; set; }
		public IDictionary<TagType, IList<Tag>> SearchTags { get; set; }
		public PagedList<Content> SearchResults { get; set; }


	}
}