<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PricingPlansViewModel>" %>
<%
//int selectPricingPlan = 0;
//int.TryParse(ViewData["SelectPricingPlan"].ToString(), out selectPricingPlan);
var plans = (Model.MyPricingPlan != null ?
    Model.PricingPlans.Where(p => p.PricingPlanId != Model.MyPricingPlan.PricingPlanId) :
				Model.PricingPlans)
	.OrderBy(p => p.PlanCharge).ThenBy(p => p.PricingPlanId);//.Where(p => p.ShowOnSite == true).OrderByDescending(p => p.IsPromo);
	
var width = (100 - 25) / plans.Count();
var myPlanClass = "cell-highlight-blue cell-border-blue";
%>
<table id="cw-tbl-plans">
<tr>
	<th style="width: 25%"></th>
	<%if (Model.MyPricingPlan != null) {%>
	<th class="text-center <%: myPlanClass %>">
	<%: Model.MyPricingPlan.PricingPlanName%>
	</th>
	<%} %>
	<%foreach (var plan in plans) { %>
	<th class="text-center <%: plan.PlanDisplayClass() %>">
	<%: plan.PricingPlanName %>
	</th>
	<%} %>
</tr>
<tr>
	<td>
	
	</td>
	<%if (Model.MyPricingPlan != null) {%>
	<td class="text-center <%: myPlanClass %>">
	<em><%: Model.MyPricingPlan.PromoMessage%></em>
	</td>
	<%} %>
	<%foreach (var plan in plans) { %>
	<td class="text-center <%: plan.PlanDisplayClass() %>"><em><%: plan.PromoMessage %></em></td>
	<%} %>
</tr>
<tr>
	<td>
	<label>Monthly Charge</label>
	</td>
	<%if (Model.MyPricingPlan != null) {%>
	<td class="text-center <%: myPlanClass %>"><%: Model.MyPricingPlan.PlanCharge.ToPriceDescription()%></td>
	<%} %>
	<%foreach (var plan in plans) { %>
	<td class="text-center <%: plan.PlanDisplayClass() %>"><%: plan.PlanCharge.ToPriceDescription() %></td>
	<%} %>
</tr>
<tr>
	<td>
	<label>Songs</label>
	</td>
	<%if (Model.MyPricingPlan != null) {%>
	<td class="text-center <%: myPlanClass %>"><%: Model.MyPricingPlan.NumberOfSongs.ToQuotaDescription()%></td>
	<%} %>
	<%foreach (var plan in plans) { %>
	<td class="text-center <%: plan.PlanDisplayClass() %>"><%: plan.NumberOfSongs.ToQuotaDescription() %></td>
	<%} %>
</tr>
<tr>
	<td>
	<label>Invited Users</label>
	</td>
	<%if (Model.MyPricingPlan != null) {%>
	<td class="text-center <%: myPlanClass %>"><%: Model.MyPricingPlan.NumberOfInvitedUsers.ToQuotaDescription()%></td>
	<%} %>
	<%foreach (var plan in plans) { %>
	<td class="text-center <%: plan.PlanDisplayClass() %>"><%: plan.NumberOfInvitedUsers.ToQuotaDescription() %></td>
	<%} %>
</tr>
<tr>
	<td>
	<label>Catalog Administrators</label>
	</td>
	<%if (Model.MyPricingPlan != null) {%>
	<td class="text-center <%: myPlanClass %>"><%: Model.MyPricingPlan.NumberOfCatalogAdmins.ToQuotaDescription()%></td>
	<%} %>
	<%foreach (var plan in plans) { %>
	<td class="text-center <%: plan.PlanDisplayClass() %>"><%: plan.NumberOfCatalogAdmins.ToQuotaDescription() %></td>
	<%} %>
</tr>
<tr>
	<td class="label">
	<label>Custom Contact Information</label>
	</td>
	<%if (Model.MyPricingPlan != null) {%>
	<td class="text-center <%: myPlanClass %>">
	<%if (Model.MyPricingPlan.CustomContactUs) {%>
	<img src="../../Public/Images/Icons/Silk/tick.png" alt="Yes" title="Custom Contact Us Information"/>	
	<%} %>
	</td>
	<%} %>
	<%foreach (var plan in plans) { %>
	<td class="text-center <%: plan.PlanDisplayClass() %>">
	<%if (plan.CustomContactUs) {%>
	<img src="../../Public/Images/Icons/Silk/tick.png" alt="Yes" title="Custom Contact Us Information"/>	
	<%} %>
	</td>
	<%} %>
</tr>
<tr>
	<td>
	<label>Custom Website Theme</label>
	</td>
	<%if (Model.MyPricingPlan != null) {%>
	<td class="text-center <%: myPlanClass %>">
	<%if (Model.MyPricingPlan.CustomSiteProfile) {%>
	<img src="../../Public/Images/Icons/Silk/tick.png" alt="Yes" title="Custom Site Profile"/>	
	<%} %>
	</td>
	<%} %>
	<%foreach (var plan in plans) { %>
	<td class="text-center <%: plan.PlanDisplayClass() %>">
	<%if (plan.CustomSiteProfile) {%>
	<img src="../../Public/Images/Icons/Silk/tick.png" alt="Yes" title="Custom Site Profile"/>	
	<%} %>
	</td>
	<%} %>
</tr>
<tr>
	<td class="cell-border-none">
	&nbsp;
	</td>
	<% if (Model.MyPricingPlan != null) { %>
	<td class="text-center cell-border-blue-closer <%: myPlanClass %>">
	<strong>My Plan</strong>
	</td>
		<%foreach (var plan in plans) { %>
		<td class="text-center cell-border-closer <%: plan.PlanDisplayClass() %>">
			<a href="#" class="cw-button cw-simple cw-medium cw-green" disabled="disabled">Change Plan</a>
		</td>
		<%} %>
	<%} else {%>
		<%foreach (var plan in plans) { %>
		<td class="text-center cell-border-closer <%: plan.PlanDisplayClass() %>">
			<%if (plan.IsEnabled) { %>
			<%: Html.RadioButton("SelectedPricingPlan", plan.PricingPlanId, (int)Model.SelectedPricingPlan == plan.PricingPlanId)%>
			<%: (int)Model.SelectedPricingPlan == plan.PricingPlanId ? "Yes!" : ""%>
			<%} else { %>
			<%: Html.RadioButton("SelectedPricingPlan", plan.PricingPlanId, false, new { disabled = "true" })%>
			<%} %>
		</td>
		<%} %>
	<%} %>
</tr>

</table>