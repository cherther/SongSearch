using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;
using System.IO;
using IdSharp.Tagging.ID3v2;

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
		public static byte[] GetContentMedia(int contentId, MediaVersion version, User user) {
			var bytes = GetContentMedia(contentId, version);

			if (!user.AppendSignatureToTitle) {
				return bytes;
			} else {

				var content = SearchService.GetContent(contentId, user);
				var sig = user.FileSignature(content);
				var tempPath = String.Concat(Settings.ZipPath.Value(), "\\", Guid.NewGuid(), ".mp3");

				File.WriteAllBytes(tempPath, bytes);
				var tag = ID3v2Helper.CreateID3v2(tempPath);

				tag.Title = String.Format("{0} - {1}", content.Title, sig);
				tag.Artist = content.Artist;
				
				ID3v2Helper.RemoveTag(tempPath);
				tag.Save(tempPath);
				var assetFile = new FileInfo(tempPath);

				if (assetFile.Exists) {
					var assetBytes = File.ReadAllBytes(tempPath);
					File.Delete(tempPath);
					return assetBytes;
				} else {
					throw new ArgumentOutOfRangeException("Content media file is missing");
				}
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

			var assetPath = version == MediaVersion.FullSong ? Settings.MediaPathFullSong.Value() : Settings.MediaPathPreview.Value();
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