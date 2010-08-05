<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.Data.UserQuotas>" %>
<div class="cw-outl-orange cw-fill-orange cw-rounded-corners" style="width:280px; float:right">
	<div class="cw-usage-heading-small"><%: Html.ActionLink("My Plan", MVC.Account.Plan(), new { title="See your Plan and upgrade options"})%> - Remaining</div>
	<div class="text-middle">
	<%: Html.Partial(MVC.Shared.Views.ctrlUserQuotaSmall, Model.NumberOfSongs) %>
	<%: Html.Partial(MVC.Shared.Views.ctrlUserQuotaSmall, Model.NumberOfInvitedUsers) %>
	<%//if (Model.NumberOfCatalogAdmins.Allowed.HasValue) {%>
	<%: Html.Partial(MVC.Shared.Views.ctrlUserQuotaSmall, Model.NumberOfCatalogAdmins) %>
	<%//} %>
	</div>
</div>