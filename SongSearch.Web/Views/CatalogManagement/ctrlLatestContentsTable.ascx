﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ContentListViewModel>" %>
<%
	var results = Model.List;
	//var request = Model.RequestUrl;
	//var sortUrl = Model.HeaderSortUrl;
	//var props = Model.SearchMenuProperties;
	var tableHeader = Model.ListHeaders; // new string[] { "Title", "Artist", "ReleaseYear", "Preview" }; ;
	var rightCols = new string[] { "Year" };
	var tableHeaderCount = tableHeader.Count();
	var headerActions = Model.HeaderActions;
	var gridActions = Model.GridActions.Where(a => a != GridAction.ShowDetails);

	var colCount = headerActions.Count() + tableHeader.Count() + gridActions.Count();

	//tableHeaderCount += Model.HeaderActions.Count();
	
 %>
<%--<%if (hasRemoveOption){ %>
<% Html.BeginForm("RemoveMultiple", "Cart", FormMethod.Post); %>
<%} %>--%>
<% if (results.Count == 0) { %>
<div class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners">
	There were no additions to your catalogs in the last <%: SystemConfig.DaysLatestContentAge %> days.
	You can easily add new songs with our <%: Html.ActionLink("Catalog Upload Wizard", MVC.CatalogUpload.Upload()) %>.
</div>
<%} else { %>
<div class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners">
	<h3>New! (Last <%: SystemConfig.DaysLatestContentAge%> days)</h3>	
	<div>&nbsp;</div>
	<table class="cw-tbl-catalog-contents">
		<thead>
		<%if (headerActions.Count() > 0) { %>
		<tr>
			<td colspan="<%: colCount %>" style="text-align: left; white-space: nowrap; padding-left: 1px">
			<%foreach (var action in headerActions) { %>
				<%if (action == GridAction.Delete) { %>
				<% var deleteMultipleLink = Url.Action("DeleteMultiple", "Content"); %>
				<a href="<%: deleteMultipleLink %>" id="cw-delete-multiple-content" class="cw-button cw-simple cw-small cw-red">
					<span class="b-delete">Delete Selected</span>
				</a>
				<%} %>
			<%} %>
			</td>
		</tr>
		<%} %>
		<tr>
			<%foreach(var action in headerActions){ %>
			<% var actionClass = action.ToString().ToLower(); %>
			<th>
				<input type="checkbox" class="cw-select-all-items-check" id="cw-row-checkbox-<%: actionClass %>" />
			</th>
			<%} %>
			<%foreach(var col in tableHeader){ %>
			<% var classId = rightCols.Contains(col) ? "text-right" : "text-left"; %>
				<th class="<%: classId %>"><%: col%></th>
			<%} %>
			<%foreach (var action in gridActions) { %>
			<th class="text-center"><%: action %></th>
			<%} %>
		</tr>
		</thead>
		<tbody>
		<% foreach (var item in results) { %>
			<%
				var mediaUrl = Url.MediaUrl(item);
		
				var titleLength = 25;
				var title = (!String.IsNullOrWhiteSpace(item.Title) ?
						item.Title.Length > titleLength ? String.Concat(item.Title.Substring(0, titleLength), "...")
						: item.Title
						: "(N/A)").ToUpper();
		   
				var artistLength = 20;
				var artist = (!String.IsNullOrWhiteSpace(item.Artist) ?
						item.Artist.Length > artistLength ? String.Concat(item.Artist.Substring(0, artistLength), "...")
						: item.Artist
						: "(N/A)").ToUpper();
				var catalogLength = 15;
				var catalog = (!String.IsNullOrWhiteSpace(item.Catalog.CatalogName) ?
							item.Catalog.CatalogName.Length > catalogLength ? String.Concat(item.Catalog.CatalogName.Substring(0, catalogLength), "...")
							: item.Catalog.CatalogName
							: "(N/A)").ToUpper();
		
				var artistUrl = String.Concat(Url.Action(MVC.Search.Results()), "?f[0].P=1&f[0].T=1&f[0].V=&f[1].P=2&f[1].T=1&f[1].V=", item.Artist, "*");
				var itemId = "deleteContentItems"; // String.Format("items[{0}]", cartIndex);
				%>
			<tr class="cw-tbl-data">
				<%foreach(var action in headerActions){ %>
				<% var actionClass = action.ToString().ToLower(); %>
				<td>
					<input type="checkbox" id="<%: itemId %>" name="<%: itemId %>" value="<%: item.ContentId %>" class="cw-row-checkbox-<%: actionClass %>"/>
				</td>
				<%} %>
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
				<td>
					<%: catalog%>
					<a href="<%: String.Format("/CatalogManagement/Detail?id={0}", item.CatalogId)%>" title="Show details for <%: catalog%>">
					<img src="/public/images/icons/arrow.gif" alt="right-arrow"/></a>
				</td>
				<td>
					<%: item.CreatedOn.ToShortDateString()%>
				</td>
				<td>
					<%: item.Creator.FullName() %>
				</td>
				<%foreach(var action in gridActions){ %>
				<td class="text-center">
				<%switch (action) {%>
				<%case GridAction.Download :%>
					<%: Html.ActionLink("Download", MVC.Media.Download(item.ContentId), new { @class = "cw-button cw-simple cw-small cw-blue", title = "Download" })%>
				<%break; %>
				<%case GridAction.Media:%>
					<%: Html.ActionLink("Add/Replace Media", MVC.Content.SaveMediaFiles(item.ContentId, null), new { rel = item.ContentId, rev = String.Format("{0} - '{1}'", item.Artist, item.Title), @class = "cw-media-upload-link cw-button cw-simple cw-small cw-blue", title = String.Format("Select & upload new media files for {0} - '{1}'", item.Artist, item.Title) })%>
				<%break; %>
				<%} %>
				</td>
				<%} %>
			</tr>
	
		<% } %>
		</tbody>
		</table>
	</div>
	<%} %>