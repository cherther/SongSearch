<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.UserBalances>" %>
<div class="cw-outl-orange cw-padded cw-rounded-corners">
	<div class="six_column section">
		<%: Html.Partial(MVC.Shared.Views.ctrlUserBalanceLarge, Model.NumberOfSongs) %>
		<%: Html.Partial(MVC.Shared.Views.ctrlUserBalanceLarge, Model.NumberOfInvitedUsers) %>
		<%: Html.Partial(MVC.Shared.Views.ctrlUserBalanceLarge, Model.NumberOfCatalogAdmins) %>
	</div>
</div>