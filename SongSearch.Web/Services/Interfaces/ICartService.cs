using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {
	// **************************************
	// ICartService
	// **************************************
	public interface ICartService : IDisposable {
		string ActiveUserName { get; set; }
//		User ActiveUser { get; set; }

		IList<Cart> MyCarts();
		Cart MyActiveCart();
		Cart MyActiveCartContents();

		int[] MyCartContents();
		bool IsInMyActiveCart(int contentId);

		void AddToMyActiveCart(int contentId);
		void AddToMyActiveCart(int[] contentIds);
		void RemoveFromMyActiveCart(int contentId);
		void RemoveFromMyActiveCart(int[] contentIds);

		void CompressMyActiveCart(string userArchiveName, IList<ContentUserDownloadable> contentNames);//, IList<Content> items);
		void CompressMyActiveCartOffline(string userArchiveName, IList<ContentUserDownloadable> contentNames);//, IList<Content> items);
		Cart DownloadCompressedCart(int cartId);

		void DeleteCart(int cartId);

	}
}