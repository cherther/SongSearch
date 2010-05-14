using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SongSearch.Web {
	public class SearchField {
		//  property names intentionally short to reduce querystrings
		public int P { get; set; }
		public SearchTypes T { get; set; }
		public string[] V { get; set; }
	}
}