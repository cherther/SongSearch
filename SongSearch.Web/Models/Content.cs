using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SongSearch.Web.Data {
	public partial class Content {
		public bool IsInMyActiveCart { get; set; }
		public string UserDownloadableName { get; set; }
	}

	public partial class ContentUserDownloadable {
		public int ContentId { get; set; }
		public string DownloadableName { get; set; }
	}
}