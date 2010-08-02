using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Text.RegularExpressions;
namespace SongSearch.Web {
	public static class StringExtensions {

		private const bool UseIndex = false;

		private const char _space = ' ';
		private const string _quote = @"""";
		private const string _leftSearchChar = @"*";
		private const int _fullSearchMin = 3;

		private static readonly string Empty = String.Empty;


		public static string PasswordHashString(this string passwordToBehashed) {
			return FormsAuthentication.HashPasswordForStoringInConfigFile(passwordToBehashed, "sha1");
		}

		public static string PasswordHash(this string passwordToBehashed) {
			return FormsAuthentication.HashPasswordForStoringInConfigFile(passwordToBehashed, "sha1");
			//            return passwordToBehashed.GetHashCode();
		}

		//
		// Removes non-alphanumeric characters
		public static string MakeIndexValue(this string value) {
			if (value != null) {
				value = Regex.Replace(value, @"[^a-zA-Z0-9 ]", Empty); //@"\W\s", "");
				value = value.ToUpper();
			}
			return value;
		}

		public static string MakeFilePathSafe(this string value) {
			string pat = @"[\\/:*?""<>|]";
			return value != null ? Regex.Replace(value, pat, Empty) : value;
		}

		public static bool IsFilePathSafe(this string value) {
			string pat = @"[^\\/:*?""<>|]*";
			return value != null && Regex.IsMatch(value, pat);
		}

		public static string TrimInside(this string value) {
			return value.TrimInside(String.Empty);
		}

		public static string TrimInside(this string value, string replaceWith) {
			//replaceWith = replaceWith.MakeFilePathSafe();
			return value.Replace(" ", !String.IsNullOrWhiteSpace(replaceWith) ? replaceWith : String.Empty);
		}

		//
		// Removes non-alphanumeric characters, except ;
		private static string _replacements = @",';:\/!?&-.";

		// **************************************
		// MakeSearchableValue
		// **************************************
		public static string MakeSearchableValue(this string value) {
			//var pattern = "";
			//_replacements.ForEach(x => pattern += String.Format(@"\{0}", x));
			//pattern = String.Format(@"[{0}]\s", pattern);
			var newValue = value;
			//var newvalue = Regex.Replace(value, pattern, String.Empty); //@"\W\s", "");
			_replacements.ForEach(x => newValue = newValue.Replace(x.ToString(), ""));
			newValue = newValue.Replace("  ", " ").ToUpper();

			return newValue;
		}

		// **************************************
		// MakeSearchableColumn
		// **************************************
		public static string MakeSearchableColumnName(this string column) {
			column = string.Format(@"{0}.ToUpper()", column);
			//var replacements = new string[] { @",", @"'", @";", @":", @"\", @"/", @"!", @"?", @"&", @"-", @"." };//, @"|", @"{", @"}", @"[", @"]", @"?", @"<", @">", @".", @"!", "*" };
			_replacements.ForEach(x => column = String.Format(@"{0}.Replace(""{1}"","""")", column, x));
			return column;
		}

		// **************************************
		// IsPreciseSearch
		// **************************************
		public static bool IsPreciseSearch(this string value) {
			return (value.StartsWith(_quote) && value.EndsWith(_quote));
		}
		public static string TrimPreciseSearch(this string value) {
			return value.Replace(_quote, "");// && value.EndsWith(_quote));
		}
		// **************************************
		// IsStartsWithSearch
		// **************************************
		public static bool IsStartsWithSearch(this string value) {
			return (value.EndsWith(_leftSearchChar) || value.Length < _fullSearchMin);
		}
		public static string TrimStartsWithSearch(this string value) {
			return value.Replace(_leftSearchChar, "");// && value.EndsWith(_quote));
		}
		// **************************************
		// IsMultiSearch
		// **************************************
		public static bool IsMultiSearch(this string[] searchValues) {
			return (searchValues.Length > 1 && searchValues.All(x => !String.IsNullOrWhiteSpace(x)));
		}

		public static int[] SplitTags(this string tags, char separator) {
			return !String.IsNullOrWhiteSpace(tags) ? 
				tags.Split(separator).Where(t => !String.IsNullOrWhiteSpace(t)).Select(t => int.Parse(t)).ToArray() 
				: new int[] {};
		}

		public static int[] SplitTags(this IEnumerable<string> tags, char separator) {
			if (tags.Count() > 0) {
				IList<int> tagIds = new List<int>();
				foreach (string tag in tags) {
					IEnumerable<string> tagIdValues = tag.Split(separator).Where(t => !String.IsNullOrWhiteSpace(t));

					foreach (string tagIdValue in tagIdValues) {
						tagIds.Add(int.Parse(tagIdValue.Trim()));
					}
				}
				return tagIds.ToArray();
			} else {
				return new int[] { };
			}
		}

		public static string[] SplitTagNames(this string tags, char separator) {
			return !String.IsNullOrWhiteSpace(tags) ?
				tags.Split(separator).Where(t => !String.IsNullOrWhiteSpace(t)).Select(t => t.Trim()).ToArray()
				: new string[] { };
		}

		public static string[] SplitTagNames(this IEnumerable<string> tags, char separator) {
			//return tags.Where(t => !String.IsNullOrWhiteSpace(t)).ToArray();

			if (tags.Count() > 0) {
				IList<string> tagStrings = new List<string>();
				foreach (string tag in tags) {
					IEnumerable<string> tagValues = tag.Split(separator).Where(t => !String.IsNullOrWhiteSpace(t));

					foreach (string tagValue in tagValues) {
						tagStrings.Add(tagValue.Trim());
					}
				}
				return tagStrings.ToArray();
			} else {
				return new string[] { };
			}
		}

		public static string JoinTags(this string[] tags, char separator) {
			return tags.Length > 0 ? String.Concat(tags.Select(a => String.Format("{0};", a)).ToArray())
					: String.Empty;
		}

		public static string JoinTags(this int[] tags, char separator) {
			return tags.Length > 0 ? String.Concat(tags.Select(a => String.Format("{0};", a)).ToArray())
				: String.Empty;
		}

		public static string Left(this string fullString, int lengthToReturn, string ellipsis) {
			return fullString.Length > lengthToReturn ? 
				String.Concat(fullString.Substring(0, lengthToReturn), ellipsis)
				: fullString;
		}

		public static string Pluralize(this string item, int numberofItems) {
			switch (item) {
				case "is":
					return numberofItems == 1 ? item : item.Replace(item, "are");
				default:
					return numberofItems == 1 ? item : String.Format("{0}s", item);

			}
		}

		public static string CamelCase(this string term, char trigger = _space, string replaceWith = null){
			if (term != null && term.Length > 0) {
				var parts = term.Split(trigger);

				for (var i = 0; i < parts.Length; i++) {
					var part = parts[i];
					parts[i] = String.Concat(part.Substring(0, 1).ToUpper(), part.Substring(1, part.Length - 1).ToLower());
				}

				var sep = replaceWith ?? trigger.ToString();
				return String.Join(sep, parts);
			} else return term;
		}

		public static string IndefArticle(this string item) {
			var vowels = "aeiou";
			foreach(var v in vowels){
				if (item.StartsWith(v.ToString(), StringComparison.InvariantCultureIgnoreCase)) {
					return "an";
				}
			}
			return "a";
		}
		public static string ToFileSizeDescription(this decimal? fileSize) {
			
			return ToFileSizeDescription(fileSize.GetValueOrDefault());
		}

		public static string ToFileSizeDescription(this decimal fileSize) {

			return String.Format("{0} MB",
				Convert.ToInt32((fileSize / (1024 * 1024)))
				);
		}

		public static string ToYesNo(this bool yes) {
			return yes ? "Yes" : "No";
		}
		public static string ToDescription(this int number) {
			return number.ToString("N0");
		}
		public static string ToQuotaDescription(this int? number) {
			return !number.HasValue ? "Unlimited" : (number.Value == 0 ? "None" : number.Value.ToString("N0"));
		}
		public static string ToPriceDescription(this decimal? price) {
			return !price.HasValue ? " -- " : (price.Value == 0 ? "Free!" : price.Value.ToString("C"));
		}
		public static string ToPercentDescription(this decimal number) {
			return number.ToString("P0").TrimInside();
		}
		public static int ToBitRate(this long lengthMilliseconds, long mediaSizeBytes){

			var kb = ((decimal)(mediaSizeBytes * (decimal)8.00) / (decimal)1024.00); //kilobits = 3MB = (3*1024*1024)*8/1024 = 24576
			var secs = ((decimal)lengthMilliseconds / (decimal)1000.00); // 3 min = 3*60*1000/1000 = 180
			return (int)(secs > 0 ? (kb / secs) : 0); //24576 / 180 = 136

		}

		public static string ToBitRateDescription(this long lengthMilliseconds, long mediaSizeBytes) {

			var kb = ((decimal)(mediaSizeBytes * (decimal)8.00) / (decimal)1024.00); //kilobits = 3MB = (3*1024*1024)*8/1024 = 24576
			var secs = ((decimal)lengthMilliseconds / (decimal)1000.00); // 3 min = 3*60*1000/1000 = 180
			return String.Format("{0} kbps", (int)(secs > 0 ? (kb / secs) : 0)); //24576 / 180 = 136

		}

		public static string ToBitRateDescription(this int bitRate) {

			return String.Format("{0} kbps", bitRate);
		}

		public static string MillSecsToTimeCode(this decimal lengthMilliseconds) {
			// convert milliseconds to mm:ss, return as object literal or string
			var secs = Math.Floor(lengthMilliseconds / 1000);
			var min = Math.Floor(secs / 60);
			var sec = secs - (min * 60);
			// if (min == 0 && sec == 0) return null; // return 0:00 as null
			return (min.ToString() + ':' + (sec < 10 ? "0" + sec.ToString() : sec.ToString()));
		}
	}
}