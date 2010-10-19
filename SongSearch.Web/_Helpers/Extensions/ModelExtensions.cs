using System;
using System.Collections.Generic;
using System.Linq;
using SongSearch.Web.Data;
using SongSearch.Web.Services;
using System.IO;
using Ninject;

namespace SongSearch.Web {
	public static class DataModelExtensions {



		// **************************************
		// Flip
		// **************************************    
		public static SortType Flip(this SortType sort) {

			return sort == SortType.Ascending ? SortType.Descending : SortType.Ascending;
		}

		// **************************************
		// IsDescending
		// **************************************    
		public static bool IsDescending(this SortType sort) {

			return sort == SortType.Descending;
		}

		// **************************************
		// DownloadableName
		// **************************************    
		public static string DownloadableName(this Content content, string signature = null) {

			signature = signature != null ? String.Format("({0})", signature) : "";

			return (String.Format("{0} by {1} {2}", content.Title.ToUpper(), content.Artist.ToUpper(), signature)).MakeFilePathSafe();

		}

		// **************************************
		// MediaPath
		// **************************************    
		//public static string MediaPath(this Content content, MediaVersion mediaVersion) {
		//    using (var mediaService = App.Container.Get<IMediaService>()) { 

		//        return mediaService.GetContentMediaPath(content, mediaVersion);
		//    }
		//}

		// **************************************
		// MediaFilePath
		// **************************************    

		public static string MediaFilePath(this ContentMedia contentMedia, bool local = false) {
			if (local) {
				return MediaService.GetContentMediaPathLocal(contentMedia);
			} else {
				return MediaService.GetContentMediaPath(contentMedia);
			}
		}
		public static ContentMedia Media(this Content content, MediaVersion version) {
			return content.ContentMedia != null ? 
				content.ContentMedia.SingleOrDefault(x => x.MediaVersion == (int)version)
				: null;
		}
		// **************************************
		// IsAvailableTo
		// **************************************    
		public static bool IsAvailableTo(this Content content, User user) {
			if (content == null) {
				return false;
			}
			if (user.IsSuperAdmin()) {
				return true;
			} else if (user.UserCatalogRoles != null &&
				user.UserCatalogRoles.AsParallel().Any(x => x.CatalogId == content.CatalogId)) {
				return true;
			} else {
				return false;
			}
		}
		// **************************************
		// CountWithChildren
		// **************************************    
		public static int CountWithChildren(this IList<User> users) {
			int count = users.Count;
			users.ForEach(u => count += CountWithChildren(u.ChildUsers.ToList()));

			return count;
		}

		// **************************************
		// LimitToAdministeredBy
		// **************************************    
		public static IQueryable<Catalog> LimitToAdministeredBy(this IQueryable<Catalog> catalogs, User user) {
			var adminCatalogIds = user.UserCatalogRoles.Where(x => x.RoleId <= (int)Roles.Admin).Select(x => x.CatalogId);
			return catalogs.Where(c => adminCatalogIds.Contains(c.CatalogId));

			//			return catalogs.Where(c => c.UserCatalogRoles.Any(x => x.UserId == user.UserId && x.RoleId <= (int)Roles.Admin));
		}
		public static IList<Catalog> LimitToAdministeredBy(this IList<Catalog> catalogs, User user) {
			if (user.IsSuperAdmin()) {
				return catalogs;
			} else {
				var adminCatalogIds = user.UserCatalogRoles.Where(x => x.RoleId <= (int)Roles.Admin).Select(x => x.CatalogId);
				return catalogs.Where(c => adminCatalogIds.Contains(c.CatalogId)).ToList();
			}
			//			return catalogs.Where(c => c.UserCatalogRoles.Any(x => x.UserId == user.UserId && x.RoleId <= (int)Roles.Admin)).ToList();
		}

		public static User Owner(this Catalog catalog) {
			return UserManagementService.GetUserDetail(catalog.CreatedByUserId);
		}
		// **************************************
		// UpdateModel:
		//	Content
		// **************************************    
		public static void UpdateModelWith(this Content currentContent, Content content) {

			//Monkey code
			currentContent.IsControlledAllIn = content.IsControlledAllIn;
			currentContent.HasMediaPreviewVersion = content.HasMediaPreviewVersion;
			currentContent.HasMediaFullVersion = content.HasMediaFullVersion;
			currentContent.Title = (content.Title ?? currentContent.Title);//.ToUpper();
			currentContent.Artist = (content.Artist ?? currentContent.Artist);//.ToUpper();
			currentContent.Writers = content.Writers;
			currentContent.Pop = content.Pop;
			currentContent.Country = content.Country;
			currentContent.ReleaseYear = content.ReleaseYear;
			currentContent.RecordLabel = content.RecordLabel;
			currentContent.Lyrics = content.Lyrics;
			currentContent.LyricsIndex = content.Lyrics.MakeIndexValue();
			currentContent.Notes = content.Notes;
			currentContent.Keywords = content.Keywords;
			currentContent.SimilarSongs = content.SimilarSongs;
			currentContent.LicensingNotes = content.LicensingNotes;
			currentContent.SoundsLike = content.SoundsLike;
			currentContent.Instruments = content.Instruments;

		}

		public static string MediaUrl(this ContentMedia contentMedia) {
			if (contentMedia == null) { return String.Empty; }
			return MediaService.GetContentMediaUrl(contentMedia);
		}
		public static ContentMedia FullVersion(this IEnumerable<ContentMedia> contentMedia) {
			return contentMedia != null ? contentMedia.SingleOrDefault(x => x.MediaVersion == (int)MediaVersion.Full)
				: null;
		}
		public static ContentMedia PreviewVersion(this IEnumerable<ContentMedia> contentMedia) {
			return contentMedia != null ? contentMedia.SingleOrDefault(x => x.MediaVersion == (int)MediaVersion.Preview)
				: null;
		}
		// **************************************
		// GetArchivePath
		// **************************************
		public static string ArchivePath(this Cart cart) {
			return Path.Combine(SystemConfig.ZipPath, cart.ArchiveName);
		}

		// **************************************
		// MarkAsCompressed
		// **************************************
		public static string ArchiveDownloadName(this Cart cart, string userArchiveName) {
			string downloadName = "";
			if (String.IsNullOrWhiteSpace(userArchiveName)) {
				DateTime cartDate = DateTime.Now;

				string zipFormat = SystemConfig.ZipFormat;
				downloadName = String.Format(zipFormat, cartDate.Year, cartDate.Month, cartDate.Day, cart.UserId, cart.CartId);
			} else {
				userArchiveName = userArchiveName.MakeFilePathSafe().TrimInside("_");
				string zipFormat = SystemConfig.ZipUserFormat;
				downloadName = String.Format(zipFormat, userArchiveName, cart.UserId, cart.CartId);
			}

			return downloadName;
		}

		// **************************************
		// MarkAsCompressed
		// **************************************
		public static Cart MarkAsCompressed(this Cart cart) {

			var zipPath = cart.ArchivePath();
			if (cart != null && File.Exists(zipPath)) {
				var zip = new FileInfo(zipPath);
				int? numberItems = cart.Contents.Count();

				cart.CompressedSize = zip.Length;
				cart.ArchiveName = zip.Name;
				cart.NumberItems = numberItems ?? 0;
				cart.CartStatus = (int)CartStatusCodes.Compressed;
				cart.IsLastProcessed = true;
				cart.LastUpdatedOn = DateTime.Now;

			}

			return cart;

		}

		//**************************************
		// MarkAsDownloaded
		// **************************************
		public static Cart MarkAsDownloaded(this Cart cart) {

			if (cart != null) {
				//SqlSession.Clear(q);
				cart.LastUpdatedOn = DateTime.Now;
				cart.IsLastProcessed = false;
				cart.CartStatus = (int)CartStatusCodes.Downloaded;
			}
			return cart;

		}

		// **************************************
		// GetLastProcessedCartId
		// **************************************
		public static int GetLastProcessedCartId(this IEnumerable<Cart> carts) {
			var lastProcessedCart = carts.Where(c => c.IsLastProcessed == true).OrderByDescending(c => c.LastUpdatedOn).FirstOrDefault();
			var lastProcessedCartId = lastProcessedCart != null ? lastProcessedCart.CartId : 0;
			return lastProcessedCartId;
		}


		public static IList<UploadFile> GetUploadFiles(this IList<string> tempFiles, MediaVersion mediaVersion) {
			var uploadFiles = new List<UploadFile>();
			tempFiles.ForEach(f =>
				uploadFiles.Add(new UploadFile() { FileMediaVersion = mediaVersion, FilePath = f, FileName = Path.GetFileName(f) })
			);

			return uploadFiles;
		}

		public static string PlanDisplayClass(this PricingPlan plan) {
			return plan.IsEnabled ? (plan.IsFeatured ? "cell-highlight-yellow" : "") : "cell-disabled";
		}

		
	}
}