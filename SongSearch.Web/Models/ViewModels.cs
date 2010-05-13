using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SongSearch.Web.Models {

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
}