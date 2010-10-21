<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.CatalogViewModel>" %>
<% var catalogDetailId = Model.Catalog != null ? Model.Catalog.CatalogId : 0; %>
<% foreach (var catalog in Model.MyCatalogs)
	{
		var rowId = String.Concat("catalog-", catalog.CatalogId.ToString());
		//var rowClass = user.ParentUserId.HasValue ? String.Concat("c-", user.ParentUserId.ToString()) : "";
		var rowClass = String.Concat("c-", catalog.CatalogId.ToString());

		//var childClass = user.ParentUserId.HasValue ? String.Concat("children-", user.UserId.ToString()) : "";
		%>

	<tr id="<%: rowId%>" class="cw-user-listing <%: rowClass%>">
		<td>-</td>
		<td>
			<%if (catalog.CatalogId == catalogDetailId) { %>
			<strong><%:  catalog.CatalogName %></strong>
			<%} else { %>
			<%: Html.ActionLink(catalog.CatalogName, MVC.CatalogManagement.Detail(catalog.CatalogId), new { @class = "cw-catalog-detail-link" })%>
			<%} %>
		</td>
	</tr>
	<% } %>