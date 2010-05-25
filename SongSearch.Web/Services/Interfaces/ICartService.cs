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
		User ActiveUser { get; set; }

		IList<Cart> MyCarts();
		Cart MyActiveCart();

		int[] MyCartContents();
		bool IsInMyActiveCart(int contentId);

		void AddToMyActiveCart(int contentId);
		void RemoveFromMyActiveCart(int contentId);

		void CompressMyActiveCart(string userArchiveName);//, IList<Content> items);
		Cart DownloadCompressedCart(int cartId);

		void DeleteCart(int cartId);

		void ArchiveExpiredCarts();
		void DeletedExpiredArchivedCarts();

	}
}