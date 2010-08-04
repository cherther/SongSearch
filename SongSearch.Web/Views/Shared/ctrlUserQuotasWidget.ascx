<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.Data.UserQuotas>" %>
<div class="cw-outl-orange cw-fill-orange cw-padded cw-rounded-corners" style="width:280px; float:right">
	<div class="cw-usage-heading-small">My Plan Usage</div>
	<%: Html.Partial(MVC.Shared.Views.ctrlUserQuotaSmall, Model.NumberOfSongs) %>
	<%: Html.Partial(MVC.Shared.Views.ctrlUserQuotaSmall, Model.NumberOfInvitedUsers) %>
	
	<%if (Model.NumberOfCatalogAdmins.Allowed.HasValue) {%>
	<%: Html.Partial(MVC.Shared.Views.ctrlUserQuotaSmall, Model.NumberOfCatalogAdmins) %>
	<%} %>
</div>