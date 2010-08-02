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
		public byte[] GetContentMedia(Content content, MediaVersion version)
		{
			return SystemConfig.UseRemoteMedia && content.IsMediaOnRemoteServer ?
				_mediaCloudService.GetContentMedia(content, version)
				: GetContentMediaLocal(content, version);
		}

		public byte[] GetContentMedia(Content content, MediaVersion version, User user) {

			var bytes = GetContentMedia(content, version);

			var tempPath = String.Concat(SystemConfig.ZipPath, "\\", Guid.NewGuid(), ".mp3");

			File.WriteAllBytes(tempPath, bytes);
			// id3
			ID3Writer.UpdateUserTag(tempPath, content, user);
				
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
		private byte[] GetContentMediaLocal(Content content, MediaVersion version)
		{

			var assetFile = new FileInfo(GetContentMediaPath(content, version));

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
		// GetContentMediaFileName
		// **************************************
		public string GetContentMediaFileName(int contentId) {

			return String.Concat(contentId, SystemConfig.MediaDefaultExtension);
	
		}

		// **************************************
		// GetContentMediaFilePath
		// **************************************
		public string GetContentMediaPath(Content content, MediaVersion version) {

			return SystemConfig.UseRemoteMedia && content.IsMediaOnRemoteServer ?
					_mediaCloudService.GetContentMediaUrl(content, version)
					: GetContentMediaPathLocal(content, version);

		}

		private string GetContentMediaPathLocal(Content content, MediaVersion version)
		{

			var assetPath = version == MediaVersion.Full ? SystemConfig.MediaPathFull : SystemConfig.MediaPathPreview;
			return Path.Combine(assetPath, GetContentMediaFileName(content.ContentId));

		}


		public void SaveContentMedia(string filePath, Content content, MediaVersion version) {
			ID3Writer.NormalizeTag(filePath, content);
			var mediaPath = GetContentMediaPath(content, version);
			FileSystem.SafeMove(filePath, mediaPath, true);
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