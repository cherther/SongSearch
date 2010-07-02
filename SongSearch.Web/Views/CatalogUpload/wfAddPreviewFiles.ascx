<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.CatalogUploadViewModel>" %>
<%: Html.Hidden("minimumFiles", 0)%>
<div>
 <p>For this step, please select short Song Preview files in MP3 format only.</p>
 <p>We will match preview files to full songs based on their embedded ID3 tags, or their file names if we can't find any tags.</p>
 <p>If you do not have previews now, you can always add them per song later.</p>
</div>
<div>&nbsp;</div>
<div id="uploadMessage" class="feedback-box feedback-box-error" style="display:none">&nbsp;</div>
<div id="wizardUploader">
	<p>You browser doesn't have Flash, Silverlight, Gears, BrowserPlus or HTML5 support.</p>
</div>
<input id="state_MediaVersion" name="state.MediaVersion" type="hidden" value="<%: MediaVersion.Preview %>" />
<div id="fileList">
</div>
<div>&nbsp;</div>
<div>
	<strong>Next</strong>, you will review your uploads and add/edit some basic information, like Title and Artist information.
</div>