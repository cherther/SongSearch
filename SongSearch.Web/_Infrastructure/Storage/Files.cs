﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Security.AccessControl;

namespace SongSearch.Web {
	public static class FileSystem {

		public static DateTime GetMediaDate(this FileInfo fi) {
			return fi.LastWriteTime > DateTime.MinValue ? fi.LastWriteTime : DateTime.Now;
		}

		public static void CreateFolder(string path) {

			DirectoryInfo dInfo = new DirectoryInfo(path);
			if (!dInfo.Exists) {
				//DirectorySecurity dSecurity = dInfo.Parent.GetAccessControl();
				dInfo.Create();
			}
		}
		public static void SafeDelete(string path, bool withArchive = false) {

			if (File.Exists(path)) {
				if (withArchive) { Archive(path); };		
				File.Delete(path);
			}

		}
		
		public static void SafeDeleteFolder(string path) {

			if (Directory.Exists(path)) {
				try {
					Directory.Delete(path, true);
				}
				catch { }
			}

		}
		public static void SafeCopy(string file, string target, bool withArchive = false) {
			if (File.Exists(target)) {

				Archive(target);
			}

			File.Copy(file, target);
		}
		public static void SafeMove(string file, string target, bool withArchive = false) {
			if (File.Exists(target)) {

				Archive(target);				
			}

			File.Move(file, target);
		}

		public static void Archive(string file) {
			if (File.Exists(file)) {

				var archiveFolder = Path.Combine(Path.GetDirectoryName(file), "Archive");

                if (!Directory.Exists(archiveFolder))
                {
                    Directory.CreateDirectory(archiveFolder);
                }

                var archiveFile = Path.Combine(
                            archiveFolder,
							String.Concat(
								Path.GetFileNameWithoutExtension(file),
								"_",
								DateTime.Now.Year.ToString(),
								DateTime.Now.Month.ToString(),
								DateTime.Now.Day.ToString(),
								DateTime.Now.Hour.ToString(),
								DateTime.Now.Minute.ToString(),
								DateTime.Now.Second.ToString(),
								Path.GetExtension(file)
								)
							);

                File.Move(file, archiveFile);
			}
		}
	}
}