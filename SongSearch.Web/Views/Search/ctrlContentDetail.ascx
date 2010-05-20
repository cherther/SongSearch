<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ContentViewModel>" %>
<%
    var content = Model.Content;
%>
    <%--<div style="float: right">
    <button id="detail-close" type="submit" title="Close" class="cw-button cw-small cw-simple cw-blue">
    <span class="b-cancel">Close</span>
    </button>
    </div>--%>
    <div id="cw-content-detail">
    <hr />
    <div id="cw-media-player">
    Mediaplayer (TBD)
    </div>
    <hr />
    <hr />
    <div id="cw-content-detail-menu">
    <%if (content.HasMediaFullVersion) { %>
    <a href="#">Play</a>&nbsp;|&nbsp;<a href="#">Preview</a>&nbsp;|&nbsp;<a href="#">Download</a>&nbsp;|&nbsp;<a href="#">Add To Cart</a>&nbsp;|&nbsp;
    <%} %>
    <a href="#">Print</a>
    <%if (Model.UserCanEdit) { %>
    &nbsp;|&nbsp;<a href="#">Edit</a>
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
	<div id="tabs-1">
	<fieldset>
        <div class="cw-content-label">#</div>
        <div class="cw-content-field"><%: content.ContentId %></div>
        
        <div class="cw-content-label">Title</div>
        <div class="cw-content-field"><%: content.Title %></div>
        
        <div class="cw-content-label">Artist</div>
        <div class="cw-content-field"><%: content.Artist %></div>
        
        <div class="cw-content-label">Pop</div>
        <div class="cw-content-field"><%: content.Pop %></div>
        
        <div class="cw-content-label">Country</div>
        <div class="cw-content-field"><%: content.Country %></div>

        <div class="cw-content-label">Release Year</div>
        <div class="cw-content-field"><%: content.ReleaseYear %></div>
        
        <div class="cw-content-label">Record Label</div>
        <div class="cw-content-field"><%: content.RecordLabel %></div>
        
        <div class="cw-content-label">Writers</div>
        <div class="cw-content-field"><%: content.Writers %></div>
                
        <div class="cw-content-label">Notes</div>
        <div class="cw-content-field"><pre><%: content.Notes %></pre></div>
        
        <div class="cw-content-label">Keywords</div>
        <div class="cw-content-field"><%: content.Keywords %></div>
        
        <div class="cw-content-label">Similar Songs</div>
        <div class="cw-content-field"><%: content.SimilarSongs %></div>

    </fieldset>
    </div>
    <div id="tabs-2">
	<fieldset>          
        <div class="cw-content-label">Lyrics</div>
        <div class="cw-content-field"><pre><%: content.Lyrics %></pre></div>

        

<%--        <div class="cw-content-label">HasMediaPreviewVersion</div>
        <div class="cw-content-field"><%: content.HasMediaPreviewVersion %></div>
        
        <div class="cw-content-label">HasMediaFullVersion</div>
        <div class="cw-content-field"><%: content.HasMediaFullVersion %></div>
--%>    </fieldset>
    </div>
    <div id="tabs-3">
	<fieldset>           
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
            <div class="cw-content-label"><%=tagType %></div>
            <% Html.RenderPartial("ctrlTagCloud", model); %>
        <%} %>
        <%} %>
    </fieldset>
    </div>
    <%if (Model.SectionsAllowed.Contains("Rights")) { %>
    <div id="tabs-4">
	<fieldset>           
        <div class="cw-content-label">Catalog</div>
        <div class="cw-content-field"><%: content.Catalog.CatalogName%></div>
                
        <div class="cw-content-label">All-in</div>
        <div class="cw-content-field"><%: Html.CheckBoxFor(x => x.Content.IsControlledAllIn, new { disabled = "disabled" })%></div>
        
       <%-- <div class="cw-content-label">Rights</div>--%>
        <table>
            <tr>
                <th>
                    <div class="cw-content-label">Rights Holder</div>
                </th>
                <th>
                    <div class="cw-content-label">Rights Type</div>
                </th>
                <th>
                    <div class="cw-content-label">Share</div>
                </th>
            </tr>
        <% foreach (var contentRight in content.ContentRights) { %>
            <tr>
                <td>
                    <div class="cw-content-field"><%: contentRight.RightsHolderName%></div>
                </td>
                <td>
                    <div class="cw-content-field"><%: (RightsTypes)contentRight.RightsTypeId%></div>
                </td>
                <td>
                    <div class="cw-content-field"><%: contentRight.RightsHolderShare.ToString("P0")%></div>
                </td>
            </tr>
        
        <%} %>
        </table>      
        
        <div class="cw-content-label">Licensing Notes</div>
        <div class="cw-content-field"><pre><%: content.LicensingNotes%></pre></div>
    </fieldset>
    </div>
    <%} %>
    <div><a href="#" id="cw-detail-close-link">Close</a></div>
    
    </div>