<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.CatalogViewModel>" %>
<%
	
	var catsOwned = Model.MyCatalogs.Where(c => c.CreatedByUserId == Model.ActiveUserId)
		.Select(c => new { CatalogId = c.CatalogId, CatalogName = c.CatalogName })
		.OrderBy(c => c.CatalogName)	
		.ToList();
	var catsOther = Model.MyCatalogs.Where(c => c.CreatedByUserId != Model.ActiveUserId)
		.OrderBy(c => c.CatalogName)
		.Select(c => new { CatalogId = c.CatalogId, CatalogName = c.CatalogName }).ToList();

	if (catsOwned.Count > 0 && catsOther.Count > 0) {
		catsOwned.Add(new { CatalogId = -1, CatalogName = "----------" });
	}
	var catalogs = catsOwned.Union(catsOther);
	var selectCatalogId = Model.Catalog != null ? Model.Catalog.CatalogId : 0;
	var selectList = new SelectList(catalogs, "CatalogId", "CatalogName", selectCatalogId);
	

	
%>
<table id="catalog-list">
	<tr>
	<%if (Model.MyCatalogs.Count() > 0) {%>
	<td>
	<%using (Html.BeginForm(MVC.CatalogManagement.Detail(), FormMethod.Get)) {  %>
	<%: Html.DropDownList("id", selectList, "-- Select a Catalog --", new { id = "cw-catalog-menu" })%>
	<%} %>
	</td>
	<%} %>
<%--	<%if (catsOther.Count() > 0) {%>
	<td>
	<%: Html.DropDownList("CatalogId", new SelectList(catsOther, "CatalogId", "CatalogName", Model.Catalog.CatalogId))%>
	</td>
	<%} %>--%>
	</tr>
</table>
