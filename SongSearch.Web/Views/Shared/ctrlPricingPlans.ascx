<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PricingPlansViewModel>" %>
<%
	var plans = Model.PricingPlans;
	
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
	<% if(!plan.IsAvailableToUser){%>
	<span style="color:red;">
	<%} %>
	<%: plan.PricingPlanName %>
	<% if(!plan.IsAvailableToUser){%>
	</span>
	<%} %>
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
	<td class="text-center <%: myPlanClass %>"><%: Model.MyPricingPlan.NumberOfSongs.ToBalanceDescription()%></td>
	<%} %>
	<%foreach (var plan in plans) { %>
	<td class="text-center <%: plan.PlanDisplayClass() %>">
	<%: plan.NumberOfSongs.ToBalanceDescription() %>
	
	</td>
	<%} %>
</tr>
<tr>
	<td>
	<label>Invited Users</label>
	</td>
	<%if (Model.MyPricingPlan != null) {%>
	<td class="text-center <%: myPlanClass %>"><%: Model.MyPricingPlan.NumberOfInvitedUsers.ToBalanceDescription()%></td>
	<%} %>
	<%foreach (var plan in plans) { %>
	<td class="text-center <%: plan.PlanDisplayClass() %>">
		<%: plan.NumberOfInvitedUsers.ToBalanceDescription() %>
	</td>
	<%} %>
</tr>
<tr>
	<td>
	<label>Catalog Administrators</label>
	</td>
	<%if (Model.MyPricingPlan != null) {%>
	<td class="text-center <%: myPlanClass %>"><%: Model.MyPricingPlan.NumberOfCatalogAdmins.ToBalanceDescription()%></td>
	<%} %>
	<%foreach (var plan in plans) { %>
	<td class="text-center <%: plan.PlanDisplayClass() %>">
		<%: plan.NumberOfCatalogAdmins.ToBalanceDescription() %>
	</td>
	<%} %>
</tr>
<tr>
	<td class="label">
	<label>Personalized Contact Information</label>
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
	<label>Your Branded Website</label>
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
		<%if (Model.ShowChangeButton && plan.IsAvailableToUser) { %>
			<a href="#" class="cw-button cw-simple cw-medium cw-green" title="Coming soon!">Change Plan</a>
		<%} %>
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