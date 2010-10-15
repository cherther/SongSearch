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

				var remoteList = GetRemoteFileList(new MediaVersion[] { MediaVersion.Preview, MediaVersion.Full });

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
							UpdateContentId3Tag(content, file);
							UpdateMediaId3info(media, file);


						} else {
							//							media.IsRemote = false;
							dbContent.ContentMedia.Remove(media);
						}
					}
					DataSession.CommitChanges();
				}

				//				DataSession.CommitChanges();

				Log.Debug("Done syncing");

			}	
		}

		private static void UpdateContentId3Tag(Content content, FileInfo file) {

			ID3Writer.NormalizeTag(file.FullName, content);
		}
		private static ContentMedia UpdateMediaId3info(ContentMedia media, FileInfo file) {

			var id3tag = ID3Reader.GetID3Metadata(file.FullName);
			media.MediaSize = file.Length;
			media.MediaLength = id3tag.MediaLength;
			media.MediaBitRate = id3tag.GetBitRate(file.Length);

			return media;
		}
		// **************************************
		// UploadToRemote
		// **************************************
		public void UploadToRemote(bool checkSize = false, bool onlyNewContent = false, MediaVersion[] mediaVersions = null) {


			Log.Debug(String.Format("Starting Amazon upload at {0}", DateTime.Now));

			using (_amazon = new AmazonCloudService()) {
				
				var query = DataSession.All<Content>();
				
				if (onlyNewContent) {
					query = query.Where(c => c.ContentMedia.Any(m => m.IsRemote == false));
				}
				

				var contents = query.ToList();
				if (contents.Count > 0) {

					Log.Debug("Getting remote file list");

					var remoteList = GetRemoteFileList(mediaVersions ?? new MediaVersion[] { MediaVersion.Preview, MediaVersion.Full });

					//var remoteFolder = _amazon.GetContentPrefix(version);

					Log.Debug("Comparing " + contents.Count + " content items");

					foreach (var content in contents.AsParallel()) {

						Log.Debug("Checking ContentId " + content.ContentId);

						var dbContent = DataSession.Single<Content>(c => c.ContentId == content.ContentId);
						var contentMedia = dbContent.ContentMedia.ToList();

						if (mediaVersions != null) {
							contentMedia = contentMedia.Where(c => mediaVersions.Contains((MediaVersion)c.MediaVersion)).ToList();
						}

						foreach (var media in contentMedia) {

							
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

										Log.Debug(String.Format("Uploaded local file {0}", file.FullName));
			
										media.IsRemote = true;
									}
									catch (Exception ex) {
										Log.Error(ex);

										media.IsRemote = false;
										continue;
									}
								} else {
									Log.Debug(String.Format("File for {0} matches", content.ContentId));
									media.IsRemote = true;
								}
							} else {
								// file is not local, let's see if it's remote
								var remoteFile = remoteMedia.SingleOrDefault(x => x.Key == key);
								if (remoteFile != null) {
									//if (checkSize) {
										//RepushMedia(content, media, filePath, file);
										media.IsRemote = true;
									//}
								} else {
									media.IsRemote = false;
								}
							}
						}
						DataSession.CommitChanges();
					}

					//				DataSession.CommitChanges();
					Log.Debug(String.Format("Completed Amazon upload at {0}", DateTime.Now));
				}
				
			}	
		}

		// **************************************
		// DownloadMissingFiles
		// **************************************
		private void RepushMedia(Content content, ContentMedia media, string filePath, FileInfo file) {

			_amazon.GetContentMedia(filePath, media);
			var tempFile = new FileInfo(filePath);

			Log.Debug(String.Format("Remote file for {0} downloaded", content.ContentId));

			UpdateContentId3Tag(content, tempFile);
			UpdateMediaId3info(media, tempFile);

			_amazon.PutContentMedia(tempFile.FullName, media);
			
			Log.Debug(String.Format("Re-uploaded remote file  {0}", file.FullName));

			//File.Delete(tempFile.FullName);

		}



		// **************************************
		// DownloadMissingFiles
		// **************************************
		public void DownloadMissingFiles() {


			using (_amazon = new AmazonCloudService()) {

				var contents = DataSession.GetObjectQuery<Content>()
					.Include("ContentMedia").ToList();

				var remoteList = GetRemoteFileList(new MediaVersion[] { MediaVersion.Preview, MediaVersion.Full });
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
									Log.Debug(file.FullName + " downloaded");

								}
								catch (Exception ex) {
									Log.Error(ex);
								}
							} else {

								Log.Debug(file.FullName + " can't be found!");
							}
						}
					}
				}

				//DataSession.CommitChanges();

				Log.Debug("Done downloading");

			}
		}
		
		
		private Dictionary<MediaVersion, IList<RemoteContent>> GetRemoteFileList(MediaVersion[] versions) {

			var remoteList = new Dictionary<MediaVersion, IList<RemoteContent>>();
			versions.ForEach(v => 
				remoteList.Add(v, _amazon.GetContentList(v))
			);
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
		