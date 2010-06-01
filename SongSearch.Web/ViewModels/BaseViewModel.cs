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
		public ViewModes ViewMode { get; set; }
		public string PageTitle { get; set; }
		public string NavigationLocation { get; set; }
		public int MyActiveCartCount { get; set; }

		public ViewModel() {
			ViewMode = ViewModes.Normal;
			PageTitle = "";
			NavigationLocation = "";
		
		}
	}


	public enum ViewModes {
		Normal,
		Embedded,
		Print
	}
	
}