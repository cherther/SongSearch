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
			return base.ActiveUser.MyAdminCatalogs();//DataSession.All<Catalog>().Where(c => c.UserCatalogRoles.Any(x => x.UserId == ActiveUser.UserId && x.RoleId == (int)Roles.Admin)).ToList();
		}

		// **************************************
		// GetCatalog
		// **************************************
		public Catalog GetCatalogDetail(int catalogId) {
			if(!ActiveUser.IsAtLeastInCatalogRole(Roles.Admin, catalogId)){
				return null;
			}
			return GetCatalog(catalogId);	
		}

		// **************************************
		// CreateCatalog
		// **************************************
		public int CreateCatalog(Catalog catalog) {

			var cat = GetCatalog(catalog.CatalogName);

			if (cat == null) {

				cat = new Catalog { CatalogName = catalog.CatalogName };
				DataSession.Add<Catalog>(cat);

				DataSession.CommitChanges();
				using (var um = new UserManagementService(DataSession)) {
					um.UpdateUserCatalogRole(ActiveUser.UserId, cat.CatalogId, (int)Roles.Admin);
				}
			} else if (!ActiveUser.IsAtLeastInCatalogRole(Roles.Admin, cat.CatalogId)) {
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

				Files.SafeDelete(content.MediaFilePath(MediaVersion.Preview), true);
				Files.SafeDelete(content.MediaFilePath(MediaVersion.FullSong), true);

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
			return DataSession.Single<Catalog>(x => x.CatalogId == catalogId);
		}
		private Catalog GetCatalog(string catalogName) {
			return DataSession.Single<Catalog>(x => x.CatalogName.Equals(catalogName, StringComparison.InvariantCultureIgnoreCase));
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