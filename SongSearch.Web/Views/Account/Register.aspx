<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.RegisterModel>" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
	Register
</asp:Content>
<asp:Content id="subNav" ContentPlaceHolderID="SuvNavContent" runat="server">
<%
	IDictionary<string, string[]> menu = new Dictionary<string, string[]>();
	menu.Add("Register", new string[4] { "Register", "Account", "", "current" });

//    menu.Add("My Playlists", new string[4] { "Index", "List", "", "" });
//    menu.Add("Add New", new string[4] { "Create", "Song", "", "" });

	Html.RenderPartial(MVC.Shared.Views.ctrlSubMenu, menu);
		
	%>
	
</asp:Content>
<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded cw-rounded-corners-bottom">
	<h2>Register</h2>
	<div>&nbsp;</div>
	<div class="cw-outl cw-padded cw-rounded-corners">
	<p>
		Use the form below to create a new account using the registration we sent to you. Or, <%= Html.ActionLink("log in", "LogIn") %> if you already have an account.
	</p>
	<p>
	If you'd like to receive an invitation code, please <%: Html.ActionLink("contact us", MVC.Home.Contact()) %>.
	</p>
	<p>
		Passwords are required to be a minimum of <%=Html.Encode(ViewData["PasswordLength"])%> characters in length.
	</p>
	<div>&nbsp;</div>
	<%//= Html.ValidationSummary("Account creation was unsuccessful. Please correct the errors and try again.") %>
	<% using (Html.BeginForm(MVC.Account.Register(), FormMethod.Post, new { @class = "cw-form-small" }))
	   { %><%=Html.AntiForgeryToken() %>
	
		
			<fieldset>
				<div>&nbsp;</div>
				<div class="cw-fe-vert cw-fe-req">
					<%= Html.LabelFor(m => m.Email) %>
				</div>
				<div class="cw-fe-vert">
					<%= Html.TextBoxFor(m => m.Email, new { @class = "cw-field-large", placeholder="Your email address", required=true, title = "This should be the e-mail address<br/>where you received your registration invitation" })%>
					<%= Html.ValidationMessageFor(m => m.Email)%>
				</div>
				<div>&nbsp;</div>
				<div class="cw-fe-vert cw-fe-req">
					<%= Html.LabelFor(m => m.InviteId)%>
				</div>
				<div class="cw-fe-vert">
					<%= Html.TextBoxFor(m => m.InviteId, new { @class = "cw-field-xlarge", placeholder = "Your invitation code", required = true, title = "The invitation code we e-mailed you" })%>
					<%= Html.ValidationMessageFor(m => m.InviteId)%>
				</div>                    
				<div>&nbsp;</div>
				 <div class="cw-fe-vert">
					<%= Html.LabelFor(m => m.FirstName)%>
				</div>
				<div class="cw-fe-vert">
					<%= Html.TextBoxFor(m => m.FirstName, new { @class = "cw-field-large", placeholder="First Name" })%>
					<%= Html.ValidationMessageFor(m => m.FirstName)%>
				</div>                    
				<div>&nbsp;</div>
				 <div class="cw-fe-vert">
					<%= Html.LabelFor(m => m.LastName)%>
				</div>
				<div class="cw-fe-vert">
					<%= Html.TextBoxFor(m => m.LastName, new { @class = "cw-field-large", placeholder = "Last Name" })%>
					<%= Html.ValidationMessageFor(m => m.LastName)%>
				</div>                    
				<div>&nbsp;</div>
				<div class="cw-fe-vert cw-fe-req">
					<%= Html.LabelFor(m => m.Password) %>
				</div>
				<div class="cw-fe-vert">
					<%= Html.PasswordFor(m => m.Password, new { @class = "cw-field-large", placeholder="Password", required=true, title = "Please enter a secure password<br />with at least 6 characters" })%>
					<%= Html.ValidationMessageFor(m => m.Password)%>
				</div>                    
				<div>&nbsp;</div>
				<div class="cw-fe-vert cw-fe-req">
					<%= Html.LabelFor(m => m.ConfirmPassword) %>
				</div>
				<div class="cw-fe-vert">
					<%= Html.PasswordFor(m => m.ConfirmPassword, new { @class = "cw-field-large", required=true, title="Please enter your password again" })%>
					<%= Html.ValidationMessageFor(m => m.ConfirmPassword)%>
				</div>                    
				<div>&nbsp;</div>
				<p>
					<button type="submit" class="cw-button cw-simple cw-blue" title="Click to Register">
					<span class="b-ok">Register</span>
					</button>
				</p>
			</fieldset>
	<% } %>
	</div>
</div>    
</asp:Content>
