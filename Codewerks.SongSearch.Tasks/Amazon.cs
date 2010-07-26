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
				foreach (var file in mediaDir.GetFiles()) {
					Console.WriteLine("Uploading " + file.Name);
					UploadFile(mediaBucket, file, mediaFolder);

				}
			}

		}

		private static void UploadFile(string mediaBucket, FileInfo file, string mediaFolder) {
			try {
				// simple object put
				PutObjectRequest request = new PutObjectRequest();
				request.WithBucketName(mediaBucket)
					.WithFilePath(file.FullName)
					.WithKey(String.Concat(mediaFolder, "/", file.Name))
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
		}
	
	
	}
}
		