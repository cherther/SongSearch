using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Services;
using SongSearch.Web.Data;
using System.Web.Routing;

namespace SongSearch.Web.Controllers
{
	[RequireAuthorization]
	[HandleError]
	public partial class SearchController : Controller
    {

		private const int _pageSize = 100;

		private User _currentUser;

		protected override void Initialize(RequestContext requestContext) {
			_currentUser = CacheService.User(requestContext.HttpContext.User.Identity.Name);
			base.Initialize(requestContext);
		}

		
        //
        // GET: /Search/
		public virtual ActionResult Index()
        {
			try {
				var vm = GetSearchViewModel();

				return View(vm);
			}
			catch {
				this.FireError("There was an error loading the Search page. Please try again in a bit.");
				return RedirectToAction(MVC.Home.Index());
			}
        }

		// ****************************************************************************
		// Search/Results/ f = SearchField, p = PageIndex, s = SortField, o = SortType
		// ****************************************************************************
		public virtual ActionResult Results(IList<SearchField> f, int? p, int? s, int? o) {

			var model = GetSearchViewModel();
			model.SortPropertyId = s;
			model.SortType = o.HasValue ? (SortType)o.GetValueOrDefault() : SortType.None;
			
			var isValid = f.Any(x => x.V.Any(v => !String.IsNullOrWhiteSpace(v)));

			if (isValid) {
				try {
					var searchFields = f.Where(x => x.V != null && 
					x.V.Any(v => 
					v != null && 
					!String.IsNullOrWhiteSpace(v) 
					&& !v.Equals(bool.FalseString, StringComparison.InvariantCultureIgnoreCase)
					)
					).ToList();

					var results = SearchService.GetContentSearchResults(searchFields, _currentUser, s, o, _pageSize, p);

				
					model.SearchResults = results;
					model.SearchFields = searchFields;
					CacheService.SessionUpdate(searchFields, "SearchFields");

					model.RequestUrl = Request.RawUrl.Replace(String.Format("&p={0}", results.PageIndex), "");
					model.PagerSortUrl = model.SortPropertyId.HasValue ?
						String.Format("&s={0}&o={1}", model.SortPropertyId.Value, (int)model.SortType) : "";
					model.HeaderSortUrl = model.RequestUrl.Replace(String.Format("&s={0}&o={1}", model.SortPropertyId.GetValueOrDefault(), (int)model.SortType), "");
					model.SearchResultsHeaders = new string[] { "Title", "Artist", "Pop", "Country", "ReleaseYear"};

					return View(model);
				}
				catch {
					this.FireError("There was an error getting your search results. Please try again in a bit.");
				}
			}

			this.FireWarning("Please try your search again.");
			return RedirectToAction(Actions.Index());
		}

		
		// **************************************
		// AutoComplete/f = fieldName, term = search term
		// **************************************
		public virtual JsonResult AutoComplete(string f, string term) {

			try {
				term = term.ToUpper();
				var values = f.Equals("CatalogId", StringComparison.InvariantCultureIgnoreCase) ?
					CacheService.Catalogs().Select(c => c.CatalogName.ToUpper()).ToArray() :
					(
						CacheService.CachedContentFields.Any(x => x.Equals(f, StringComparison.InvariantCultureIgnoreCase)) ?
						CacheService.ContentField(f).Where(v => v != null).ToArray() :
						(
						CacheService.CachedContentRightsFields.Any(x => x.Equals(f, StringComparison.InvariantCultureIgnoreCase)) ?
							CacheService.ContentRightsField(f).Where(v => v != null).ToArray() :
							new string[] { "" }
						)
					);

				if (term.Length < 3) {
					values = values != null ? values.Where(v => v.MakeSearchableValue().StartsWith(term.MakeSearchableValue())).ToArray() : values;
				} else {

					values = values != null ? values.Where(v => v.MakeSearchableValue().Contains(term.MakeSearchableValue())).ToArray() : values;
				}

				return Json(values, JsonRequestBehavior.AllowGet);
			}
			catch {
					
				return Json("", JsonRequestBehavior.AllowGet);

			}
		}

		// **************************************
		// GetSearchViewModel
		// **************************************
		private SearchViewModel GetSearchViewModel() {

			return new SearchViewModel() {
				NavigationLocation = "Search",
				SearchMenuProperties = CacheService.SearchProperties((Roles)_currentUser.RoleId),
				SearchTags = CacheService.TopTags()
			};


		}

		
		

    }
}
