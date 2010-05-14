using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SongSearch.Web.Services;

namespace SongSearch.Web.Controllers
{
	[RequireAuthorization]
	public class SearchController : Controller
    {
        //
        // GET: /Search/
		public ActionResult Index()
        {
			// Build Search Menu Choices from Search Properties
			return View();
        }

		public ActionResult Results(IList<SearchField> s) {

			var isValid = s.Any(x => x.V.Any(v => !String.IsNullOrWhiteSpace(v)));

			if (isValid) {
				var searchFields = s.Where(x => x.V.Any(v => !String.IsNullOrWhiteSpace(v))).ToList();

				var results = SearchService.SearchContent(searchFields);
				return View(results);
			}

			return RedirectToAction("Index");
		}
    }
}
