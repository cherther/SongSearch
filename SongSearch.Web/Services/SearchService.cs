using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SongSearch.Web.Data;

namespace SongSearch.Web.Services {
	public static class SearchService {

		public static IList<SearchProperty> GetSearchMenuProperties(Roles role) {

			using (ISession session = new EFSession()) {

				// Get Search Properties
				return session.All<SearchProperty>().Where(x => x.IncludeInSearchMenu && x.AccessLevel >= (int)role).ToList();
			}
		}

		public static IList<Content> SearchContent(IList<SearchField> searchFields) {

			using (ISession session = new EFSession()) {
				
				// Get Search Properties
				var props = session.All<SearchProperty>().ToList();
				var query = session.All<Content>();

				// Build Search string based on search type
				foreach (var field in searchFields) {
					
					var value = String.Join(" ", field.V).MakeSearchableValue();

					var prop = props.Where(p => p.PropertyId == field.P).SingleOrDefault();
					if (prop != null) {
						// build search string

						//----------------
						// SPIKE
						//----------------
						System.Diagnostics.Debug.WriteLine(prop.PropertyName + ":" + value );
						if (prop.PropertyCode.Equals("Title", StringComparison.InvariantCultureIgnoreCase)) {

							query = query.Where(x => x.Title.Replace(" ", "").Replace("'","").Contains(value));
							break;
						}
						//----------------
					}

				}


				query = query.DefaultSearchSort();

				var results = query.ToList();
				return results;
			}

			
		
		}


		public static IQueryable<Content> DefaultSearchSort(this IQueryable<Content> query) {

			return query
					.OrderBy(c => !c.PopCharts.HasValue ? 1000 : c.PopCharts)
					.ThenBy(c => !c.CountryCharts.HasValue ? 1000 : c.CountryCharts)
					.ThenBy(c => c.Title.Length == 0 ? "zzz" : c.Title)
					.ThenBy(c => c.Artist.Length == 0 ? "zzz" : c.Artist);

		}

	}
}