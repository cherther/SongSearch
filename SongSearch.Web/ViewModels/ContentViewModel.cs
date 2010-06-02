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


	public class ContentRightViewModel {

		public int ContentRightId { get; set; }
		public int ContentId { get; set; }
		public RightsTypes RightsTypeId { get; set; }
//		public int RightsHolderId { get; set; }
		public string RightsHolderName { get; set; }
		public string RightsHolderShare { get; set; }

		public IList<int> Territories { get; set; }
	}
	
	public class ContentRightTerritoryViewModel {
		//public int ContentRightId { get; set; }
		public int TerritoryId { get; set; }
	}

	public class ContentTagViewModel {
		public int TagType { get; set; }
		public IList<int> Tags { get; set; }
	}
}