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
<div id="content" class="cw-outl cw-padded">

    <h3>Reset Password</h3>
    <p>
        Use the form below to reset your password. 
    </p>
    <p>
        New passwords are required to be a minimum of <%= Html.Encode(ViewData["PasswordLength"]) %> characters in length.
    </p>
    <%= Html.ValidationSummary("Password reset was unsuccessful. Please correct the errors and try again.")%>

    <% using (Html.BeginForm(MVC.Account.ResetPasswordRespond(), FormMethod.Post, new { @class = "cw-form-small" }))
       {%><%=Html.AntiForgeryToken() %>
            <fieldset>
                <legend>Password Information</legend>
                <%= Html.HiddenFor(m => m.ResetCode) %>
                <div class="cw-fe-vert cw-fe-req">
                    <%= Html.LabelFor(m => m.Email) %>
                </div>
                <div class="cw-fe-vert">
                    <%= Html.TextBoxFor(m => m.Email, new { @class = "cw-field-large" })%>
                    <%= Html.ValidationMessageFor(m => m.Email)%>
                </div>

                <div class="cw-fe-vert cw-fe-req">
                    <%= Html.LabelFor(m => m.NewPassword) %>
                </div>
                <div class="cw-fe-vert">
                    <%= Html.PasswordFor(m => m.NewPassword, new { @class = "cw-field-large" })%>
                    <%= Html.ValidationMessageFor(m => m.NewPassword) %>
                </div>
                
                <div class="cw-fe-vert cw-fe-req">
                    <%= Html.LabelFor(m => m.ConfirmPassword) %>
                </div>
                <div class="cw-fe-vert">
                    <%= Html.PasswordFor(m => m.ConfirmPassword, new { @class = "cw-field-large" })%>
                    <%= Html.ValidationMessageFor(m => m.ConfirmPassword) %>
                </div>
                
                <p>
                    <button type="submit" class="cw-button cw-simple cw-blue">
                    <span class="ok">Reset Password</span>
                    </button>
                </p>
            </fieldset>
    <% } %>
</div>    
</asp:Content>
