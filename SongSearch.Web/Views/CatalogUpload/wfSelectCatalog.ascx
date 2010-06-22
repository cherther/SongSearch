<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.CatalogUploadViewModel>" %>

<div>
Enter the name of the new catalog:
	<div>
		<%: Html.TextBox("state.CatalogName", null, new { @class = "cw-field-large" })%>
	</div>
</div>
<div>&nbsp;</div>
<div>
Or, select one of your existing catalogs:
	<div>
		<%: Html.DropDownList("state.CatalogId", new SelectList(Model.MyCatalogs, "CatalogId", "CatalogName"), "")%>
	</div>
</div>
<div>&nbsp;</div>
<div>
	<strong>Next</strong>, you select which songs to upload. For this step, you’ll need to have MP3 files of your songs on your local or connected USB drive.
</div>