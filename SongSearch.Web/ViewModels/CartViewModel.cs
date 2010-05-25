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
	}

	
}