<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.CatalogUploadViewModel>" %>
<%var maxFiles = 
	  Model.MyUserQuotas.NumberOfSongs.Remaining < 0 ||
			Model.MyUserQuotas.NumberOfSongs.Remaining > Model.DefaultSongQuota ? Model.DefaultSongQuota : 
	  Model.MyUserQuotas.NumberOfSongs.Remaining; %>
<%: Html.Hidden("minimumFiles", 1) %>
<%: Html.Hidden("maxFiles", maxFiles)%>
<%: Html.Hidden("maxBytes", Model.DefaultSongQuota * 10 * 1024 * 1024)%>

<div>
For this step, please select full song files in MP3 format only. We will deal with short previews in the next step.
</div>
<div>&nbsp;</div>
<div>
Based on your current Plan, you can upload <strong><%: Model.MyUserQuotas.NumberOfSongs.Remaining.ToQuotaDescription()%></strong> songs.
<%if (Model.MyUserQuotas.NumberOfSongs.Remaining < 0 || Model.MyUserQuotas.NumberOfSongs.Remaining > maxFiles) { %>
However, to make sure everything goes smoothly, please upload only <strong><%: maxFiles %> songs at a time</strong>.
<%} %>
</div>
<div id="uploadMessage" class="feedback-box feedback-box-error" style="display:none"></div>
<div>&nbsp;</div>
<div id="wizardUploader">
	<p>Unfortunately, you browser doesn't seem to support our Upload Widget, which requires either Flash, Silverlight or HTML5 support.</p>
</div>
<input id="state_MediaVersion" name="state.MediaVersion" type="hidden" value="<%: MediaVersion.Full %>" />
<div id="fileList">
</div>
<div>&nbsp;</div>
<div>
	<strong>Next</strong>, you will select which song preview files to upload. For this step, you’ll need to have MP3 files of your song previews on your local or connected USB drive. More details in the next step.

</div>
