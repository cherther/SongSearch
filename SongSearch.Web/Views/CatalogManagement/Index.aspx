<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.CatalogViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.PageTitle %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SuvNavContent" runat="server">
<%
	IDictionary<string, string[]> menu = new Dictionary<string, string[]>();

	menu.Add("Manage Users", new string[4] { "Index", "UserManagement", "Admin", "" });
	menu.Add("Manage Catalogs", new string[4] { "Index", "CatalogManagement", "Admin", "current" });
	menu.Add("Catalog Upload", new string[4] { "Upload", "CatalogUpload", "Admin", "" });
	menu.Add("Invite", new string[4] { "Invite", "UserManagement", "Admin", "" });

	Html.RenderPartial(MVC.Shared.Views.ctrlSubMenu, menu);
		
%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded cw-rounded-corners-bottom">
	<h2><%: Model.PageTitle %></h2>

	<%if (Model != null) {
		var catalogs = Model.MyCatalogs;
		catalogs = catalogs.OrderBy(c => c.CatalogName).ToList();
	%>
	<div>&nbsp;</div>
	<h3>Catalogs</h3>
	<div>&nbsp;</div>
	<table>
	<tr>
		<td style="vertical-align: top">
			<%if (catalogs.Count() > 0) { %>
			<div class = "cw-outl cw-padded cw-rounded-corners" style="overflow:auto ; height: 480px; width: 300px;">
				<table id="catalog-list" class="cw-tbl-cat">
					<% Html.RenderPartial(MVC.CatalogManagement.Views.ctrlCatalogList, catalogs); %>
				</table>
			</div>
			<%} else { %>
			<div>You do not have any catalogs yet.</div>
			<div>&nbsp;</div>
			<div>Get started with our <%: Html.ActionLink("Catalog Upload Wizard", MVC.CatalogUpload.Upload()) %>.</div>
			<div>&nbsp;</div>
			<%} %>
		</td>
		<td style="vertical-align: top">
			<div id="cw-catalog-detail" class="cw-outl cw-padded cw-rounded-corners" style="display: none">
			</div>
		</td>
	</tr>
	</table>
	<%} %>
</div>
</asp:Content>