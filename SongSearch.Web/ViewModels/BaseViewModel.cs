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

	
}