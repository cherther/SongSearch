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
        SortType = Model.SortType
    };
    
     %>
     <div class="cw-outl cw-padded">
    <h2>Results</h2>
    <% Html.RenderPartial(MVC.Search.Views.ctrlBreadcrumb, Model); %>
    <p><%= results.TotalCount %> songs found.</p>
	<% if (results.TotalCount > 0) { %>
	<% Html.RenderPartial(MVC.Search.Views.ctrlPager, Model); %>
    <% Html.RenderPartial(MVC.Search.Views.ctrlSearchResultsTable, contentListViewModel); %>
    <% } %>
	</div>
    
  