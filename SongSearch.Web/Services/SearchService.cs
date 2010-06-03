﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using SongSearch.Web.Data;
using System.Data.Objects;
using System.Data.EntityClient;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using Ninject;

namespace SongSearch.Web.Services {
	public static class SearchService {

		private const string _quote = @"""";
		private const string _leftSearchChar = @"*";
		private const string _add = " and ";
		private const int _fullSearchMin = 3;


		// **************************************
		// GetLookupList: 
		//	returns simple listing of table contents
		// **************************************
		public static IList<T> GetLookupList<T>() where T : class, new() {

			using (var session = App.DataSessionReadOnly) {//Container.Get<IDataSession>()) {

				var query = session.All<T>(); ;
				return query.ToList();

			}
		
		}

		// **************************************
		// GetLookupListContent
		//	returns simple listing of 
		//	contentModel field values
		// **************************************
		public static IList<string> GetLookupListContent(string fieldName) {

			if (String.IsNullOrWhiteSpace(fieldName)){
				throw new ArgumentException("Missing fieldName argument");
			}

			using (var session = App.DataSessionReadOnly) {//App.Container.Get<IDataSession>()) {
				
				var query = session.All<Content>().Select<string>(fieldName).Distinct().Where(v => v != null).Select(v => v.ToUpper());
				return query.ToList();							
			
			}
		}

		// **************************************
		// GetLookupListContentRights
		//	returns simple listing of 
		//	contentModel contentRight field values
		// **************************************
		public static IList<string> GetLookupListContentRights(string fieldName) {

			if (String.IsNullOrWhiteSpace(fieldName)) {
				throw new ArgumentException("Missing fieldName argument");
			}

			using (var session = App.DataSessionReadOnly) {//App.Container.Get<IDataSession>()) {

				var query = session.All<ContentRight>().Select<string>(fieldName).Distinct().Select(v => v.ToUpper());
				return query.ToList();

			}
		}
		// **************************************
		// GetTopTags
		// **************************************
		public static IList<Tag> GetTopTags(TagType tagType, int? limitToTop = null) {


			using (var session = App.DataSessionReadOnly) {//App.Container.Get<IDataSession>()) {

				var tags = session.All<Tag>().Where(t => t.TagTypeId == (int)tagType);
				tags = tags.OrderByDescending(t => t.Contents.Count);
				if (limitToTop.HasValue) {
					tags = tags.Take(limitToTop.Value);
				}
				var topTags  = tags.ToList();
				
				return topTags;
			}
		}

		// **************************************
		// GetSearchMenuProperties
		// **************************************
		public static IList<SearchProperty> GetSearchMenuProperties() {


			using (var session = App.DataSessionReadOnly) {//App.Container.Get<IDataSession>()) {

				// Get Search Properties
				return session.All<SearchProperty>().Where(x => x.IncludeInSearchMenu).ToList();
			}
		}
		public static IList<SearchProperty> GetSearchMenuProperties(Roles role) {


			using (var session = App.DataSessionReadOnly) {//App.Container.Get<IDataSession>()) {

				// Get Search Properties
				return session.All<SearchProperty>().Where(x => x.IncludeInSearchMenu && x.AccessLevel >= (int)role).ToList();
			}
		}

		// **************************************
		// GetContent
		// **************************************
		public static Content GetContent(int contentId, User user) {

			using (var session = App.DataSessionReadOnly) {

				var content = session.GetObjectQuery<Content>()
					.Where(c => c.ContentId == contentId).SingleOrDefault();// && user.UserCatalogRoles.Any(x => x.CatalogId == c.CatalogId)).SingleOrDefault();

				// check if user has access to catalog
				if (content != null && !user.UserCatalogRoles.AsParallel().Any(x => x.CatalogId == content.CatalogId)) {
					return null;
				}
				if (content != null) {
					content.UserDownloadableName = content.DownloadableName(user.FileSignature());
					//contentModel.IsInMyActiveCart = CacheService.IsInMyActiveCart(contentId, user.UserName);// myActiveCart != null && myActiveCart.Contents != null && myActiveCart.Contents.Any(c => c.ContentId == contentId);
				}
				return content;
				
			}
		}
		
		// **************************************
		// GetContentDetails
		// **************************************
		public static Content GetContentDetails(int contentId, User user) {

			using (var session = App.DataSessionReadOnly) {

				var content = session.GetObjectQuery<Content>()
					.Include("Tags")
					.Include("Catalog")
					.Include("ContentRights")
					.Include("ContentRights.Territories")
				.Where(c => c.ContentId == contentId).SingleOrDefault();// && user.UserCatalogRoles.Any(x => x.CatalogId == c.CatalogId)).SingleOrDefault();

				// check if user has access to catalog
				if (content == null || (content != null && !user.UserCatalogRoles.AsParallel().Any(x => x.CatalogId == content.CatalogId))) {
					throw new ArgumentOutOfRangeException("Content does not exist or you do not have access");					
				}
				if (content != null) {
					content.UserDownloadableName = content.DownloadableName(user.FileSignature());
					content.IsInMyActiveCart = CacheService.IsInMyActiveCart(contentId, user.UserName);// myActiveCart != null && myActiveCart.Contents != null && myActiveCart.Contents.Any(c => c.ContentId == contentId);
				}
				return content;
			}
		}

		// **************************************
		// GetContentSearchResults
		// **************************************
		public static PagedList<Content> GetContentSearchResults(
											IList<SearchField> searchFields, 
											User user, 
											int? sortPropertyId = null,
											int? sortType = null,
											int? pageSize = null, 
											int? pageIndex = null) {


			using (var session = App.DataSessionReadOnly){//App.Container.Get<IDataSession>()) {

				// Get all Search Properties
				var props = CacheService.SearchProperties((Roles)user.RoleId);//.dataSession.All<SearchProperty>().ToList();
				var contentQuery = session.All<Content>();
				contentQuery = contentQuery.BuildSearchDynamicLinqSql(searchFields, props);//Where("Title.Contains(@0)", "love");
				//var preCountCommand = sbPre.Append(sbCommand.ToString()).ToString();

				//limit to user catalogs
				if (user == null) {
					throw new ArgumentException("Invalid user");
				}

				if (!user.IsSuperAdmin()) {
					var userId = user.UserId;
					var userCatalogs = session.All<UserCatalogRole>();

					contentQuery = from c in contentQuery
								   join u in userCatalogs on c.CatalogId equals u.CatalogId
								   where u.UserId == userId
								   select c;//.Where(c => c.Catalog.UserCatalogRoles.Any(rm => rm.UserId == userId));
					
				}

				//System.Diagnostics.Debug.Write(contentQuery.Expression.ToString());

				// Get user request sort field and direction
				var sortProp = sortPropertyId.HasValue ? props.Where(p => p.PropertyId == sortPropertyId.Value).SingleOrDefault() : null;

				contentQuery = sortProp != null ? contentQuery.UserSearchSort(sortProp, sortType) : contentQuery.DefaultSearchSort();

				pageIndex = pageIndex ?? 0;
				pageSize = pageSize ?? 100;

				var pagedResults = contentQuery.ToPagedList(pageIndex.Value, pageSize.Value);

				return pagedResults;
			}
		}

		// **************************************
		// BuildSearchSql
		// **************************************
		private static IQueryable<Content> BuildSearchDynamicLinqSql(this IQueryable<Content> query,
																		IList<SearchField> searchFields,
																		IList<SearchProperty> properties) {

			var currentlyIndexed = new string[] { "Lyrics" };
			
			foreach (var field in searchFields) {

				var searchableValues = field.V.Select(v => v.MakeSearchableValue()).ToArray();

				var prop = properties.Where(p => p.PropertyId == field.P).SingleOrDefault();
				if (prop != null) {
					// build search 
					string columnName = prop.PropertyCode.Trim();
					
					switch ((SearchTypes)prop.SearchTypeId) {

						case SearchTypes.Contains:

							columnName = !prop.IsIndexable ? 
								(
									currentlyIndexed.Contains(columnName) ? 
									String.Concat(columnName, "Index") : columnName
									) :
								columnName.MakeSearchableColumnName();
							
							if (searchableValues.First() != null & prop.SearchPredicate != null) {

								var startsWithSearch = field.V.First().IsStartsWithSearch();
								var preciseSearch = field.V.First().IsPreciseSearch();
								var search = searchableValues.First().Split(' ');

								if (!startsWithSearch && !preciseSearch && search.IsMultiSearch()) {
									foreach (var val in search) {
										string predicate = String.Format("{0}.Contains(@0)", columnName);
										query = query.Where(predicate, val);

									}
								} else {
									var val = searchableValues.First();

									string predicate = String.Format(
										startsWithSearch ? "{0}.StartsWith(@0)" : "{0}.Contains(@0)"
										, columnName);//.MakeSearchableColumn(),
									query = query.Where(predicate, val);
								}
							}
							break;

						case SearchTypes.Join:
							if (searchableValues.First() != null & prop.SearchPredicate != null) {
								var joinTable = prop.LookupName;
								var joinField = prop.PropertyCode;
								var searchField = prop.PropertyName;

								//sbJoin.AppendLine(String.Format(@"inner join dbo.{0} r_{1} on c.{1} = r_{1}.{1}", joinTable, joinField));
								//sbWhere.AppendLine(String.Format(@"r_{0}.{1} {2}", joinField, searchField,
								//    String.Format(prop.SearchPredicate, searchableValues.First())));
							}
							break;

						case SearchTypes.HasValue:
							if (searchableValues.First() != null) {
								
								string predicate = String.Format("{0} != null", columnName);//.MakeSearchableColumn(),
								query = query.Where(predicate);
							}
							break;

						case SearchTypes.IsTrue:
							if (searchableValues.First() != null) {
								
								string predicate = String.Format("{0} == true", columnName);//.MakeSearchableColumn(),
								query = query.Where(predicate);
							}
							break;

						case SearchTypes.Range:
							int i;

							var range = searchableValues.Select(
								x => (String.IsNullOrWhiteSpace(x) || int.TryParse(x, out i) == false)
									? null
									: (int?)int.Parse(x)
								);
							if (range.Any(v => v != null)) {

								if (range.All(v => v.HasValue)) {
									// two valid values
									string predicate = String.Format("{0} >= @0 && {0} <= @1", columnName);//.MakeSearchableColumn(),

									query = query.Where(predicate, range.First(), range.Last());
								} else {
									//one valid value
									if (range.First().HasValue) {
										// first value only
										string predicate = String.Format("{0} == @0", columnName);//.MakeSearchableColumn(),

										query = query.Where(predicate, range.First());

									} else {
										// second value only
										string predicate = String.Format("{0} <= @0", columnName);//.MakeSearchableColumn(),

										query = query.Where(predicate, range.Last());
									}
								}
							}
							break;
						case SearchTypes.Tag:

							var tagValues = searchableValues.SplitTags(';').Distinct().ToArray(); //could also just replace, but this way it throws for non-numeric values
							
							foreach (var itm in tagValues) {
								var tagId = itm;
								query = query.Where(c => c.Tags.Any(t => t.TagId == tagId));

							}
							break;

						default:
							goto case (SearchTypes.Contains);

					}


				}
			}

			return query;
		}

		// ----------------------------------------------------------------------------
		// (Private Extensions)
		// ----------------------------------------------------------------------------
		// **************************************
		// Sort Expressions
		// **************************************
		private static Expression<Func<Content, string>> _titleSort = c => c.Title.Length == 0 ? "zzz" : c.Title;
		private static Expression<Func<Content, string>> _titleSortDesc = c => c.Title.Length == 0 ? "---" : c.Title;
		private static Expression<Func<Content, string>> _artistSort = c => c.Artist.Length == 0 ? "zzz" : c.Artist;
		private static Expression<Func<Content, string>> _artistSortDesc = c => c.Artist.Length == 0 ? "--" : c.Artist;
		private static Expression<Func<Content, int>> _popSort = c => !c.Pop.HasValue ? 1000 : c.Pop.Value;
		private static Expression<Func<Content, int>> _countrySort = c => !c.Country.HasValue ? 1000 : c.Country.Value;
		private static Expression<Func<Content, int>> _releaseYearSort = c => !c.ReleaseYear.HasValue ? 10000 : c.ReleaseYear.Value;
		private static Expression<Func<Content, int>> _releaseYearSortDesc = c => !c.ReleaseYear.HasValue ? 0 : c.ReleaseYear.Value;

		// **************************************
		// DefaultSearchSort
		// **************************************
		private static IQueryable<Content> DefaultSearchSort(this IQueryable<Content> query) {

			return query
					.OrderBy(_popSort)
					.ThenBy(_countrySort)
					.ThenBy(_titleSort)
					.ThenBy(_artistSort);
		}
		

		// **************************************
		// UserSearchSort
		// **************************************
		private static IQueryable<Content> UserSearchSort(this IQueryable<Content> query, SearchProperty sortProperty, int? sortType) {
			//products.OrderBy("Category.CategoryName, UnitPrice descending");
			var sortField = sortProperty != null ? sortProperty.PropertyCode : null;
			var sortDirection = (SortType)sortType.GetValueOrDefault();

			//for special cases, we use lambdas, otherwise we use DynamicLinq
			switch (sortField) {
				case "Title":
					query = (sortDirection.IsDescending() ?
						query.OrderByDescending(_titleSortDesc) :
						query.OrderBy(_titleSort)
						)
						.ThenBy(_artistSort);
					break;
				case "Artist":
					query = (sortDirection.IsDescending() ?
						query.OrderByDescending(_artistSortDesc) :
						query.OrderBy(_artistSort)
						)
						.ThenBy(_titleSort);
					break;
				case "Pop":
					query = (sortDirection.IsDescending() ?
						query.OrderByDescending(_popSort) :
						query.OrderBy(_popSort))
						.ThenBy(_titleSort).ThenBy(_artistSort);
					break;
				case "Country":
					query = (sortDirection.IsDescending() ?
						query.OrderByDescending(_countrySort) :
						query.OrderBy(_countrySort))
						.ThenBy(_titleSort).ThenBy(_artistSort);
					break;
				case "ReleaseYear":
					query = (sortDirection.IsDescending() ?
						query.OrderByDescending(_releaseYearSortDesc) :
						query.OrderBy(_releaseYearSort))
						.ThenBy(_titleSort).ThenBy(_artistSort);
					break;
				default:
					
					sortField = sortField != null && sortDirection != SortType.None ?
							String.Format("{0} {1}", sortField, (sortDirection)) :
							null;
					query = sortField != null ? query.OrderBy(sortField) : query;
					break;
			}

			return query;
	
			
		}

	
		// **************************************
		// IsPreciseSearch
		// **************************************
		private static bool IsPreciseSearch(this string value) {
			return (value.StartsWith(_quote) && value.EndsWith(_quote));
		}
		// **************************************
		// IsStartsWithSearch
		// **************************************
		private static bool IsStartsWithSearch(this string value) {
			return (value.EndsWith(_leftSearchChar) || value.Length < _fullSearchMin);
		}
		// **************************************
		// IsMultiSearch
		// **************************************
		private static bool IsMultiSearch(this string[] searchValues) {
			return (searchValues.Length > 1 && searchValues.All(x => !String.IsNullOrWhiteSpace(x)));
		}

		//// **************************************
		//// MakeSearchableColumn
		//// **************************************
		//private static string MakeSearchableColumn(this string value) {
		//    value = string.Format(@"upper({0})", value);
		//    var replacements = new string[] { @",", @"''", @";", @":", @"\\", @"/" };//, @"|", @"{", @"}", @"[", @"]", @"?", @"<", @">", @".", @"!", "*" };
		//    replacements.ForEach(x => value = String.Format(@"replace({0}, '{1}','')", value, x));
		//    return value;
		//}

		// **************************************
		// MakeSearchableColumn
		// **************************************
		private static string MakeSearchableColumnName(this string column) {
			column = string.Format(@"{0}.ToUpper()", column);
			var replacements = new string[] { @",", @"'", @";", @":", @"\", @"/", @"!", @"?" };//, @"|", @"{", @"}", @"[", @"]", @"?", @"<", @">", @".", @"!", "*" };
			replacements.ForEach(x => column = String.Format(@"{0}.Replace(""{1}"","""")", column, x));
			return column;
		}

	}
}