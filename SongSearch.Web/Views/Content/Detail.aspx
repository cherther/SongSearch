<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.ContentViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Detail
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Javascripts" runat="server">
<script type="text/javascript">
	$(document).ready(function () {
		setupContentPanelUIControls();
	});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class = "cw-outl cw-rounded-corners">
	<% Html.RenderPartial(MVC.Content.Views.ctrlContentDetail, Model); %>
   </div>
</asp:Content>
