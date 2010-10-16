using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web {

	// **************************************
	// PricingPlansViewModel
	// **************************************
	// **************************************
	// ContactUsModel
	// **************************************
	public class PricingPlansViewModel : ViewModel {

		public PricingPlan MyPricingPlan { get; set; }
		public UserBalances MyUserBalances { get; set; }
		public PricingPlans SelectedPricingPlan { get; set; }
	}

	
	
}