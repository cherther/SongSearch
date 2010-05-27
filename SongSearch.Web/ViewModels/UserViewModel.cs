using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web {
	
	// **************************************
	// UserViewModel
	// **************************************
	public class UserViewModel : ViewModel {

		public bool AllowEdit { get; set; }
		public bool IsThisUser { get; set; }
		public IList<User> MyUsers { get; set; }
		public IList<Invitation> MyInvites { get; set; }
		public IList<Catalog> Catalogs { get; set; }
		public int[] Roles { get; set; }
		public int[] CatalogRoles { get; set; }

	}
}