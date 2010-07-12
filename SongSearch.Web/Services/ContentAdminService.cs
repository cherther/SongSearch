using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;
using Ninject;
using System.IO;
using IdSharp.Tagging.ID3v2;

namespace SongSearch.Web.Services {

	public class ContentAdminService : BaseService, IContentAdminService {

		private bool _disposed;
		
		public ContentAdminService(IDataSession session) : base(session) {}
		public ContentAdminService(string activeUserIdentity): base(activeUserIdentity) { }

		// **************************************
		//  UpdateModelWith
		// **************************************
		public void Update(Content contentModel, 
			IList<int> tagsModel,
			IDictionary<TagType, string> newTagsModel,
			IList<ContentRightViewModel> rightsModel) {

			//UpdateModelWith Content
			var content = DataSession.GetObjectQuery<Content>()
					.Include("Tags")
					.Include("Catalog")
					.Include("ContentRights")
					.Include("ContentRights.Territories")
				.Where(c => c.ContentId == contentModel.ContentId).SingleOrDefault();// && user.UserCatalogRoles.Any(x => x.CatalogId == c.CatalogId)).SingleOrDefault();

			if (content == null) {
				throw new ArgumentOutOfRangeException("Content does not exist");
			}

			content.UpdateModelWith(contentModel);

			//UpdateModelWith Tags
			tagsModel = tagsModel.Where(t => t > 0).ToList();
			// create new tags
			var newTags = CreateTags(newTagsModel);
			tagsModel = tagsModel.Union(newTags).ToList();

			// add to tagsModel
			content = UpdateTags(content, tagsModel);

			//UpdateModelWith Rights
			content = UpdateRights(content, rightsModel);

			DataSession.CommitChanges();

			CacheService.InitializeApp(true);
			//CacheService.CacheUpdate(CacheService.CacheKeys.Content);
			//CacheService.CacheUpdate(CacheService.CacheKeys.Rights);
			//CacheService.CacheUpdate(CacheService.CacheKeys.TopTags);
			//CacheService.CacheUpdate(CacheService.CacheKeys.Tags);
			//CacheService.CacheUpdate(CacheService.CacheKeys.Territories);

		}

		// **************************************
		//  UpdateContentMedia
		// **************************************
		public void UpdateContentMedia(int contentId, IList<UploadFile> uploadFiles) {

			if (contentId > 0) {
				var content = DataSession.Single<Content>(c => c.ContentId == contentId);

				foreach (var file in uploadFiles) {
					if (file.FileName != null) {
						content.HasMediaFullVersion = content.HasMediaFullVersion || file.FileMediaVersion == MediaVersion.FullSong;
						content.HasMediaPreviewVersion = content.HasMediaPreviewVersion || file.FileMediaVersion == MediaVersion.Preview;
						var mediaPath = content.MediaFilePath(file.FileMediaVersion);
						var filePath = Account.User().UploadFile(fileName: file.FileName, mediaVersion: file.FileMediaVersion.ToString());
						FileSystem.SafeMove(filePath, mediaPath, true);
					}
				}

				DataSession.CommitChanges();
			}
		}
		
		// **************************************
		//  SaveMetaDataToFile
		// **************************************
		public void SaveMetaDataToFile(int contentId) {

			var content = DataSession.Single<Content>(c => c.ContentId == contentId);
			if (content != null) {

				var filePath = content.MediaFilePath(MediaVersion.FullSong);
				//var file = new FileInfo(filePath);

				var tag = ID3v2Helper.CreateID3v2(filePath);

				tag.Title = content.Title;
				tag.Artist = content.Artist;
				tag.Year = content.ReleaseYear.HasValue ? content.ReleaseYear.Value.ToString() : tag.Year;
				
				IdSharp.Tagging.ID3v1.ID3v1Helper.RemoveTag(filePath);
				ID3v2Helper.RemoveTag(filePath);
				tag.Save(filePath);
			}

		}

		
		// **************************************
		//  Delete
		// **************************************
		public void Delete(int contentId) {

			var content = DataSession.Single<Content>(c => c.ContentId == contentId);
			if (content != null) {
				FileSystem.SafeDelete(content.MediaFilePath(MediaVersion.Preview));
				FileSystem.SafeDelete(content.MediaFilePath(MediaVersion.FullSong));
				DataSession.Delete<Content>(content);
			}

			DataSession.CommitChanges();
		}

		public void Delete(int[] contentIds) {

			foreach (var contentId in contentIds) {

				var content = DataSession.Single<Content>(c => c.ContentId == contentId);
				if (content != null) {
					FileSystem.SafeDelete(content.MediaFilePath(MediaVersion.Preview));
					FileSystem.SafeDelete(content.MediaFilePath(MediaVersion.FullSong));
					DataSession.Delete<Content>(content);
				}
			}

			DataSession.CommitChanges();

		}

		// **************************************
		//  DeleteTag
		// **************************************
		public void DeleteTag(int tagId) {
			var userId = Account.User().UserId;
			var tag = DataSession.Single<Tag>(t => t.TagId == tagId && t.CreatedByUserId == userId); 
			if (tag != null) {
				DataSession.Delete<Tag>(tag);
				DataSession.CommitChanges();
			}
		}

		// ----------------------------------------------------------------------------
		// Private
		// ----------------------------------------------------------------------------
		// **************************************
		// UpdateTags
		// **************************************    
		private IList<int> CreateTags(IDictionary<TagType, string> newTagModel) {

			IList<int> newTagIds = new List<int> { };
			var tags = DataSession.All<Tag>();//.ToList();
			var newTags = newTagModel.Where(t => !String.IsNullOrWhiteSpace(t.Value));
			foreach (var newTag in newTags) {

				var values = newTag.Value.Split(',').Where(v => !String.IsNullOrWhiteSpace(v)).Select(v => v.Trim());

				foreach (var value in values) {
					var tag = tags.SingleOrDefault(t =>
						t.TagTypeId == (int)newTag.Key &&
						t.TagName.ToUpper().Equals(value.ToUpper())) ??
						new Tag() {
							TagName = value,
							CreatedByUserId = Account.User().UserId,
							CreatedOn = DateTime.Now,
							TagTypeId = (int)newTag.Key
						};

					if (tag.TagId == 0) {

						DataSession.Add<Tag>(tag);
						DataSession.CommitChanges();
						newTagIds.Add(tag.TagId);
					}
				}
			}
			return newTagIds;
		}
			

		// **************************************
		// UpdateTags
		// **************************************    
		private Content UpdateTags(Content content, IList<int> tagsModel) {

			if (tagsModel == null) { return content; }

			var tags = DataSession.All<Tag>().ToList();
			var contentTags = content.Tags.ToList();

			var contentTagsToRemove = contentTags.Where(t => !tagsModel.Contains(t.TagId));
			contentTagsToRemove.ForEach(x => content.Tags.Remove(x));

			var contentTagsToAdd = tagsModel.Except(contentTags.Select(t => t.TagId).Intersect(tagsModel));

			foreach (var contentTag in contentTagsToAdd) {
				var tag = tags.SingleOrDefault(t => t.TagId == contentTag);
				if (tag != null) {
					content.Tags.Add(tag);
				}
			}

			return content;
		}



		// **************************************
		// UpdateRights:
		// **************************************    
		private Content UpdateRights(Content content, IList<ContentRightViewModel> rightsModel) {

			if (rightsModel == null) { return content; }

			var territories = DataSession.All<Territory>().ToList();

			// get rid of empty items or items to delete
			rightsModel = rightsModel.Where(r => r.ModelAction != ModelAction.Delete && r.RightsHolderName != null && r.RightsHolderShare != null).ToList();

			var contentRights = content.ContentRights.ToList();

			var removeRights = contentRights.Where(x => !rightsModel.Select(r => r.ContentRightId).Contains(x.ContentRightId)).ToList();

			foreach (var right in removeRights) {

				var rightTerritories = right.Territories.ToList();
				rightTerritories.ForEach(x => right.Territories.Remove(x));
				content.ContentRights.Remove(right);

				DataSession.Delete<ContentRight>(right);
			}

			foreach (var rm in rightsModel) {

				ContentRight contentRight = contentRights.SingleOrDefault(x => x.ContentRightId == rm.ContentRightId) ??
					new ContentRight() { CreatedByUserId = Account.User().UserId, CreatedOn = DateTime.Now };

				// RightsHolderName
				contentRight.RightsHolderName = rm.RightsHolderName.ToUpper();

				// RightsTypeId
				contentRight.RightsTypeId = (int)rm.RightsTypeId;

				// Share %
				var share = rm.RightsHolderShare.Replace("%", "").Trim();
				decimal shareWhole;

				if (decimal.TryParse(share, out shareWhole)) {
					contentRight.RightsHolderShare = decimal.Divide(Math.Abs(shareWhole), 100);
				}

				// Territories
				var territoryModel = rm.Territories.Where(x => x > 0).ToList();

				var rightTerritories = contentRight.Territories.ToList();
				var removeTerritories = rightTerritories.Where(x => !territoryModel.Contains(x.TerritoryId));
				removeTerritories.ForEach(x => contentRight.Territories.Remove(x));
				var addTerritories = territoryModel.Except(rightTerritories.Select(x => x.TerritoryId).Intersect(territoryModel));

				foreach (var tm in addTerritories) {
					var ter = territories.Single(t => t.TerritoryId == tm);
					contentRight.Territories.Add(ter);
				}

				// Add to collection if a new contentRight
				if (contentRight.ContentRightId == 0) {

					content.ContentRights.Add(contentRight);
				}
			}

			return content;

		}

		// ----------------------------------------------------------------------------
		// (Dispose)
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
					if (ReadSession != null) {
						ReadSession.Dispose();
						ReadSession = null;
					}
				}

				_disposed = true;
			}
		}
	}
}