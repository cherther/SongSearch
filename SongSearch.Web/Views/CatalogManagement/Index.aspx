<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.CatalogViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.PageTitle %>
</asp:Content>
<asp:Content id="subNav" ContentPlaceHolderID="SuvNavContent" runat="server">
<%ViewData["SubMenuLocation"] = "CatalogManagement";%>
<%: Html.Partial(MVC.Shared.Views.ctrlAdminMenu) %>
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
				<%: Html.Partial(MVC.Shared.Views.ctrlUserBalancesWidget, Account.User().MyBalances()) %>
			<%} %>
		</div>
	</div>
	<%if (Model != null) {
	   
	%>
	<div class="nine_column section">
		<div class="two column text-top" style="padding-right: 15px;">
			<%if (Model.MyCatalogs.Count() > 0) { %>
			<div class = "cw-outl-thick cw-fill cw-padded cw-rounded-corners">
				
				<%: Html.Partial(MVC.CatalogManagement.Views.ctrlCatalogList, Model) %>
				
			</div>
			<%} else { %>
			<div>You do not have any catalogs yet.</div>
			<div>&nbsp;</div>
			<div>Get started with our <%: Html.ActionLink("Catalog Upload Wizard", MVC.CatalogUpload.Upload()) %>.</div>
			<div>&nbsp;</div>
			<%} %>
		</div>
		<div class="seven column text-top">
			<div id="cw-catalog-detail" class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners" style="display: none">
			
			</div>
			<%
	   
				var model = new ContentListViewModel() {
					List = Model.CatalogContents,
					ListHeaders = new string[] { "Title", "Artist", "Catalog", "Created On", "Created By" },
					HeaderActions = new GridAction[] { },
					GridActions = new GridAction[] { GridAction.ShowDetails },
					IsSortable = true
				};
			%>
			<%: Html.Partial(MVC.CatalogManagement.Views.ctrlLatestContentsTable, model) %>	
		</div>
	</div>
	<%} %>
</div>
</asp:Content>