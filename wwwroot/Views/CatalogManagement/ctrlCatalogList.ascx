<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<SongSearch.Web.Data.Catalog>>" %>
<%
	var catalogs = Model.OrderBy(c => c.CatalogName);
%>

<% foreach (var catalog in catalogs)
   {
	   var rowId = String.Concat("catalog-", catalog.CatalogId.ToString());
	   //var rowClass = user.ParentUserId.HasValue ? String.Concat("c-", user.ParentUserId.ToString()) : "";
	   var rowClass = String.Concat("c-", catalog.CatalogId.ToString());

	   //var childClass = user.ParentUserId.HasValue ? String.Concat("children-", user.UserId.ToString()) : "";
	   %>

    <tr id="<%: rowId%>" class="cw-user-listing <%: rowClass%>">
		<td>-</td>
		<td>
		    <%: Html.ActionLink(catalog.CatalogName, MVC.CatalogManagement.Detail(catalog.CatalogId), new { @class = "cw-catalog-detail-link" })%>
		</td>
	</tr>
<% } %>
