<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ContentListViewModel>" %>
<%
    var results = Model.List;
    //var request = Model.RequestUrl;
    //var sortUrl = Model.HeaderSortUrl;
    //var props = Model.SearchMenuProperties;
    var tableHeader = Model.ListHeaders; // new string[] { "Title", "Artist", "ReleaseYear", "Preview" }; ;
    
 %>
<table class="cw-tbl-cart-contents">
    <tr>
    <%foreach(var col in tableHeader){ %>
        <th><%: col%></th>
        <%} %>
        <th>&nbsp;</th>
    </tr>
    <%var cartIndex = 0; %>
    <% foreach (var item in results) { %>
    
        <tr>
            <td width="300">
                <%: Html.ActionLink(!String.IsNullOrWhiteSpace(item.Title) ? item.Title.ToUpper() : "(N/A)", "Detail", "Search", new { id = item.ContentId }, new { @class = "cw-content-detail-link" })%>
            </td>
            
            <td width="200">
                <%: !String.IsNullOrWhiteSpace(item.Artist) ? item.Artist.ToUpper() : "(N/A)"%>
            </td>
            <td>
                <%: item.ReleaseYear %>
            </td>
            <td>
            <%= Html.Hidden("contentNames[" + cartIndex + "].ContentId", item.ContentId)%>
			<%= Html.TextBox("contentNames[" + cartIndex + "].DownloadableName", item.UserDownloadableName, new { size = "50", maxlength = "120" })%>
			<% cartIndex++;%>
            
            </td>
            <td>
            <%= Html.ActionLink("Remove", "Remove", "Cart", new { id = item.ContentId }, new { id = String.Format("q-{0}", item.ContentId), @class = "cw-remove-from-cart cw-button cw-simple cw-small cw-blue", title = "Remove from Cart" })%>							
            </td>
        </tr>
    
    <% } %>

    </table>