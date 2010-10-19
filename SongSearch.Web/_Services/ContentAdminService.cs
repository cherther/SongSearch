using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;
using Ninject;
using System.IO;
using IdSharp.Tagging.ID3v2;

namespace SongSearch.Web.Services {

	public static class ContentAdminService { //: BaseService, IContentAdminService {

		// **************************************
		//  Update
		// **************************************
		public static void Update(Content contentModel, 
			IList<int> tagsModel,
			IDictionary<TagType, string> newTagsModel,
			IList<ContentRepresentationUpdateModel> representationModel) {

				using (var ctx = new SongSearchContext()) {
					//UpdateModelWith Content
					var content = ctx.Contents
							.Include("Tags")
							.Include("Catalog")
							.Include("ContentRepresentations")
							.Include("ContentRepresentations.Territories")
						.Where(c => c.ContentId == contentModel.ContentId).SingleOrDefault();// && user.UserCatalogRoles.Any(x => x.CatalogId == c.CatalogId)).SingleOrDefault();

					if (content == null) {
						throw new ArgumentOutOfRangeException("Content does not exist");
					}

					content.UpdateModelWith(contentModel);

					//UpdateModelWith Tags
					tagsModel = tagsModel.Where(t => t > 0).ToList();
					// create new tags
					var newTags = ctx.CreateTags(newTagsModel);
					tagsModel = tagsModel.Union(newTags).ToList();

					// add to tagsModel
					content = ctx.UpdateTags(content, tagsModel);

					//UpdateModelWith Representation
					content = ctx.UpdateRepresentation(content, representationModel);

					content.LastUpdatedByUserId = Account.User().UserId;
					content.LastUpdatedOn = DateTime.Now;

					ctx.SaveChanges();

					CacheService.InitializeApp(true);
					//CacheService.CacheUpdate(CacheService.CacheKeys.Content);
					//CacheService.CacheUpdate(CacheService.CacheKeys.Rights);
					//CacheService.CacheUpdate(CacheService.CacheKeys.TopTags);
					//CacheService.CacheUpdate(CacheService.CacheKeys.Tags);
					//CacheService.CacheUpdate(CacheService.CacheKeys.Territories);
				}
		}

		// **************************************
		//  UpdateContentMedia
		// **************************************
		public static void UpdateContentMedia(int contentId, IList<UploadFile> uploadFiles) {

			if (contentId > 0) {

				using (var ctx = new SongSearchContext()) {
					var content = ctx.Contents.SingleOrDefault(c => c.ContentId == contentId);

					foreach (var uploadFile in uploadFiles) {

						if (uploadFile.FileName != null) {

							var filePath = Account.User().UploadFile(uploadFile.FileName, uploadFile.FileMediaVersion.ToString());
							var file = new FileInfo(filePath);
							var id3 = ID3Reader.GetID3Metadata(filePath);

							content.IsMediaOnRemoteServer = false;

							var media = content.ContentMedia.SingleOrDefault(x => x.MediaVersion == (int)uploadFile.FileMediaVersion) ??
								new ContentMedia();

							media.MediaVersion = (int)uploadFile.FileMediaVersion;
							media.MediaType = "mp3";
							media.MediaSize = file.Length;
							media.MediaLength = id3.MediaLength;
							media.MediaDate = file.GetMediaDate();
							media.MediaBitRate = id3.GetBitRate(file.Length);


							media.IsRemote = false;

							if (media.ContentId == 0) {
								content.ContentMedia.Add(media);
							}

							MediaService.SaveContentMedia(filePath, media);
						}
					}
					content.LastUpdatedByUserId = Account.User().UserId;
					content.LastUpdatedOn = DateTime.Now;

					ctx.SaveChanges();
				}
			}
		}
		
		// **************************************
		//  SaveMetaDataToFile
		// **************************************
		public static void SaveMetaDataToFile(int contentId) {

			using (var ctx = new SongSearchContext()) {
				var content = ctx.Contents.SingleOrDefault(c => c.ContentId == contentId);
				
				if (content != null) {

					var filePath = content.ContentMedia.PreviewVersion().MediaFilePath(true);
					//var file = new FileInfo(filePath);
					if (File.Exists(filePath)) {
						var tag = ID3v2Helper.CreateID3v2(filePath);

						tag.Title = content.Title;
						tag.Artist = content.Artist;
						tag.Year = content.ReleaseYear.HasValue ? content.ReleaseYear.Value.ToString() : tag.Year;

						IdSharp.Tagging.ID3v1.ID3v1Helper.RemoveTag(filePath);
						ID3v2Helper.RemoveTag(filePath);
						tag.Save(filePath);
					}
				}
			}

		}

		
		// **************************************
		//  Delete
		// **************************************
		public static void Delete(int contentId) {
			using (var ctx = new SongSearchContext()) {
				ctx.DeleteContent(contentId);

				ctx.SaveChanges();
			}
		}

		

		public static void Delete(int[] contentIds) {
			using (var ctx = new SongSearchContext()) {
				foreach (var contentId in contentIds) {

					ctx.DeleteContent(contentId);
				}

				ctx.SaveChanges();
			}
		}

		// **************************************
		//  DeleteTag
		// **************************************
		public static void DeleteTag(int tagId) {
			var userId = Account.User().UserId;
			using (var ctx = new SongSearchContext()) {
				var tag = ctx.Tags.SingleOrDefault(t => t.TagId == tagId && t.CreatedByUserId == userId);
				if (tag != null) {
					ctx.Tags.DeleteObject(tag);
					ctx.SaveChanges();
				}
			}
		}

		// ----------------------------------------------------------------------------
		// Private
		// ----------------------------------------------------------------------------
		// **************************************
		// UpdateTags
		// **************************************    
		private static IList<int> CreateTags(this SongSearchContext ctx, IDictionary<TagType, string> newTagModel) {

			IList<int> newTagIds = new List<int> { };
			
			var newTags = newTagModel.Where(t => !String.IsNullOrWhiteSpace(t.Value));
			foreach (var newTag in newTags) {

				var values = newTag.Value.Split(',').Where(v => !String.IsNullOrWhiteSpace(v)).Select(v => v.Trim());

				foreach (var value in values) {
					var tag = ctx.Tags.SingleOrDefault(t =>
						t.TagTypeId == (int)newTag.Key &&
						t.TagName.ToUpper().Equals(value.ToUpper())) ??
						new Tag() {
							TagName = value.CamelCase(),
							CreatedByUserId = Account.User().UserId,
							CreatedOn = DateTime.Now,
							TagTypeId = (int)newTag.Key
						};

					if (tag.TagId == 0) {

						ctx.Tags.AddObject(tag);
						ctx.SaveChanges();
						newTagIds.Add(tag.TagId);
					}
				}
			}
			return newTagIds;
		}
			

		// **************************************
		// UpdateTags
		// **************************************    
		private static Content UpdateTags(this SongSearchContext ctx, Content content, IList<int> tagsModel) {

			if (tagsModel == null) { return content; }

			var contentTags = content.Tags.ToList();

			var contentTagsToRemove = contentTags.Where(t => !tagsModel.Contains(t.TagId));
			contentTagsToRemove.ForEach(x => content.Tags.Remove(x));

			var contentTagsToAdd = tagsModel.Except(contentTags.Select(t => t.TagId).Intersect(tagsModel));

			foreach (var contentTag in contentTagsToAdd) {
				var tag = ctx.Tags.SingleOrDefault(t => t.TagId == contentTag);
				if (tag != null) {
					content.Tags.Add(tag);
				}
			}

			return content;
		}


		// **************************************
		// DeleteContent:
		// **************************************    
		private static void DeleteContent(this SongSearchContext ctx, int contentId) {
			var content = ctx.Contents.SingleOrDefault(c => c.ContentId == contentId);
			if (content != null) {
				foreach (var media in content.ContentMedia) {

					FileSystem.SafeDelete(media.MediaFilePath(true));

				}
				var user = Account.User();

				ctx.Contents.DeleteObject(content);
				ctx.RemoveFromSongsBalance(user);

			}
		}

		// **************************************
		// UpdateRepresentation:
		// **************************************    
		private static Content UpdateRepresentation(this SongSearchContext ctx, Content content, IList<ContentRepresentationUpdateModel> representationModel) {

			if (representationModel == null) { return content; }

			var territories = ctx.Territories.ToList();

			// get rid of empty items or items to delete
			representationModel = representationModel.Where(r => r.ModelAction != ModelAction.Delete 
				//&& r.RightsHolderName != null
				&& r.RepresentationShare != null
				).ToList();

			var contentRepresentations = content.ContentRepresentations.ToList();

			var removeReps = contentRepresentations.Where(x => !representationModel
				.Select(r => r.ContentRepresentationId)
				.Contains(x.ContentRepresentationId)
				)
				.ToList();

			foreach (var rep in removeReps) {

				var repTerritories = rep.Territories.ToList();
				repTerritories.ForEach(x => rep.Territories.Remove(x));
				content.ContentRepresentations.Remove(rep);

				ctx.ContentRepresentations.DeleteObject(rep);
			}

			foreach (var rm in representationModel) {

				ContentRepresentation contentRepresentation = contentRepresentations.SingleOrDefault(x => x.ContentRepresentationId == rm.ContentRepresentationId) ??
					new ContentRepresentation() { 
						CreatedByUserId = Account.User().UserId, 
						CreatedOn = DateTime.Now };

				// RightsHolderName
				//contentRight.RightsHolderName = rm.RightsHolderName.AsEmptyIfNull().ToUpper();

				// RightsTypeId
				contentRepresentation.RightsTypeId = (int)rm.RightsTypeId;

				// Share %
				var share = rm.RepresentationShare.Replace("%", "").Trim();
				decimal shareWhole;

				if (decimal.TryParse(share, out shareWhole)) {
					contentRepresentation.RepresentationShare = decimal.Divide(Math.Abs(shareWhole), 100);
				}

				// Territories
				var territoryModel = rm.Territories.Where(x => x > 0).ToList();

				var repTerritories = contentRepresentation.Territories.ToList();
				var removeTerritories = repTerritories.Where(x => !territoryModel.Contains(x.TerritoryId));
				removeTerritories.ForEach(x => contentRepresentation.Territories.Remove(x));
				var addTerritories = territoryModel.Except(repTerritories.Select(x => x.TerritoryId).Intersect(territoryModel));

				foreach (var tm in addTerritories) {
					var ter = territories.Single(t => t.TerritoryId == tm);
					contentRepresentation.Territories.Add(ter);
				}

				// Add to collection if a new contentRight
				if (contentRepresentation.ContentRepresentationId == 0 && contentRepresentation.RepresentationShare > 0) {					
					content.ContentRepresentations.Add(contentRepresentation);
				}
			}

			return content;

		}

	}
}