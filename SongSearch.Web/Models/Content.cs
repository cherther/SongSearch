using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

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
		public List<UploadFile> UploadFiles { get; set; }
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

		public object CreatedByUserId { get; set; }
		public object CreatedOn { get; set; }
		public object LastUpdatedByUserId { get; set; }
		public object LastUpdatedOn { get; set; }

		[DisplayName("All-in")]
		public object IsControlledAllIn { get; set; }

		[DisplayName("Song Excerpt")]
		public object HasMediaPreviewVersion { get; set; }

		[DisplayName("Full Song")]
		public object HasMediaFullVersion { get; set; }

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

		[DataType(DataType.MultilineText)]
		public object Lyrics { get; set; }

		[HiddenInput(DisplayValue = false)]
		public object LyricsIndex { get; set; }

		[DataType(DataType.MultilineText)]
		public object Notes { get; set; }

		[DisplayName("Similar Songs")]
		public object SimilarSongs { get; set; }

		[DisplayName("Licensing Notes")]
		[DataType(DataType.MultilineText)]
		public object LicensingNotes { get; set; }

		[DisplayName("Date Uploaded")]
		public object MediaDate { get; set; }

		[DisplayName("Bitrate")]
		public object MediaBitRate { get; set; }

		[DisplayName("Media Type")]
		public object MediaType { get; set; }

		[DisplayName("Size")]
		public object MediaSize { get; set; }

		[DisplayName("Media Length")]
		public object MediaLength { get; set; }

		[DisplayName("Instruments")]
		[DataType(DataType.MultilineText)]
		public object Instruments { get; set; }

		[DisplayName("Sounds Like")]
		[DataType(DataType.MultilineText)]
		public object SoundsLike { get; set; }

	}

	public partial class ContentUserDownloadable {
		public int ContentId { get; set; }
		public string DownloadableName { get; set; }
	}

	public class UploadFile {
		public string FileName { get; set; }
		public string FilePath { get; set; }
		public MediaVersion FileMediaVersion { get; set; }
	}


}