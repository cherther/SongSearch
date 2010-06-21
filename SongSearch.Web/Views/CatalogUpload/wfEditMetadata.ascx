<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.CatalogUploadViewModel>" %>
<%: Html.Hidden("state.CatalogId", Model.CatalogUploadState.CatalogId) %>
<%: Html.Hidden("state.CatalogName", Model.CatalogUploadState.CatalogName)%>

<%if (Model.CatalogUploadState.Content != null){ %>
<table width="100%">
	<tr>
		<th>Title
		</th>
		<th>Artist
		</th>
		<th>Year
		</th>
		<th>Full Song?
		</th>
		<th>Preview?
		</th>
	</tr>
<%var i = 0; %>
<%foreach(var content in Model.CatalogUploadState.Content){ %>
	<tr>
	<td>
	<%: Html.TextBox(String.Format("state.Content[{0}].Title", i), content.Title) %>
	</td>
	<td>
	<%: Html.TextBox(String.Format("state.Content[{0}].Artist", i), content.Artist.AsNullIfWhiteSpace() ?? "(N/A)")%>
	</td>
	<td>
	<%: Html.TextBox(String.Format("state.Content[{0}].ReleaseYear", i), content.ReleaseYear)%>
	</td>
	<td>
	<%: Html.CheckBox(String.Format("state.Content[{0}].HasMediaFullVersion", i), content.HasMediaFullVersion, new { disabled = "disabled" }) %>
	</td>
	<td>
	<%: Html.CheckBox(String.Format("state.Content[{0}].HasMediaPreviewVersion", i), content.HasMediaPreviewVersion, new { disabled = "disabled" }) %>
	</td>
	</tr>
	<% i++; %>
<%} %>
</table>
<%} %>