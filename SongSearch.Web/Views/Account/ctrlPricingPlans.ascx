<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<SongSearch.Web.Data.PricingPlan>>" %>
<%
int pricingPlan = 0;
int.TryParse(ViewData["PricingPlan"].ToString(), out pricingPlan);
var plans = Model.OrderByDescending(p => p.IsPromo);
var width = (100 - 25) / plans.Count();
%>
<table id="cw-tbl-plans">
<tr>
	<th style="width: 25%"></th>
	<%foreach (var plan in plans) { %>
	<th class="text-center <%: plan.PlanDisplayClass() %>">
	<%: plan.PricingPlanName %>
	</th>
	<%} %>
</tr>
<tr>
	<td>
	
	</td>
	<%foreach (var plan in plans) { %>
	<td class="text-center <%: plan.PlanDisplayClass() %>"><em><%: plan.PromoMessage %></em></td>
	<%} %>
</tr><tr>
	<td>
	<label>Monthly Charge</label>
	</td>
	<%foreach (var plan in plans) { %>
	<td class="text-center <%: plan.PlanDisplayClass() %>"><%: plan.PlanCharge > 0 ? plan.PlanCharge.ToString("C") : "FREE!" %></td>
	<%} %>
</tr>
<tr>
	<td>
	<label>Songs</label>
	</td>
	<%foreach (var plan in plans) { %>
	<td class="text-center <%: plan.PlanDisplayClass() %>"><%: plan.NumberOfSongs > 0 ? plan.NumberOfSongs.ToString("N0") : "Unlimited" %></td>
	<%} %>
</tr>
<tr>
	<td>
	<label>Invited Users</label>
	</td>
	<%foreach (var plan in plans) { %>
	<td class="text-center <%: plan.PlanDisplayClass() %>"><%: plan.NumberOfInvitedUsers > 0 ? plan.NumberOfInvitedUsers.ToString("N0") : "Unlimited"%></td>
	<%} %>
</tr>
<tr>
	<td>
	<label>Catalog Administrators</label>
	</td>
	<%foreach (var plan in plans) { %>
	<td class="text-center <%: plan.PlanDisplayClass() %>"><%: plan.NumberOfCatalogAdmins > 0 ? plan.NumberOfCatalogAdmins.ToString("N0") : "Unlimited"%></td>
	<%} %>
</tr>
<tr>
	<td class="label">
	<label>Custom Contact Information</label>
	</td>
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
	<%foreach (var plan in plans) { %>
	<td class="text-center <%: plan.PlanDisplayClass() %>">
	<%if (plan.CustomSiteProfile) {%>
	<img src="../../Public/Images/Icons/Silk/tick.png" alt="Yes" title="Custom Site Profile"/>	
	<%} %>
	</td>
	<%} %>
</tr>
<tr>
	<td>
	&nbsp;
	</td>
	<%foreach (var plan in plans) { %>
	<td class="text-center <%: plan.PlanDisplayClass() %>">
	<%if (plan.IsEnabled) { %>
	<%: Html.RadioButton("PricingPlan", plan.PricingPlanId,
	ViewData["PricingPlan"].ToString() == plan.PricingPlanId.ToString())%>
	<%} else { %>
	<%: Html.RadioButton("PricingPlan", plan.PricingPlanId, false, new { disabled = "true" })%>
	<%} %>
	</td>
	<%} %>
</tr>

</table>