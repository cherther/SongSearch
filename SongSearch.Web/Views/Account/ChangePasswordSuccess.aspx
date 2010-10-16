<%@Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.ViewModel>" %>

<asp:Content ID="updateProfileTitle" ContentPlaceHolderID="TitleContent" runat="server">
	Update Profile
</asp:Content>
<asp:Content id="subNav" ContentPlaceHolderID="SuvNavContent" runat="server">
<%ViewData["SubMenuLocation"] = "ChangePassword";%>
<%: Html.Partial(MVC.Shared.Views.ctrlAccountMenu)%>
 
</asp:Content>
<asp:Content ID="updateProfileSuccessContent" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded cw-rounded-corners-bottom">
	<h2>Change Password</h2>
	<div>&nbsp;</div>
	<p>
		Your password has been changed.
	</p>
	</div>
</asp:Content>
