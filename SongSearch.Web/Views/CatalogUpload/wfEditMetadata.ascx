<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.CatalogUploadViewModel>" %>
<%: Html.Hidden("state.CatalogId", Model.CatalogUploadState.CatalogId) %>
<%: Html.Hidden("state.CatalogName", Model.CatalogUploadState.CatalogName)%>

<%if (Model.CatalogUploadState.Content != null){ %>
<table width="100%">
	<tr>
		<th>Uploaded File
		</th>
		<th>Preview?
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
	<%if (file.FileMediaVersion == MediaVersion.FullSong) { %>
		<%: file.FileName.AsNullIfWhiteSpace() ?? "(No Full Song File!)" %>
	<%} %>
	<%} %>
	</td>
	<td>
<%--	<%: Html.CheckBox(String.Format("state.Content[{0}].HasMediaPreviewVersion", i), content.HasMediaPreviewVersion, new { disabled = "disabled" }) %>
--%>
	<%: content.HasMediaPreviewVersion.ToYesNo() %>
	<%: Html.Hidden(String.Format("state.Content[{0}].HasMediaPreviewVersion", i), content.HasMediaPreviewVersion)%>
	<%: Html.Hidden(String.Format("state.Content[{0}].HasMediaFullVersion", i), content.HasMediaFullVersion)%>
	</td>
	<td>
	<%: Html.TextBox(String.Format("state.Content[{0}].Title", i), content.Title, new { @class = "cw-field-xlarge" })%>
	</td>
	<td>
	<%: Html.TextBox(String.Format("state.Content[{0}].Artist", i), content.Artist.AsNullIfWhiteSpace() ?? "(N/A)", new { @class = "cw-field-large" })%>
	</td>
	<td>
	<%: Html.TextBox(String.Format("state.Content[{0}].RecordLabel", i), content.RecordLabel.AsNullIfWhiteSpace() ?? "(N/A)", new { @class = "cw-field-medium" })%>
	</td>
	<td>
	<%: Html.TextBox(String.Format("state.Content[{0}].ReleaseYear", i), content.ReleaseYear, new { @class = "cw-field-xsmall" })%>
	</td>
	
	</tr>
	<% i++; %>
<%} %>
</table>
<%} %>