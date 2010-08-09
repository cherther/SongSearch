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
		<%--<%: Html.Hidden("DefaultRightsHolderName", Page.User.User().GetContactInfo().DefaultRightsHolderName)%>
		<%: Html.Hidden("DefaultTerritoryId", Page.User.User().GetContactInfo().DefaultTerritoryId)%>--%>
		<%: Html.AntiForgeryToken() %>

	<%}%>
	<%if (Model.ViewMode == ViewModes.Normal | (Model.ViewMode == ViewModes.Embedded && Model.EditMode == EditModes.Viewing)) {%>
		<div id="cw-media-player-panel">
			<% Html.RenderPartial(MVC.Shared.Views.ctrlMediaplayer); %>
		</div>
		<hr />
		<div id="cw-content-detail-menu">
		<%if (content.HasMediaFullVersion) { %>
		<%: Html.ActionLink("Download", MVC.Media.Download(content.ContentId), new { @class = menuButtonClass, title = "Download the song now (unzipped)"})%>
		<%if(!content.IsInMyActiveCart){ %>
		<%: Html.ActionLink("Add To Cart", MVC.Cart.Add(content.ContentId), new { @class = String.Concat(menuButtonClass, " cw-cart-add-link"), title = "Add to your song cart" })%>
		<%} else { %>
		<%: Html.ActionLink("In Cart", MVC.Cart.Index(), new { @class = menuButtonClass })%>
		<%} %>
		<%} %>
		<%: Html.ActionLink("Print", MVC.Content.Print(content.ContentId), new { @class = menuButtonClass, target = "_new", title = "Print the song details" })%>
		<%if (Model.UserCanEdit) { %>
		<%	
			var linkClass = String.Concat(menuButtonClass, " cw-content-edit-link");
			var linkAction = Model.EditMode == EditModes.Viewing ? "Edit" : "Save";
		%>
			<% if (Model.ViewMode == ViewModes.Embedded || Model.EditMode == EditModes.Viewing) {%>
			<%: Html.ActionLink(linkAction, linkAction, "Content", new { id = content.ContentId }, new { rel = linkAction, @class = linkClass, title = linkAction + " this song" })%>
			<%} else { %>
			<button type="submit" class="<%: menuButtonClass %>"><%: linkAction %></button>
			<%}%>
			<%--<%if (isEditing){ %>
			<%: Html.ActionLink("Cancel", linkAction, "Content", new { id = content.ContentId }, new { rel = "Cancel", @class = linkClass })%>
			<%} %>--%>
		<%} %>
		</div>
		<hr />
	<%}%>

	<div id="cw-content-detail-data">

		<div id="cw-content-detail-tabs">
			<%if (Model.ViewMode == ViewModes.Print) {%>
				<h2>'<%: content.Title%>' - <%: content.Artist%></h2>
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

	<%
		var sectionSize = "nine";
		var columnOne = "two";
		var columnTwo = "seven";
	%>
			<div id="tabs-1">
				<div class="<%: sectionSize%>_column section cw-spaced">
					<div class="<%: columnOne%> column"><%: Html.LabelFor(m => m.Content.Title)%></div>
					<div class="<%: columnTwo%> column"><%: isEditing ? Html.EditorFor(m => m.Content.Title) : Html.DisplayFor(m => m.Content.Title)%></div>
				</div>
				<div class="<%: sectionSize%>_column section cw-spaced">
					<div class="<%: columnOne%> column"><%: Html.LabelFor(m => m.Content.Artist)%></div>
					<div class="<%: columnTwo%> column"><%: isEditing ? Html.EditorFor(m => m.Content.Artist, "StringAutoComplete") : Html.DisplayFor(m => m.Content.Artist)%></div>
				</div>
				<div class="<%: sectionSize%>_column section cw-spaced">
					<div class="<%: columnOne%> column"><%: Html.LabelFor(m => m.Content.Pop)%></div>
					<div class="<%: columnTwo%> column"><%: isEditing ? Html.EditorFor(m => m.Content.Pop) : Html.DisplayFor(m => m.Content.Pop)%></div>
				</div>
				<div class="<%: sectionSize%>_column section cw-spaced">
					<div class="<%: columnOne%> column"><%: Html.LabelFor(m => m.Content.Country)%></div>
					<div class="<%: columnTwo%> column"><%: isEditing ? Html.EditorFor(m => m.Content.Country) : Html.DisplayFor(m => m.Content.Country)%></div>
				</div>
				<div class="<%: sectionSize%>_column section cw-spaced">
					<div class="<%: columnOne%> column"><%: Html.LabelFor(m => m.Content.ReleaseYear)%></div>
					<div class="<%: columnTwo%> column"><%: isEditing ? Html.EditorFor(m => m.Content.ReleaseYear) : Html.DisplayFor(m => m.Content.ReleaseYear)%></div>
				</div>
				<div class="<%: sectionSize%>_column section cw-spaced">
					<div class="<%: columnOne%> column"><%: Html.LabelFor(m => m.Content.RecordLabel)%></div>
					<div class="<%: columnTwo%> column"><%: isEditing ? Html.EditorFor(m => m.Content.RecordLabel, "StringAutoComplete") : Html.DisplayFor(m => m.Content.RecordLabel)%></div>
				</div>
				<div class="<%: sectionSize%>_column section cw-spaced">
					<div class="<%: columnOne%> column"><%: Html.LabelFor(m => m.Content.Writers)%></div>
					<div class="<%: columnTwo%> column"><%: isEditing ? Html.EditorFor(m => m.Content.Writers, "StringAutoComplete") : Html.DisplayFor(m => m.Content.Writers)%></div>
				</div>
				<div class="<%: sectionSize%>_column section cw-spaced">
					<div class="<%: columnOne%> column"><%: Html.LabelFor(m => m.Content.Keywords)%></div>
					<div class="<%: columnTwo%> column"><%: isEditing ? Html.EditorFor(m => m.Content.Keywords) : Html.DisplayFor(m => m.Content.Keywords)%></div>
				</div>
				<div class="<%: sectionSize%>_column section cw-spaced">
					<div class="<%: columnOne%> column"><%: Html.LabelFor(m => m.Content.SimilarSongs)%></div>
					<div class="<%: columnTwo%> column"><%: isEditing ? Html.EditorFor(m => m.Content.SimilarSongs) : Html.DisplayFor(m => m.Content.SimilarSongs)%></div>
				</div>
				<%if (Model.SectionsAllowed.Contains("Notes")) { %>
				<%
					var notesSearch = searchFields.Where(s => s.P == 19).SingleOrDefault();
					Model.Content.Notes = isEditing ? content.Notes : Html.HighlightSearchTerm(Model.Content.Notes, notesSearch); 
				%> 
				<div class="<%: sectionSize%>_column section cw-spaced">
					<div class="<%: columnOne%> column text-top"><%: Html.LabelFor(m => m.Content.Notes)%></div>
					<div class="<%: columnTwo%> column"><%: isEditing ? Html.EditorFor(m => m.Content.Notes) : Html.DisplayFor(m => m.Content.Notes)%></div>
				</div>      
				<%} %>
				<%if (Model.SectionsAllowed.Contains("Media")) { %>
				<div>&nbsp;</div>
				<div class="<%: sectionSize%>_column section cw-spaced">
						<div class="<%: columnOne%> column text-small"> 
						<%: Html.LabelFor(m => m.Content.CreatedOn)%> 
						</div>
						<div class="<%: columnTwo%> column text-small">
						<%: content.CreatedOn%>
						</div>
				</div>
				<div class="<%: sectionSize%>_column section cw-spaced">
						<div class="<%: columnOne%> column text-small"> 
						<%: Html.LabelFor(m => m.Content.LastUpdatedOn)%> 
						</div>
						<div class="<%: columnTwo%> column text-small">
						<%: content.LastUpdatedOn%>
						</div>
				</div>
				<div class="<%: sectionSize%>_column section cw-spaced">
						<div class="<%: columnOne%> column text-small"> 
						<%: Html.LabelFor(m => m.Content.MediaDate)%> 
						</div>
						<div class="<%: columnTwo%> column text-small">
						<%: content.MediaDate%>
						</div>
				</div>
				
				<%} %>
			</div>
			<%if (Model.ViewMode != ViewModes.Print || (Model.ViewMode == ViewModes.Print && !String.IsNullOrWhiteSpace(Model.Content.Lyrics))){%>
				<%if (Model.ViewMode == ViewModes.Print){%>
				<hr />
				<%} %>
			<div id="tabs-2">
			<%
				columnOne = "two";
				columnTwo = "seven";
				const int lyricsProp = 7;

				var lyricsSearch = searchFields.Where(s => s.P == lyricsProp).SingleOrDefault();

				Model.Content.Lyrics = isEditing ? content.Lyrics : Html.HighlightSearchTerm(Model.Content.Lyrics, lyricsSearch); 
			%>
				<div class="<%: sectionSize%>_column section cw-spaced">
					<div class="<%: columnOne%> column text-top"><%: Html.LabelFor(m => m.Content.Lyrics)%></div>
					<div class="<%: columnTwo%> column"><%: isEditing ? Html.EditorFor(m => m.Content.Lyrics) : Html.DisplayFor(m => m.Content.Lyrics,"MultilineText")%></div>
				</div>
			</div>
			<%} %>
			<%if (Model.ViewMode != ViewModes.Print) {%>
			<div id="tabs-3">
				<%var tagCreationMsg = "Separate new tags with commas. They will be created and applied to the song when you click Save."; %>
				<%if (isEditing) { %>
				<div>
				<label><em><%: tagCreationMsg %></em></label>
				</div>
				<%} %>
				<%
	  
				var tagTypes = ModelEnums.GetTagTypes();//.Where(t => t != TagType.SoundsLike && t != TagType.Instrument);//.OrderBy(t => t);
				var tagCount = 0;
				var tagTypeCount = 0;
				%>
				<%foreach (var tagType in tagTypes) { %>
				<%    
					//var tagTypeId = String.Format("tags[{0}].", tt++);
					Func<Tag, bool> func = x => x.TagTypeId == (int)tagType;
					var selectedTags = content.Tags.Where(func).Select(x => x.TagId).ToArray();
					var tags = (isEditing ? Model.Tags : content.Tags.ToList()).Where(func).OrderBy(t => t.TagName).ToList();
					var model = new TagCloudViewModel<Tag>() { 
						EditMode = Model.EditMode,
						TagCountSeed = tagCount,
						Tags = tags, 
						TagType = tagType, 
						SelectedTags = selectedTags,
						TagClass = "cw-tagbox-label",
						TagIdTemplate = "t_{0}",
						TagNameTemplate = "tags[{0}]",
						NumberTagsInRow = 5,
						ActiveUserId = Model.ActiveUserId
					};
					tagCount += tags.Count();

					columnOne = "one";
					columnTwo = "eight";
	
				%>   
	
					<%if (tags.Count() > 0) {%>
					<div class="<%: sectionSize%>_column section cw-spaced">
						<div class="<%: columnOne%> column"><label><%:tagType%></label></div>
						<div class="<%: columnTwo%> column">
						<%--<%if (isEditing) { %>
						<% Html.RenderPartial(MVC.Shared.Views.ctrlTagEdit, model); %>
						<%} else {%>--%>
						<% Html.RenderPartial(MVC.Shared.Views.ctrlTagCloud, model); %>
			
						<%//} %>
						</div>
					</div>
					<%} %>
					<%if (isEditing) { %>
					<div class="<%: sectionSize%>_column section cw-spaced">
						<div class="<%: columnOne%> column"></div>
						<div class="<%: columnTwo%> column">
							<label>New <%: tagType %> tags:</label>
				
							<input type="hidden" name="newTags[<%: tagTypeCount %>].Key" value="<%: tagType%>" />				
							<input type="text" name="newTags[<%: tagTypeCount %>].Value" title="<%: tagCreationMsg %>" />
				
			<%--				<a href="#" class="cw-add-tag-link" rel="<%: tagType %>" title="Add a <%: tagType %> tag">
								<img src="../../public/images/icons/silk/add.png" alt="Add" />
							</a>--%>
						</div>
					</div>
					<% tagTypeCount++; %>
					<%} %>
					<%} %>
					<div class="<%: sectionSize%>_column section cw-spaced">
						<div class="<%: columnOne%> column text-top"><%: Html.LabelFor(m => m.Content.SoundsLike)%></div>
						<div class="<%: columnTwo%> column"><%: isEditing ? Html.EditorFor(m => m.Content.SoundsLike) : Html.DisplayFor(m => m.Content.SoundsLike)%></div>
					</div>
					<div class="<%: sectionSize%>_column section cw-spaced">
						<div class="<%: columnOne%> column text-top"><%: Html.LabelFor(m => m.Content.Instruments)%></div>
						<div class="<%: columnTwo%> column"><%: isEditing ? Html.EditorFor(m => m.Content.Instruments) : Html.DisplayFor(m => m.Content.Instruments)%></div>
					</div>
			</div>

			<%} %>
			<%if (Model.SectionsAllowed.Contains("Rights")) { %>
			<%
				columnOne = "two";
				columnTwo = "seven";	  
			%>
			<%if (Model.ViewMode == ViewModes.Print){%>
			<hr />
			<%} %>
			<div id="tabs-4">
					<div class="<%: sectionSize%>_column section cw-spaced">      
						<div class="<%: columnOne%> column"><%: Html.LabelFor(m => m.Content.ContentId)%></div>
						<div class="<%: columnTwo%> column"><%: content.ContentId%></div>
					</div>
					<div class="<%: sectionSize%>_column section cw-spaced">    
						<div class="<%: columnOne%> column"><%: Html.LabelFor(m => m.Content.Catalog.CatalogName)%></div>
						<div class="<%: columnTwo%> column"><%: content.Catalog.CatalogName%></div>
					</div>
					<div class="<%: sectionSize%>_column section cw-spaced">    
						<div class="<%: columnOne%> column"><%: Html.LabelFor(m => m.Content.IsControlledAllIn)%></div>
						<div class="<%: columnTwo%> column">
						<%if (!isEditing) { %>
						<%: Html.CheckBoxFor(x => x.Content.IsControlledAllIn, new { disabled = "disabled" } )%>
						<%: content.IsControlledAllIn.ToYesNo()%>
						<%} else { %>
						<%: Html.CheckBoxFor(x => x.Content.IsControlledAllIn, new { title = "Check if this song is controlled 100%" })%>
						<%} %>
						</div>
					</div>
					<div class="<%: sectionSize%>_column section cw-spaced">    
						<div class="<%: columnOne%> column"><label>Share %</label></div>
						<div class="<%: columnTwo%> column">
							<%if (!isEditing && (content.ContentRights == null || content.ContentRights.Count() == 0)) { %>
							No Rightsholder information available.
							<%} else { %>
							<table class="cw-tbl-content-rights">
								<tr>
									<th>
										Rights Holder
									</th>
									<th>
										Rights Type
									</th>
									<th class="text-right">
										Share
									</th>
									<th>
									</th>
								</tr>
							<%var r = 0; %>
							<%if (content.ContentRights != null && content.ContentRights.Count() > 0) { %>
							<%foreach (var contentRight in content.ContentRights) { %>
							<%	
								var rightName = String.Format("rights[{0}].", r);
								var rightId = String.Format("r_{0}_", r);
								r++;
		  
								var rightsHolderShare = contentRight.RightsHolderShare.ToString("P2");
							%>
								<tr>
									<td class="cw-content-field">
										<%if (isEditing) {%> 
										<%: Html.Hidden(String.Concat(rightName, "ContentId"), contentRight.ContentId)%>
										<%: Html.Hidden(String.Concat(rightName, "ContentRightId"), contentRight.ContentRightId)%>
										<%: Html.TextBox(String.Concat(rightName, "RightsHolderName"), contentRight.RightsHolderName, new { @class = "cw-autocomplete cw-field-large", alt = "RightsHolderName", title = "Enter the name of the rights holder" })%>
										<%} else {%> 
										<%: contentRight.RightsHolderName%>
										<%} %>
									</td>
									<td class="cw-content-field">
										<%if (isEditing) {%> 
										<%: Html.DropDownList(String.Concat(rightName, "RightsTypeId"), new SelectList(ModelEnums.GetRightsTypes(), (RightsTypes)contentRight.RightsTypeId), new { title = "Select a rights type from the list"})%>
										<%} else {%> 
										<%: (RightsTypes)contentRight.RightsTypeId%>
										<%} %>
									</td>
									<td class="cw-content-field text-right">
										<%if (isEditing) {%> 
										<%: Html.TextBox(String.Concat(rightName, "RightsHolderShare"), rightsHolderShare, new { @class = "cw-field-small", title="Enter a percentage, e.g. 33.33% = 33.33"})%>
										<%} else {%> 
										<%: rightsHolderShare%>
										<%} %>
									</td>
									<td>
									<%if (isEditing) {%>
									<%: Html.Hidden(String.Concat(rightName, "ModelAction"), (int)ModelAction.Update, new { @class = "cw-model-action" })%>
									<a href="#" class="cw-delete-right-link" title="Delete this rights holder"><img src="../../public/images/icons/silk/delete.png" alt="Delete" /></a>
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
									NumberTagsInRow = 7,
									TagClass = "cw-tagbox-label",
									TagIdTemplate = String.Concat(rightId, "t_{0}"),
									TagNameTemplate = String.Concat(rightName, "Territories[{0}]")
									};              
									%>    
									Territories:
									<% Html.RenderPartial(MVC.Shared.Views.ctrlTerritoryCloud, model); %>
									<%//} %>
									</td>
									<td>
									</td>
								</tr>
							<%} %>
							<%} %>
							<%if (isEditing) {%>
							<%	
								var rightName = String.Format("rights[{0}].", r);
								var rightId = String.Format("r_{0}_", r);
								r++;
								%>
								<tr>
									<td class="cw-content-field">
										<%: Html.Hidden(String.Concat(rightName, "ContentId"), Model.Content.ContentId)%>
										<%: Html.Hidden(String.Concat(rightName, "ContentRightId"), 0)%>
										<%: Html.TextBox(String.Concat(rightName, "RightsHolderName"), null, new { @class = "cw-autocomplete cw-field-large", alt = "RightsHolderName", title = "Enter the name of the rights holder" })%>
									</td>
									<td class="cw-content-field">
										<%: Html.DropDownList(String.Concat(rightName, "RightsTypeId"), new SelectList(ModelEnums.GetRightsTypes()), new { title = "Select a rights type from the list"})%>
									</td>
									<td class="cw-content-field text-right">
										<%: Html.TextBox(String.Concat(rightName, "RightsHolderShare"), null, new { @class = "cw-field-small", title="Enter a percentage, e.g. 33.33% = 33.33" })%>
									</td>
									<td>
									<%: Html.Hidden(String.Concat(rightName, "ModelAction"), (int)ModelAction.Add, new { @class = "cw-model-action" })%>
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
									NumberTagsInRow = 7,
									TagClass = "cw-tagbox-label",
									TagIdTemplate = String.Concat(rightId, "Territory_{0}"),
									TagNameTemplate = String.Concat(rightName, "Territories[{0}]")
								};              
									%>
									Territories:    
									<% Html.RenderPartial(MVC.Shared.Views.ctrlTerritoryCloud, model); %>
									</td>
									<td>
									</td>
								</tr>
							<%} %>

							</table>
							<%}%>
							</div>
					</div>
					<div class="<%: sectionSize%>_column section cw-spaced">    
						<div class="<%: columnOne%> column text-top"><%: Html.LabelFor(m => m.Content.LicensingNotes)%></div>
						<div class="<%: columnTwo%> column"><%: isEditing ? Html.EditorFor(m => m.Content.LicensingNotes) : Html.DisplayFor(m => m.Content.LicensingNotes)%></div>
					</div>

			</div>
			<%} %>

	<%if (isEditing) {%>

	<% Html.EndForm();%>
	<%}%>

	<%--<%if (Model.ViewMode != ViewModes.Print) {%>
	</div>
	<%} %>--%>
	</div>
	<%if (Model.ViewMode == ViewModes.Embedded) { %>
		<div><a href="#" id="cw-detail-close-link">Close</a></div>
	<%} %>
	</div>
</div>
<%if (isEditing) { %>
<div id="dialog-confirm-save-changes" class="cw-hidden-dialog" title="Save Changes?">
	<p><span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>
	You're currently in Edit mode. Do you want to save any changes you've made?</p>
</div>
<%} %>
<%if (Model.ViewMode != ViewModes.Print) { %>
<script language="javascript" type="text/javascript">
	$(document).ready(function () {
		//alert('here');
		setupTooltips();
		//setupMediaUploader('fullUploadContainer', 'fullVersionUpload', 'fullVersionFilelist', 'Full', 0);
		//setupMediaUploader('previewVersionUploadContainer', 'previewVersionUpload','previewVersionFilelist','Preview', 1);
	});
</script>
<%} %>