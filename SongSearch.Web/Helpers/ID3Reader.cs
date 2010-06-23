using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace SongSearch.Web {

	public class ID3Data {
		public string Title { get; set; }
		public string Artist { get; set; }
		public string Album { get; set; }
		public string Year { get; set; }
	}
	public class ID3Reader {

		public static ID3Data GetID3Metadata(string fileName) {

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

		
	}
}