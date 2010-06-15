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

		private const char Space = ' ';
		private const string Quote = @"""";
		private const string LeftSearchChar = @"*";
		private const int FullSearchMin = 3;
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
		public static string MakeSearchableValue(this string value) {
			value = Regex.Replace(value, @"[^a-zA-Z0-9; ]", Empty); //@"\W\s", "");
			value = value.Replace("  ", " ").ToUpper();

			return value;
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


		public static string ToFileSizeDescription(this decimal? fileSize) {
			
			return String.Format("{0} MB", 
				fileSize.HasValue ? Convert.ToInt32((fileSize.Value/(1024*1024))) : 0
				);
		}

		public static string ToYesNo(this bool yes) {
			return yes ? "Yes" : "No";
		}

	}
}