using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web {

	// **************************************
	// ViewModel
	// **************************************
	public class MenuViewModel : ViewModel {

		public IList<MenuItem> MenuItems { get; set; }
		public MenuViewModel() {
			MenuItems = new List<MenuItem>();
		}
	}

	public class MenuItem {

		public string Name { get; set; }
		public string LinkDisplayName { get; set; }
		public string MainMenuNavigationLocation { get; set; }
		public string LinkActionName { get; set; }
		public string LinkControllerName { get; set; }
		public bool IsAdmin { get; set; }
		public string CurrentLocation { get; set; }
		public bool IsCurrent {
			get {
				return Name.Equals(CurrentLocation, StringComparison.InvariantCultureIgnoreCase);
			} 
		}
	}

	
}