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
<% Html.EnableClientValidation(); %>
<div id="content" class="cw-outl cw-padded cw-rounded-corners-bottom">
	<h2>Register</h2>
	<div>&nbsp;</div>
	<div class="cw-outl cw-padded cw-rounded-corners">
	<p>
		Access to <strong><%: Model.SiteProfile.CompanyName %></strong> is by invitation only. Use the form below to create a new account using the invitation code sent to you.
	</p>
	<p>
	If you'd like to receive an invitation code, please <%: Html.ActionLink("contact us", MVC.Home.Contact()) %>.  Or, <%: Html.ActionLink("log in", "LogIn") %> if you already have an account.
	</p>
	<div>&nbsp;</div>
	<%//= Html.ValidationSummary("Account creation was unsuccessful. Please correct the errors and try again.") %>
	<% using (Html.BeginForm(MVC.Account.Register(), FormMethod.Post, new { @class = "cw-form-small" }))
	   { %><%:Html.AntiForgeryToken() %>
			<div style="width: 75%">
			<div>&nbsp;</div>
			<div class="six_column section">
				<div class="three column">
					<div>
						<%: Html.LabelFor(m => m.Email) %>
					</div>
					<div>
						<%: Html.TextBoxFor(m => m.Email, new { @class = "cw-field-large", placeholder="Your email address", required=true, title = "This should be the e-mail address<br/>where you received your registration invitation" })%>
						<div><%: Html.ValidationMessageFor(m => m.Email)%></div>
					</div>
				</div>
				<div class="three column">
					<div>
						<%: Html.LabelFor(m => m.InviteId)%>
					</div>
					<div>
						<%: Html.TextBoxFor(m => m.InviteId, new { @class = "cw-field-xlarge", placeholder = "Your invitation code", required = true, title = "The invitation code we e-mailed you" })%>
						<div><%: Html.ValidationMessageFor(m => m.InviteId)%></div>
					</div>                    
				</div>
			</div>
			<div>&nbsp;</div>
			<div class="six_column section">
				<div class="three column">
					<div><%: Html.LabelFor(m => m.FirstName)%></div>
					<div><%: Html.TextBoxFor(m => m.FirstName, new { @class = "cw-field-large", placeholder="First Name" })%></div>
					<div><%: Html.ValidationMessageFor(m => m.FirstName)%></div>
				</div>                    
				<div class="three column">
					<div><%: Html.LabelFor(m => m.LastName)%></div>
					<div><%: Html.TextBoxFor(m => m.LastName, new { @class = "cw-field-large", placeholder = "Last Name" })%></div>
					<div><%: Html.ValidationMessageFor(m => m.LastName)%></div>
				</div>                    
			</div>                    
			<div>&nbsp;</div>
			<p>
				Passwords are required to be a minimum of <%: ViewData["PasswordLength"]%> characters in length.
			</p>
			<div class="six_column section">
				<div class="three column">
					<div>
						<%: Html.LabelFor(m => m.Password) %>
					</div>
					<div>
						<%: Html.PasswordFor(m => m.Password, new { @class = "cw-field-large", placeholder="Password", required=true, title = "Please enter a secure password with at least " + ViewData["PasswordLength"] + " characters" })%>
						<br /><%: Html.ValidationMessageFor(m => m.Password)%>
					</div>                    
				</div>                    
				<div class="three column">
					<div>
						<%: Html.LabelFor(m => m.ConfirmPassword) %>
					</div>
					<div>
						<%: Html.PasswordFor(m => m.ConfirmPassword, new { @class = "cw-field-large", required=true, title="Please enter your password again" })%>
						<br /><%: Html.ValidationMessageFor(m => m.ConfirmPassword)%>
					</div>          
				</div>                    
			</div>           
			</div>         
			<div>&nbsp;</div>
			<div class="cw-outl-orange cw-padded cw-rounded-corners" style="width: 75%;<%: User.UserIsSuperAdmin() ? "display:none" : ""%>">
			<div>
				<%: Html.LabelFor(m => m.PricingPlan) %>
			</div>
			<div>&nbsp;</div>
				<% ViewData["BaseModel"] = "RegisterModel"; %>
				<% ViewData["PricingPlan"] = (int)Model.PricingPlan; %>
				<% Html.RenderPartial("ctrlPricingPlans", Model.PricingPlans); %>
				<br /><%: Html.ValidationMessageFor(m => m.PricingPlan)%>
			</div>          
			<div>&nbsp;</div>
			<div>
				<%: Html.CheckBoxFor(m => m.HasAgreedToPrivacyPolicy) %>
				<%: Html.LabelFor(m => m.HasAgreedToPrivacyPolicy) %>
				<%: Html.ActionLink("(Please Review)", MVC.Home.PrivacyPolicy(), new { target ="_new"})%>
				<%: Html.ValidationMessageFor(m => m.HasAgreedToPrivacyPolicy)%>
			</div>
			<div>&nbsp;</div>
			<div>
				<%: Html.CheckBoxFor(m => m.HasAllowedCommunication) %>
				<%: Html.LabelFor(m => m.HasAllowedCommunication)%>
				<%: Html.ValidationMessageFor(m => m.HasAllowedCommunication)%>
			</div>
			<div>&nbsp;</div>
			<p>
				<button type="submit" class="cw-button cw-simple cw-blue" title="Click to Register">
				<span class="b-ok">Register</span>
				</button>
			</p>
	<% } %>
	</div>
</div>    
</asp:Content>
