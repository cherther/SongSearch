using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;
using System.IO;

namespace SongSearch.Web.Services {

	// **************************************
	// MediaService
	// **************************************
	public class MediaService : IDisposable {

		// **************************************
		// ContentMediaExtension
		// **************************************
		public static string ContentMediaExtension {

			get { return ".mp3"; }
		}

		// **************************************
		// GetContentMedia
		// **************************************
		public static byte[] GetContentMedia(int contentId, MediaVersion version) {

			var assetFile = new FileInfo(GetContentMediaFilePath(contentId, version));

			if (assetFile.Exists) {
				var assetBytes = File.ReadAllBytes(assetFile.FullName);

				return assetBytes;
			} else {
				throw new ArgumentOutOfRangeException("Content media file is missing");
			}

		}

		// **************************************
		// GetContentMediaFileName
		// **************************************
		public static string GetContentMediaFileName(int contentId) {

			return String.Concat(contentId, ContentMediaExtension);
	
		}

		// **************************************
		// GetContentMediaFilePath
		// **************************************
		public static string GetContentMediaFilePath(int contentId, MediaVersion version) {

			var assetPath = version == MediaVersion.FullSong ? Settings.MediaPathFullSong.Text() : Settings.MediaPathPreview.Text();
			return Path.Combine(assetPath, GetContentMediaFileName(contentId));

		}
		
		// ----------------------------------------------------------------------------
		// (Dispose)
		// ----------------------------------------------------------------------------
		private bool _disposed;

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool disposing) {
			if (!_disposed) {
				// If disposing equals true, dispose all managed
				// and unmanaged resources.if (disposing)
				{
					//if (DataSession != null) {
					//    DataSession.Dispose();
					//    DataSession = null;
					//}
				}

				// Call the appropriate methods to clean up
				// unmanaged resources here.
				//CloseHandle(handle);
				//handle = IntPtr.Zero;

				_disposed = true;
			}
		}
	}

}