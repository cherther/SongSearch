<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.CatalogViewModel>" %>
<%
	
	var catsOwned = Model.MyCatalogs.Where(c => c.CreatedByUserId == Model.ActiveUserId)
		.Select(c => new { CatalogId = c.CatalogId, CatalogName = String.Concat("*", c.CatalogName) }).ToList();
	var catsOther = Model.MyCatalogs.Where(c => c.CreatedByUserId != Model.ActiveUserId)
		.Select(c => new { CatalogId = c.CatalogId, CatalogName = c.CatalogName }).ToList();
	
	catsOwned.Add(new { CatalogId = -1, CatalogName = "----------" });
	
	var catalogs = catsOwned.Union(catsOther);

	var selectList = new SelectList(catalogs, "CatalogId", "CatalogName", Model.Catalog.CatalogId);
	

	
%>
<table id="catalog-list">
	<tr>
	<%if (Model.MyCatalogs.Count() > 0) {%>
	<td>
	<%: Html.DropDownList("CatalogId", selectList, new { id = "cw-catalog-menu" })%>
	</td>
	<%} %>
<%--	<%if (catsOther.Count() > 0) {%>
	<td>
	<%: Html.DropDownList("CatalogId", new SelectList(catsOther, "CatalogId", "CatalogName", Model.Catalog.CatalogId))%>
	</td>
	<%} %>--%>
	</tr>
</table>
