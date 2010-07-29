using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amazon.S3.Model;
using Amazon.S3;
using System.IO;
using SongSearch.Web.Data;
using System.Runtime.Remoting.Messaging;

namespace SongSearch.Web.Services {

	// **************************************
	// MediaServiceRemote
	// **************************************
	public class zMediaServiceRemote : IMediaService {

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

		public byte[] GetContentMedia(Content content, MediaVersion version, User user) {

			var bytes = GetContentMedia(content, version);

			//var content = SearchService.GetContent(contentId, user);

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
		}

		public string GetContentMediaFileName(int contentId) {
			return String.Concat(contentId, SystemConfig.MediaDefaultExtension);
		}

		public string GetContentMediaPath(Content content, MediaVersion version) {
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

		private delegate void SaveContentMediaRemoteDelegate(string fromFilePath, Content content, MediaVersion version);
		delegate void EndInvokeDelegate(IAsyncResult result);
		public void SaveContentMedia(string fromFilePath, Content content, MediaVersion version) {

			SaveContentMediaRemote(fromFilePath, content, version);
			//SaveContentMediaRemoteDelegate saveContentMediaDelegate = new SaveContentMediaRemoteDelegate(SaveContentMediaRemote);
			//// Define the AsyncCallback delegate.
			//AsyncCallback callBack = new AsyncCallback(this.SaveContentMediaCallback);

			//saveContentMediaDelegate.BeginInvoke(fromFilePath, content, version,
			//    callBack,
			//    content.ContentId
			//    );

		}

		public void SaveContentMediaCallback(IAsyncResult asyncResult) {
			// Extract the delegate from the 
			// System.Runtime.Remoting.Messaging.AsyncResult.
			SaveContentMediaRemoteDelegate saveContentMediaDelegate = (SaveContentMediaRemoteDelegate)((AsyncResult)asyncResult).AsyncDelegate;
			int cartId = (int)asyncResult.AsyncState;

			saveContentMediaDelegate.EndInvoke(asyncResult);
		}

		public void SaveContentMediaRemote(string fromFilePath, Content content, MediaVersion version) {
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