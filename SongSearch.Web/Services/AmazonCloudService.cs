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
	public struct RemoteMediaConfiguration {

		public static string AccessKeyID {
			get {
				return SystemConfig.AWSAccessKey;
			}
		}
		public static string SecretAccessKeyID {
			get {
				return SystemConfig.AWSSecretKey;
			}
		}
		public static string BucketName {
			get {
				return SystemConfig.AWSMediaBucket;
			}
		}
		public static string MediaUrlFormat {
			get {
				return SystemConfig.MediaUrlFormat;
			}
		}
	}

	public class RemoteContent {

		public string Key { get; set; }
		public DateTime LastUpdatedOn { get; set; }
		public long Size { get; set; }
//		public string Key { get; set; }
	}

	public class AmazonCloudService : IMediaCloudService {

		public string GetContentKey(Content content, MediaVersion version) {
			return String.Concat(GetContentPrefix(version), content.ContentId, SystemConfig.MediaDefaultExtension);
		}

		public string GetContentPrefix(MediaVersion version) {
			return String.Format(SystemConfig.MediaFolderUrlFormat, version.ToString());
		}
		public IList<RemoteContent> GetContentList(MediaVersion version, string filter = null) {

			var mediaFolder = GetContentPrefix(version);
			var s3objects = GetBucketList(mediaFolder, RemoteMediaConfiguration.BucketName, filter);

			return s3objects.Select(x => new RemoteContent() { 
				Key = x.Key, 
				LastUpdatedOn = DateTime.Parse(x.LastModified),
				Size = x.Size
			}).ToList();
		}

		private IList<S3Object> GetBucketList(string mediaFolder, string mediaBucket, string marker = null) {

			using (var awsclient = Amazon.AWSClientFactory.CreateAmazonS3Client(
				RemoteMediaConfiguration.AccessKeyID,
				RemoteMediaConfiguration.SecretAccessKeyID)) {

				var listRequest = new ListObjectsRequest()
								   .WithBucketName(mediaBucket)
								   .WithPrefix(mediaFolder);//GetAwsKey(file, mediaFolder));
				if (marker != null) {
					listRequest.WithMarker(marker);
				}
				var response = awsclient.ListObjects(listRequest);
				var list = response.S3Objects;
				if (list.Count == 1000) {

					list = list.Union(GetBucketList(mediaFolder, mediaBucket, response.NextMarker)).ToList();
				}
				return list;
			}
		
		}

		// **************************************
		// GetContentMedia
		// **************************************
		public byte[] GetContentMedia(Content content, MediaVersion version) {

			byte[] mediaBytes = null;

			using (var awsclient = Amazon.AWSClientFactory.CreateAmazonS3Client(
				RemoteMediaConfiguration.AccessKeyID,
				RemoteMediaConfiguration.SecretAccessKeyID)) {

				var key = String.Format(RemoteMediaConfiguration.MediaUrlFormat, version, content.ContentId);

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

		public string GetContentMediaUrl(Content content, MediaVersion version) {
		
			using (var awsclient = Amazon.AWSClientFactory.CreateAmazonS3Client(
				RemoteMediaConfiguration.AccessKeyID,
				RemoteMediaConfiguration.SecretAccessKeyID)) {

				var key = String.Format(RemoteMediaConfiguration.MediaUrlFormat, version, content.ContentId);


				var request = new GetPreSignedUrlRequest()
					.WithProtocol(Protocol.HTTP)
					.WithBucketName(RemoteMediaConfiguration.BucketName)
					.WithKey(key)
					.WithExpires(DateTime.Now.Date.AddDays(365));

				return awsclient.GetPreSignedURL(request);
			}
		}

		public void SaveContentMedia(string fromFilePath, Content content, MediaVersion version) {
			using (var awsclient = Amazon.AWSClientFactory.CreateAmazonS3Client(RemoteMediaConfiguration.AccessKeyID,
				RemoteMediaConfiguration.SecretAccessKeyID)) {

				var key = String.Format(RemoteMediaConfiguration.MediaUrlFormat, version, content.ContentId);
				try {
					// simple object put
					PutObjectRequest request = new PutObjectRequest();
					request.WithBucketName(RemoteMediaConfiguration.BucketName)
						.WithFilePath(fromFilePath)
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