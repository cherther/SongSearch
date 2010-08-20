<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.SearchViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Results
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-rounded-corners-bottom">
	
	<div class="nine_column section">
	
		<div class="two column" style="min-width:220px!important;">
			<!--div id="cw-search-options-panel"-->
			<% Html.RenderPartial(MVC.Search.Views.ctrlSearchOptions); %>
			<!--/div-->
		</div>
		<div class="seven column">
			<div class="cw-outl cw-padded cw-rounded-corners cw-buffered-left">
				<% Html.RenderPartial(MVC.Search.Views.ctrlSearchResults); %>
			</div>
		</div>
	</div>
</div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SuvNavContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Javascripts" runat="server">
</asp:Content>

