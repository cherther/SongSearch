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
		// GetSearchMenuProperties
		// **************************************
		public static IList<SearchProperty> GetSearchMenuProperties(Roles role) {

			using (ISession session = new EFSession()) {

				// Get Search Properties
				return session.All<SearchProperty>().Where(x => x.IncludeInSearchMenu && x.AccessLevel >= (int)role).ToList();
			}
		}

		// **************************************
		// SearchContentDynamic
		// **************************************
		public static PagedList<Content> SearchContentDynamic(IList<SearchField> searchFields, string userName, int? pageSize = null, int? pageIndex = null) {


			using (ISession session = new EFSession()) {

				// Get Search Properties
				var props = session.All<SearchProperty>().ToList();
				var contentQuery = session.All<Content>();
				contentQuery = contentQuery.BuildSearchDynamicLinqSql(searchFields, props, userName);//Where("Title.Contains(@0)", "love");
				//var preCountCommand = sbPre.Append(sbCommand.ToString()).ToString();

				//limit to user catalogs
				var user = AccountData.User(userName);
				if (user == null) {
					throw new ArgumentException("Invalid username");
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

				contentQuery = contentQuery.DefaultSearchSort();

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
			IList<SearchProperty> properties,
			string userName) {


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
								//sbWhere.AppendLine(String.Format(@"c.{0} {1}", prop.PropertyCode, prop.SearchPredicate));
								string predicate = String.Format("{0} != null", columnName);//.MakeSearchableColumn(),
								query = query.Where(predicate);
							}
							break;
						
						case SearchTypes.IsTrue:
							if (searchableValues.First() != null) {
								//sbWhere.AppendLine(String.Format(@"c.{0} {1}", prop.PropertyCode, prop.SearchPredicate));
								string predicate = String.Format("{0} == true", columnName);//.MakeSearchableColumn(),
								query = query.Where(predicate);
							}
							break;

						case SearchTypes.Range:
							int i;
							
							var range = searchableValues.Select(
								x => (String.IsNullOrWhiteSpace(x) || int.TryParse(x, out i) == false) 
									? null 
									: (int?) int.Parse(x)
								);
							if (range.Any(v => v != null)) {

								if (range.All(v => v.HasValue)) {
									// two valid values
									string predicate = String.Format("{0} >= @0 && {0} <= @1", columnName);//.MakeSearchableColumn(),

									query = query.Where(predicate, range.First(), range.Last());
								}
								else {
									//one valid value
									if (range.First().HasValue) {
										// first value only
										string predicate = String.Format("{0} == @0", columnName);//.MakeSearchableColumn(),

										query = query.Where(predicate, range.First());
								
									}
									else {
										// second value only
										string predicate = String.Format("{0} <= @0", columnName);//.MakeSearchableColumn(),

										query = query.Where(predicate, range.Last());
									}
								}
							}
							break;
						case SearchTypes.Tag:

							var tagValues = searchableValues.SplitTags(';').Distinct(); //could also just replace, but this way it throws for non-numeric values
							//var tagSearchValue = String.Join(",", tagValues);
							/*
								select c1.contentid 
								from dbo.contenttags c1 inner join dbo.tags t on c1.tagid = t.tagid
								where
								t.tagid in (7,13,14,18,19)
								group by c1.ContentId
							 * */
							foreach (var tagId in tagValues) {
								query = query.Where(c => c.Tags.Any(t => t.TagId == tagId));

								//sbJoin.AppendLine(@"inner join");
								//sbJoin.AppendLine(@"(select ContentId from dbo.ContentTags");
								//sbJoin.AppendLine(String.Format(@"where TagId = {0}", tagId));
								//sbJoin.AppendLine(String.Format(@"group by ContentId) ct_{0} on c.ContentId = ct_{0}.ContentId", tagId));
							}
							break;

						default:
							goto case (SearchTypes.Contains);

					}


				}
				// check if we're on the last field
				//if (sbWhere.Length > 0 && field != searchFields.Last()) {
				//    sbWhere.Append(_add);
				//}
			}

			return query;
		}

		// **************************************
		// BuildSearchSql
		// **************************************
		private static string BuildSearchSql(IList<SearchField> searchFields, IList<SearchProperty> properties, string userName) {

			// Build Search string based on search type
			//var query = session.All<Content>();

			var sbJoin = new StringBuilder();
			var sbWhere = new StringBuilder();

			foreach (var field in searchFields) {

				var searchableValues = field.V.Select(v => v.MakeSearchableValue()).ToArray();

				var prop = properties.Where(p => p.PropertyId == field.P).SingleOrDefault();
				if (prop != null) {
					// build search string
					switch ((SearchTypes)prop.SearchTypeId) {

						case SearchTypes.Contains:

							if (searchableValues.First() != null & prop.SearchPredicate != null) {

								var startsWithSearch = field.V.First().IsStartsWithSearch();
								var preciseSearch = field.V.First().IsPreciseSearch();
								var search = searchableValues.First().Split(' ');

								if (!startsWithSearch && !preciseSearch && search.IsMultiSearch()) {
									foreach (var val in search) {
										string valFormat = @"%{0}%";
										string column = String.Concat("c.", prop.PropertyCode);
										sbWhere.AppendLine(
											String.Format(@"{0} {1}", column.MakeSearchableColumn(),
												String.Format(prop.SearchPredicate,
													String.Format(valFormat, val)
													)
												)
											);

										if (val != search.Last()) { sbWhere.Append(_add); }
									}
								} else {
									var val = searchableValues.First();

									string valFormat = startsWithSearch ? @"{0}%" : @"%{0}%";
									string column = String.Concat("c.", prop.PropertyCode);
									sbWhere.AppendLine(String.Format(@"{0} {1}",
										column.MakeSearchableColumn(),
											String.Format(prop.SearchPredicate,
												String.Format(valFormat, val)
											)));
								}
							}
							break;

						case SearchTypes.Join:
							if (searchableValues.First() != null & prop.SearchPredicate != null) {
								var joinTable = prop.LookupName;
								var joinField = prop.PropertyCode;
								var searchField = prop.PropertyName;

								sbJoin.AppendLine(String.Format(@"inner join dbo.{0} r_{1} on c.{1} = r_{1}.{1}", joinTable, joinField));
								sbWhere.AppendLine(String.Format(@"r_{0}.{1} {2}", joinField, searchField,
									String.Format(prop.SearchPredicate, searchableValues.First())));
							}
							break;

						case SearchTypes.HasValue:
							if (searchableValues.First() != null & prop.SearchPredicate != null) {
								sbWhere.AppendLine(String.Format(@"c.{0} {1}", prop.PropertyCode, prop.SearchPredicate));
							}
							break;

						case SearchTypes.Range:
							if (searchableValues.Any(v => !String.IsNullOrWhiteSpace(v)) & prop.SearchPredicate != null) {
								// two valid values
								if (searchableValues.All(v => !String.IsNullOrWhiteSpace(v))) {
									sbWhere.AppendLine(String.Format(@"c.{0} {1}", prop.PropertyCode,
										String.Format(prop.SearchPredicate, searchableValues.First(), searchableValues.Last())));
								}
									//one valid value
								else {
									// first value only
									if (!String.IsNullOrEmpty(searchableValues[searchableValues.GetLowerBound(0)])) {
										sbWhere.AppendLine(String.Format(@"c.{0} = {1}", prop.PropertyCode, searchableValues.First()));
									}
										// second value only
									else {
										sbWhere.AppendLine(String.Format(@"c.{0} <= {1}", prop.PropertyCode, searchableValues.Last()));
									}
								}
							}
							break;
						case SearchTypes.Tag:

							var tagValues = searchableValues.SplitTags(';').Distinct(); //could also just replace, but this way it throws for non-numeric values
							//var tagSearchValue = String.Join(",", tagValues);
							/*
								select c1.contentid 
								from dbo.contenttags c1 inner join dbo.tags t on c1.tagid = t.tagid
								where
								t.tagid in (7,13,14,18,19)
								group by c1.ContentId
							 * */
							foreach (var tagId in tagValues) {
								sbJoin.AppendLine(@"inner join");
								sbJoin.AppendLine(@"(select ContentId from dbo.ContentTags");
								sbJoin.AppendLine(String.Format(@"where TagId = {0}", tagId));
								sbJoin.AppendLine(String.Format(@"group by ContentId) ct_{0} on c.ContentId = ct_{0}.ContentId", tagId));
							}
							break;

						default:
							goto case (SearchTypes.Contains);

					}


				}
				// check if we're on the last field
				if (sbWhere.Length > 0 && field != searchFields.Last()) {
					sbWhere.Append(_add);
				}


			}

			if (sbWhere.Length == 0 && sbJoin.Length == 0) {

				throw new ArgumentException("No valid search values");
			}

			//limit to user catalogs
			var user = AccountData.User(userName);
			if (user == null) {
				throw new ArgumentException("Invalid username");
			}

			if (!user.IsSuperAdmin()) {
				var userId = user.UserId;
				sbJoin.AppendLine("inner join dbo.UserCatalogRoles ucr on c.CatalogId = ucr.CatalogId");
				var hasConcat = sbWhere.ToString().EndsWith(_add);
				sbWhere.AppendLine(String.Concat((!hasConcat ? _add : String.Empty), "ucr.UserId = ", userId));
			}

			var sbQuery = new StringBuilder();
			sbQuery
				.AppendLine("select * from dbo.Contents c")
				.AppendLine(sbJoin.ToString())
				.AppendLine("where")
				.AppendLine(sbWhere.ToString());

			var sql = sbQuery.ToString();
			sbQuery = null;
			sbJoin = null;
			sbWhere = null;
			user = null;

			return sql;
		}


		// ----------------------------------------------------------------------------
		// (Private Extensions_
		// ----------------------------------------------------------------------------
		// **************************************
		// DefaultSearchSort
		// **************************************
		private static IQueryable<Content> DefaultSearchSort(this IQueryable<Content> query) {

			return query
					.OrderBy(c => !c.PopCharts.HasValue ? 1000 : c.PopCharts)
					.ThenBy(c => !c.CountryCharts.HasValue ? 1000 : c.CountryCharts)
					.ThenBy(c => c.Title.Length == 0 ? "zzz" : c.Title)
					.ThenBy(c => c.Artist.Length == 0 ? "zzz" : c.Artist);

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