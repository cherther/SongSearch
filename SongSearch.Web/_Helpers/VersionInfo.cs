using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace SongSearch.Web {

	// **************************************
	// VersionInfo
	// **************************************
	public static class VersionInfo
	{
		public static string BuildVersion() {

			return Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}

		public static string BuildDate() {

			return System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToShortDateString();
		}
		public static string BuildTime() {

			return System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToShortTimeString();
		}
	}
}