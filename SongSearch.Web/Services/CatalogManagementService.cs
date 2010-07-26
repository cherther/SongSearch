using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {

	public class CatalogManagementService : BaseService, ICatalogManagementService {

		// ----------------------------------------------------------------------------
		// (Properties)
		// ----------------------------------------------------------------------------
		private bool _disposed;
		
				
		public CatalogManagementService(IDataSession session) : base(session) {}
		public CatalogManagementService(string activeUserIdentity) : base(activeUserIdentity) { }


		// ----------------------------------------------------------------------------
		// (Public)
		// ----------------------------------------------------------------------------
		public IList<Catalog> GetMyCatalogs() {
			return Account.User().MyAdminCatalogs();//DataSession.All<Catalog>().Where(c => c.UserCatalogRoles.Any(x => x.UserId == ActiveUser.UserId && x.RoleId == (int)Roles.Admin)).ToList();
		}

		// **************************************
		// GetCatalog
		// **************************************
		public Catalog GetCatalogDetail(int catalogId) {
			if (!Account.User().IsAtLeastInCatalogRole(Roles.Admin, catalogId)) {
				return null;
			}
			var cat = GetCatalog(catalogId);

			//cat.Owner = cat.Owner();
			return cat;
		}

		// **************************************
		// CreateCatalog
		// **************************************
		public int CreateCatalog(Catalog catalog) {

			var cat = GetCatalog(catalog.CatalogName);
			var user = Account.User();

			if (cat == null) {

				cat = new Catalog { CatalogName = catalog.CatalogName };
				DataSession.Add<Catalog>(cat);

				DataSession.CommitChanges();
				using (var um = new UserManagementService(DataSession)) {
					um.UpdateUserCatalogRole(user.UserId, cat.CatalogId, (int)Roles.Admin);
				}
			} else if (!user.IsAtLeastInCatalogRole(Roles.Admin, cat.CatalogId)) {
					throw new AccessViolationException("You do not have admin rights to this catalog");				
			}

			return cat.CatalogId;

		}

		// **************************************
		// CreateContent
		// **************************************
		public int CreateContent(Content item) {
			throw new NotImplementedException();
		}

		public int CreateContent(IList<Content> items) {
			throw new NotImplementedException();
		}

		// **************************************
		// DeleteCatalog
		// **************************************
		public void DeleteCatalog(int catalogId) {

			var catalog = DataSession.Single<Catalog>(c => c.CatalogId == catalogId);

			var contents = catalog.Contents.ToList();
			foreach (var content in contents) {

				FileSystem.SafeDelete(content.MediaFilePath(MediaVersion.Preview), true);
				FileSystem.SafeDelete(content.MediaFilePath(MediaVersion.Full), true);

				DataSession.Delete<Content>(content);

			}

			DataSession.Delete<Catalog>(catalog);
			DataSession.CommitChanges();

		}

		// ----------------------------------------------------------------------------
		// Private
		// ----------------------------------------------------------------------------

		// **************************************
		// GetCatalog
		// **************************************
		private Catalog GetCatalog(int catalogId) {
			return DataSession.GetObjectQuery<Catalog>()
				.Include("Contents")
				.Include("Creator")
				.Where(x => x.CatalogId == catalogId).SingleOrDefault();			
	//		return DataSession.Single<Catalog>(x => x.CatalogId == catalogId);
		}
		private Catalog GetCatalog(string catalogName) {
			return DataSession.GetObjectQuery<Catalog>()
//				.Include("Contents")
				.Where(x => x.CatalogName.Equals(catalogName, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();	
//			return DataSession.Single<Catalog>(x => x.CatalogName.Equals(catalogName, StringComparison.InvariantCultureIgnoreCase));
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