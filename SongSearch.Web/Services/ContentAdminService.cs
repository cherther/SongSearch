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

		public void Update(Content content) {
			throw new NotImplementedException();
		}

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