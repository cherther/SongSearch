using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {

	public static class CatalogManagementService { //: BaseService, ICatalogManagementService {

		// ----------------------------------------------------------------------------
		// (Properties)
		// ----------------------------------------------------------------------------
		// ----------------------------------------------------------------------------
		// (Public)
		// ----------------------------------------------------------------------------
		public static IList<Catalog> GetMyCatalogs() {
			return Account.User().MyAdminCatalogs();//DataSession.All<Catalog>().Where(c => c.UserCatalogRoles.Any(x => x.UserId == ActiveUser.UserId && x.RoleId == (int)Roles.Admin)).ToList();
		}

		// **************************************
		// GetCatalog
		// **************************************
		public static Catalog GetCatalogDetail(int catalogId) {
			if (!Account.User().IsAtLeastInCatalogRole(Roles.Admin, catalogId)) {
				return null;
			}
			using (var ctx = new SongSearchContext()) {
				return ctx.GetCatalogGraph(catalogId);
			}
		}

		// **************************************
		// CreateCatalog
		// **************************************
		public static int CreateCatalog(Catalog catalog) {
			using (var ctx = new SongSearchContext()) {
				var cat = ctx.GetCatalog(catalog.CatalogName);
				var user = Account.User();

				if (cat == null) {

					cat = new Catalog { CatalogName = catalog.CatalogName };
					ctx.Catalogs.AddObject(cat);

					ctx.SaveChanges();

					UserManagementService.UpdateUserCatalogRole(user.UserId, cat.CatalogId, (int)Roles.Admin);

				} else if (!user.IsAtLeastInCatalogRole(Roles.Admin, cat.CatalogId)) {
					throw new AccessViolationException("You do not have admin rights to this catalog");
				}

				return cat.CatalogId;
			}
		}

		// **************************************
		// CreateContent
		// **************************************
		public static int CreateContent(Content item) {
			throw new NotImplementedException();
		}

		public static int CreateContent(IList<Content> items) {
			throw new NotImplementedException();
		}

		// **************************************
		// DeleteCatalog
		// **************************************
		public static void DeleteCatalog(int catalogId) {
			using (var ctx = new SongSearchContext()) {
				var catalog = ctx.Catalogs.SingleOrDefault(c => c.CatalogId == catalogId);

				var contents = catalog.Contents.ToList();
				foreach (var content in contents) {

					foreach (var media in content.ContentMedia) {
						FileSystem.SafeDelete(media.MediaFilePath(true), true);
					}

					ctx.Contents.DeleteObject(content);
				}

				ctx.Catalogs.DeleteObject(catalog);
				ctx.SaveChanges();
			}
		}

		// ----------------------------------------------------------------------------
		// Private
		// ----------------------------------------------------------------------------

		// **************************************
		// GetCatalog
		// **************************************
		private static Catalog GetCatalogGraph(this SongSearchContext ctx, int catalogId) {
			return ctx.Catalogs
				.Include("Contents")
				.Include("Contents.ContentMedia")
				.Include("UserCatalogRoles")
				.Include("Creator")
				.SingleOrDefault(x => x.CatalogId == catalogId);			
	//		return DataSession.Single<Catalog>(x => x.CatalogId == catalogId);
		}
		private static Catalog GetCatalog(this SongSearchContext ctx, string catalogName) {
			return ctx.Catalogs
//				.Include("Contents")
				.SingleOrDefault(x => x.CatalogName.Equals(catalogName, StringComparison.InvariantCultureIgnoreCase));	
//			return DataSession.Single<Catalog>(x => x.CatalogName.Equals(catalogName, StringComparison.InvariantCultureIgnoreCase));
		}
	}
}