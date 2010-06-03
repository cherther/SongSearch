﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;
using SongSearch.Web.Services;
using System.IO;

namespace SongSearch.Web {
	public static class DataModelExtensions {
		
		public static int CountWithChildren(this IList<User> users) {
			int count = users.Count;
			users.ForEach(u => count += CountWithChildren(u.ChildUsers.ToList()));

			return count;
		}

		public static SortType Flip(this SortType sort){

			return sort == SortType.Ascending ? SortType.Descending : SortType.Ascending;
		}

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
		// UpdateModel:
		//	Content
		// **************************************    
		public static void UpdateModelWith(this Content currentContent, Content content) {

			//Monkey code
			currentContent.IsControlledAllIn = content.IsControlledAllIn;
			currentContent.HasMediaPreviewVersion = content.HasMediaPreviewVersion;
			currentContent.HasMediaFullVersion = content.HasMediaFullVersion;
			currentContent.Title = (content.Title ?? currentContent.Title).ToUpper();
			currentContent.Artist = (content.Artist ?? currentContent.Artist).ToUpper();
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

		}


		// **************************************
		// GetArchivePath
		// **************************************
		public static string ArchivePath(this Cart cart) {
			string zipPath = Settings.ZipPath.Text();
			return Path.Combine(zipPath, cart.ArchiveName);
		}

		// **************************************
		// MarkAsCompressed
		// **************************************
		public static string ArchiveDownloadName(this Cart cart, string userArchiveName) {
			string downloadName = "";
			if (String.IsNullOrWhiteSpace(userArchiveName)) {
				DateTime cartDate = DateTime.Now;

				string zipFormat = Settings.ZipFormat.Text();
				downloadName = String.Format(zipFormat, cartDate.Year, cartDate.Month, cartDate.Day, cart.CartId);
			} else {
				userArchiveName = userArchiveName.MakeFilePathSafe().TrimInside("_");
				string zipFormat = Settings.ZipUserFormat.Text();
				downloadName = String.Format(zipFormat, userArchiveName, cart.CartId);
			}

			return downloadName;
		}

		// **************************************
		// MarkAsCompressed
		// **************************************
		public static Cart MarkAsCompressed(this Cart cart, string zipPath) {


			if (cart != null && File.Exists(zipPath)) {
				var zip = new FileInfo(zipPath);
				int? numberItems = cart.Contents.Count;

				cart.CompressedSize = zip.Length;
				cart.ArchiveName = zip.Name;
				cart.NumberItems = numberItems ?? 0;
				cart.CartStatus = (int)CartStatusCodes.Compressed;
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
				cart.CartStatus = (int)CartStatusCodes.Downloaded;
			}
			return cart;

		}

		//public static IQueryable<User> ChildUsers(this User user, IQueryable<User> users) {
		//    return users.Where(u => u.ParentUserId == user.UserId)
		//                        .AsHierarchy(u => u.UserId, u => u.ParentUserId);
		//}
	}
}