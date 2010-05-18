using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SongSearch.Web;
using SongSearch.Web.Data;
using SongSearch.Web.Services;

namespace Codewerks.SongSearch.Tasks {
	public class Indexer {
		
		public static void Index() {

			using (ISession session = new EFSession()) {

				var contents = session.All<Content>().Where(x => x.Lyrics != null);

				foreach (var content in contents) {

					content.LyricsIndex = content.Lyrics.MakeIndexValue();
				}
				session.CommitChanges();
			}
	
		}
	}
}
		