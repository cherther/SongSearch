<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.ResetPasswordModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Reset Password
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded cw-rounded-corners-bottom">

	<h2>Reset Password</h2>
	<div>&nbsp;</div>
	<div class="cw-outl cw-padded cw-rounded-corners">
	<p>
		Please enter your e-mail address and we will send you a link to help you select a new password. Please <%= Html.ActionLink("register", "Register") %> if you don't have an account.
	</p>
	 <%= Html.ValidationSummary("Password reset request was unsuccessful. Please correct the errors and try again.") %>
	<% using (Html.BeginForm(MVC.Account.ResetPassword(), FormMethod.Post, new { @class = "cw-form-small" }))
	   {%>
	   <%=Html.AntiForgeryToken() %>
		<div>&nbsp;</div>
		<div class="editor-label">
			<%= Html.LabelFor(model => model.Email) %>
		</div>
		<div class="editor-field">
			<%= Html.TextBoxFor(model => model.Email, new { @class = "cw-field-large", placeholder="Your email address", required=true })%>
			<%= Html.ValidationMessageFor(model => model.Email) %>
		</div>
			<p>&nbsp;</p>
		<p>
			<button type="submit" class="cw-button cw-simple cw-blue">
				<span class="b-email">Request Password Reset</span>
				</button>
		</p>
	<% } %>
</div>
</div>

</asp:Content>


