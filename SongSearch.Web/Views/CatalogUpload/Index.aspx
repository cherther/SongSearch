<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.CatalogUploadViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Upload
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="SuvNavContent" runat="server">
<%
	IDictionary<string, string[]> menu = new Dictionary<string, string[]>();

	menu.Add("Manage Users", new string[4] { "Index", "UserManagement", "Admin", "" });
	menu.Add("Manage Catalogs", new string[4] { "Index", "CatalogManagement", "Admin", "" });
	menu.Add("Catalog Upload", new string[4] { "Upload", "CatalogUpload", "Admin", "current" });
	menu.Add("Invite", new string[4] { "Invite", "UserManagement", "Admin", "" });

	Html.RenderPartial(MVC.Shared.Views.ctrlSubMenu, menu);
		
%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded cw-rounded-corners-bottom">
</div>
</asp:Content>
