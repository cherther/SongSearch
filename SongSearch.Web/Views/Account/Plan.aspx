<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.PricingPlansViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.PageTitle %>
</asp:Content>
<asp:Content id="subNav" ContentPlaceHolderID="SuvNavContent" runat="server">
<%
ViewData["SubMenuLocation"] = "Plan";
Html.RenderPartial(MVC.Shared.Views.ctrlAccountMenu);
%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%// Html.EnableClientValidation(); %>
<div id="content" class="cw-outl cw-fill-lite cw-padded cw-rounded-corners-bottom">
	<h2><%: Model.PageTitle %></h2>
	<div>&nbsp;</div>
	<div class="cw-outl-thick cw-fill-white cw-padded cw-rounded-corners">
	<h3>Usage</h3>
	<div>&nbsp;</div>
	<%: Html.Partial(MVC.Shared.Views.ctrlUserQuotas, Model.MyUserQuotas) %>
	<div>&nbsp;</div>
	<%: Html.Partial(MVC.Shared.Views.ctrlPricingPlans, Model) %>
	</div>
</div>
</asp:Content>
