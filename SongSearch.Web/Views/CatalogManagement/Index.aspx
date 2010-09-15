<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.CatalogViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.PageTitle %>
</asp:Content>
<asp:Content id="subNav" ContentPlaceHolderID="SuvNavContent" runat="server">
<%
ViewData["SubMenuLocation"] = "CatalogManagement";
Html.RenderPartial(MVC.Shared.Views.ctrlAdminMenu);
%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-fill-lite cw-padded cw-rounded-corners-bottom">
	<div class="six_column section">
	<div class="three column text-top">
			<h2><%: Model.PageTitle %></h2>
			<div>&nbsp;</div>
			<h3>Catalogs</h3>
			<div>&nbsp;</div>
		</div>
		<div class="three column text-top">
			<%if (App.IsLicensedVersion) {%>
				<%: Html.Partial("ctrlUserQuotasWidget", Account.User().MyQuotas()) %>
			<%} %>
		</div>
	</div>
	<%if (Model != null) {
	   var catsOwned = Model.MyCatalogs.Where(c => c.CreatedByUserId == Model.ActiveUserId).ToList();
	   var catsOther = Model.MyCatalogs.Where(c => c.CreatedByUserId != Model.ActiveUserId).ToList();
	   
	%>
	
	<table>
	<tr>
		<td style="vertical-align: top">
			<%if (Model.MyCatalogs.Count() > 0) { %>
			<div class = "cw-outl-thick cw-fill-white cw-padded cw-rounded-corners" style="overflow:auto ; height: 480px; width: 300px;">
				<table id="catalog-list" class="cw-tbl-cat">
					<%if (catsOwned.Count() > 0) {%><tr>
					<td colspan="2">
					Created by you:
					</td>
					</tr>
					<% Html.RenderPartial(MVC.CatalogManagement.Views.ctrlCatalogList, catsOwned); %>
					<%} %>
					<%if (catsOther.Count() > 0) {%>
					<tr>
					<td colspan="2">
					Administered by you:
					</td>
					</tr>
					<% Html.RenderPartial(MVC.CatalogManagement.Views.ctrlCatalogList, catsOther); %>
					<%} %>
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
			<div id="cw-catalog-detail" class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners" style="display: none">
			</div>
		</td>
	</tr>
	</table>
	<%} %>
</div>
</asp:Content>