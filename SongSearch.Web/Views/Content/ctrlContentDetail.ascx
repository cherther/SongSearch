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
<div id="cw-content-detail">
<%if (isEditing) { %>
	<%Html.BeginForm(MVC.Content.Save(), FormMethod.Post, new { id = "cw-content-editor" }); %>

	<%: Html.HiddenFor(m => m.Content.ContentId)%>
	<%: Html.HiddenFor(m => m.Content.CatalogId)%>
	<%: Html.HiddenFor(m => m.Content.CreatedByUserId)%>
	<%: Html.HiddenFor(m => m.Content.CreatedOn)%>
	<%: Html.HiddenFor(m => m.Content.LastUpdatedByUserId)%>
	<%: Html.HiddenFor(m => m.Content.LastUpdatedOn)%>
	<%: Html.HiddenFor(m => m.Content.LastUpdatedByUserId)%>
	<%: Html.HiddenFor(m => m.Content.HasMediaPreviewVersion)%>
	<%: Html.HiddenFor(m => m.Content.HasMediaFullVersion)%>
	<%: Html.AntiForgeryToken() %>

<%}%>
<%if (Model.ViewMode == ViewModes.Normal | (Model.ViewMode == ViewModes.Embedded && Model.EditMode == EditModes.Viewing)) {%>
	<div id="cw-media-player-panel">
	<% Html.RenderPartial(MVC.Shared.Views.ctrlMediaplayer); %>
	</div>
	<hr />
	<div id="cw-content-detail-menu">
	<%if (content.HasMediaFullVersion) { %>
	<%: Html.ActionLink("Download", MVC.Media.Download(content.ContentId), new { @class = menuButtonClass })%>
	<%if(!content.IsInMyActiveCart){ %>
	<%: Html.ActionLink("Add To Cart", MVC.Cart.Add(content.ContentId), new { @class = String.Concat(menuButtonClass, " cw-cart-add-link") })%>
	<%} else { %>
	<%: Html.ActionLink("In Cart", MVC.Cart.Index(), new { @class = menuButtonClass })%>
	<%} %>
	<%} %>
	<%: Html.ActionLink("Print", MVC.Content.Print(content.ContentId), new { @class = menuButtonClass, target = "_new" })%>
	<%if (Model.UserCanEdit) { %>
	<%	
		var linkClass = String.Concat(menuButtonClass, " cw-content-edit-link");
		var linkAction = Model.EditMode == EditModes.Viewing ? "Edit" : "Save";
	%>
		<% if (Model.ViewMode == ViewModes.Embedded || Model.EditMode == EditModes.Viewing) {%>
		<%: Html.ActionLink(linkAction, linkAction, "Content", new { id = content.ContentId }, new { rel = linkAction, @class = linkClass })%>
		<%} else { %>
		<button type="submit" class="<%: menuButtonClass %>"><%: linkAction %></button>
		<%}%> 
	<%} %>
	</div>
	<hr />
<%}%>

	<div id="cw-content-detail-data">

	<div id="cw-content-detail-tabs">
<%if (Model.ViewMode == ViewModes.Print) {%>
	<h3><%: content.Title%> by <%: content.Artist%></h3>
	<div>&nbsp;</div>
<%} else {%>
	<ul>
		<li><a href="#tabs-1">Overview</a></li>
		<li><a href="#tabs-2">Lyrics</a></li>
		<li><a href="#tabs-3">Tags</a></li>
		<%if (Model.SectionsAllowed.Contains("Rights")) { %>
		<li><a href="#tabs-4">Rights</a></li>
		
		<%} %>
	</ul>
<%} %>


<div id="tabs-1">
	<table class="cw-tbl-content-detail">
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.Title)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.EditorFor(m => m.Content.Title) : Html.DisplayFor(m => m.Content.Title)%></td>
		</tr>
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.Artist)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.EditorFor(m => m.Content.Artist, "StringAutoComplete") : Html.DisplayFor(m => m.Content.Artist)%></td>
		</tr>
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.Pop)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.EditorFor(m => m.Content.Pop) : Html.DisplayFor(m => m.Content.Pop)%></td>
		</tr>
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.Country)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.EditorFor(m => m.Content.Country) : Html.DisplayFor(m => m.Content.Country)%></td>
		</tr>
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.ReleaseYear)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.EditorFor(m => m.Content.ReleaseYear) : Html.DisplayFor(m => m.Content.ReleaseYear)%></td>
		</tr>
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.RecordLabel)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.EditorFor(m => m.Content.RecordLabel, "StringAutoComplete") : Html.DisplayFor(m => m.Content.RecordLabel)%></td>
		</tr>
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.Writers)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.EditorFor(m => m.Content.Writers, "StringAutoComplete") : Html.DisplayFor(m => m.Content.Writers)%></td>
		</tr>
		<%if (Model.SectionsAllowed.Contains("Notes")) { %>
		<%
	var notesSearch = searchFields.Where(s => s.P == 19).SingleOrDefault();
	Model.Content.Notes = isEditing ? content.Notes : Html.HighlightSearchTerm(Model.Content.Notes, notesSearch); 
			%> 
			<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.Notes)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.EditorFor(m => m.Content.Notes) : Html.DisplayFor(m => m.Content.Notes)%></td>
		</tr>      
		<%} %>
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.Keywords)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.EditorFor(m => m.Content.Keywords) : Html.DisplayFor(m => m.Content.Keywords)%></td>
		</tr>
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.SimilarSongs)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.EditorFor(m => m.Content.SimilarSongs) : Html.DisplayFor(m => m.Content.SimilarSongs)%></td>
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
			<td class="cw-content-field"><%: isEditing ? Html.EditorFor(m => m.Content.Lyrics) : Html.DisplayFor(m => m.Content.Lyrics,"MultilineText")%></td>
		</tr>
	</table>
</div>
<%if (Model.ViewMode != ViewModes.Print) {%>
<div id="tabs-3">
<table class="cw-tbl-content-detail">           
	<%
	var tagTypes = ModelEnums.GetTagTypes();//.OrderBy(t => t);
	var tt = 0;
	%>
	<%foreach (var tagType in tagTypes) { %>
	<%    
		//var tagTypeId = String.Format("tags[{0}].", tt++);
		Func<Tag, bool> func = x => x.TagTypeId == (int)tagType;
		var selectedTags = content.Tags.Where(func).Select(x => x.TagId).ToArray();
		var tags = (isEditing ? Model.Tags : content.Tags.ToList()).Where(func).ToList();
		var model = new TagCloudViewModel<Tag>() { 
			EditMode = Model.EditMode, 
			TagCountSeed = tt,
			Tags = tags, 
			TagType = tagType, 
			SelectedTags = selectedTags,
			TagClass = "cw-tagbox-label",
			TagIdTemplate = isEditing ? "tags[{0}]" : "t",
		};

		tt += tags.Count();
	%>    
		<%if (tags.Count() > 0) {%>
		<tr>
			<td class="cw-content-label"><%=tagType%></td>
			<td>
			<% Html.RenderPartial(MVC.Shared.Views.ctrlTagCloud, model); %>
			</td>
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
			<td class="cw-content-field"><%: Html.CheckBoxFor(x => x.Content.IsControlledAllIn, !isEditing ? new { disabled = "disabled" } : null)%>
			<%: !isEditing ? content.IsControlledAllIn.ToYesNo() : "" %>
			</td>
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
						<th>
						</th>
					</tr>
				<%var r = 0; %>
				<%foreach (var contentRight in content.ContentRights) { %>
				<%	
					var rightId = String.Format("rights[{0}].", r++);
					var rightsHolderShare = contentRight.RightsHolderShare.ToString("P2");
				%>
					<tr>
						<td class="cw-content-field">
							<%if (isEditing) {%> 
							<%: Html.Hidden(String.Concat(rightId, "ContentId"), contentRight.ContentId)%>
							<%: Html.Hidden(String.Concat(rightId, "ContentRightId"), contentRight.ContentRightId)%>
							<%: Html.TextBox(String.Concat(rightId, "RightsHolderName"), contentRight.RightsHolderName, new { @class = "cw-autocomplete", alt = "RightsHolderName" })%>
							<%} else {%> 
							<%: contentRight.RightsHolderName%>
							<%} %>
						</td>
						<td class="cw-content-field">
							<%if (isEditing) {%> 
							<%: Html.DropDownList(String.Concat(rightId, "RightsTypeId"), new SelectList(ModelEnums.GetRightsTypes(), (RightsTypes)contentRight.RightsTypeId))%>
							<%} else {%> 
							<%: (RightsTypes)contentRight.RightsTypeId%>
							<%} %>
						</td>
						<td class="cw-content-field text-right">
							<%if (isEditing) {%> 
							<%: Html.TextBox(String.Concat(rightId, "RightsHolderShare"), rightsHolderShare, new { width = 20 })%>
							<%} else {%> 
							<%: rightsHolderShare%>
							<%} %>
						</td>
						<td>
						<%if (isEditing) {%>
						<%: Html.Hidden(String.Concat(rightId, "ModelAction"), (int)ModelAction.Update, new { @class = "cw-model-action" })%>
					  	<a href="#" class="cw-delete-right-link"><img src="../../public/images/icons/silk/delete.png" alt="Delete" /></a>
						<%} %>
						</td>
					</tr>
					<tr>
						<td colspan="3">
						<%//if (contentRight.Territories != null && contentRight.Territories.Count() > 0) {%>
						<%    
							var selectedTerritories = contentRight.Territories.Select(x => x.TerritoryId).ToArray();
							var territories = isEditing ? Model.Territories : contentRight.Territories.ToList();
							var model = new TagCloudViewModel<Territory>() { 
								EditMode = Model.EditMode, 
								Tags = territories, 
								SelectedTags = selectedTerritories,
								TagClass = "cw-tagbox-label",
								TagIdTemplate = isEditing ? String.Concat(rightId, "territories[{0}]") : "tr"
							};              
						%>    
							<% Html.RenderPartial(MVC.Shared.Views.ctrlTerritoryCloud, model); %>
						<%//} %>
						</td>
						<td>
						</td>
					</tr>
				<%} %>
				<%if (isEditing) {%>
				<%	var rightId = String.Format("rights[{0}].", r++); %>
					<tr>
						<td class="cw-content-field">
							<%: Html.Hidden(String.Concat(rightId, "ContentId"), Model.Content.ContentId)%>
							<%: Html.Hidden(String.Concat(rightId, "ContentRightId"), 0)%>
							<%: Html.TextBox(String.Concat(rightId, "RightsHolderName"),null, new { @class = "cw-autocomplete", alt = "RightsHolderName" })%>
						</td>
						<td class="cw-content-field">
							<%: Html.DropDownList(String.Concat(rightId, "RightsTypeId"), new SelectList(ModelEnums.GetRightsTypes()))%>
						</td>
						<td class="cw-content-field text-right">
							<%: Html.TextBox(String.Concat(rightId, "RightsHolderShare"), null, new { width = 20 })%>
						</td>
						<td>
						<%: Html.Hidden(String.Concat(rightId, "ModelAction"), (int)ModelAction.Add, new { @class = "cw-model-action" })%>
						<a href="#" class="cw-add-right-link"><img src="../../public/images/icons/silk/add.png" alt="Add" /></a>
						</td>
					</tr>
					<tr>
						<td colspan="3">
						<%    
						var territories = Model.Territories;
						var model = new TagCloudViewModel<Territory>() {
							EditMode = Model.EditMode,
							Tags = territories,
							TagClass = "cw-tagbox-label",
							TagIdTemplate = isEditing ? String.Concat(rightId, "territories[{0}]") : "tr"
						};              
						%>    
						<% Html.RenderPartial(MVC.Shared.Views.ctrlTerritoryCloud, model); %>
						</td>
						<td>
						</td>
					</tr>
				<%} %>

				</table>
				</td>
		</tr>
		<tr>
			<td class="cw-content-label"><%: Html.LabelFor(m => m.Content.LicensingNotes)%></td>
			<td class="cw-content-field"><%: isEditing ? Html.EditorFor(m => m.Content.LicensingNotes) : Html.DisplayFor(m => m.Content.LicensingNotes)%></td>
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
</div>
	