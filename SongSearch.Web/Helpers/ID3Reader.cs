using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using IdSharp.Tagging.ID3v2;

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

			var id3 = new ID3Data();
			var tag = ID3v2Helper.CreateID3v2(fileName);

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

		public static ID3Data GetID3MetadataV2(string fileName) {

			// Reads the MP3 tag details
			var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			var br = new BinaryReader(fs);
			var id3 = new ID3Data();

			try {
				// Move to the end of the stream - 127 places from the end...
				br.BaseStream.Seek(-128, SeekOrigin.End);

				//'Begin reading the data
				var tag = new String(br.ReadChars(3));

				//'Does a tag exist?
				if (tag == "TAG") {
					id3.Title = new String(br.ReadChars(30)).TrimEnd('\0');
					id3.Artist = new String(br.ReadChars(30)).TrimEnd('\0');
					id3.Album = new String(br.ReadChars(30)).TrimEnd('\0');
					id3.Year = new String(br.ReadChars(4)).TrimEnd('\0');
				}
				return id3;
			}
			finally {
				//Cleanup
				br.Close();
				fs.Close();
				br = null;
				fs = null;
			}
		}

		public static ID3Data GetID3MetadataV4(string fileName) {

			// Reads the MP3 tag details
			var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			var br = new BinaryReader(fs);
			var id3 = new ID3Data();

			try {
				//byte[] tag = new byte[128];
				//var pos = br.BaseStream.Read(tag, 0, 128);
				//var tagChars = new String(br.ReadChars(128));
				
				//'Begin reading the data
				var header = new String(br.ReadChars(10)).TrimEnd('\0');
				
				//'Does a tag exist?
				if (header.StartsWith("ID3")) {
					id3.Title = new String(br.ReadChars(30)).TrimEnd('\0');
					id3.Artist = new String(br.ReadChars(30)).TrimEnd('\0');
					id3.Album = new String(br.ReadChars(30)).TrimEnd('\0');
					id3.Year = new String(br.ReadChars(4)).TrimEnd('\0');
				}
				return id3;
			}
			finally {
				//Cleanup
				br.Close();
				fs.Close();
				br = null;
				fs = null;
			}
		}
		
	}
}