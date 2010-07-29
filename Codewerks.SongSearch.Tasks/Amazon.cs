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
		static AmazonCloudService _amazon;

		public static void UpdateMediaRemoteStatus()
		{
			using (_amazon = new AmazonCloudService()) {

				using (var session = new SongSearchDataSession()) {
					var contents = session.All<Content>();
					var remoteContents = _amazon.GetContentList(MediaVersion.Full);

					foreach (var content in contents) {
						string key = _amazon.GetContentKey(content, MediaVersion.Full);
						var remoteObject = remoteContents.FirstOrDefault(x => x.Key == key);

						Console.WriteLine(String.Format("{0} '{1}' - {2}", content.ContentId,
							content.Title,
							remoteObject != null ? "synced!" : "----------------------> missing"));

						if (remoteObject != null) {
							content.IsMediaOnRemoteServer = true;
						} else {
							content.IsMediaOnRemoteServer = false;

						}
					}

					session.CommitChanges();
				}
			}
		}

		public static void Upload(MediaVersion version) {

		
			using (_amazon = new AmazonCloudService()) {

				using (var session = new SongSearchDataSession()) {
					
					var contents = session.All<Content>().ToList();
					var remoteContents = _amazon.GetContentList(version);
					var remoteFolder = _amazon.GetContentPrefix(version);
						
					foreach (var content in contents) {
						var dbContent = session.Single<Content>(c => c.ContentId == content.ContentId);
						var key = _amazon.GetContentKey(content, version);
						var filePath = Path.Combine(version == MediaVersion.Full ?
							SystemConfig.MediaPathFull :
							SystemConfig.MediaPathPreview,
							String.Concat(content.ContentId,SystemConfig.MediaDefaultExtension)
							);

						var file = new FileInfo(filePath);
						if (file.Exists) {
							var remoteFile = remoteContents.SingleOrDefault(x => x.Key == key && x.Size == file.Length);

							if (remoteFile == null) {

								

								try {
									Console.WriteLine("Uploading " + file.Name + " to " + remoteFolder);
									_amazon.SaveContentMedia(file.FullName, content, version);
									dbContent.IsMediaOnRemoteServer = true;
								}
								catch {
									Console.WriteLine("FAILED: " + file.Name + "-------------------");
									dbContent.IsMediaOnRemoteServer = false;
								}
							} else {
								dbContent.IsMediaOnRemoteServer = true;
							}
						} else {
							dbContent.IsMediaOnRemoteServer = false;
						}
						session.CommitChanges();
					}

					

					Console.WriteLine("Done uploading");
				
				}
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
		