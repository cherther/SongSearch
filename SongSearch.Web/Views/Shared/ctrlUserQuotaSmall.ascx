<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.Data.Quota>" %>
<%
	decimal tipping = 0.8000M;
	var perc = Model.Usage.ToPercentDescription();
	var usageClass = Model.Usage == 0 ? " cw-usage-none" : "";
	var usageBarClass = Model.Usage <= tipping ? " cw-usage-good" : " cw-usage-danger";
	
	%>
	<span style="padding: 3px">
	<span class="cw-usage-item-small">
	<%: Model.QuotaName %> - 
	</span>
	<span class="cw-usage-numbers-small">
	<%: Model.Used.ToDescription()%>/<%: Model.Allowed.ToQuotaDescription()%>
	</span>
	</span>