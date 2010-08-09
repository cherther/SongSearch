using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
//using IdSharp.Tagging.ID3v2;
using SongSearch.Web;
using SongSearch.Web.Data;
using SongSearch.Web.Services;

namespace SongSearch.Web.Tasks {

	// **************************************
	// AmazonRemoteMedia
	// **************************************
	public class AmazonRemoteMedia : BaseService, IDisposable {

		private bool _disposed;

		private AmazonCloudService _amazon;

		public AmazonRemoteMedia(IDataSession dataSession, IDataSessionReadOnly readSession)
			: base(dataSession, readSession) {
		}

		public AmazonRemoteMedia(string activeUserIdentity) : base(activeUserIdentity) { }

		// **************************************
		// UpdateMediaRemoteStatus
		// **************************************
		public void UpdateContentMedia(bool checkSize = false)
		{
			using (_amazon = new AmazonCloudService()) {

				var contents = DataSession.All<Content>().ToList();

				var remoteList = GetRemoteFileList();

				//var remoteFolder = _amazon.GetContentPrefix(version);

				foreach (var content in contents.AsParallel()) {

					var dbContent = DataSession.Single<Content>(c => c.ContentId == content.ContentId);

					foreach (var media in dbContent.ContentMedia.AsParallel()) {

						var key = _amazon.GetContentKey(media);
						var remoteMedia = remoteList[(MediaVersion)media.MediaVersion];

						var filePath = Path.Combine((MediaVersion)media.MediaVersion == MediaVersion.Full ?
							SystemConfig.MediaPathFull :
							SystemConfig.MediaPathPreview,
							String.Concat(content.ContentId, SystemConfig.MediaDefaultExtension)
							);

						var file = new FileInfo(filePath);

						if (file.Exists) {
							var hasRemoteFile = checkSize ? remoteMedia.Any(x => x.Key == key && x.Size == file.Length)
								: remoteMedia.Any(x => x.Key == key);

							media.IsRemote = hasRemoteFile;

							var id3tag = ID3Reader.GetID3Metadata(file.FullName);
							
							media.MediaSize = file.Length;
							media.MediaLength = id3tag.MediaLength;
							media.MediaBitRate = id3tag.GetBitRate(file.Length);


						} else {
							//							media.IsRemote = false;
							dbContent.ContentMedia.Remove(media);
						}
					}
					DataSession.CommitChanges();
				}

				//				DataSession.CommitChanges();

				Debug.WriteLine("Done syncing");

			}	
		}

		// **************************************
		// UploadToRemote
		// **************************************
		public void UploadToRemote(bool checkSize = false) {

		
			using (_amazon = new AmazonCloudService()) {

				var contents = DataSession.All<Content>().ToList();

				var remoteList = GetRemoteFileList();

				//var remoteFolder = _amazon.GetContentPrefix(version);

				foreach (var content in contents) {

					var dbContent = DataSession.Single<Content>(c => c.ContentId == content.ContentId);

					foreach (var media in dbContent.ContentMedia) {

						var key = _amazon.GetContentKey(media);
						var remoteMedia = remoteList[(MediaVersion)media.MediaVersion];

						var filePath = Path.Combine((MediaVersion)media.MediaVersion == MediaVersion.Full ?
							SystemConfig.MediaPathFull :
							SystemConfig.MediaPathPreview,
							String.Concat(content.ContentId, SystemConfig.MediaDefaultExtension)
							);

						var file = new FileInfo(filePath);

						if (file.Exists) {
							var remoteFile = checkSize ? remoteMedia.SingleOrDefault(x => x.Key == key && x.Size == file.Length)
								: remoteMedia.SingleOrDefault(x => x.Key == key);

							if (remoteFile == null) {

								try {
									_amazon.PutContentMedia(file.FullName, media);
									media.IsRemote = true;
								}
								catch (Exception ex) {
									App.Logger.Error(ex);
									media.IsRemote = false;
								}
							} else {
								media.IsRemote = true;
							}
						} else {
//							media.IsRemote = false;
							dbContent.ContentMedia.Remove(media);
						}
					}
					DataSession.CommitChanges();
				}
					
//				DataSession.CommitChanges();

				Debug.WriteLine("Done uploading");
				
			}	
		}



		// **************************************
		// DownloadMissingFiles
		// **************************************
		public void DownloadMissingFiles() {


			using (_amazon = new AmazonCloudService()) {

				var contents = DataSession.GetObjectQuery<Content>()
					.Include("ContentMedia").ToList();

				var remoteList = GetRemoteFileList();
				//var remoteFolder = _amazon.GetContentPrefix(version);

				foreach (var content in contents) {

					foreach (var media in content.ContentMedia) {

						var key = _amazon.GetContentKey(media);
						var remoteMedia = remoteList[(MediaVersion)media.MediaVersion];

						var filePath = content.Media((MediaVersion)media.MediaVersion).MediaFilePath(true);

						var file = new FileInfo(filePath);

						// there's supposed to be a local file
						if (!file.Exists) {
							var remoteFile = remoteMedia.SingleOrDefault(x => x.Key == key);
							// let's see if we have it on the remote server
							if (remoteFile != null) {

								try {

									_amazon.GetContentMedia(file.FullName, media);
									Debug.WriteLine(file.FullName + " downloaded");

								}
								catch (Exception ex) {
									App.Logger.Error(ex);
								}
							} else {

								Debug.WriteLine(file.FullName + " can't be found!");
							}
						}
					}
				}

				//DataSession.CommitChanges();

				Debug.WriteLine("Done downloading");

			}
		}
		
		
		private Dictionary<MediaVersion, IList<RemoteContent>> GetRemoteFileList() {

			var remoteList = new Dictionary<MediaVersion, IList<RemoteContent>>();

			remoteList.Add(MediaVersion.Preview, _amazon.GetContentList(MediaVersion.Preview));
			remoteList.Add(MediaVersion.Full, _amazon.GetContentList(MediaVersion.Full));
			return remoteList;
		}

		// ----------------------------------------------------------------------------
		// Dispose
		// ----------------------------------------------------------------------------

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}


		private void Dispose(bool disposing) {
			if (!_disposed) {
				{
					if (DataSession != null) {
						DataSession.Dispose();
						DataSession = null;
					}
				}

				_disposed = true;
			}
		}
	
	}
}
		