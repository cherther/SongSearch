<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.ResetPasswordModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Reset Password
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded">
    <h3>Reset Password</h3>
    <p>
        Please enter your e-mail address and we will send you a link to help you select a new password. Please <%= Html.ActionLink("register", "Register") %> if you don't have an account.
    </p>
     <%= Html.ValidationSummary("Password reset request was unsuccessful. Please correct the errors and try again.") %>
    <% using (Html.BeginForm(MVC.Account.ResetPassword(), FormMethod.Post, new { @class = "cw-form-small" }))
       {%>
       <%=Html.AntiForgeryToken() %>
        <fieldset>
            <legend></legend>
            
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
        </fieldset>

    <% } %>
</div>

</asp:Content>


