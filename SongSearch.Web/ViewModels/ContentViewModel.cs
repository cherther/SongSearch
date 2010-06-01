using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web {
	public class ContentViewModel : ViewModel {

		public Content Content { get; set; }
		public bool UserCanEdit { get; set; }
		public bool IsEdit { get; set; }
		public IList<string> SectionsAllowed { get; set; }
		public IList<SearchField> SearchFields { get; set; }
		public IList<Tag> Tags { get; set; }
		public IList<Territory> Territories { get; set; }
	}
}