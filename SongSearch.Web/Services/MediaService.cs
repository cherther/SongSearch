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
	public static class RemoteMediaConfiguration {

		public static string AccessKeyID {
			get {
				return Settings.AWSAccessKey.Value();
			}
		}
		public static string SecretAccessKeyID {
			get {
				return Settings.AWSSecretKey.Value();
			}
		}
		public static string BucketName {
			get {
				return Settings.AWSMediaBucket.Value();
			}
		}
		public static string MediaUrlFormat {
			get {
				return Settings.MediaUrlFormat.Value();
			}
		}
	}

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

			if (SystemSetting.UseRemoteMedia) {

				return GetContentMediaRemoteBytes(contentId, version);
	
			} else { 
				var assetFile = new FileInfo(GetContentMediaFilePath(contentId, version));

				if (assetFile.Exists) {
					var assetBytes = File.ReadAllBytes(assetFile.FullName);

					return assetBytes;
				} else {
					throw new ArgumentOutOfRangeException("Content media file is missing");
				}
			}

		}
		public static byte[] GetContentMedia(int contentId, MediaVersion version, User user) {

			var bytes = GetContentMedia(contentId, version);

			var content = SearchService.GetContent(contentId, user);
				
			var tempPath = String.Concat(Settings.ZipPath.Value(), "\\", Guid.NewGuid(), ".mp3");

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

			var assetPath = version == MediaVersion.Full ? Settings.MediaPathFull.Value() : Settings.MediaPathPreview.Value();
			return Path.Combine(assetPath, GetContentMediaFileName(contentId));

		}

		// **************************************
		// GetContentMediaFilePath
		// **************************************
		public static string GetContentMediaRemoteUrl(int contentId, MediaVersion version) {

			using (var awsclient = Amazon.AWSClientFactory.CreateAmazonS3Client(
				RemoteMediaConfiguration.AccessKeyID, 
				RemoteMediaConfiguration.SecretAccessKeyID)) {

				var key = String.Format(RemoteMediaConfiguration.MediaUrlFormat, version, contentId);

				var request = new GetPreSignedUrlRequest()
					.WithProtocol(Protocol.HTTP)
					.WithBucketName(RemoteMediaConfiguration.BucketName)
					.WithKey(key)
					.WithExpires(DateTime.Now.Date.AddDays(365));

				return awsclient.GetPreSignedURL(request);
			}

		}
		// **************************************
		// GetContentMediaFilePath
		// **************************************
		public static byte[] GetContentMediaRemoteBytes(int contentId, MediaVersion version) {

			byte[] mediaBytes = null;

			using (var awsclient = Amazon.AWSClientFactory.CreateAmazonS3Client(
				RemoteMediaConfiguration.AccessKeyID,
				RemoteMediaConfiguration.SecretAccessKeyID)) {

				var key = String.Format(RemoteMediaConfiguration.MediaUrlFormat, version, contentId);

				var request = new GetObjectRequest()
					.WithBucketName(RemoteMediaConfiguration.BucketName)
					.WithKey(key);

				try {
					using (S3Response response = awsclient.GetObject(request)) {
						using (Stream s = response.ResponseStream) {
							using (var fs = new MemoryStream()) {
								byte[] data = new byte[32768];
								int bytesRead = 0;
								do {
									bytesRead = s.Read(data, 0, data.Length);
									fs.Write(data, 0, bytesRead);
								}
								while (bytesRead > 0);

								mediaBytes = fs.ToArray();
								return mediaBytes;
							}
						}
					}
				}
				catch (AmazonS3Exception amazonS3Exception) {
					if (amazonS3Exception.ErrorCode != null &&
						(amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
						amazonS3Exception.ErrorCode.Equals("InvalidSecurity"))) {
						App.Logger.Error("Please check the provided AWS Credentials.");
					} else {
						App.Logger.Error(String.Format("An error occurred with the message '{0}' when writing an object",
							amazonS3Exception.Message));
					}
					return mediaBytes;
				}

				
			}

		}
		public static void SaveContentMediaRemote(string filePath, int contentId, MediaVersion version) {

			using (var awsclient = Amazon.AWSClientFactory.CreateAmazonS3Client(RemoteMediaConfiguration.AccessKeyID,
				RemoteMediaConfiguration.SecretAccessKeyID)) {

					var key = String.Format(RemoteMediaConfiguration.MediaUrlFormat, version, contentId);
					try {
						// simple object put
						PutObjectRequest request = new PutObjectRequest();
						request.WithBucketName(RemoteMediaConfiguration.BucketName)
							.WithFilePath(filePath)
							.WithKey(key)
							.WithStorageClass(S3StorageClass.ReducedRedundancy);

						awsclient.PutObject(request);

					}
					catch (AmazonS3Exception amazonS3Exception) {
						if (amazonS3Exception.ErrorCode != null &&
							(amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
							amazonS3Exception.ErrorCode.Equals("InvalidSecurity"))) {
							App.Logger.Error("Please check the provided AWS Credentials.");
						} else {
							App.Logger.Error(String.Format("An error occurred with the message '{0}' when writing an object", 
								amazonS3Exception.Message));
						}
					}

		
			}

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