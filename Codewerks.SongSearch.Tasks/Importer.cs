using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SongSearch.Web;
using SongSearch.Web.Data;
using SongSearch.Web.Services;
using IdSharp.Tagging.ID3v2;
using System.IO;
namespace Codewerks.SongSearch.Tasks {
	public class Importer {

		//private static IQueryable<Tag> _tags;
		//private static IDataSession _session;
		public static void GetID3() {
			string fullpath = @"D:\Inetpub\wwwroot\Assets\Music\Full";
			string previewPath = @"D:\Inetpub\wwwroot\Assets\Music\Previews";

			using (var session = new SongSearchDataSession()) {

				var contents = session.All<Content>();
				
				foreach (var content in contents) {

					string full = Path.Combine(fullpath, String.Concat(content.ContentId, ".mp3"));
					var file = new FileInfo(full);
					if (file.Exists) {
						var tag = ID3v2Helper.CreateID3v2(full);
						System.Diagnostics.Debug.WriteLine(tag.EncoderSettings);

						content.HasMediaFullVersion = true;
						content.FileSize = (int)(file.Length);

						content.FileType = tag.FileType ?? content.FileType;
						var kb = (int)(file.Length * 8 / 1024);
						var secs = (tag.LengthMilliseconds / 1000);
						content.BitRate = secs > 0 ? (kb / secs) : 0;


					} else {
						content.HasMediaFullVersion = false;
					}

					if (File.Exists(Path.Combine(previewPath, String.Concat(content.ContentId, ".mp3")))) {
						content.HasMediaPreviewVersion = true;
					} else {
						content.HasMediaPreviewVersion = false;
					}

				}
//				session.CommitChanges();
			}
			
		}
		public static void MakeTags() {

			//_session = new SongSearchDataSession();
			var ctx = new SongSearchContext();

			var import = ctx.Import_SongData;

			foreach (var row in import) {

				MakeATag(row.SongID, row.SoundsLike, TagType.SoundsLike);
				MakeATag(row.SongID, row.Instrumentation, TagType.Instruments);
				//_session.CommitChanges();
			}
	
		}

		private static void MakeATag(int contentId, string field, TagType tagType) {
			
			if (field != null) {
				
				var fieldvalues = field.Split(',').Select(v => v.Trim()).Where(s => !String.IsNullOrWhiteSpace(s));

				using (var ctx = new SongSearchContext()) {

					var content = ctx.Contents.SingleOrDefault<Content>(x => x.ContentId == contentId);
					if (content != null) {
						foreach (var itm in fieldvalues) {
							var value = itm.Trim();
							Tag tag = ctx.Tags.SingleOrDefault<Tag>(t => t.TagName.Equals(value, StringComparison.InvariantCultureIgnoreCase)) ??
								new Tag() { TagName = value, CreatedByUserId = 1, TagTypeId = (int)tagType }
								;
							if (tag.TagId == 0) {
								ctx.Tags.AddObject(tag);
								ctx.SaveChanges();
							}
							if (!content.Tags.Contains(tag)) {
								content.Tags.Add(tag);
							}
						}
						ctx.SaveChanges();
					}
				}
			}
		}
	}
}
		