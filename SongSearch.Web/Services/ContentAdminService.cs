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
		public void Update(Content content, IList<int> tags, IList<ContentRightViewModel> rights) {

			//UpdateModelWith Content
			var currentContent = DataSession.GetObjectQuery<Content>()
					.Include("Tags")
					.Include("Catalog")
					.Include("ContentRights")
					.Include("ContentRights.Territories")
				.Where(c => c.ContentId == content.ContentId).SingleOrDefault();// && user.UserCatalogRoles.Any(x => x.CatalogId == c.CatalogId)).SingleOrDefault();

			if (currentContent == null) {
				throw new ArgumentOutOfRangeException("Content does not exist");
			}

			currentContent.UpdateModelWith(content);

			//UpdateModelWith Tags
			var contentTags = tags.Where(t => t > 0).ToList();
			var allTags = DataSession.All<Tag>().ToList();
			currentContent.UpdateModelWith(contentTags, allTags);

			//UpdateModelWith Rights
			currentContent.UpdateModelWith(rights);


			DataSession.CommitChanges();

		}

		

		// **************************************
		//  UpdateModelWith
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