<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Print.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.ContentViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
<% var content = Model.Content;%>
'<%: content.Title%>' - <%: content.Artist%>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Javascripts" runat="server">
<script type="text/javascript">
$(document).ready(function () {

	window.print();

}
);
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class = "cw-outl">
	<% Html.RenderPartial(MVC.Content.Views.ctrlContentDetail, Model); %>
   </div>
</asp:Content>
