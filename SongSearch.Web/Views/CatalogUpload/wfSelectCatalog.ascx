<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.CatalogUploadViewModel>" %>

<div>
Enter a name for a new Catalog:
	<div>
		<%: Html.TextBox("state.CatalogName") %>
	</div>
</div>
<div>&nbsp;</div>
<div>
Or, select an existing Catalog:
	<div>
		<%: Html.DropDownList("state.CatalogId", new SelectList(Model.MyCatalogs, "CatalogId", "CatalogName"), "")%>
	</div>
</div>