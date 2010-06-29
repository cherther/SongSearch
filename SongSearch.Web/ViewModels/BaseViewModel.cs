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
		public EditModes EditMode { get; set; }
		public string PageTitle { get; set; }
		public string PageMessage { get; set; }
		public string[] NavigationLocation { get; set; }
		public int MyActiveCartCount { get; set; }
		public ModelAction ModelAction { get; set; }
		public int ActiveUserId { get; set; }

		public ViewModel() {
			ViewMode = ViewModes.Normal;
			EditMode = EditModes.Viewing;
			PageTitle = "";
			PageMessage = "";
			NavigationLocation = new string [] {""};
		}
	}


	public enum ViewModes {
		Normal,
		Embedded,
		Print
	}
	public enum EditModes {
		Viewing,
		Editing,
		Saving,
	}

	public enum ModelAction {
		Update = 0, 
		Add = 1,
		Delete = 2
	}
	
}