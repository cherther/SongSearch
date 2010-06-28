<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ContentListViewModel>" %>
<%
	var results = Model.List;
	//var request = Model.RequestUrl;
	//var sortUrl = Model.HeaderSortUrl;
	//var props = Model.SearchMenuProperties;
	var tableHeader = Model.ListHeaders; // new string[] { "Title", "Artist", "ReleaseYear", "Preview" }; ;
	var rightCols = new string[] { "Year" };
	var tableHeaderCount = tableHeader.Count();
	var headerActions = Model.GridActions.Where(a => a != GridAction.ShowDetails);
	tableHeaderCount += headerActions.Count();
	var removeAllFromCartLink = Url.Action("DeleteMultiple", "Content");
 %>
<%--<%if (hasRemoveOption){ %>
<% Html.BeginForm("RemoveMultiple", "Cart", FormMethod.Post); %>
<%} %>--%>
<table class="cw-tbl-catalog-contents">
	<thead>
	<tr>
		<td colspan="<%: tableHeader.Count()+1 %>" style="text-align: left; white-space: nowrap; padding-left: 1px">
			<input type="submit" id="cw-delete-multiple-content" class="cw-button cw-simple cw-small cw-red" value="Delete Selected" />
		</td>
	</tr>
	<tr>
		<th>
			<input type="checkbox" class="cw-select-all-items-check" />
		</th>
	<%foreach(var col in tableHeader){ %>
	<% var classId = rightCols.Contains(col) ? "text-right" : "text-left"; %>
		<th class="<%: classId %>"><%: col%></th>
		<%} %>
		
	</tr>
	</thead>
	<tbody>
	<% foreach (var item in results) { %>
		<%
			var mediaUrl = Url.SiteRoot();
			mediaUrl = item.HasMediaFullVersion ?
				String.Concat(mediaUrl, Url.Action(MVC.Media.Stream(item.ContentId, MediaVersion.FullSong))) : "";

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
			var itemId = "deleteContentItems"; // String.Format("items[{0}]", cartIndex);
			%>
		<tr class="cw-tbl-data">
			<td>
				<input type="checkbox" id="<%: itemId %>" name="<%: itemId %>" value="<%: item.ContentId %>" class="cw-row-checkbox"/>
			</td>
			<td width="40%">
			<%if (Model.GridActions.Contains(GridAction.ShowDetails)) { %>
				<%: Html.ActionLink(title, MVC.Content.Detail(item.ContentId), new { @class = "cw-content-detail-link", rel = mediaUrl })%>
			<%} else { %>
				<%: title %>
			<%} %>
			</td>
			
			<td width="30%">
				<%: artist%>
			</td>
			<td class="text-right">
				<%: item.ReleaseYear %>
			</td>

			<%if (Model.GridActions.Contains(GridAction.Download)) { %>
			<td>
				<%: Html.ActionLink("Download", MVC.Media.Download(item.ContentId), new { @class = "cw-button cw-simple cw-small cw-blue", title = "Download" })%>
			</td>
			<%} %>
		</tr>
	
	<% } %>
	</tbody>
	</table>
