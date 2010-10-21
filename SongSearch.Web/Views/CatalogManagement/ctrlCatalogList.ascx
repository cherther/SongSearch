<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.CatalogViewModel>" %>
<%
	
	var catsOwned = Model.MyCatalogs.Where(c => c.CreatedByUserId == Model.ActiveUserId).ToList();
	var catsOther = Model.MyCatalogs.Where(c => c.CreatedByUserId != Model.ActiveUserId).ToList();
	   
%>
<table id="catalog-list" class="cw-tbl-cat">
	<%if (catsOwned.Count() > 0) {%><tr>
	<td colspan="2">
	Created by you:
	</td>
	</tr>
	<% 
	   var m = Model;
	   m.MyCatalogs = catsOwned; 
	%>
	<%: Html.Partial("ctrlCatalogListItem", m) %>

	<%} %>
	<%if (catsOther.Count() > 0) {%>
	<tr>
	<td colspan="2">
	Administered by you:
	</td>
	</tr>
	<% 
	   var m = Model;
	   m.MyCatalogs = catsOther; 
	%>
	<%: Html.Partial("ctrlCatalogListItem", m) %>

	<%} %>
</table>
