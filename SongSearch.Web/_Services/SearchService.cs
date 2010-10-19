using System;
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

		
		//private const string _add = " and ";
		

		// **************************************
		// GetLookupList: 
		//	returns simple listing of table contents
		// **************************************
		public static IList<T> GetLookupList<T>() where T : class, new() {

			using (var ctx = new SongSearchContext()) {
				ctx.ContextOptions.LazyLoadingEnabled = false;
				return ctx.CreateObjectSet<T>().ToList();

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

			using (var ctx = new SongSearchContext()) {
				ctx.ContextOptions.LazyLoadingEnabled = false;

				return ctx.Contents
					.Select<string>(fieldName)
					.Distinct()
					.Where(v => v != null)
					.Select(v => v.ToUpper())
					.ToList();
			}
		}

		// **************************************
		// GetLookupListRepresentations
		//	returns simple listing of 
		//	contentModel contentRight field values
		// **************************************
		public static IList<string> GetLookupListContentRepresentation(string fieldName) {

			if (String.IsNullOrWhiteSpace(fieldName)) {
				throw new ArgumentException("Missing fieldName argument");
			}

			using (var ctx = new SongSearchContext()) {
				ctx.ContextOptions.LazyLoadingEnabled = false;

				return ctx.ContentRepresentations
					.Select<string>(fieldName)
					.Distinct()
					.Select(v => v.ToUpper())
					.ToList();
			}
		}
		// **************************************
		// GetTopTags
		// **************************************
		public static IList<Tag> GetTopTags(TagType tagType, int? limitToTop = null) {


			using (var ctx = new SongSearchContext()) {
				ctx.ContextOptions.LazyLoadingEnabled = false;

				var tags = ctx.Tags.Where(t => t.TagTypeId == (int)tagType);

				switch (tagType) {
					case TagType.Tempo:
						tags = tags.OrderBy(t => t.TagId);
						break;
					default:
						tags = tags.OrderByDescending(t => t.Contents.Count);
						break;
				}
				
				if (limitToTop.HasValue) {
					tags = tags.Take(limitToTop.Value);
				}
				
				return tags.ToList();
			}
		}

		// **************************************
		// GetSearchMenuProperties
		// **************************************
		public static IList<SearchProperty> GetSearchMenuProperties() {


			using (var ctx = new SongSearchContext()) {
				ctx.ContextOptions.LazyLoadingEnabled = false;
				// Get Search Properties
				return ctx.SearchProperties.Where(x => x.IncludeInSearchMenu).ToList();
			}
		}
		public static IList<SearchProperty> GetSearchMenuProperties(Roles role) {


			using (var ctx = new SongSearchContext()) {
				ctx.ContextOptions.LazyLoadingEnabled = false;
				// Get Search Properties
				return ctx.SearchProperties.Where(x => x.IncludeInSearchMenu && x.AccessLevel >= (int)role).ToList();
			}
		}

		// **************************************
		// GetContent
		// **************************************
		public static Content GetContent(int contentId, User user) {

			using (var ctx = new SongSearchContext()) {
				ctx.ContextOptions.LazyLoadingEnabled = false;

				Content content;

//				var query = session.All<Content>().Where(c => c.ContentId == contentId);
				var query = ctx.Contents
					.Include("ContentMedia")
					.Where(c => c.ContentId == contentId);

				if (!user.IsSuperAdmin() && user.UserCatalogRoles == null) {
					query = query.Where(c => c.Catalog.UserCatalogRoles.Any(u => u.UserId == user.UserId));				
				}
	
				content = query.SingleOrDefault();

				if (!content.IsAvailableTo(user)) {
					throw new ArgumentOutOfRangeException("Content does not exist or you do not have access");
				}

				if (content != null) {
					content.UserDownloadableName = content.DownloadableName(user.FileSignature(content));
				}
				return content;
				
			}
		}
		
		// **************************************
		// GetContentDetails
		// **************************************
		public static Content GetContentDetails(int contentId, User user) {

			using (var ctx = new SongSearchContext()) {
				ctx.ContextOptions.LazyLoadingEnabled = false;

				var content = ctx.Contents
					.Include("Tags")
					.Include("Catalog")
					.Include("ContentMedia")
					.Include("ContentRepresentations")
					.Include("ContentRepresentations.Territories")
				.SingleOrDefault(c => c.ContentId == contentId);// && user.UserCatalogRoles.Any(x => x.CatalogId == c.CatalogId)).SingleOrDefault();

				// check if user has access to catalog
				if (!content.IsAvailableTo(user)){
				//if (content == null || (content != null && !user.IsSuperAdmin() && !user.UserCatalogRoles.AsParallel().Any(x => x.CatalogId == content.CatalogId))) {
					throw new ArgumentOutOfRangeException("Content does not exist or you do not have access");					
				}

				if (content != null) {
					content.UserDownloadableName = content.DownloadableName(user.FileSignature(content));
					content.IsInMyActiveCart = Account.CartContents().Contains(content.ContentId);// myActiveCart != null && myActiveCart.Contents != null && myActiveCart.Contents.Any(c => c.ContentId == contentId);
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


			using (var ctx = new SongSearchContext()) {
				ctx.ContextOptions.LazyLoadingEnabled = false;
				// Get all Search Properties
				var props = CacheService.SearchProperties((Roles)user.RoleId);//.dataSession.All<SearchProperty>().ToList();
				//var contentQuery = session.All<Content>();
				var contentQuery = ctx.Contents
					.Include("ContentMedia").AsQueryable();

				contentQuery = contentQuery.BuildSearchDynamicLinqSql(searchFields, props);//Where("Title.Contains(@0)", "love");
				//var preCountCommand = sbPre.Append(sbCommand.ToString()).ToString();

				//limit to user catalogs
				if (user == null) {
					throw new ArgumentException("Invalid user");
				}

				if (!user.IsSuperAdmin()) {

					var userId = user.UserId;
					var userCatalogs = user.UserCatalogRoles.Select(c => c.CatalogId);//.ToList();//session.All<UserCatalogRole>();

					contentQuery = contentQuery.Where(c => userCatalogs.Contains(c.CatalogId));
					//contentQuery = from c in contentQuery
					//               join u in userCatalogs on c.CatalogId equals u.CatalogId
					//               where u.UserId == userId
					//               select c;//.Where(c => c.Catalog.UserCatalogRoles.Any(rm => rm.UserId == userId));
					
				}

				//System.Diagnostics.Debug.Write(contentQuery.Expression.ToString());

				// Get user request sort field and direction
				var sortProp = sortPropertyId.HasValue ? props.Where(p => p.PropertyId == sortPropertyId.Value).SingleOrDefault() : null;

				contentQuery = sortProp != null ? contentQuery.UserSearchSort(sortProp, sortType) : contentQuery.DefaultSearchSort();

				pageIndex = pageIndex ?? 0;
				pageSize = pageSize ?? 100;

				return contentQuery.ToPagedList(pageIndex.Value, pageSize.Value);

			}
		}

		// **************************************
		// BuildSearchDynamicLinqSql
		// **************************************
		private static IQueryable<Content> BuildSearchDynamicLinqSql(this IQueryable<Content> query,
																		IList<SearchField> searchFields,
																		IList<SearchProperty> properties) {

			var currentlyFieldIndexed = new string[] { "Lyrics" }; //Columns that have dedicated index fields

	
			var searchGroups = from p in properties
							   group p by p.SearchGroup into g
							   select new { SearchGroup = g.Key, SearchProperties = g };
			

			foreach (var field in searchFields) {

				var sb = new StringBuilder();
				int prmIndx = 0;
				// get group for each field
				var grp = searchGroups.Where(
					g => g.SearchProperties.Any(
						p => p.PropertyId == field.P
						)
					).SingleOrDefault();

				var props = grp.SearchProperties.ToArray();
				var searchType = (SearchTypes)props.First().SearchTypeId;
				var searchValues = new string[] {};
				var searchParams = new List<string>();

				//var prop = properties.Where(p => p.PropertyId == field.P).SingleOrDefault();
				foreach (var prop in props) {

					if (prop != null) {
						// build search 

						string columnName = prop.PropertyName.Trim();
						string predicate = String.Empty;

						switch (searchType){//(SearchTypes)prop.SearchTypeId) {

							case SearchTypes.Contains:

								searchValues = field.V.Select(v => v.MakeSearchableValue()).ToArray();
								columnName = !prop.IsIndexable ?
									(
										currentlyFieldIndexed.Contains(columnName) ?
										String.Concat(columnName, "Index") : columnName
										) :
									columnName.MakeSearchableColumnName();

								if (searchValues.First() != null) {

									var startsWithSearch = field.V.First().IsStartsWithSearch();
									var preciseSearch = field.V.First().IsPreciseSearch();
									var searchValuesParsed = searchValues.First().Split(' ');

									if (!startsWithSearch && !preciseSearch && searchValuesParsed.IsMultiSearch()) {

										foreach (var val in searchValuesParsed) {

											predicate = String.Format("{0}.Contains(@{1})", columnName, prmIndx);
											searchParams.Add(val);
											prmIndx++;
											
											sb.Append(predicate);
											
											if (Array.IndexOf(searchValuesParsed, val) < searchValuesParsed.Length - 1) {
												sb.Append(" and ");
											}

										}
			
										//sb.Append(predicate);

									} else {
										var val = searchValues.First();

										predicate = String.Format(
											startsWithSearch ? "{0}.StartsWith(@{1})" : "{0}.Contains(@{1})"
											, columnName
											, prmIndx);//.MakeSearchableColumn(),
										val = val.TrimPreciseSearch().TrimStartsWithSearch();
										searchParams.Add(val);
											
										prmIndx++;
										sb.Append(predicate);

									}
								}
								break;

							case SearchTypes.HasValue:
								searchValues = field.V.Select(v => v.MakeSearchableValue()).ToArray();

								if (searchValues.First() != null) {

									predicate = String.Format("{0} != null", columnName);
									sb.Append(predicate);
								}
								break;

							case SearchTypes.IsTrue:
								searchValues = field.V.Select(v => v.MakeSearchableValue()).ToArray();

								if (searchValues.First() != null) {

									predicate = String.Format("{0} == true", columnName);
									sb.Append(predicate);
								}
								break;

							case SearchTypes.Range:
								int i;
								searchValues = field.V.Select(v => v.MakeSearchableValue()).ToArray();

								var range = searchValues.Select(
									x => (String.IsNullOrWhiteSpace(x) || int.TryParse(x, out i) == false)
										? null
										: (int?)int.Parse(x)
									);
								if (range.Any(v => v != null)) {

									if (range.All(v => v.HasValue)) {
										// two valid values
										predicate = String.Format("{0} >= {1} && {0} <= {2}", columnName, range.First(), range.Last());
										sb.Append(predicate);

									} else {
										//one valid value
										if (range.First().HasValue) {
											// first value only
											predicate = String.Format("{0} == {1}", columnName, range.First());
											sb.Append(predicate);

										} else {
											// second value only
											predicate = String.Format("{0} <= {1}", columnName, range.Last());
											sb.Append(predicate);
										}
									}
								}
								break;
							case SearchTypes.Tag:

								searchValues = field.V;//.Select(v => v.MakeSearchableValue()).ToArray();

								var tagValues = searchValues.SplitTags(';').Distinct().ToArray(); //could also just replace, but this way it throws for non-numeric values

								foreach (var itm in tagValues) {
									var tagId = itm;
									predicate = String.Format("Tags.Any(TagId == {0})", tagId);
									sb.Append(predicate);
									if (Array.IndexOf(tagValues, itm) < tagValues.Length - 1) {
										sb.Append(" and ");
									}

								}
								break;
							
							default:
								goto case (SearchTypes.Contains);

						}


					}

					if (Array.IndexOf(props, prop) < props.Length - 1) {
						sb.Append(" or ");
					}
				}

				//attach predicate to the query;
				query = query.Where(sb.ToString(), searchParams.ToArray());
				searchParams.Clear();
				sb.Clear();
			}

			return query;
		}

		// **************************************
		// GetQueryFilter
		// **************************************
		private delegate IQueryable<Content> GetQueryFilterDelegate(IQueryable<Content> query, SearchField field, string dbColumnName);
		private static IDictionary<SearchTypes, GetQueryFilterDelegate> _filterMatrix = new Dictionary<SearchTypes, GetQueryFilterDelegate>();

		private static void SetUpFilterMatrix() {

			_filterMatrix.Add(SearchTypes.Contains, GetQueryFilterContains);
			_filterMatrix.Add(SearchTypes.HasValue, GetQueryFilterHasValue);
			_filterMatrix.Add(SearchTypes.IsTrue, GetQueryFilterIsTrue);
			_filterMatrix.Add(SearchTypes.Range, GetQueryFilterRange);
			//_filterMatrix.Add(SearchTypes.Tag, GetQueryFilterTag);
			_filterMatrix.Add(SearchTypes.TagText, GetQueryFilterTagText);


		}
		private static IQueryable<Content> GetQueryFilterContains(IQueryable<Content> query, SearchField field, string dbColumnName) {
			// for each value in searchfield, build dynamic ling predicate
			return query;// String.Empty;
		}
		private static IQueryable<Content> GetQueryFilterHasValue(IQueryable<Content> query, SearchField field, string dbColumnName) {
			var searchValues = field.V.Select(v => v.MakeSearchableValue()).ToArray();
			return searchValues.First() != null ?
				query.Where(String.Format("{0} != null", dbColumnName)) :
				query;
		}
		private static IQueryable<Content> GetQueryFilterRange(IQueryable<Content> query, SearchField field, string dbColumnName) {
			return query;
		}
		private static IQueryable<Content> GetQueryFilterIsTrue(IQueryable<Content> query, SearchField field, string dbColumnName) {
			var searchValues = field.V.Select(v => v.MakeSearchableValue()).ToArray();
			return searchValues.First() != null ?
				query.Where(String.Format("{0} == true", dbColumnName)) :
				query;
		}

		private static IQueryable<Content> GetQueryFilterTagText(IQueryable<Content> query, SearchField field, string dbColumnName) {
			return query;
		}

		// **************************************
		// AddSearchPropertyCriteria
		// **************************************

		private static IQueryable<Content> AddSearchPropertyCriteria(this IQueryable<Content> query,
																		IList<SearchField> searchFields,
																		IList<SearchProperty> properties) {

			// loop through searchFields
			var currentlyFieldIndexed = new string[] { "Lyrics" }; //Columns that have dedicated index fields
			var searchGroups = from p in properties
							   group p by p.SearchGroup into g
							   select new { SearchGroup = g.Key, SearchProperties = g };

			foreach (var field in searchFields) {

				// get group for each field
				var grp = searchGroups.Where(
					g => g.SearchProperties.Any(
						p => p.PropertyId == field.P
						)
					).SingleOrDefault();
			
				var props = grp.SearchProperties;
				var searchType = (SearchTypes)props.First().SearchTypeId;
				
				// loop through props for each group
				foreach (var prop in props) {

					var columnName = prop.PropertyName.Trim();

					// process according to SearchType
					query = _filterMatrix[searchType](query, field, columnName);
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
			var sortField = sortProperty != null ? sortProperty.PropertyName : null;
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

	
		
		//// **************************************
		//// MakeSearchableColumn
		//// **************************************
		//private static string MakeSearchableColumn(this string value) {
		//    value = string.Format(@"upper({0})", value);
		//    var replacements = new string[] { @",", @"''", @";", @":", @"\\", @"/" };//, @"|", @"{", @"}", @"[", @"]", @"?", @"<", @">", @".", @"!", "*" };
		//    replacements.ForEach(x => value = String.Format(@"replace({0}, '{1}','')", value, x));
		//    return value;
		//}

		

	}
}