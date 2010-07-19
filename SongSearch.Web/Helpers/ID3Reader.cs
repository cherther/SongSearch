using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using IdSharp.Tagging.ID3v2;
using SongSearch.Web.Data;

namespace SongSearch.Web {

	public class ID3Data {
		public string Title { get; set; }
		public string Artist { get; set; }
		public string Album { get; set; }
		public string Year { get; set; }
		public long? MediaSize { get; set; }
		public long? MediaLength { get; set; }
		public int? MediaBitRate { get; set; }
		public string MediaType { get; set; }
	}
	public class ID3Reader {

		public static ID3Data GetID3Metadata(string fileName) {


			

			try {
				var tag = ID3v2Helper.CreateID3v2(fileName);
				return GetId3Data(tag);
			}
			finally { }
			
		}

		public static ID3Data GetId3Data(IID3v2 tag) {
			var id3 = new ID3Data();
			//'Does a tag exist?
			id3.Artist = tag.Artist;
			id3.Title = tag.Title;
			id3.Album = tag.Album;
			//id3.Genre = tag.Genre;
			id3.Year = tag.OriginalReleaseYear.AsNullIfWhiteSpace() ?? tag.Year;
			id3.MediaLength = tag.LengthMilliseconds;
			id3.MediaSize = tag.FileSizeExcludingTag;

			return id3;
		}		
	}

	public class ID3Writer {

		public static ID3Data NormalizeTag(string filePath, Content content) {

			var file = new FileInfo(filePath);
			if (file.Exists) {
				var oldTag = ID3v2Helper.CreateID3v2(file.FullName);
				var newTag = ID3v2Helper.CreateID3v2();
				newTag.Title = content.Title;
				newTag.Artist = content.Artist;
				newTag.OriginalArtist = content.Artist;
				newTag.Album = oldTag.Album;
				newTag.Year = content.ReleaseYear.HasValue ? content.ReleaseYear.Value.ToString() : "";
				newTag.OriginalReleaseYear = newTag.Year;
				newTag.LengthMilliseconds = oldTag.LengthMilliseconds;
				newTag.FileSizeExcludingTag = oldTag.FileSizeExcludingTag;

				ID3v2Helper.RemoveTag(file.FullName);
				newTag.Save(file.FullName);

				return ID3Reader.GetId3Data(newTag);

			} else {
				return new ID3Data();
			}
		}

		public static void UpdateUserTag(string filePath, Content content, User user) {

			var sig = user.AppendToTitleData(content);
			var tag = ID3v2Helper.CreateID3v2(filePath);
			
			if (!String.IsNullOrWhiteSpace(sig)) {
				tag.Title = !String.IsNullOrWhiteSpace(sig) ? String.Format("{0} - {1}", content.Title, sig) : content.Title;
			}
			
			tag.Artist = content.Artist;

			ID3v2Helper.RemoveTag(filePath);
			tag.Save(filePath);
				
		}

	}
}