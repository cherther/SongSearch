<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ContentListViewModel>" %>
<%
    var results = Model.List;
    //var request = Model.RequestUrl;
    //var sortUrl = Model.HeaderSortUrl;
    //var props = Model.SearchMenuProperties;
    var tableHeader = Model.ListHeaders; // new string[] { "Title", "Artist", "ReleaseYear", "Preview" }; ;
	var rightCols = new string[] { "Pop", "Country", "ReleaseYear" };
	var removeAllFromCartLink = Url.Action("RemoveMultiple", "Cart");
	var hasRemoveOption = tableHeader.Contains("Remove");
 %>
<%--<%if (hasRemoveOption){ %>
<% Html.BeginForm("RemoveMultiple", "Cart", FormMethod.Post); %>
<%} %>--%>
<table class="cw-tbl-cart-contents">
	<%if (hasRemoveOption){ %>
	<tr>
		<td colspan="<%: tableHeader.Count() + 1%>" style="text-align: left; white-space: nowrap; padding-left: 1px">
			<input type="submit" id="cw-remove-all-from-cart" class="cw-button cw-simple cw-small cw-red" value="Remove Selected" />
		</td>
	</tr>
    <%} %>
	<tr>
	<%if (hasRemoveOption) { %>
	<th>
		<input type="checkbox" id="cw-select-all-cart-items-check" />
	</th>
	<%} %>
	<%foreach(var col in tableHeader.Where(t => !t.Equals("Remove"))){ %>
	<% var classId = rightCols.Contains(col) ? "text-right" : ""; %>
        <th class="<%: classId %>"><%: col%></th>
        <%} %>
        
    </tr>
    <%var cartIndex = 0; %>
    <% foreach (var item in results) { %>
        <%
			var titleLength = 35;
			var title = (!String.IsNullOrWhiteSpace(item.Title) ?
					item.Title.Length > titleLength ? String.Concat(item.Title.Substring(0, titleLength), "...")
					: item.Title
					: "(N/A)").ToUpper();

			var artistLength = 25;
			var artist = (!String.IsNullOrWhiteSpace(item.Artist) ?
					item.Artist.Length > artistLength ? String.Concat(item.Artist.Substring(0, artistLength), "...")
					: item.Artist
					: "(N/A)").ToUpper();
			var artistUrl = String.Concat(Url.Action(MVC.Search.Results()), "?f[0].P=1&f[0].T=1&f[0].V=&f[1].P=2&f[1].T=1&f[1].V=", item.Artist, "*");
			var itemId = "removeFromCartItems"; // String.Format("items[{0}]", cartIndex);
		    %>
        <tr class="cw-tbl-data">
			<%if (hasRemoveOption) { %>
			<td>
				<input type="checkbox" id="<%: itemId %>" name="<%: itemId %>" value="<%: item.ContentId %>" class="remove-from-cart-checkbox"/>
			</td>
			<%} %>
            <td width="40%">
            <%if (Model.ShowDetails) { %>
                <%: Html.ActionLink(title, MVC.Content.Detail(item.ContentId), new { @class = "cw-content-detail-link" })%>
            <%} else { %>
                <%: title %>
            <%} %>
            </td>
            
            <td width="30%">
                <%: artist%>
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
            <%--<%if (hasRemoveOption) { %>
            <td>
                <%//: Html.ActionLink("Remove", MVC.Cart.Remove(item.ContentId), new { @class = "cw-cart-remove-link cw-button cw-simple cw-small cw-red", title = "Remove from Cart" })%>
            </td>
            <%} %>--%>
        </tr>
    
    <% } %>

    </table>
<%--<%if (hasRemoveOption){ %>
<% Html.EndForm(); %>
<%} %>--%>
