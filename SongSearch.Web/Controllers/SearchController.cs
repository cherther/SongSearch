﻿using System;
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
	public class SearchController : Controller
    {

		private const int _pageSize = 100;

		private User _currentUser;

		protected override void Initialize(RequestContext requestContext) {
			_currentUser = CacheService.User(requestContext.HttpContext.User.Identity.Name);
			base.Initialize(requestContext);
		}

		
        //
        // GET: /Search/
		public ActionResult Index()
        {
			var model = GetSearchViewModel();
			
			return View(model);
        }

		// ****************************************************************************
		// Search/Results/ f = SearchField, p = PageIndex, s = SortField, o = SortType
		// ****************************************************************************
		public ActionResult Results(IList<SearchField> f, int? p, int? s, int? o) {

			var model = GetSearchViewModel();
			model.SortPropertyId = s;
			model.SortType = o.HasValue ? (SortType)o.GetValueOrDefault() : SortType.None;

			var isValid = f.Any(x => x.V.Any(v => !String.IsNullOrWhiteSpace(v)));

			if (isValid) {
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
				return View(model);
			}

			return RedirectToAction("Index");
		}

		// **************************************
		// Detail/5
		// **************************************
		[OutputCache(Duration = 60, VaryByParam = "id")]
		public ActionResult Detail(int id) {

			//var user = AccountData.User(User.Identity.Name);
			var content = SearchService.GetContentDetails(id, _currentUser);
			var model = GetContentViewModel();
			model.IsEdit = false;
			model.Content = content;
			if (_currentUser.IsAtLeastInCatalogRole(Roles.Plugger, content.Catalog)) {
				model.SectionsAllowed.Add("Rights");
			}

			if (_currentUser.IsAtLeastInCatalogRole(Roles.Admin, content.Catalog)) {
				model.UserCanEdit = true;
			}

			if (Request.IsAjaxRequest()) {

				return View("ctrlContentDetail", model);
			
			} else {
				return View(model);
			}
		}

		// **************************************
		// Edit/5
		// **************************************
		[RequireAuthorization(MinAccessLevel=Roles.Admin)]
		public ActionResult Edit(int id) {

			var content = SearchService.GetContentDetails(id, _currentUser);
			var model = GetContentViewModel();
			model.IsEdit = true;
			model.Content = content;
			//if (_currentUser.IsAtLeastInCatalogRole(Roles.Plugger, content.Catalog)) {
				model.SectionsAllowed.Add("Rights");
			//}
			//if (_currentUser.IsAtLeastInCatalogRole(Roles.Admin, content.Catalog)) {
				model.UserCanEdit = true;
			//}
			if (Request.IsAjaxRequest()) {

				return View("ctrlContentDetail", model);

			} else {
				return View(model);
			}
		}

		public JsonResult AutoComplete(string f, string term) {

			var values = CacheService.ContentField(f).Where(v => v != null).ToArray();
			if (term.Length < 3) {
				values = values != null ? values.Where(v => v.ToUpper().StartsWith(term.ToUpper())).ToArray() : values;
			} else {

				values = values != null ? values.Where(v => v.ToUpper().Contains(term.ToUpper())).ToArray() : values;
			}

			return Json(values, JsonRequestBehavior.AllowGet); //Json(values);
			
		}

		// **************************************
		// GetSearchViewModel
		// **************************************
		private SearchViewModel GetSearchViewModel() {

			var role = (Roles)_currentUser.RoleId;
			var model = new SearchViewModel() {
				NavigationLocation = "Search",				
				SearchMenuProperties = CacheService.SearchProperties(role),
				SearchTags = CacheService.TopTags()
			};
			return model;

		}

		// **************************************
		// GetSearchViewModel
		// **************************************
		private ContentViewModel GetContentViewModel() {

			var model = new ContentViewModel() {
				NavigationLocation = "Search",
				Tags = CacheService.Tags(),
				
				SectionsAllowed = new List<string> { "Overview", "Lyrics", "Tags" }
			};

			return model;

		}
    }
}
