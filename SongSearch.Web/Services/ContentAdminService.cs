using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;
using Ninject;

namespace SongSearch.Web.Services {

	public class ContentAdminService : BaseService, IContentAdminService {

		private bool _disposed;
		
		public ContentAdminService(IDataSession session) : base(session) {}
		public ContentAdminService(string activeUserIdentity): base(activeUserIdentity) { }

		// **************************************
		//  UpdateModelWith
		// **************************************
		public void Update(Content contentModel, IList<int> tagsModel, IList<ContentRightViewModel> rightsModel) {

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
			content = UpdateTags(content, tagsModel);

			//UpdateModelWith Rights
			content = UpdateRights(content, rightsModel);

			DataSession.CommitChanges();

			CacheService.CacheUpdate(CacheService.CacheKeys.Content);
			CacheService.CacheUpdate(CacheService.CacheKeys.Rights);
			CacheService.CacheUpdate(CacheService.CacheKeys.TopTags);
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
				var tag = tags.Single(t => t.TagId == contentTag);
				content.Tags.Add(tag);
			}

			return content;
		}



		// **************************************
		// UpdateModel:
		//	Rights
		// **************************************    
		private Content UpdateRights (Content content, IList<ContentRightViewModel> rightsModel) {

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

				ContentRight contentRight = contentRights.SingleOrDefault(x => x.ContentRightId == rm.ContentRightId) ?? new ContentRight();

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

		// **************************************
		//  Delete
		// **************************************
		public void Delete(int contentId) {
			throw new NotImplementedException();
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