<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.Data.Content>" %>
    <%--<div style="float: right">
    <button id="detail-close" type="submit" title="Close" class="cw-button cw-small cw-simple cw-blue">
    <span class="b-cancel">Close</span>
    </button>
    </div>--%>
    <div id="cw-content-detail">
    
    <fieldset>
        <div class="cw-content-label">#</div>
        <div class="cw-content-field"><%: Model.ContentId %></div>
        
        <div class="cw-content-label">CatalogId</div>
        <div class="cw-content-field"><%: Model.Catalog.CatalogName %></div>
                
        <div class="cw-content-label">All-in</div>
        <div class="cw-content-field"><%: Model.IsControlledAllIn %></div>
        
        <div class="cw-content-label">Title</div>
        <div class="cw-content-field"><%: Model.Title %></div>
        
        <div class="cw-content-label">Artist</div>
        <div class="cw-content-field"><%: Model.Artist %></div>
        
        <div class="cw-content-label">Pop</div>
        <div class="cw-content-field"><%: Model.Pop %></div>
        
        <div class="cw-content-label">Country</div>
        <div class="cw-content-field"><%: Model.Country %></div>

        <div class="cw-content-label">Release Year</div>
        <div class="cw-content-field"><%: Model.ReleaseYear %></div>
        
        <div class="cw-content-label">Record Label</div>
        <div class="cw-content-field"><%: Model.RecordLabel %></div>
        
        <div class="cw-content-label">Writers</div>
        <div class="cw-content-field"><%: Model.Writers %></div>
        
        <div class="cw-content-label">Lyrics</div>
        <div class="cw-content-field"><pre><%: Model.Lyrics %></pre></div>
        
        <div class="cw-content-label">Notes</div>
        <div class="cw-content-field"><pre><%: Model.Notes %></pre></div>
        
        <div class="cw-content-label">Keywords</div>
        <div class="cw-content-field"><%: Model.Keywords %></div>
        
        <div class="cw-content-label">Similar Songs</div>
        <div class="cw-content-field"><%: Model.SimilarSongs %></div>
        
        <div class="cw-content-label">LicensingNotes</div>
        <div class="cw-content-field"><pre><%: Model.LicensingNotes %></pre></div>

        <div class="cw-content-label">HasMediaPreviewVersion</div>
        <div class="cw-content-field"><%: Model.HasMediaPreviewVersion %></div>
        
        <div class="cw-content-label">HasMediaFullVersion</div>
        <div class="cw-content-field"><%: Model.HasMediaFullVersion %></div>
        
        <div class="cw-content-label">Tags</div>
        <%foreach (var tag in Model.Tags) { %>
            <div class="cw-content-field"><%: tag.TagName%></div>
        <%} %>
        
        <div class="cw-content-label">ContentRights</div>
        <%foreach (var contentRight in Model.ContentRights) { %>
            <div class="cw-content-label">Rights Holder</div>
            <div class="cw-content-field"><%: contentRight.RightsHolderName%></div>
            <div class="cw-content-label">Rights Type</div>
            <div class="cw-content-field"><%: (RightsTypes) contentRight.RightsTypeId%></div>
            <div class="cw-content-label">Share</div>
            <div class="cw-content-field"><%: contentRight.RightsHolderShare%></div>

        
        <%} %>
    </fieldset>
    <div><a href="#" id="cw-detail-close-link">Close</a></div>
    
    </div>