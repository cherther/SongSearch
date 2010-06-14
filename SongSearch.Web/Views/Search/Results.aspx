﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.SearchViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Results
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class = "cw-outl">
<table>
	<tr>
	<td align="left" width="200" id="cw-search-options-panel">
		<% Html.RenderPartial(MVC.Search.Views.ctrlSearchOptions); %>
	</td>
	<td  align="left">
		
		<div id="cw-search-results-panel">
		
		<% Html.RenderPartial(MVC.Search.Views.ctrlSearchResults); %>
		</div>
	</td>
	</tr>
</table>
</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SuvNavContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Javascripts" runat="server">
</asp:Content>

