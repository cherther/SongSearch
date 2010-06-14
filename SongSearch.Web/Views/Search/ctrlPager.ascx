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
    <a href="<%=request%>&p=<%=pageStart-1%>">&lt;&lt;</a>
    <%} %>
    <%else { %>
        &lt;&lt;
    <%} %>
       &nbsp;
       <%for (var i = pageStart; i < pageEnd; i++) { %>
        <%if (i == results.PageIndex) {%>
            <strong><%=i + 1%></strong>
        <%} %>
        <%else { %>
            <a href="<%=request%>&p=<%=i%><%= sortUrl%>"><%=i + 1%></a>
        <%} %>
        &nbsp;
    <%} %>
    
    <%if (pageEnd < results.TotalPages) { %>
    <a href="<%=request%>&p=<%=pageEnd%>">&gt;&gt;</a>
    <%} %>
    <%else { %>
    &gt;&gt;
    <%} %>
    &nbsp;
    <%if (results.HasPreviousPage) {%>
        [<a href="<%=request%>&p=<%=results.LastPage%><%= sortUrl%>">Prev</a>]
    <%} %>    
    <%else { %>
        [Prev]
    <%} %>
    <%if (results.HasNextPage) {%>
        [<a href="<%=request%>&p=<%=results.NextPage%><%= sortUrl%>">Next</a>]
    <%} %>
    <%else { %>
        [Next]
    <%} %>
    &nbsp;
    <%} %>
    </div>

