﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;
using System.IO;
using IdSharp.Tagging.ID3v2;
using System.Configuration;
using Amazon.S3.Model;
using Amazon.S3;
using Amazon.CloudFront.Model;

namespace SongSearch.Web.Services {

	// **************************************
	// RemoteMediaConfiguration
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

	// **************************************
	// RemoteContent
	// **************************************
	public class RemoteContent {

		public string Key { get; set; }
		public DateTime LastUpdatedOn { get; set; }
		public long Size { get; set; }
//		public string Key { get; set; }
	}

	// **************************************
	// AmazonExtensions
	// **************************************
	public static class AmazonExtensions {

		public static string AmazonExceptionMessage(this AmazonS3Exception amazonS3Exception) {
			if (amazonS3Exception.IsAmazonSecurityException()) {
				return @"Please check the provided AWS Credentials. If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3";
			} else {
				return amazonS3Exception.Message;
			}
		}

		public static bool IsAmazonSecurityException(this AmazonS3Exception amazonS3Exception) {
			return amazonS3Exception.ErrorCode != null &&
								(amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
								amazonS3Exception.ErrorCode.Equals("InvalidSecurity"));
		}
	}


	// **************************************
	// AmazonCloudService
	// **************************************
	public class AmazonCloudService : IMediaCloudService {

		// **************************************
		// GetContentKey
		// **************************************
		public string GetContentKey(ContentMedia contentMedia) {
			return String.Concat(GetContentPrefix((MediaVersion)contentMedia.MediaVersion),
				contentMedia.ContentId, SystemConfig.MediaDefaultExtension);
		}

		// **************************************
		// GetContentPrefix
		// **************************************
		public string GetContentPrefix(MediaVersion version) {
			return String.Format(SystemConfig.MediaFolderUrlFormat, version.ToString());
		}

		// **************************************
		// GetContentList
		// **************************************
		public IList<RemoteContent> GetContentList(MediaVersion version, string filter = null) {

			var mediaFolder = GetContentPrefix(version);
			var s3objects = GetBucketList(mediaFolder, RemoteMediaConfiguration.BucketName, filter);

			return s3objects.Select(x => new RemoteContent() { 
				Key = x.Key, 
				LastUpdatedOn = DateTime.Parse(x.LastModified),
				Size = x.Size
			}).ToList();
		}

		// **************************************
		// GetBucketList
		// **************************************
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
				if (response.IsTruncated) {

					list = list.Union(GetBucketList(mediaFolder, mediaBucket, response.NextMarker)).ToList();
				}
				return list;
			}
		
		}

		// **************************************
		// GetContentMedia
		// **************************************
		public void GetContentMedia(string target, ContentMedia contentMedia) {

			

			using (var awsclient = Amazon.AWSClientFactory.CreateAmazonS3Client(
				RemoteMediaConfiguration.AccessKeyID,
				RemoteMediaConfiguration.SecretAccessKeyID)) {

				var key = GetContentMediaKey(contentMedia);

				var request = new GetObjectRequest()
					.WithBucketName(RemoteMediaConfiguration.BucketName)
					.WithKey(key);

				try {
					using (S3Response response = awsclient.GetObject(request)) {
						using (Stream s = response.ResponseStream) {

							using (var fs = File.OpenWrite(target)) {
								byte[] data = new byte[262144];//[32768];
								int bytesRead = 0;
								do {
									bytesRead = s.Read(data, 0, data.Length);
									fs.Write(data, 0, bytesRead);
								}
								while (bytesRead > 0);

								fs.Flush();
								fs.Close();
							}
						}
					}
				}
				catch (AmazonS3Exception amazonS3Exception) {
					if (amazonS3Exception.IsAmazonSecurityException()) {
						Log.Error(amazonS3Exception.AmazonExceptionMessage());
					} else {
						Log.Error(amazonS3Exception);
					}
				}


			}
		
		}

		// **************************************
		// GetContentMedia
		// **************************************
		public byte[] GetContentMedia(ContentMedia contentMedia) {

			byte[] mediaBytes = null;

			using (var awsclient = Amazon.AWSClientFactory.CreateAmazonS3Client(
				RemoteMediaConfiguration.AccessKeyID,
				RemoteMediaConfiguration.SecretAccessKeyID)) {

				var key = GetContentMediaKey(contentMedia);
				
				var request = new GetObjectRequest()
					.WithBucketName(RemoteMediaConfiguration.BucketName)
					.WithKey(key);

				try {
					using (S3Response response = awsclient.GetObject(request)) {
						using (Stream s = response.ResponseStream) {
							using (var fs = new MemoryStream()) {
								byte[] data = new byte[contentMedia.MediaSize.GetValueOrDefault(262144)];
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
					if (amazonS3Exception.IsAmazonSecurityException()) {
						Log.Error(amazonS3Exception.AmazonExceptionMessage());
					} else {
						Log.Error(amazonS3Exception);
					}
					return mediaBytes;
				}


			}
		}

		// **************************************
		// GetContentMediaUrl
		// **************************************
		public string GetContentMediaUrl(ContentMedia contentMedia) {
		
			using (var awsclient = Amazon.AWSClientFactory.CreateAmazonS3Client(
				RemoteMediaConfiguration.AccessKeyID,
				RemoteMediaConfiguration.SecretAccessKeyID)) {

				var key = GetContentMediaKey(contentMedia);
				
				var request = new GetPreSignedUrlRequest()
					.WithProtocol(Amazon.S3.Model.Protocol.HTTP)
					.WithBucketName(RemoteMediaConfiguration.BucketName)
					.WithKey(key)
					.WithExpires(DateTime.Now.Date.AddDays(365));
				
				return awsclient.GetPreSignedURL(request);
			}
		}

		// **************************************
		// PutContentMedia
		// **************************************
		public void PutContentMedia(string source, ContentMedia contentMedia) {
			using (var awsclient = Amazon.AWSClientFactory.CreateAmazonS3Client(RemoteMediaConfiguration.AccessKeyID,
				RemoteMediaConfiguration.SecretAccessKeyID)) {

				var key = GetContentMediaKey(contentMedia);
				try {
					// simple object put
					PutObjectRequest request = new PutObjectRequest();
					request.WithBucketName(RemoteMediaConfiguration.BucketName)
						.WithFilePath(source)
						.WithKey(key)
						.WithStorageClass(S3StorageClass.ReducedRedundancy);

					awsclient.PutObject(request);

				}
				catch (AmazonS3Exception amazonS3Exception) {
					if (amazonS3Exception.IsAmazonSecurityException()) {
						Log.Error(amazonS3Exception.AmazonExceptionMessage());
					} else {
						Log.Error(amazonS3Exception);
					}
					throw amazonS3Exception;
				}
				catch (Exception ex) {
					Log.Error(String.Format("An error occurred with the message '{0}' when writing an object",
						ex.Message));
					throw ex;
				}

			}
		}

		// **************************************
		// GetContentMediaKey
		// **************************************
		private static string GetContentMediaKey(ContentMedia contentMedia) {
			var key = String.Format(RemoteMediaConfiguration.MediaUrlFormat, (MediaVersion)contentMedia.MediaVersion, contentMedia.ContentId);

			return key;
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