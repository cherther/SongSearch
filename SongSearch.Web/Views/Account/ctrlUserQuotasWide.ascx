<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.Data.UserQuotas>" %>
<div class="cw-outl-orange cw-padded cw-rounded-corners">
	<div class="six_column section">
	<%
		decimal tipping = 0.8000M;
		var perc = Model.NumberOfSongs.Usage.ToPercentDescription();
		var title = Model.NumberOfSongs.UsageDescription("Songs");
		var usageClass = Model.NumberOfSongs.Usage == 0 ? " cw-usage-none" : "";
		var usageBarClass = Model.NumberOfSongs.Usage <= tipping ? " cw-usage-good" : " cw-usage-danger";
	%>
		<div class="two column">
			<div class="cw-usage-numbers"><%: Model.NumberOfSongs.Used.ToDescription()%>/<%: Model.NumberOfSongs.Allowed.ToQuotaDescription()%></div>
			<div class="cw-usage-item">Songs</div>
			<div title="<%: title %>" class="cw-usage-box cw-bar-round<%: usageClass %>">
				<span class="cw-usage-bar cw-bar-round<%: usageBarClass %>" style="width:<%: perc %>"><%: perc %></span> 
			</div>
		</div>
		<%
			perc = Model.NumberOfInvitedUsers.Usage.ToPercentDescription();
			title = Model.NumberOfInvitedUsers.UsageDescription("Users");
			usageClass = Model.NumberOfInvitedUsers.Usage == 0 ? " cw-usage-none" : "";
			usageBarClass = Model.NumberOfInvitedUsers.Usage <= tipping ? " cw-usage-good" : " cw-usage-danger";
		%>
		<div class="two column">
			<div class="cw-usage-numbers"><%: Model.NumberOfInvitedUsers.Used.ToDescription()%>/<%: Model.NumberOfInvitedUsers.Allowed.ToQuotaDescription()%></div>
			<div class="cw-usage-item">Users</div>
			<div title="<%: title %>" class="cw-usage-box cw-bar-round<%: usageClass %>">
				<span class="cw-usage-bar cw-bar-round<%: usageBarClass %>" style="width:<%: perc %>"><%: perc %></span> 
			</div>
		</div>
		<%
			perc = perc = Model.NumberOfCatalogAdmins.Usage.ToPercentDescription();
			title = Model.NumberOfCatalogAdmins.UsageDescription("Catalog Administrators");
			usageClass = Model.NumberOfCatalogAdmins.Usage == 0 ? " cw-usage-none" : "";
			usageBarClass = Model.NumberOfCatalogAdmins.Usage <= tipping ? " cw-usage-good" : " cw-usage-danger";
		%>
		<div class="two column">
			<div class="cw-usage-numbers"><%: Model.NumberOfCatalogAdmins.Used.ToDescription()%>/<%: Model.NumberOfCatalogAdmins.Allowed.ToQuotaDescription()%></div>
			<div class="cw-usage-item">Catalog Administrators</div>
			<div title="<%: title %>" class="cw-usage-box cw-bar-round<%: usageClass %>">
				<span class="cw-usage-bar cw-bar-round<%: usageBarClass %>" style="width:<%: perc %>"><%: perc %></span> 
			</div>
		</div>
	</div>
</div>