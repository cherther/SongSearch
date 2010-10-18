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

	public static class MediaService { //: BaseService, IMediaService {

				// **************************************
		// GetContentMedia
		// **************************************
		public static byte[] GetContentMedia(ContentMedia contentMedia)
		{
			return SystemConfig.UseRemoteMedia && contentMedia.IsRemote ?
				AmazonCloudService.GetContentMedia(contentMedia)
				: GetContentMediaLocal(contentMedia);
		}

		public static byte[] GetContentMedia(ContentMedia contentMedia, User user) {

			//try {

				return GetContentMedia(contentMedia);

            //}
			//catch (Exception ex) {
			//    Log.Error(ex);
			//    throw ex;
			//}
		}

		public static byte[] WriteMediaSignature(byte[] mediaFile, Content content, User user) {
        
        		var tempPath = String.Concat(SystemConfig.ZipPath, "\\", Guid.NewGuid(), ".mp3");

				File.WriteAllBytes(tempPath, mediaFile);
				// id3
				
				ID3Writer.UpdateUserTag(tempPath, content, user);

				var assetFile = new FileInfo(tempPath);

				if (assetFile.Exists) {
					var assetBytes = File.ReadAllBytes(tempPath);
					File.Delete(tempPath);
					return assetBytes;
				} else {
					var ex = new ArgumentOutOfRangeException("Content media file is missing");
					Log.Error(ex);
					throw ex;
				}
		
        }

		private static byte[] GetContentMediaLocal(ContentMedia contentMedia)
		{

			var assetFile = new FileInfo(GetContentMediaPathLocal(contentMedia));
			
			if (assetFile.Exists)
			{
				var assetBytes = File.ReadAllBytes(assetFile.FullName);

				return assetBytes;
			}
			else
			{
				var ex = new ArgumentOutOfRangeException("Content media file is missing");
				Log.Error(ex);
				throw ex;
			}
		}

		

		// **************************************
		// GetContentMediaFilePath
		// **************************************
		public static string GetContentMediaPath(ContentMedia contentMedia) {

			return SystemConfig.UseRemoteMedia && contentMedia.IsRemote ?
					AmazonCloudService.GetContentMediaUrl(contentMedia)
					: GetContentMediaPathLocal(contentMedia);

		}

		public static string GetContentMediaUrl(ContentMedia contentMedia) {

			return SystemConfig.UseRemoteMedia && contentMedia.IsRemote ?
					AmazonCloudService.GetContentMediaUrl(contentMedia)
					: GetContentMediaUrlLocal(contentMedia);

		}

		// **************************************
		// SaveContentMedia
		// **************************************
		public static void SaveContentMedia(string filePath, ContentMedia contentMedia) {
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

			return String.Format("{0}/Media/Stream/{1}?version={2}", 
				"", //SystemConfig.BaseUrl,
				contentMedia.ContentId.ToString(), 
				(MediaVersion)contentMedia.MediaVersion);
			
		}

	}

}