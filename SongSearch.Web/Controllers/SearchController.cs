using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Services;
using SongSearch.Web.Data;

namespace SongSearch.Web.Controllers
{
	[RequireAuthorization]
	public class SearchController : Controller
    {
        //
        // GET: /Search/
		public ActionResult Index()
        {
			var model = GetSearchViewModel();
			return View(model);
        }

		// **************************************
		// GetAdvancedSearchMenu
		// **************************************
		public ActionResult Results(IList<SearchField> s) {

			var model = GetSearchViewModel();
			var isValid = s.Any(x => x.V.Any(v => !String.IsNullOrWhiteSpace(v)));

			if (isValid) {
				var searchFields = s.Where(x => x.V.Any(v => !String.IsNullOrWhiteSpace(v))).ToList();

				var results = SearchService.SearchContent(searchFields);
				model.SearchResults = results;
				model.SearchFields = searchFields;
				return View(model);
			}

			return RedirectToAction("Index");
		}

		// **************************************
		// GetSearchViewModel
		// **************************************
		private SearchViewModel GetSearchViewModel() {

			var role = (Roles) AccountData.User(User.Identity.Name).RoleId;
			var model = new SearchViewModel() {
				NavigationLocation = "Search",				
				SearchMenuProperties = SearchService.GetSearchMenuProperties(role)
			};

			return model;

		}
    }
}
