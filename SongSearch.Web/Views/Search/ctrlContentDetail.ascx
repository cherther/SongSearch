<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ContentViewModel>" %>
<%@ Import Namespace = "SongSearch.Web.Data" %>
<%
    var content = Model.Content;
	var searchFields = Model.SearchFields;
	var menuButtonClass = "cw-button cw-simple cw-small cw-gray"; //'.cw-content-detail-menu
	var isEditing = Model.EditMode == EditModes.Editing;
%>
    <%--<div style="float: right">
    <button id="detail-close" type="submit" title="Close" class="cw-button cw-small cw-simple cw-blue">
    <span class="b-cancel">Close</span>
    </button>
    </div>--%>

<%if (Model.EditMode == EditModes.Viewing) {%>
	<div id="cw-media-player-panel">
	<% Html.RenderPartial("ctrlMediaplayer"); %>
	</div>
	<hr />
	<div id="cw-content-detail-menu">
	<%if (content.HasMediaFullVersion) { %>
	<%: Html.ActionLink("Download", "Download", "Media", new { id = content.ContentId }, new { @class = menuButtonClass })%>
	<%if(!content.IsInMyActiveCart){ %>
	<%: Html.ActionLink("Add To Cart", "Add", "Cart", new { id = content.ContentId }, new { @class = String.Concat(menuButtonClass, " cw-cart-add-link") })%>
	<%} else { %>
	<%: Html.ActionLink("In Cart", "Index", "Cart", null, new { @class = menuButtonClass })%>
	<%} %>
	<%} %>
	<%: Html.ActionLink("Print", "Print", "Search", new { id = content.ContentId }, new { @class = menuButtonClass, target = "_new" })%>
	<%if (Model.UserCanEdit) { %>
	<%: Html.ActionLink("Edit", "Edit", "Search", new { id = content.ContentId }, new { rel = "Edit", @class = String.Concat(menuButtonClass, " cw-content-edit-link") })%>
	<%} %>
	</div>
	<hr />
<%}%>
<div id="cw-content-detail">
	<div id="cw-content-detail-tabs">
	<ul>
		<li><a href="#tabs-1">Overview</a></li>
		<li><a href="#tabs-2">Lyrics</a></li>
		<li><a href="#tabs-3">Tags</a></li>
		<%if (Model.SectionsAllowed.Contains("Rights")) { %>
		<li><a href="#tabs-4">Rights</a></li>
		
		<%} %>
	</ul>
<%if (Model.ViewMode == ViewModes.Print) {%>
	<h3><%: content.Title %> by <%: content.Artist %></h3>
<%} %>
<%if (isEditing) { Html.BeginForm("Save", "Search", FormMethod.Post, new { id = " cw-content-form" }); }%>
<div id="tabs-1">
	<table class="cw-tbl-content-detail">
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.Title)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.TextBoxFor(m => m.Content.Title) : Html.DisplayFor(m => m.Content.Title)%></td>
		</tr>
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.Artist)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.TextBoxFor(m => m.Content.Artist) : Html.DisplayFor(m => m.Content.Artist)%></td>
		</tr>
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.Pop)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.TextBoxFor(m => m.Content.Pop) : Html.DisplayFor(m => m.Content.Pop)%></td>
		</tr>
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.Country)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.TextBoxFor(m => m.Content.Country) : Html.DisplayFor(m => m.Content.Country)%></td>
		</tr>
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.ReleaseYear)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.TextBoxFor(m => m.Content.ReleaseYear) : Html.DisplayFor(m => m.Content.ReleaseYear)%></td>
		</tr>
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.RecordLabel)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.TextBoxFor(m => m.Content.RecordLabel) : Html.DisplayFor(m => m.Content.RecordLabel)%></td>
		</tr>
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.Writers)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.TextBoxFor(m => m.Content.Writers) : Html.DisplayFor(m => m.Content.Writers)%></td>
		</tr>
		<%if (Model.SectionsAllowed.Contains("Notes")) { %>
		<%
	var notesSearch = searchFields.Where(s => s.P == 19).SingleOrDefault();
	Model.Content.Notes = isEditing ? content.Notes : Html.HighlightSearchTerm(Model.Content.Notes, notesSearch); 
			%> 
			<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.Notes)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.TextAreaFor(m => m.Content.Notes) : Html.DisplayFor(m => m.Content.Notes, "StringPreformatted")%></td>
		</tr>      
		<%} %>
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.Keywords)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.TextBoxFor(m => m.Content.Keywords) : Html.DisplayFor(m => m.Content.Keywords)%></td>
		</tr>
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.SimilarSongs)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.TextBoxFor(m => m.Content.SimilarSongs) : Html.DisplayFor(m => m.Content.SimilarSongs)%></td>
		</tr>
	</table>
</div>
<div id="tabs-2">
<%
	var lyricsSearch = searchFields.Where(s => s.P == 7).SingleOrDefault();

	Model.Content.Lyrics = isEditing ? content.Lyrics : Html.HighlightSearchTerm(Model.Content.Lyrics, lyricsSearch); 
%>
	<table class="cw-tbl-content-detail">         
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.Lyrics)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.TextAreaFor(m => m.Content.Lyrics) : Html.DisplayFor(m => m.Content.Lyrics, "StringPreformatted")%></td>
		</tr>
	</table>
</div>
<%if (Model.ViewMode != ViewModes.Print) {%>
<div id="tabs-3">
<table class="cw-tbl-content-detail">           
	<%
	var tagTypes = ModelEnums.GetTagTypes();//.OrderBy(t => t);
	%>
	<%foreach (var tagType in tagTypes) { %>
	<%    
	Func<Tag, bool> func = t => t.TagTypeId == (int)tagType;
	var selectedTags = content.Tags.Where(func).Select(t => t.TagId).ToArray();
	var tags = (isEditing ? Model.Tags : content.Tags.ToList()).Where(func).ToList();
	var model = new TagCloudViewModel<Tag>() { Tags = tags, TagType = tagType, SelectedTags = selectedTags, TagClass = "cw-tagbox-detail", TagIdTemplate = "t" };              
	%>    
		<%if (tags.Count() > 0) {%>
		<tr>
			<td class="cw-content-label"><%=tagType%></td>
			<td><% Html.RenderPartial("ctrlTagCloud", model); %></td>
		</tr>
		<%} %>
	<%} %>
</table>
</div>

<%} %>
<%if (Model.SectionsAllowed.Contains("Rights")) { %>
<div id="tabs-4">
	<table class="cw-tbl-content-detail">         
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.ContentId)%></td>
			<td class="cw-content-field"><%: content.ContentId%></td>
		</tr>
        <tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.Catalog.CatalogName)%></td>
			<td class="cw-content-field"><%: content.Catalog.CatalogName%></td>
		</tr>
        <tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.IsControlledAllIn)%></td>
			<td class="cw-content-field"><%: Html.CheckBoxFor(x => x.Content.IsControlledAllIn, isEditing ? new { disabled = "disabled" } : null)%></td>
		</tr>
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.LicensingNotes)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.TextAreaFor(m => m.Content.LicensingNotes) : Html.DisplayFor(m => m.Content.LicensingNotes, "StringPreformatted")%></td>
		</tr>
			<tr>
			<td class="cw-content-label">Share %</td>
			<td class="cw-content-field">
				<table class="cw-tbl-content-rights">
					<tr>
						<th>
							Rights Holder
						</th>
						<th>
							Rights Type
						</th>
						<th>
							Share
						</th>
					</tr>
				<% foreach (var contentRight in content.ContentRights) { %>
					<tr>
						<td class="cw-content-field">
							<%if (isEditing) {%> 
							<%: Html.TextBox("RightsHolderName", contentRight.RightsHolderName)%>
							<%} else {%> 
							<%: contentRight.RightsHolderName%>
							<%} %>
						</td>
						<td class="cw-content-field">
							<%if (isEditing) {%> 
							<%: Html.DropDownList("RightsTypeId", new SelectList(ModelEnums.GetRightsTypes()))%>
							<%} else {%> 
							<%: (RightsTypes)contentRight.RightsTypeId%>
							<%} %>
						</td>
						<td class="cw-content-field">
							<%if (isEditing) {%> 
							<%: Html.TextBox("RightsHolderShare", contentRight.RightsHolderShare.ToString("P0"))%>
							<%} else {%> 
							<%: contentRight.RightsHolderShare.ToString("P0")%>
							<%} %>
						</td>
					</tr>
					<tr>
						<td colspan="3">
						<%if (contentRight.Territories != null && contentRight.Territories.Count() > 0) {%>
						<%    
	var selectedTerritories = contentRight.Territories.Select(t => t.TerritoryId).ToArray();
	var territories = isEditing ? Model.Territories : contentRight.Territories.ToList();
	var model = new TagCloudViewModel<Territory>() { Tags = territories, SelectedTags = selectedTerritories, TagClass = "cw-tagbox-detail", TagIdTemplate = "tr" };              
						%>    
							<% Html.RenderPartial("ctrlTerritoryCloud", model); %>
						<%} %>
						</td>
					</tr>
				<%} %>
				</table>
				</td>
		</tr>    
        </table>

</div>
<%} %>
<%if (isEditing) { Html.EndForm();  }%>
<%if (Model.ViewMode == ViewModes.Embedded) { %>
<div><a href="#" id="cw-detail-close-link">Close</a></div>
<%} %>
<%if (Model.ViewMode != ViewModes.Print) {%>
</div>
<%} %>
</div>
	