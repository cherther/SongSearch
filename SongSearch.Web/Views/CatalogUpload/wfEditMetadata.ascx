﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.CatalogUploadViewModel>" %>
<%: Html.Hidden("state.CatalogId", Model.CatalogUploadState.CatalogId) %>
<%: Html.Hidden("state.CatalogName", Model.CatalogUploadState.CatalogName)%>

<div>
We have tried to automatically populate a few fields for you based on the embedded ID3 tags we've found.
</div>
<div>&nbsp;</div>

<%if (Model.CatalogUploadState.Content != null){ %>
<table width="100%">
	<tr>
		<th>Uploaded File
		</th>
		<th class="text-center">Preview?
		</th>
		<th>Title
		</th>
		<th>Artist
		</th>
		<th>Record Label
		</th>
		<th>Year
		</th>
	</tr>
<%var i = 0; %>
<%foreach(var content in Model.CatalogUploadState.Content){ %>
	<tr>
	<td>
	<%for (var j = 0; j < content.UploadFiles.Count(); j++) { %>
	<%var file = content.UploadFiles[j]; %>
	<%: Html.Hidden(String.Format("state.Content[{0}].UploadFiles[{1}].FileName", i, j), file.FileName)%>
	<%: Html.Hidden(String.Format("state.Content[{0}].UploadFiles[{1}].FilePath", i, j), file.FilePath)%>
	<%: Html.Hidden(String.Format("state.Content[{0}].UploadFiles[{1}].FileMediaVersion", i, j), file.FileMediaVersion)%>
	<%if (file.FileMediaVersion == MediaVersion.Full) { %>
		<%: file.FileName.AsNullIfWhiteSpace() ?? "(No Full Song File!)" %>
	<%} %>
	<%} %>
	</td>
	<td class="text-center">
<%--	<%: Html.CheckBox(String.Format("state.Content[{0}].HasMediaPreviewVersion", i), content.HasMediaPreviewVersion, new { disabled = "disabled" }) %>
--%>
	<% if (content.HasMediaPreviewVersion) {%>
	<img src="../../Public/Images/Icons/Silk/tick.png" alt="Ok" title="Found matching preview file"/>	
	<% } else {%>
	<img src="../../Public/Images/Icons/Silk/error.png" alt="Error" title="No matching preview file!"/>
	<%} %>
	<%: Html.Hidden(String.Format("state.Content[{0}].HasMediaPreviewVersion", i), content.HasMediaPreviewVersion)%>
	<%: Html.Hidden(String.Format("state.Content[{0}].HasMediaFullVersion", i), content.HasMediaFullVersion)%>
	<%: Html.Hidden(String.Format("state.Content[{0}].Notes", i), content.Notes)%>
	</td>
	<td>
	<%: Html.TextBox(String.Format("state.Content[{0}].Title", i), content.Title, new { @class = "cw-field-xlarge", title = "The name of the song we extracted from the song's id3 tag" })%>
	</td>
	<td>
	<%: Html.TextBox(String.Format("state.Content[{0}].Artist", i), content.Artist.AsNullIfWhiteSpace() ?? "UNKNOWN", new { @class = "cw-field-large", title = "The artist name we extracted from the song's id3 tag" })%>
	</td>
	<td>
	<%: Html.TextBox(String.Format("state.Content[{0}].RecordLabel", i), content.RecordLabel.AsNullIfWhiteSpace() ?? "", new { @class = "cw-field-medium", title = "Please enter the name of the record label" })%>
	</td>
	<td>
	<%: Html.TextBox(String.Format("state.Content[{0}].ReleaseYear", i), content.ReleaseYear, new { @class = "cw-field-xsmall", title = "The release year we extracted from the song's id3 tag" })%>
	</td>
	
	</tr>
	<% i++; %>
<%} %>
</table>
<%} %>