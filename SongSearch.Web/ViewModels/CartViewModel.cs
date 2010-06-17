using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web {

	// **************************************
	// ViewModel
	// **************************************
	public class CartViewModel : ViewModel {

		public IList<Cart> MyCarts { get; set; }
		public int? SortPropertyId { get; set; }
		public SortType SortType { get; set; }
		public string RequestUrl { get; set; }
		public string HeaderSortUrl { get; set; }
		public int CartToHighlight { get; set; }
		public IList<SearchProperty> SearchMenuProperties { get; set; }
		public string[] CartContentHeaders { get; set; }
	}

	
}