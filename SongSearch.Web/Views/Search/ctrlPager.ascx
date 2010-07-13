<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.SearchViewModel>" %>
<%
	var results = Model.SearchResults;
	var request = Model.RequestUrl;
	var sortUrl = Model.PagerSortUrl;
 %>
<div class="cw-pager">
	<%if (results.TotalPages > 1) { %>
	You're on page <%= results.CurrentPage%> of <%= results.TotalPages%> total.&nbsp;
	Go to:&nbsp;
	<%
		const int pageSetSize = 10;
		var pageStart = results.PageIndex - (results.PageIndex % pageSetSize);
		var pageEnd = pageStart + pageSetSize > results.TotalPages ? results.TotalPages : pageStart + pageSetSize;

	%>
	<%if (pageStart > 0) { %>
	<a href="<%=request%>&p=<%=pageStart-1%>" class="cw-page-number-less" title="See the previous <%: pageSetSize %> pages">&lt;&lt;</a>
	<%} %>
	<%else { %>
		&lt;&lt;
	<%} %>
	   <%for (var i = pageStart; i < pageEnd; i++) { %>
		<%if (i == results.PageIndex) {%>
			<strong class="cw-page-number"><%=i + 1%></strong>
		<%} %>
		<%else { %>
		<a href="<%=request%>&p=<%=i%><%= sortUrl%>" class="cw-page-number" title="Go to page <%: i + 1%>"><%: i + 1%></a>
		<%} %>
		
	<%} %>
	
	<%if (pageEnd < results.TotalPages) { %>
	<a href="<%=request%>&p=<%=pageEnd%>" class="cw-page-number-more" title="See the next <%: pageSetSize %> pages">&gt;&gt;</a>
	<%} %>
	<%else { %>
	&gt;&gt;
	<%} %>
	<%if (results.HasPreviousPage) {%>
		[<a href="<%=request%>&p=<%=results.LastPage%><%= sortUrl%>" title="Go to the previous page">Prev</a>]
	<%} %>    
	<%else { %>
		[Prev]
	<%} %>
	<%if (results.HasNextPage) {%>
		[<a href="<%=request%>&p=<%=results.NextPage%><%= sortUrl%>" title="Go to the next page">Next</a>]
	<%} %>
	<%else { %>
		[Next]
	<%} %>
	<%} %>
	</div>

