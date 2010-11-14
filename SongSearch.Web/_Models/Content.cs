using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using SongSearch.Web.Services;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
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
		public Contact LicensingContact { get; set; }

		private bool _hasMediaFullVersion;

		public bool HasMediaFullVersion {

			get {

				_hasMediaFullVersion =
					this.ContentMedia != null &&
					this.ContentMedia.SingleOrDefault(x => x.MediaVersion == (int)MediaVersion.Full) != null;

				return _hasMediaFullVersion;
			}
			set {
				_hasMediaFullVersion = value;
			}
		}
		private bool _hasMediaPreviewVersion;
		public bool HasMediaPreviewVersion {

			get {

				_hasMediaPreviewVersion =
					this.ContentMedia != null &&
					this.ContentMedia.SingleOrDefault(x => x.MediaVersion == (int)MediaVersion.Preview) != null;

				return _hasMediaPreviewVersion;
			}
			set {
				_hasMediaPreviewVersion = value;
			}
		}

		private bool _isMediaOnRemoteServer;
		public bool IsMediaOnRemoteServer {

			get {

				_isMediaOnRemoteServer =
					this.HasMediaFullVersion &&
					this.ContentMedia.Single(x => x.MediaVersion == (int)MediaVersion.Full).IsRemote;


				return _isMediaOnRemoteServer;
			}
			set {
				_isMediaOnRemoteServer = value;
			}
		}

		private DateTime _mediaDate;
		public DateTime MediaDate {

			get {

				_mediaDate =
					this.HasMediaFullVersion ?
					this.ContentMedia.Single(x => x.MediaVersion == (int)MediaVersion.Full).MediaDate :
					DateTime.Now;


				return _mediaDate;
			}
			set {
				_mediaDate = value;
			}
		}

		
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

		[DisplayName("Created By")]
		public object CreatedByUserId { get; set; }

		[DisplayName("Created")]
		public object CreatedOn { get; set; }

		[DisplayName("Last Updated By")]
		public object LastUpdatedByUserId { get; set; }

		[DisplayName("Last Updated")]
		public object LastUpdatedOn { get; set; }

		[DisplayName("100% All-in Master & Composition")]
		public object IsControlledAllIn { get; set; }

		[DisplayName("Song Excerpt")]
		public object HasMediaPreviewVersion { get; set; }

		[DisplayName("Full Song")]
		public object HasMediaFullVersion { get; set; }

		[DisplayName("Artist")]
		[Required]
		public object Artist { get; set; }

		[DisplayName("Billboard Pop Chart")]
		public object Pop { get; set; }

		[DisplayName("Billboard Country Chart")]
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

		[DisplayName("Feat. Instruments")]
		[DataType(DataType.MultilineText)]
		public object Instruments { get; set; }

		[DisplayName("Sounds Like")]
		[DataType(DataType.MultilineText)]
		public object SoundsLike { get; set; }

		[DisplayName("Media File Date")]
		public object MediaDate { get; set; }

		[DisplayName("Licensing Contact")]
		public object LicensingContact { get; set; }

	}

	[MetadataType(typeof(ContentMediaMetadata))]
	public partial class ContentMedia {
	}
	public class ContentMediaMetadata {

		[DisplayName("Media File Date")]
		public object MediaDate { get; set; }

		[DisplayName("Bitrate")]
		public object MediaBitRate { get; set; }

		[DisplayName("Media Type")]
		public object MediaType { get; set; }

		[DisplayName("Size")]
		public object MediaSize { get; set; }

		[DisplayName("Media Length")]
		public object MediaLength { get; set; }

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