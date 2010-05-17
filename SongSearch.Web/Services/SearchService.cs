using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using SongSearch.Web.Data;
using System.Data.Objects;
using System.Data.EntityClient;
using System.Linq.Dynamic;

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
		public static IList<T> GetLookupList<T>() where T : class {

			using (ISession session = new EFSession()) {

				var query = session.All<T>(); ;
				return query.ToList();

			}
		
		}

		// **************************************
		// GetLookupListContent
		//	returns simple listing of 
		//	content field values
		// **************************************
		public static IList<string> GetLookupListContent(string fieldName) {

			if (String.IsNullOrWhiteSpace(fieldName)){
				throw new ArgumentException("Missing fieldName argument");
			}
						
			using (ISession session = new EFSession()) {
				
				var query = session.All<Content>().Select<string>(fieldName).Distinct();
				return query.ToList();							
			
			}
		}

		// **************************************
		// GetTopTags
		// **************************************
		public static IList<Tag> GetTopTags(TagType tagType, int? limitToTop = null) {


			using (ISession session = new EFSession()) {

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
		public static IList<SearchProperty> GetSearchMenuProperties(Roles role) {


			using (ISession session = new EFSession()) {

				// Get Search Properties
				return session.All<SearchProperty>().Where(x => x.IncludeInSearchMenu && x.AccessLevel >= (int)role).ToList();
			}
		}

		// **************************************
		// GetContentDetails
		// **************************************
		public static Content GetContentDetails(int contentId, User user) {
			using (var ctx = new SongSearchContext(Connections.ConnectionString(ConnectionStrings.SongSearchContext))) {

				ctx.ContextOptions.LazyLoadingEnabled = false;
				//var set = ctx.CreateObjectSet<Content>();
				var content = ctx.Contents
					.Include("Tags")
					.Include("Catalog")
					.Include("ContentRights")
					.Include("ContentRights.Territories")
					.Where(c => c.ContentId == contentId).SingleOrDefault();
				return content;
				// check if user has access to catalog
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


			using (ISession session = new EFSession()) {

				// Get Search Properties
				var props = session.All<SearchProperty>().ToList();
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
								   select c;//.Where(c => c.Catalog.UserCatalogRoles.Any(r => r.UserId == userId));

					//sbJoin.AppendLine("inner join dbo.UserCatalogRoles ucr on c.CatalogId = ucr.CatalogId");
					//var hasConcat = sbWhere.ToString().EndsWith(_add);
					//sbWhere.AppendLine(String.Concat((!hasConcat ? _add : String.Empty), "ucr.UserId = ", userId));
				}

				System.Diagnostics.Debug.Write(contentQuery.Expression.ToString());

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


			foreach (var field in searchFields) {

				var searchableValues = field.V.Select(v => v.MakeSearchableValue()).ToArray();

				var prop = properties.Where(p => p.PropertyId == field.P).SingleOrDefault();
				if (prop != null) {
					// build search 
					string columnName = prop.PropertyCode.Trim();

					switch ((SearchTypes)prop.SearchTypeId) {

						case SearchTypes.Contains:

							if (searchableValues.First() != null & prop.SearchPredicate != null) {

								var startsWithSearch = field.V.First().IsStartsWithSearch();
								var preciseSearch = field.V.First().IsPreciseSearch();
								var search = searchableValues.First().Split(' ');

								if (!startsWithSearch && !preciseSearch && search.IsMultiSearch()) {
									foreach (var val in search) {
										string predicate = String.Format("{0}.Contains(@0)", columnName);//.MakeSearchableColumn(),
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

							var tagValues = searchableValues.SplitTags(';').Distinct(); //could also just replace, but this way it throws for non-numeric values
							
							foreach (var tagId in tagValues) {
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
		// DefaultSearchSort
		// **************************************
		private static Func<Content, string> _titleSort = c => c.Title.Length == 0 ? "zzz" : c.Title;
		private static Func<Content, string> _artistSort = c => c.Artist.Length == 0 ? "zzz" : c.Artist;
		private static Func<Content, int> _popSort = c => !c.PopCharts.HasValue ? 1000 : c.PopCharts.Value;
		private static Func<Content, int> _countrySort = c => !c.CountryCharts.HasValue ? 1000 : c.CountryCharts.Value;
		private static Func<Content, int> _releaseYearSort = c => !c.ReleaseYear.HasValue ? 10000 : c.ReleaseYear.Value;

		private static IQueryable<Content> DefaultSearchSort(this IQueryable<Content> query) {

			return query
					.OrderBy(c => !c.PopCharts.HasValue ? 1000 : c.PopCharts)
					.ThenBy(c => !c.CountryCharts.HasValue ? 1000 : c.CountryCharts)
					.ThenBy(c => c.Title.Length == 0 ? "zzz" : c.Title)
					.ThenBy(c => c.Artist.Length == 0 ? "zzz" : c.Artist);

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
					return sortDirection.IsDescending() ?
						query.OrderByDescending(c => c.Title.Length == 0 ? "---" : c.Title) :
						query.OrderBy(c => c.Title.Length == 0 ? "zzz" : c.Title);
					break;
				case "Artist":
					return sortDirection.IsDescending() ?
						query.OrderByDescending(c => c.Artist.Length == 0 ? "---" : c.Artist) :
						query.OrderBy(c => c.Artist.Length == 0 ? "zzz" : c.Artist);
					break;
				case "PopCharts":
					return sortDirection.IsDescending() ?
						query.OrderByDescending(c => !c.PopCharts.HasValue ? 1000 : c.PopCharts) : 
						query.OrderBy(c => !c.PopCharts.HasValue ? 1000 : c.PopCharts);
					break;
				case "CountryCharts":
					return sortDirection.IsDescending() ?
						query.OrderByDescending(c => !c.CountryCharts.HasValue ? 1000 : c.CountryCharts) :
						query.OrderBy(c => !c.CountryCharts.HasValue ? 1000 : c.CountryCharts);
					break;

				case "ReleaseYear":
					return sortDirection.IsDescending() ?
						query.OrderByDescending(c => !c.ReleaseYear.HasValue ? 0 : c.ReleaseYear) :
						query.OrderBy(c => !c.ReleaseYear.HasValue ? 10000 : c.ReleaseYear);
					break;
		
				default:
					
					sortField = sortField != null && sortDirection != SortType.None ?
							String.Format("{0} {1}", sortField, (sortDirection)) :
							null;
					return sortField != null ? query.OrderBy(sortField) : query;

					break;
		
			}
	
			
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

		// **************************************
		// IsMultiSearch
		// **************************************
		private static string MakeSearchableColumn(this string value) {
			value = string.Format(@"upper({0})", value);
			var replacements = new string[] { @",", @"''", @";", @":", @"\\", @"/" };//, @"|", @"{", @"}", @"[", @"]", @"?", @"<", @">", @".", @"!", "*" };
			replacements.ForEach(x => value = String.Format(@"replace({0}, '{1}','')", value, x));
			return value;
		}

	}
}