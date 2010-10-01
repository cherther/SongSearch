<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.SearchViewModel>" %>
<%  var results = Model.SearchResults;
	
	var contentListViewModel = new ContentListViewModel()
	{
		List = results,
		ListHeaders = Model.SearchResultsHeaders,
		HeaderSortUrl = Model.HeaderSortUrl,
		PagerSortUrl = Model.PagerSortUrl,
		SearchMenuProperties = Model.SearchMenuProperties,
		RequestUrl = Model.RequestUrl,
		SortPropertyId = Model.SortPropertyId,
		SortType = Model.SortType,
		ViewMode = Model.ViewMode
	};
	
	 %>
	<%--<h2>Results</h2>--%>
	<% Html.RenderPartial(MVC.Search.Views.ctrlBreadcrumb, Model); %>
	<p><%: results.TotalCount.ToString("N0") %> songs found.
	<%if (results.TotalPages > 1) { %>
	Showing  <%: results.Count.ToString("N0") %> results per page. 
	<%} %>
	</p>
	<% if (results.TotalCount > 0){%>
		<%// if (Model.ViewMode != ViewModes.Print) { %>
			<% Html.RenderPartial(MVC.Search.Views.ctrlPager, Model); %>
		<%//} %>

	<% Html.RenderPartial(MVC.Search.Views.ctrlSearchResultsTable, contentListViewModel); %>
	
	<% Html.RenderPartial(MVC.Search.Views.ctrlPager, Model); %>
	<% } %>
	
  