using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace SongSearch.Web {
	public static class Files {
		public static void Delete(string path) {

			if (File.Exists(path)) {
				File.Delete(path);
			}

		}
	}
}