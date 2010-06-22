<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.UpdateProfileModel>" %>

<asp:Content ID="updateProfileTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Change Password
</asp:Content>
<asp:Content id="subNav" ContentPlaceHolderID="SuvNavContent" runat="server">
<%
    IDictionary<string, string[]> menu = new Dictionary<string, string[]>();

    menu.Add("Your Profile", new string[4] { "UpdateProfile", "Account", "", "" });
    menu.Add("Change Password", new string[4] { "ChangePassword", "Account", "", "current" });
    
    menu.Add("Log Out", new string[4] { "LogOut", "Account", "", "" });

//    menu.Add("My Playlists", new string[4] { "Index", "List", "", "" });
//    menu.Add("Add New", new string[4] { "Create", "Song", "", "" });
    
    Html.RenderPartial(MVC.Shared.Views.ctrlSubMenu, menu);
        
    %>
    
</asp:Content>
<asp:Content ID="updateProfileContent" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded">

    <h3>Change Password</h3>
    <p>
        Use the form below to change your password. 
    </p>
    <p>
        New passwords are required to be a minimum of <%= Html.Encode(ViewData["PasswordLength"]) %> characters in length.
    </p>
    <%//= Html.ValidationSummary("Password change was unsuccessful. Please correct the errors and try again.")%>
    <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm(MVC.Account.ChangePassword(), FormMethod.Post, new { @class = "cw-form-small" }))
       {%><%=Html.AntiForgeryToken() %>
            <fieldset>
                <legend>Password Information</legend>
                <div class="cw-fe-vert cw-fe-req">
                    <%= Html.LabelFor(m => m.OldPassword) %>
                </div>
                <div class="cw-fe-vert">
                    <%= Html.PasswordFor(m => m.OldPassword, new { @class = "cw-field-large" } )%>
                    <%= Html.ValidationMessageFor(m => m.OldPassword) %>
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
                 <p>&nbsp;</p>
                <p>
                   <button type="submit" class="cw-button cw-simple cw-blue">
                    <span class="b-save">Change Password</span>
                    </button>
                </p>
            </fieldset>
    <% } %>
</div>    
</asp:Content>
