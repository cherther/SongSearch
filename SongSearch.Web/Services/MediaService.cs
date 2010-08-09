using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;
using System.IO;
using IdSharp.Tagging.ID3v2;
using System.Configuration;
using Amazon.S3.Model;
using Amazon.S3;

namespace SongSearch.Web.Services {

	// **************************************
	// MediaService
	// **************************************
	
	public class MediaService : BaseService, IMediaService {

		IMediaCloudService _mediaCloudService;

		public MediaService(IDataSession dataSession, IDataSessionReadOnly readSession, IMediaCloudService mediaCloudService)
			: base(dataSession, readSession) {
				_mediaCloudService = mediaCloudService;
		}

		// **************************************
		// GetContentMedia
		// **************************************
		public byte[] GetContentMedia(ContentMedia contentMedia)
		{
			return SystemConfig.UseRemoteMedia && contentMedia.IsRemote ?
				_mediaCloudService.GetContentMedia(contentMedia)
				: GetContentMediaLocal(contentMedia);
		}

		public byte[] GetContentMedia(ContentMedia contentMedia, User user) {

			var bytes = GetContentMedia(contentMedia);

			var tempPath = String.Concat(SystemConfig.ZipPath, "\\", Guid.NewGuid(), ".mp3");

			File.WriteAllBytes(tempPath, bytes);
			// id3
			//ID3Writer.UpdateUserTag(tempPath, content, user);
				
			var assetFile = new FileInfo(tempPath);

			if (assetFile.Exists) {
				var assetBytes = File.ReadAllBytes(tempPath);
				File.Delete(tempPath);
				return assetBytes;
			} else {
				throw new ArgumentOutOfRangeException("Content media file is missing");
			}
			//}
		}
		private byte[] GetContentMediaLocal(ContentMedia contentMedia)
		{

			var assetFile = new FileInfo(GetContentMediaPathLocal(contentMedia));
			
			if (assetFile.Exists)
			{
				var assetBytes = File.ReadAllBytes(assetFile.FullName);

				return assetBytes;
			}
			else
			{
				throw new ArgumentOutOfRangeException("Content media file is missing");
			}
		}

		

		// **************************************
		// GetContentMediaFilePath
		// **************************************
		public string GetContentMediaPath(ContentMedia contentMedia) {

			return SystemConfig.UseRemoteMedia && contentMedia.IsRemote ?
					_mediaCloudService.GetContentMediaUrl(contentMedia)
					: GetContentMediaPathLocal(contentMedia);

		}

		public string GetContentMediaUrl(ContentMedia contentMedia) {

			return SystemConfig.UseRemoteMedia && contentMedia.IsRemote ?
					_mediaCloudService.GetContentMediaUrl(contentMedia)
					: GetContentMediaUrlLocal(contentMedia);

		}

		// **************************************
		// SaveContentMedia
		// **************************************
		public void SaveContentMedia(string filePath, ContentMedia contentMedia) {
			//ID3Writer.NormalizeTag(filePath, content);
			var mediaPath = GetContentMediaPathLocal(contentMedia);
			FileSystem.SafeMove(filePath, mediaPath, true);
		}


		// **************************************
		// GetContentMediaFileName
		// **************************************
		public static string GetContentMediaFileName(int contentId) {

			return String.Concat(contentId, SystemConfig.MediaDefaultExtension);

		}
		public static string GetContentMediaPathLocal(ContentMedia contentMedia)
		{

			var assetPath = (MediaVersion)contentMedia.MediaVersion == MediaVersion.Full ? 
				SystemConfig.MediaPathFull : 
				SystemConfig.MediaPathPreview;
			return Path.Combine(assetPath, GetContentMediaFileName(contentMedia.ContentId));

		}
		public static string GetContentMediaUrlLocal(ContentMedia contentMedia) {

			return String.Format("/Media/Stream/{0}?version={1}", contentMedia.ContentId.ToString(), (MediaVersion)contentMedia.MediaVersion);
			
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