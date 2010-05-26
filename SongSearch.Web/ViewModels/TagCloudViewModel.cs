using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web {

	// **************************************
	// ViewModel
	// **************************************
	public class TagCloudViewModel {

		public string TagClass { get; set; }
		public string TagIdTemplate { get; set; }
		public int InitialTagNumber { get; set; }
		public int[] SelectedTags { get; set; }
		public TagType TagType { get; set; }
		public string TagTypeName { get; set; }
		public IList<Tag> Tags { get; set; }
	}

	
}