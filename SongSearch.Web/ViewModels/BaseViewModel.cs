using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;
using SongSearch.Web.Services;

namespace SongSearch.Web {

	// **************************************
	// ViewModel
	// **************************************
	public class ViewModel {
		//public SiteProfile SiteProfile { get; set; }
		public ViewModes ViewMode { get; set; }
		public EditModes EditMode { get; set; }
		public string PageTitle { get; set; }
		public string PageMessage { get; set; }
		public string[] NavigationLocation { get; set; }
		public int MyActiveCartCount { get; set; }
		public ModelAction ModelAction { get; set; }
		public int ActiveUserId { get; set; }
		public bool ShowQuotaWidget { get; set; }

		public ViewModel(int? siteProfileId = null) {
            //SiteProfile = siteProfileId.HasValue ? SiteProfileData.SiteProfile(siteProfileId.Value, true) : SiteProfileData.SiteProfile();
			ViewMode = ViewModes.Normal;
			EditMode = EditModes.Viewing;
			PageTitle = "";
			PageMessage = "";
			NavigationLocation = new string [] {""};
		}

		//public class Lookups {
		private IList<PricingPlan> _pricingPlans;

		public IList<PricingPlan> PricingPlans {
			get {
				return _pricingPlans ?? CacheService.PricingPlans();
			}
			set {
				_pricingPlans = value;
			}
				
		}
		//}
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