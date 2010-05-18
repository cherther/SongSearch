<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.SearchViewModel>" %>
<%  var props = Model.SearchMenuProperties;
    var results = Model.SearchResults;
    var request = Request.RawUrl.Replace(String.Format("&p={0}", results.PageIndex), "");
    var sortUrl = Model.SortPropertyId.HasValue ?
        String.Format("&s={0}&o={1}", Model.SortPropertyId.Value, (int)Model.SortType) : "";
     %>
    <h2>Results</h2>
    <p><%= results.TotalCount %> songs found.</p>
    <div>
    <%if (results.TotalPages > 1) { %>
    Page <%= results.CurrentPage%> of <%= results.TotalPages%> total.&nbsp;
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
<%
        
        var tableHeader = new string[] { "Title", "Artist", "Pop", "Country", "ReleaseYear", "Preview" };
        var headerUrl = request.Replace(String.Format("&s={0}&o={1}", Model.SortPropertyId, (int)Model.SortType), "");
        
    %>
    <table width="100%" class="cw-tbl-search-results">
        <tr>
        <%foreach(var col in tableHeader){ %>
            <th>
                <%
                    var prop = props.Where(p => p.PropertyCode.Equals(col)).SingleOrDefault();
                    if (prop != null){
                        var sp = prop.PropertyId;
                        var ord = Model.SortPropertyId.GetValueOrDefault().Equals(sp) ? (int)Model.SortType.Flip() : (int)SortType.Ascending;
                        var colUrl = String.Format("{0}&s={1}&o={2}", headerUrl, sp, ord);
                    %>
                    <a href="<%=colUrl%>"><%: prop.PropertyName%></a>
                    <%} else { %>
                    <%: col%>
                    <%}%>
            </th>
            <%} %>
        </tr>

    <% foreach (var item in results) { %>
    
        <tr>
            <td width="300">
                <%: Html.ActionLink(!String.IsNullOrWhiteSpace(item.Title) ? item.Title.ToUpper() : "(N/A)", "Detail", new { id = item.ContentId }, new { @class = "cw-content-detail-link" })%>
            </td>
            <td width="200">
                <%: !String.IsNullOrWhiteSpace(item.Artist) ? item.Artist.ToUpper() : "(N/A)"%>
            </td>
            <td>
                <%: item.Pop %>
            </td>
            <td>
                <%: item.Country %>
            </td>
            <td>
                <%: item.ReleaseYear %>
            </td>
           
            <td>
                <%: item.HasMediaPreviewVersion %>
            </td>
        </tr>
    
    <% } %>

    </table>
  