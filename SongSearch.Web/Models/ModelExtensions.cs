using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;
using SongSearch.Web.Services;

namespace SongSearch.Web {
	public static class ModelExtensions {
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
			var items = source;
			foreach (var item in items) {
				var i = item;
				action(i);
			}
		}
	}
}