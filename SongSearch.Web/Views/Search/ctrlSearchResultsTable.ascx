<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ContentListViewModel>" %>
<%
    var results = Model.List;
    var request = Model.RequestUrl;
    var sortUrl = Model.HeaderSortUrl;
    var props = Model.SearchMenuProperties;
    var tableHeader = Model.ListHeaders;
    var rightCols = new string[]{ "Pop", "Country", "ReleaseYear" };
 %>
<table width="100%" class="cw-tbl-search-results">
        <tr>
        <%foreach(var col in tableHeader){ %>
		<% var classId = rightCols.Contains(col) ? "text-right" : ""; %>
            <th class="<%: classId %>">
                <%
                    var prop = props.Where(p => p.PropertyCode.Equals(col)).SingleOrDefault();
                    if (prop != null){
                        var sp = prop.PropertyId;
                        var ord = Model.SortPropertyId.GetValueOrDefault().Equals(sp) ? (int)Model.SortType.Flip() : (int)SortType.Ascending;
                        var colUrl = String.Format("{0}&s={1}&o={2}", sortUrl, sp, ord);
                    %>
                    <a href="<%=colUrl%>"><%: prop.PropertyShortName%></a>
                    <%} else { %>
                    <%: col%>
                    <%}%>
            </th>
            <%} %>
        </tr>

    <% foreach (var item in results) { %>
		<% 
			var mediaUrl = Url.SiteRoot();
			mediaUrl = item.HasMediaFullVersion ? 
				String.Concat(mediaUrl, Url.Action("Stream", "Media", new { id = item.ContentId, version = MediaVersion.FullSong })) : "";
		   
		   var artistUrl = String.Concat(Url.Action("Results", "Search"), "?f[0].P=1&f[0].T=1&f[0].V=&f[1].P=2&f[1].T=1&f[1].V=", item.Artist, "*");
		   %>
        <tr>
            <td width="40%">
                <%: Html.ActionLink(!String.IsNullOrWhiteSpace(item.Title) ? item.Title.ToUpper() : "(N/A)", "Detail", "Content",  
                	new { id = item.ContentId }, new { @class = "cw-content-detail-link", rel = mediaUrl })%>
            </td>
            <td width="30%">
                <%: !String.IsNullOrWhiteSpace(item.Artist) ? item.Artist.ToUpper() : "(N/A)"%>
				&nbsp;<a href="<%: artistUrl%>" title="More by this Artist"><img src="/public/images/icons/arrow.gif" alt="right-arrow"/></a>
			</td>
            <td class="text-right">
                <%: item.Pop %>
            </td>
            <td class="text-right">
                <%: item.Country %>
            </td>
            <td class="text-right">
                <%: item.ReleaseYear %>
            </td>
           
        </tr>
    
    <% } %>

    </table>