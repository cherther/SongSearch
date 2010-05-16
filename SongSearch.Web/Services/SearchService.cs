using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using SongSearch.Web.Data;

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
		// SearchContent
		// **************************************
		public static IList<Content> SearchContent(IList<SearchField> searchFields, string userName, int? pageSize = null, int? pageNumber = null) {

			using (ISession session = new EFSession()) {
				
				// Get Search Properties
				var props = session.All<SearchProperty>().ToList();
				
				// Build Search string based on search type
				//var query = session.All<Content>();
				var sbJoin = new StringBuilder();
				var sbWhere = new StringBuilder();
				
				
				foreach (var field in searchFields) {
					
					var searchableValues = field.V.Select(v => v.MakeSearchableValue()).ToArray();

					var prop = props.Where(p => p.PropertyId == field.P).SingleOrDefault();
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
											string valFormat = "%{0}%";
											sbWhere.AppendLine(
												String.Format("c.{0} {1}", prop.PropertyCode, 
													String.Format(prop.SearchPredicate, 
														String.Format(valFormat, val)
														)
													)
												);

											if (val != search.Last()) { sbWhere.Append(_add); }
										}
									} else {
										var val = searchableValues.First();

										string valFormat = startsWithSearch ? "{0}%" : "%{0}%";
										sbWhere.AppendLine(String.Format("c.{0} {1}", 
											prop.PropertyCode, 
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

									sbJoin.AppendLine(String.Format("inner join dbo.{0} r_{1} on c.{1} = r_{1}.{1}", joinTable, joinField));
									sbWhere.AppendLine(String.Format("r_{0}.{1} {2}", joinField, searchField,
										String.Format(prop.SearchPredicate, searchableValues.First())));
								}
								break;

							case SearchTypes.HasValue:
								if (searchableValues.First() != null & prop.SearchPredicate != null) {
									sbWhere.AppendLine(String.Format("c.{0} {1}", prop.PropertyCode, prop.SearchPredicate));
								}
								break;

							case SearchTypes.Range:
								if (searchableValues.Any(v => !String.IsNullOrWhiteSpace(v)) & prop.SearchPredicate != null) {
									// two valid values
									if (searchableValues.All(v => !String.IsNullOrWhiteSpace(v))) {
										sbWhere.AppendLine(String.Format("c.{0} {1}", prop.PropertyCode, 
											String.Format(prop.SearchPredicate, searchableValues.First(), searchableValues.Last())));
									}
										//one valid value
									else {
										// first value only
										if (!String.IsNullOrEmpty(searchableValues[searchableValues.GetLowerBound(0)])) {
											sbWhere.AppendLine(String.Format("c.{0} = {1}", prop.PropertyCode, searchableValues.First()));
										}
											// second value only
										else {
											sbWhere.AppendLine(String.Format("c.{0} <= {1}", prop.PropertyCode, searchableValues.Last()));
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
									sbJoin.AppendLine("inner join");
									sbJoin.AppendLine("(select ContentId from dbo.ContentTags");
									sbJoin.AppendLine(String.Format("where TagId = {0}", tagId));
									sbJoin.AppendLine(String.Format("group by ContentId) ct_{0} on c.ContentId = ct_{0}.ContentId", tagId));
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

				var sbCommand = new StringBuilder();
				sbCommand.AppendLine("select * from dbo.Contents c");
				sbCommand.AppendLine(sbJoin.ToString());
				sbCommand.AppendLine("where").AppendLine(sbWhere.ToString());
				var commandText = sbCommand.ToString();
				System.Diagnostics.Debug.Write(commandText);

				sbCommand = null;
				sbWhere = null;
				sbJoin = null;
				user = null;
				
				var query = session.All<Content>(commandText: commandText, parameters: null);
				query = query.DefaultSearchSort();

				//if (pageNumber.HasValue && pageSize.HasValue){

				//    var skip = (pageNumber.Value - 1) * pageSize.Value;
				//    query = query.Skip(skip).Take(pageSize.Value);
				//}

				var results = query.ToList();

				return results;
			}

			
		
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
			return (value.EndsWith(_leftSearchChar) && value.Length >= _fullSearchMin);
		}
		// **************************************
		// IsMultiSearch
		// **************************************
		private static bool IsMultiSearch(this string[] searchValues) {
			return (searchValues.Length > 1 && searchValues.All(x => !String.IsNullOrWhiteSpace(x)));
		}

		
	}
}