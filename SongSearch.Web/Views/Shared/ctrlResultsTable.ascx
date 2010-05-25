<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ContentListViewModel>" %>
<%
    var results = Model.List;
    var request = Model.RequestUrl;
    var sortUrl = Model.HeaderSortUrl;
    var props = Model.SearchMenuProperties;
    var tableHeader = Model.ListHeaders;
    
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
                        var colUrl = String.Format("{0}&s={1}&o={2}", sortUrl, sp, ord);
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