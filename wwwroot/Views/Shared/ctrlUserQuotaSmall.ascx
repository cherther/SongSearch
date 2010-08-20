<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.Data.Quota>" %>
<%
	decimal tipping = 0.8000M;
	var perc = Model.Usage.ToPercentDescription();
	var usageClass = Model.Usage == 0 ? " cw-usage-none" : "";
	var usageBarClass = //Model.Allowed.HasValue ? 
		(Model.Usage <= tipping ? " cw-usage-good" : " cw-usage-danger"); //:
		//"cw-usage-none";
	var title = String.Format("You have {0} {1} remaining", Model.Remaining.ToQuotaLongDescription(), Model.QuotaName);
	%>
	<div style="padding: 3px; display: inline; cursor: help;">
	<div class="cw-usage-item-small" style="display: inline;"><%: Model.QuotaName %></div>
	<div class="cw-usage-numbers-small<%: usageBarClass %> cw-bar-round" style="display: inline;" title="<%: title%>">
	<%: Model.Remaining.ToQuotaDescription()%>
	</div>
	</div>