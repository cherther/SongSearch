<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.UpdateProfileModel>" %>

<asp:Content ID="updateProfileTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Change Password
</asp:Content>
<asp:Content id="subNav" ContentPlaceHolderID="SuvNavContent" runat="server">
<%
ViewData["SubMenuLocation"] = "ChangePassword";
Html.RenderPartial(MVC.Shared.Views.ctrlAccountMenu);
%>
</asp:Content>
<asp:Content ID="updateProfileContent" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded cw-rounded-corners-bottom">

    <h2>Change Password</h2>
	<div>&nbsp;</div>
	<div class="cw-outl cw-padded cw-rounded-corners">
	<p>
        Use the form below to change your password. 
    </p>
    <p>
        New passwords are required to be a minimum of <%: ViewData["PasswordLength"] %> characters in length.
    </p>
    <%//= Html.ValidationSummary("Password change was unsuccessful. Please correct the errors and try again.")%>
	<div>&nbsp;</div>
    <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm(MVC.Account.ChangePassword(), FormMethod.Post, new { @class = "cw-form-small" }))
       {%><%:Html.AntiForgeryToken() %>
            <fieldset>
                <div>&nbsp;</div>
				<div>
                    <%: Html.LabelFor(m => m.OldPassword) %>
                </div>
                <div>
                    <%: Html.PasswordFor(m => m.OldPassword, new { @class = "cw-field-large" } )%>
                    <%: Html.ValidationMessageFor(m => m.OldPassword) %>
                </div>
                <div>&nbsp;</div>
                <div>
                    <%: Html.LabelFor(m => m.NewPassword) %>
                </div>
                <div>
                    <%: Html.PasswordFor(m => m.NewPassword, new { @class = "cw-field-large", title="Please enter a new secure password<br/>with at least 6 characters" })%>
                    <%: Html.ValidationMessageFor(m => m.NewPassword) %>
                </div>
                <div>&nbsp;</div>
                <div>
                    <%: Html.LabelFor(m => m.ConfirmPassword) %>
                </div>
                <div>
                    <%: Html.PasswordFor(m => m.ConfirmPassword, new { @class = "cw-field-large", title="Please enter your new password again"  })%>
                    <%: Html.ValidationMessageFor(m => m.ConfirmPassword) %>
                </div>
                 <p>&nbsp;</p>
                <p>
                   <button type="submit" class="cw-button cw-simple cw-blue" title="Click to change your password">
                    <span class="b-save">Change Password</span>
                    </button>
                </p>
            </fieldset>
    <% } %>
	</div>
    
</div>    
</asp:Content>
