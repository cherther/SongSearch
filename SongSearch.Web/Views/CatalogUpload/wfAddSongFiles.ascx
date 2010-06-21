<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.CatalogUploadViewModel>" %>
<div id="uploader">
	<p>You browser doesn't have Flash, Silverlight, Gears, BrowserPlus or HTML5 support.</p>
</div>
<input id="state_MediaVersion" name="state.MediaVersion" type="hidden" value="<%: MediaVersion.FullSong %>" />
<div id="fileList">
</div>
