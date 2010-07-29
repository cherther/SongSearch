<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.PricingPlanModel>" %>

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
<div id="content" class="cw-outl cw-padded cw-rounded-corners-bottom">
	<h2><%: Model.PageTitle %></h2>
	<div>&nbsp;</div>
	<h3>Usage</h3>
	<div>&nbsp;</div>
	<%: Html.Partial("ctrlUserQuotasWide", Model.MyUserQuotas) %>
	<div>&nbsp;</div>
	<%
		ViewData["PricingPlan"] = Model.MyPricingPlan;	
	%>
	<%: Html.Partial("ctrlPricingPlans", Model.PricingPlans) %>
</div>
</asp:Content>
