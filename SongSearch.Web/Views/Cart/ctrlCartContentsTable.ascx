<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ContentListViewModel>" %>
<%
    var results = Model.List;
    //var request = Model.RequestUrl;
    //var sortUrl = Model.HeaderSortUrl;
    //var props = Model.SearchMenuProperties;
    var tableHeader = Model.ListHeaders; // new string[] { "Title", "Artist", "ReleaseYear", "Preview" }; ;
	var rightCols = new string[] { "Pop", "Country", "ReleaseYear" };
 %>
<table class="cw-tbl-cart-contents">
    <tr>
    <%foreach(var col in tableHeader){ %>
	<% var classId = rightCols.Contains(col) ? "text-right" : ""; %>
        <th class="<%: classId %>"><%: col%></th>
        <%} %>
        
    </tr>
    <%var cartIndex = 0; %>
    <% foreach (var item in results) { %>
        <%
		   var title = !String.IsNullOrWhiteSpace(item.Title) ? item.Title.ToUpper() : "(N/A)";
		   var artistUrl = String.Concat(Url.Action(MVC.Search.Results()), "?f[0].P=1&f[0].T=1&f[0].V=&f[1].P=2&f[1].T=1&f[1].V=", item.Artist, "*");
		   
		    %>
        <tr class="cw-tbl-data">
            <td width="40%">
            <%if (Model.ShowDetails) { %>
                <%: Html.ActionLink(title, MVC.Content.Detail(item.ContentId), new { @class = "cw-content-detail-link" })%>
            <%} else { %>
                <%: title %>
            <%} %>
            </td>
            
            <td width="30%">
                <%: !String.IsNullOrWhiteSpace(item.Artist) ? item.Artist.ToUpper() : "(N/A)"%>
				&nbsp;<a href="<%: artistUrl%>" title="More by this Artist" style="vertical-align: middle"><img src="/public/images/icons/arrow.gif" alt="right-arrow" /></a>
            </td>
            <td class="text-right">
                <%: item.ReleaseYear %>
            </td>
            <%if (tableHeader.Contains("File Name")) { %>
            <td>
                <%= Html.Hidden("contentNames[" + cartIndex + "].ContentId", item.ContentId)%>
			    <%= Html.TextBox("contentNames[" + cartIndex + "].DownloadableName", item.UserDownloadableName, new { size = "40", maxlength = "120" })%>
			    <% cartIndex++;%>
            </td>
            <%} %>
            <%if (tableHeader.Contains("Download")) { %>
            <td>
                <%: Html.ActionLink("Download", MVC.Media.Download(item.ContentId), new { @class = "cw-button cw-simple cw-small cw-blue", title = "Download" })%>
            </td>
            <%} %>
            <%if (tableHeader.Contains("Remove")) { %>
            <td>
                <%: Html.ActionLink("Remove", MVC.Cart.Remove(item.ContentId), new { @class = "cw-cart-remove-link cw-button cw-simple cw-small cw-red", title = "Remove from Cart" })%>
            </td>
            <%} %>
        </tr>
    
    <% } %>

    </table>