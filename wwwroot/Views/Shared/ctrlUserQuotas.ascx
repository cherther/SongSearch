<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.Data.UserQuotas>" %>
<div class="cw-outl-orange cw-padded cw-rounded-corners">
	<div class="six_column section">
		<%: Html.Partial(MVC.Shared.Views.ctrlUserQuotaLarge, Model.NumberOfSongs) %>
		<%: Html.Partial(MVC.Shared.Views.ctrlUserQuotaLarge, Model.NumberOfInvitedUsers) %>
		<%: Html.Partial(MVC.Shared.Views.ctrlUserQuotaLarge, Model.NumberOfCatalogAdmins) %>
	</div>
</div>