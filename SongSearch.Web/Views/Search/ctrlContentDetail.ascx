<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ContentViewModel>" %>
<%
    var content = Model.Content;
	var searchFields = Model.SearchFields;
	var menuButtonClass = "cw-button cw-simple cw-small cw-gray"; //'.cw-content-detail-menu
%>
    <%--<div style="float: right">
    <button id="detail-close" type="submit" title="Close" class="cw-button cw-small cw-simple cw-blue">
    <span class="b-cancel">Close</span>
    </button>
    </div>--%>

    <div id="cw-content-detail">
		<%if (Model.ViewMode != ViewModes.Print) {%>
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
			<%: Html.ActionLink("Edit", "Edit", "Search", new { id = content.ContentId }, new { @class = menuButtonClass })%>
			<%} %>
			</div>
			<hr />
			<div id="cw-content-detail-tabs">
			<ul>
				<li><a href="#tabs-1">Overview</a></li>
				<li><a href="#tabs-2">Lyrics</a></li>
				<li><a href="#tabs-3">Tags</a></li>
				<%if (Model.SectionsAllowed.Contains("Rights")) { %>
				<li><a href="#tabs-4">Rights</a></li>
				<%} %>
			</ul>
		<%} else {%>
			<h3><%: content.Title %> by <%: content.Artist %></h3>
		<%} %>
		<div id="tabs-1">
			<table class="cw-tbl-content-detail">
				<tr>
					<td class="cw-content-label">Title</td>
					<td class="cw-content-field"><%: content.Title %></td>
				</tr>
				<tr>
					<td class="cw-content-label">Artist</td>
					<td class="cw-content-field"><%: content.Artist%></td>
				</tr>
				<tr>
					<td class="cw-content-label">Pop</td>
					<td class="cw-content-field"><%: content.Pop%></td>
				</tr>
				<tr>
					<td class="cw-content-label">Country</td>
					<td class="cw-content-field"><%: content.Country%></td>
				</tr>
				<tr>
					<td class="cw-content-label">Title</td>
					<td class="cw-content-field"><%: content.Title %></td>
				</tr>
				<tr>
					<td class="cw-content-label">Release Year</td>
					<td class="cw-content-field"><%: content.ReleaseYear%></td>
				</tr>
				<tr>
					<td class="cw-content-label">Record Label</td>
					<td class="cw-content-field"><%: content.RecordLabel%></td>
				</tr>
				<tr>
					<td class="cw-content-label">Writers</td>
					<td class="cw-content-field"><%: content.Writers%></td>
				</tr>
				<%if (Model.SectionsAllowed.Contains("Notes")) { %>
				<%
					var notesSearch = searchFields.Where(s => s.P == 19).SingleOrDefault();
					var notes = Html.HighlightSearchTerm(content.Notes, notesSearch); 
				 %> 
				 <tr>
					<td class="cw-content-label">Notes</td>
					<td class="cw-content-field"><pre><%= notes%></pre></td>
				</tr>      
				<%} %>
				 <tr>
					<td class="cw-content-label">Keywords</td>
					<td class="cw-content-field"><%: content.Keywords%></td>
				</tr>
				<tr>
					<td class="cw-content-label">Similar Songs</td>
					<td class="cw-content-field"><%: content.SimilarSongs%></td>
				</tr>
			</table>
		</div>
		<div id="tabs-2">
		<%
				var lyricsSearch = searchFields.Where(s => s.P == 7).SingleOrDefault();
				var lyrics = Html.HighlightSearchTerm(content.Lyrics, lyricsSearch);
		%>
			<table class="cw-tbl-content-detail">         
				<tr>
					<td class="cw-content-label">Lyrics</td>
					<td class="cw-content-field"><pre><%= lyrics%></pre></td>
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
			var selectedTags = content.Tags.Where(t => t.TagTypeId == (int)tagType).Select(t => t.TagId).ToArray();
			var tags = content.Tags.Where(t => t.TagTypeId == (int)tagType).ToList();//Model.Tags.Where(t => t.TagTypeId ==(int)tagType).ToList();
			var model = new TagCloudViewModel() { Tags = tags, TagType = tagType, SelectedTags = selectedTags, TagClass = "cw-tagbox-detail", TagIdTemplate = "t" };              
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
					<td class="cw-content-label">Song ID</td>
					<td class="cw-content-field"><%: content.ContentId %></td>
				</tr>
        		<tr>
					<td class="cw-content-label">Catalog</td>
					<td class="cw-content-field"><%: content.Catalog.CatalogName%></td>
				</tr>
                <tr>
					<td class="cw-content-label">All-in</td>
					<td class="cw-content-field"><%: Html.CheckBoxFor(x => x.Content.IsControlledAllIn, new { disabled = "disabled" })%></td>
				</tr>
			   <tr>
					<td class="cw-content-label">Licensing Notes</td>
					<td class="cw-content-field"><pre><%: content.LicensingNotes%></pre></td>
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
								<td class="cw-content-field"><%: contentRight.RightsHolderName%>
								</td>
								<td class="cw-content-field"><%: (RightsTypes)contentRight.RightsTypeId%>
								</td>
								<td class="cw-content-field"><%: contentRight.RightsHolderShare.ToString("P0")%>
								</td>
							</tr>
        
						<%} %>
						</table>
						</td>
				</tr>    
        		</table>

		</div>
		<%} %>
    
		<%if (Model.ViewMode == ViewModes.Embedded) { %>
		<div><a href="#" id="cw-detail-close-link">Close</a></div>
		<%} %>
		<%if (Model.ViewMode != ViewModes.Print) {%>
		</div>
		<%} %>
		</div>
	</div>
	