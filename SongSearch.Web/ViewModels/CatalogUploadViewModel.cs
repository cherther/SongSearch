using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;
using SongSearch.Web.Services;

namespace SongSearch.Web {
	// **************************************
	// CatalogViewModel
	// **************************************
	public class CatalogUploadViewModel : ViewModel {

		public CatalogUploadState CatalogUploadState { get; set; }
		public IList<Catalog> MyCatalogs { get; set; }
		public string StepView { get; set; }
		public string StepActionName { get; set; }
		public PricingPlan MyPricingPlan { get; set; }
		public UserQuotas MyUserQuotas { get; set; }
	}
}