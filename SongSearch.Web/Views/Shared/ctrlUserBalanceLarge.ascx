<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.Balance>" %>
<%
	decimal tipping = 0.8000M;
	var perc = Model.Usage.ToPercentDescription();
	var usageClass = Model.Usage == 0 ? " cw-usage-none" : "";
	var usageBarClass = Model.Usage <= tipping ? " cw-usage-good" : " cw-usage-danger";
	
	%>

<div class="two column">
	<div class="cw-usage-numbers"><%: Model.Used.ToDescription()%>/<%: Model.Allowed.ToBalanceDescription()%></div>
	<div class="cw-usage-item"><%: Model.BalanceName%></div>
	<div title="<%: Model.UsageDescription %>" class="cw-usage-box cw-bar-round<%: usageClass %>">
		<span class="cw-usage-bar cw-bar-round<%: usageBarClass %>" style="width:<%: perc %>"><%: perc %></span> 
	</div>
</div>