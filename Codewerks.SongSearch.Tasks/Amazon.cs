using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SongSearch.Web;
using SongSearch.Web.Data;
using SongSearch.Web.Services;
using IdSharp.Tagging.ID3v2;
using System.IO;
using Amazon.S3.Model;
using Amazon.S3;
using System.Configuration;
using System.Collections.Specialized;
using Amazon;
namespace Codewerks.SongSearch.Tasks {
	public class AmazonAWS {

		static string _accessKeyID = "";
		static string _secretAccessKeyID = "";
		static string _bucketName = "songsearchassets";
		static string _mediaPath = "";

		static AmazonS3 client = null;

		public static void Upload(string mediaFolder) {

			NameValueCollection appConfig = ConfigurationManager.AppSettings;

			_accessKeyID = appConfig["AWSAccessKey"];
			_secretAccessKeyID = appConfig["AWSSecretKey"];
			_mediaPath= appConfig[mediaFolder];
			var mediaBucket = _bucketName;// String.Concat(_bucketName, mediaFolder);

			var mediaDir = new DirectoryInfo(_mediaPath);

			using (client = AWSClientFactory.CreateAmazonS3Client(_accessKeyID, _secretAccessKeyID)) {

				var remoteObjects = GetBucketList(mediaFolder, mediaBucket);

				foreach (var file in mediaDir.GetFiles()) {

					var remoteFile = remoteObjects.SingleOrDefault(x => x.Key == GetAwsKey(file, mediaFolder) && x.Size == file.Length);
					
					if (remoteFile == null) {
						Console.WriteLine("Uploading " + file.Name + " to " + mediaFolder);
						UploadFile(mediaBucket, file, mediaFolder);
					}			
				}

				Console.WriteLine("Done uploading");
			}

		}

		private static List<S3Object> GetBucketList(string mediaFolder, string mediaBucket, string marker= null) {

			var listRequest = new ListObjectsRequest()
							   .WithBucketName(mediaBucket)
							   .WithPrefix(mediaFolder);//GetAwsKey(file, mediaFolder));
			if (marker != null) {
				listRequest.WithMarker(marker);
			}
			var response = client.ListObjects(listRequest);
			var list = response.S3Objects;
			if (list.Count == 1000) {

				list = list.Union(GetBucketList(mediaFolder, mediaBucket, response.NextMarker)).ToList();
			}

			return list;
		}

		private static void UploadFile(string mediaBucket, FileInfo file, string mediaFolder) {
			try {
				// simple object put
				PutObjectRequest request = new PutObjectRequest();
				request.WithBucketName(mediaBucket)
					.WithFilePath(file.FullName)
					.WithKey(GetAwsKey(file, mediaFolder))					
					.WithStorageClass(S3StorageClass.ReducedRedundancy);

				S3Response response = client.PutObject(request);

				response.Dispose();

			}
			catch (AmazonS3Exception amazonS3Exception) {
				if (amazonS3Exception.ErrorCode != null &&
					(amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
					amazonS3Exception.ErrorCode.Equals("InvalidSecurity"))) {
					Console.WriteLine("Please check the provided AWS Credentials.");
					Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
				} else {
					Console.WriteLine("An error occurred with the message '{0}' when writing an object", amazonS3Exception.Message);
				}
			}
			catch (Exception ex) {

				Console.WriteLine("An error occurred with the message '{0}' when writing an object", ex.Message);

			}
		}

		private static string GetAwsKey(FileInfo file, string mediaFolder) {
			return String.Concat(mediaFolder, "/", file.Name);
		}
	
	
	}
}
		