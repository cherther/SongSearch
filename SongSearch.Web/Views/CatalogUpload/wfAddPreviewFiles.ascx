<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.CatalogUploadViewModel>" %>
<%var maxFiles = Model.MyUserBalances.NumberOfSongs.Max;%>
<%: Html.Hidden("minimumFiles", 0) %>
<%: Html.Hidden("maxFiles", maxFiles)%>
<%: Html.Hidden("maxBytes", Model.MyUserBalances.NumberOfSongs.Default * 10 * 1024 * 1024)%>
<div>
 <p>For this step, please select short Song Preview files in MP3 format only.</p>
 <p>We will match preview files to full songs based on their embedded ID3 tags, or their file names if we can't find any tags.</p>
 <p>If you do not have previews now, you can always add them per song later.</p>
</div>
<div>&nbsp;</div>
<div id="uploadMessage" class="feedback-box feedback-box-error" style="display:none">&nbsp;</div>
<div id="wizardUploader">
	<p>Unfortunately, you browser doesn't seem to support our Upload Widget, which requires either Flash, Silverlight or HTML5 support.</p>
</div>
<input id="state_MediaVersion" name="state.MediaVersion" type="hidden" value="<%: MediaVersion.Preview %>" />
<div id="fileList">
</div>
<div>&nbsp;</div>
<div>
	<strong>Next</strong>, you will review your uploads and add/edit some basic information, like Title and Artist information.
</div>