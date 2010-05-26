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
    <h2>Results</h2>
    <p><%= results.TotalCount %> songs found.</p>
    <% Html.RenderPartial("ctrlPager", Model); %>
    <% Html.RenderPartial("ctrlSearchResultsTable", contentListViewModel); %>
    
  