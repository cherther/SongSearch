using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;
using SongSearch.Web.Services;

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

		//public static IQueryable<User> ChildUsers(this User user, IQueryable<User> users) {
		//    return users.Where(u => u.ParentUserId == user.UserId)
		//                        .AsHierarchy(u => u.UserId, u => u.ParentUserId);
		//}
	}
}