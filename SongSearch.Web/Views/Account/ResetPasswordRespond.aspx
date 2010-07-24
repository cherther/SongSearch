<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.ResetPasswordModel>" %>

<asp:Content ID="UpdateProfileTitle" ContentPlaceHolderID="TitleContent" runat="server">
	Reset Password
</asp:Content>
<asp:Content id="subNav" ContentPlaceHolderID="SuvNavContent" runat="server">
<%
	IDictionary<string, string[]> menu = new Dictionary<string, string[]>();

	menu.Add("Your Profile", new string[4] { "UpdateProfile", "Account", "", "current" });
	
	menu.Add("Log Out", new string[4] { "LogOut", "Account", "", "" });

//    menu.Add("My Playlists", new string[4] { "Index", "List", "", "" });
//    menu.Add("Add New", new string[4] { "Create", "Song", "", "" });

	Html.RenderPartial(MVC.Shared.Views.ctrlSubMenu, menu);
		
	%>
	
</asp:Content>
<asp:Content ID="resetPasswordContent" ContentPlaceHolderID="MainContent" runat="server">
<% Html.EnableClientValidation(); %>
<div id="content" class="cw-outl cw-padded cw-rounded-corners-bottom">

	<h2>Reset Password</h2>
	<div>&nbsp;</div>
	<div class="cw-outl cw-padded cw-rounded-corners">
	<p>
		Use the form below to reset your password. 
	</p>
	<p>
		New passwords are required to be a minimum of <%: ViewData["PasswordLength"] %> characters in length.
	</p>
	<%: Html.ValidationSummary("Password reset was unsuccessful. Please correct the errors and try again.")%>

	<% using (Html.BeginForm(MVC.Account.ResetPasswordRespond(), FormMethod.Post, new { @class = "cw-form-small" }))
	   {%><%:Html.AntiForgeryToken() %>
			<%: Html.HiddenFor(m => m.ResetCode) %>
			<div>
				<%: Html.LabelFor(m => m.Email) %>
			</div>
			<div>
				<%: Html.TextBoxFor(m => m.Email, new { @class = "cw-field-large", placeholder = "Your e-mail address", required = true })%>
				<%: Html.ValidationMessageFor(m => m.Email)%>
			</div>

			<div>
				<%: Html.LabelFor(m => m.NewPassword) %>
			</div>
			<div>
				<%: Html.PasswordFor(m => m.NewPassword, new { @class = "cw-field-large", placeholder = "New password", required = true })%>
				<%: Html.ValidationMessageFor(m => m.NewPassword) %>
			</div>
				
			<div>
				<%: Html.LabelFor(m => m.ConfirmPassword) %>
			</div>
			<div>
				<%: Html.PasswordFor(m => m.ConfirmPassword, new { @class = "cw-field-large" })%>
				<%: Html.ValidationMessageFor(m => m.ConfirmPassword) %>
			</div>
				
			<p>
				<button type="submit" class="cw-button cw-simple cw-blue">
				<span class="b-ok">Reset Password</span>
				</button>
			</p>
	<% } %>
	</div>

</div>    
</asp:Content>
