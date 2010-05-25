using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web {
	public class ContentListViewModel : ViewModel {

		public IList<SearchProperty> SearchMenuProperties { get; set; }
		
		public int? SortPropertyId { get; set; }
		public SortType SortType { get; set; }
		public string RequestUrl { get; set; }
		public string PagerSortUrl { get; set; }
		public string HeaderSortUrl { get; set; }

		public PagedList<Content> List { get; set; }
		public string[] ListHeaders { get; set; }

	}
}