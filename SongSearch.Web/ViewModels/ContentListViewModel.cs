using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web {
	public class ContentListViewModel : ViewModel {

		public string[] ListHeaders { get; set; }
		public PagedList<Content> List { get; set; }
		public IList<SearchProperty> SearchMenuProperties { get; set; }
		
		public bool IsSortable { get; set; }
		public int? SortPropertyId { get; set; }
		public SortType SortType { get; set; }
		public string RequestUrl { get; set; }
		public string PagerSortUrl { get; set; }
		public string HeaderSortUrl { get; set; }

		public bool ShowDetails { get; set; }

		public GridAction[] GridActions { get; set; }
	}

	public enum GridAction {
		ShowDetails,
		Download,
		AddToCart,
		RemoveFromCart,
		Delete
	}

}