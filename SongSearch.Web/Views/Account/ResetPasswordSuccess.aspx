<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.ResetPasswordModel>" %>

<asp:Content ID="resetPasswordTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Reset Password
</asp:Content>

<asp:Content ID="resetPasswordSuccessContent" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded cw-rounded-corners-bottom">

    <h2>Reset Password</h2>
	<div>&nbsp;</div>
	<div class="cw-outl cw-padded cw-rounded-corners">
    <p>
        Please check your email inbox for an email to help you reset your password.
    </p>
    </div>
    </div>
</asp:Content>
