﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SongSearch.Web {
	public static class Extensions {

		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
			var items = source;
			foreach (var item in items) {
				var i = item;
				action(i);
			}
		}

		public static string AsNullIfWhiteSpace(this string item) {
			return String.IsNullOrWhiteSpace(item) ? null : item;
		}

		public static string AsNullIfEmpty(this string item) {
			return String.IsNullOrEmpty(item) ? null : item;
		}

		public static int? AsNullIfZero(this int number) {
			return number > 0 ? (int?)number : null;
		}

		public static IEnumerable<T> AsNullIfEmptyElements<T>(this IEnumerable<T> items) {
			return items.AsNullIfEmpty() == null ? null : items.Where(i => i != null);			
		}

		public static IEnumerable<T> AsNullIfEmpty<T>(this IEnumerable<T> items) {
			return (items == null || items.Any()) ? null : items;			
		}

		public static string AsEmptyIfNull(this string item) {
			return String.IsNullOrEmpty(item) ? String.Empty : item;
		}

	}
}