using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web {
	// **************************************
	// CatalogViewModel
	// **************************************
	public class CatalogViewModel : ViewModel {

		public bool AllowEdit { get; set; }
		public IList<Catalog> MyCatalogs { get; set; }
		public IList<Invitation> MyInvites { get; set; }
		public IList<User> MyUsers { get; set; }
		public int[] Roles { get; set; }
		public int[] CatalogRoles { get; set; }
		public int HierarchyLevel { get; set; }
		public Catalog Catalog { get; set; }
		public IList<Content> CatalogContents { get; set; }
	}
}