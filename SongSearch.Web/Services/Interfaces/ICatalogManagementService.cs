using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {
	
	// **************************************
	// ICatalogManagementService
	// **************************************
	public interface ICatalogManagementService : IDisposable {
		string ActiveUserName { get; set; }
		//User ActiveUser { get; set; }

		IList<Catalog> GetMyCatalogs();
		Catalog GetCatalogDetail(int catalogId);

		int CreateCatalog(Catalog catalog);
		int CreateContent(Content item);
		int CreateContent(IList<Content> items);
		//int CreateAsset(DisplayAsset asset);
		//int CreateAssets(IList<DisplayAsset> assets);

		void DeleteCatalog(int catalogId);
	
	}

}