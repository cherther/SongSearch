<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Print.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.SearchViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Results
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

   <div id="cw-search-results-panel">
		<% Html.RenderPartial(MVC.Search.Views.ctrlSearchResults); %>
	</div>
</asp:Content>
