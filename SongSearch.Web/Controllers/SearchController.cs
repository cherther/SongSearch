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

		protected override void Initialize(RequestContext requestContext) {
			if (!String.IsNullOrWhiteSpace(requestContext.HttpContext.User.Identity.Name)) {
				_cartService.ActiveUserName = requestContext.HttpContext.User.Identity.Name;				
			}
			base.Initialize(requestContext);
		}

		ICartService _cartService;

		public SearchController(ICartService cartService) {
			_cartService = cartService;
		}

		//
		// GET: /Search/
		public virtual ActionResult Index()
		{
			try {
				var lastCart = GetLastCart();
				var msg = lastCart != 0 ? this.HttpContext.Session.ProcessingCartMessage(lastCart) : null;
				if (msg != null) {
					this.FeedbackInfo(msg);
				}
				var user = Account.User();
				var vm = GetSearchViewModel(user);

				return View(vm);
			}
			catch (Exception ex) {
				Log.Error(ex);

				this.FeedbackError("There was an error loading the Search page. Please try again in a bit.");
				return RedirectToAction(MVC.Home.Index());
			}
		}


		// ****************************************************************************
		// Artist/Johnny
		// ****************************************************************************
		public virtual ActionResult Artist(string name) {

			SearchField fld = new SearchField() {
				P = 2,
				T = SearchTypes.Contains,
				V = new string[] { name }
			};

			IList<SearchField> f = new List<SearchField>() { fld };
			return Results(f, null, null, null);
		}

		// ****************************************************************************
		// Catalog/Ford
		// ****************************************************************************
		[RequireAuthorization(MinAccessLevel=Roles.Admin)]
		public virtual ActionResult Catalog(string name) {

			SearchField fld = new SearchField() {
				P = 16,
				T = SearchTypes.Contains,
				V = new string[] { name }
			};
			IList<SearchField> f = new List<SearchField>();
			f.Add(fld);
			return RedirectToAction("Results", new { f = f, p = 0, s = 0, o = 0 });
		}
		
		// ****************************************************************************
		// Search/Results/ f = SearchField, p = PageIndex, s = SortField, o = SortType
		// ****************************************************************************
		public virtual ActionResult Results(IList<SearchField> f, int? p, int? s, int? o) {

			var isValid = true; f.Any(x => x.V.Any(v => !String.IsNullOrWhiteSpace(v)));

			if (isValid) {

				try {

					var model = GetSearchResults(f, p, s, o);
					model.ViewMode = ViewModes.Embedded;

					var lastCart = GetLastCart();
					var msg = lastCart != 0 ? this.HttpContext.Session.ProcessingCartMessage(lastCart) : null;
					if (msg != null) {
						this.FeedbackInfo(msg);
					}

					return View("Results", model);
				}
				catch (Exception ex) {
					Log.Error(ex);

					this.FeedbackError("There was an error getting your search results. Please try again in a bit.");
				}
			}

			this.FeedbackWarning("Please try your search again.");
			return RedirectToAction(Actions.Index());
		}

		

		// **************************************
		// Print/5
		// **************************************
		public virtual ActionResult Print(IList<SearchField> f, int? p, int? s, int? o) {
			var isValid = f.Any(x => x.V.Any(v => !String.IsNullOrWhiteSpace(v)));

			if (isValid) {

				try {

					var model = GetSearchResults(f, p, s, o);
					model.ViewMode = ViewModes.Print;
					return View(Views.PrintResults, model);
				}
				catch (Exception ex) {
					Log.Error(ex);

					this.FeedbackError("There was an error getting your search results. Please try again in a bit.");
				}
			}

			this.FeedbackWarning("Please try your search again.");
			return RedirectToAction(Actions.Index());
		}
		// **************************************
		// AutoComplete/f = fieldName, term = search term
		// **************************************
		public virtual JsonResult AutoComplete(string f, string term) {

			try {
				term = term.ToUpper().MakeSearchableValue();
				var values = f.Equals("Catalog.CatalogName", StringComparison.InvariantCultureIgnoreCase) ?
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

				var searchValues = values.Select(v => new { Value = v, SearchValue = v.MakeSearchableValue() }).AsParallel().ToArray();
				if (term.Length < 3) {
					values = searchValues != null ? searchValues.Where(v => v.SearchValue.StartsWith(term)).Select(v => v.Value).ToArray() : values;
				} else {

					values = searchValues != null ? searchValues.Where(v => v.SearchValue.Contains(term)).Select(v => v.Value).ToArray() : values;
				}

				return Json(values, JsonRequestBehavior.AllowGet);
			}
			catch { //(Exception ex) {
					
				return Json("", JsonRequestBehavior.AllowGet);

			}
		}

		// **************************************
		// GetSearchResults
		// **************************************
		private SearchViewModel GetSearchResults(IList<SearchField> f, int? p, int? s, int? o) {
			var user = Account.User();
			var model = GetSearchViewModel(user);

			model.SortPropertyId = s;
			model.SortType = o.HasValue ? (SortType)o.GetValueOrDefault() : SortType.None;

			var searchFields = f.Where(x => x.V != null &&
				x.V.Any(v =>
					v != null &&
					!String.IsNullOrWhiteSpace(v)
					&& !v.Equals(bool.FalseString, StringComparison.InvariantCultureIgnoreCase)
					)
				).ToList();

			
			var results = SearchService.GetContentSearchResults(searchFields, user, s, o, _pageSize, p);

			var session = SessionService.Session();
			var activeCart = Account.CartContents();
			if (activeCart != null) {
				results.ForEach(c => c.IsInMyActiveCart = activeCart.Contains(c.ContentId));
			}
			model.SearchResults = results;
			model.SearchFields = searchFields;
			session.SessionUpdate(searchFields, "SearchFields");

			model.RequestUrl = Request.RawUrl.Replace(String.Format("&p={0}", results.PageIndex), "");
			model.PagerSortUrl = model.SortPropertyId.HasValue ?
				String.Format("&s={0}&o={1}", model.SortPropertyId.Value, (int)model.SortType) : "";
			model.HeaderSortUrl = model.RequestUrl.Replace(String.Format("&s={0}&o={1}", model.SortPropertyId.GetValueOrDefault(), (int)model.SortType), "");
			model.SearchResultsHeaders = new string[] { "Title", "Artist", "Pop", "Country", "ReleaseYear" };
			return model;
		}

		// **************************************
		// GetSearchViewModel
		// **************************************
		private SearchViewModel GetSearchViewModel(User user) {

			return new SearchViewModel() {
				NavigationLocation = new string[] { "Search" },
				SearchMenuProperties = CacheService.SearchProperties((Roles)user.RoleId).OrderBy(x => x.SearchMenuOrder).ToList(),
				SearchTags = CacheService.TopTags()
			};


		}

		private int GetLastCart() {
			var lastCart = _cartService.MyCarts().GetLastProcessedCartId();
			return lastCart;
		}
	}
}
