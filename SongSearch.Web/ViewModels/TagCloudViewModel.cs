using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web {

	// **************************************
	// ViewModel
	// **************************************
	public class TagCloudViewModel<T> : ViewModel {
		public string TagClass { get; set; }
		public string TagIdTemplate { get; set; }
		public int TagCountSeed { get; set; }
		public int InitialTagNumber { get; set; }
		public TagType TagType { get; set; }
		public string TagTypeName { get; set; }
		public int[] SelectedTags { get; set; }
		public IList<T> Tags { get; set; }
	}


	
}