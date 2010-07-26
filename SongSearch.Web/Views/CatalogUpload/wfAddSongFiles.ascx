<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.CatalogUploadViewModel>" %>
<%: Html.Hidden("minimumFiles", 1) %>
<%: Html.Hidden("maxFiles", 25) %>
<%: Html.Hidden("maxBytes", 25*10*1024*1024) %>

<div>
For this step, please select full song files in MP3 format only. We will deal with short previews in the next step.
</div>
<div id="uploadMessage" class="feedback-box feedback-box-error" style="display:none"></div>
<div>&nbsp;</div>
<div id="wizardUploader">
	<p>You browser doesn't have Flash, Silverlight, Gears, BrowserPlus or HTML5 support.</p>
</div>
<input id="state_MediaVersion" name="state.MediaVersion" type="hidden" value="<%: MediaVersion.Full %>" />
<div id="fileList">
</div>
<div>&nbsp;</div>
<div>
	<strong>Next</strong>, you will select which song preview files to upload. For this step, you’ll need to have MP3 files of your song previews on your local or connected USB drive. More details in the next step.

</div>