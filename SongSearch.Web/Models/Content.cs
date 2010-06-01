using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SongSearch.Web.Data {

	[MetadataType(typeof(CatalogMetaData))]
	public partial class Catalog {
	}
	public class CatalogMetaData {

		[DisplayName("Catalog")]
		[Required]
		public object CatalogId { get; set; }

		[DisplayName("Catalog")]
		[Required]
		public object CatalogName { get; set; }
	}

	[MetadataType(typeof(ContentMetaData))]
	public partial class Content {
		public bool IsInMyActiveCart { get; set; }
		public string UserDownloadableName { get; set; }
	}

	public class ContentMetaData {
		//[Required]
		//public object Title { get; set; }

		//[Required]
		//[StringLength(5)]
		//public object Director { get; set; }


		[DisplayName("Song Code")]
		[Required]
		public object ContentId { get; set; }

		[DisplayName("Catalog")]
		[Required]
		public object CatalogId { get; set; }

		[DisplayName("All-in")]
		public object IsControlledAllIn { get; set; }
			
		[DisplayName("Artist")]
		[Required]
		public object Artist { get; set; }

		[DisplayName("Pop Charts")]
		public object Pop { get; set; }

		[DisplayName("Country Charts")]
		public object Country { get; set; }
			
		[DisplayName("Release Year")]
		public object ReleaseYear { get; set; }

		[DisplayName("Record Label")]
		public object RecordLabel { get; set; }

		[DisplayName("Similar Songs")]
		public object SimilarSongs { get; set; }

		[DisplayName("Licensing Notes")]
		public object LicensingNotes { get; set; }

	}

	public partial class ContentUserDownloadable {
		public int ContentId { get; set; }
		public string DownloadableName { get; set; }
	}




}