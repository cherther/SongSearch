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
    
    Html.RenderPartial("ctrlSubMenu", menu);
        
    %>
    
</asp:Content>
<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class = "cw-outl">
    <h3>Create a New Account</h3>
    <p>
        Use the form below to create a new account. Or, <%= Html.ActionLink("log in", "LogIn") %> if you already have an account.
    </p>
    <p>
        Passwords are required to be a minimum of <%=Html.Encode(ViewData["PasswordLength"])%> characters in length.
    </p>
    <%//= Html.ValidationSummary("Account creation was unsuccessful. Please correct the errors and try again.") %>
    <% using (Html.BeginForm("Register", "Account", null, FormMethod.Post, new { @class = "cw-form-small" }))
       { %><%=Html.AntiForgeryToken() %>
    
        
            <fieldset>
                <legend>Account Information</legend>
                <div class="cw-fe-vert cw-fe-req">
                    <%= Html.LabelFor(m => m.Email) %>
                </div>
                <div class="cw-fe-vert">
                    <%= Html.TextBoxFor(m => m.Email)%>
                    <%= Html.ValidationMessageFor(m => m.Email)%>
                </div>
                <div class="cw-fe-vert cw-fe-req">
                    <%= Html.LabelFor(m => m.InviteId)%>
                </div>
                <div class="cw-fe-vert">
                    <%= Html.TextBoxFor(m => m.InviteId)%>
                    <%= Html.ValidationMessageFor(m => m.InviteId)%>
                </div>                    
                 <div class="cw-fe-vert">
                    <%= Html.LabelFor(m => m.FirstName)%>
                </div>
                <div class="cw-fe-vert">
                    <%= Html.TextBoxFor(m => m.FirstName)%>
                    <%= Html.ValidationMessageFor(m => m.FirstName)%>
                </div>                    
                 <div class="cw-fe-vert">
                    <%= Html.LabelFor(m => m.LastName)%>
                </div>
                <div class="cw-fe-vert">
                    <%= Html.TextBoxFor(m => m.LastName)%>
                    <%= Html.ValidationMessageFor(m => m.LastName)%>
                </div>                    
                <div class="cw-fe-vert cw-fe-req">
                    <%= Html.LabelFor(m => m.Password) %>
                </div>
                <div class="cw-fe-vert">
                    <%= Html.PasswordFor(m => m.Password)%>
                    <%= Html.ValidationMessageFor(m => m.Password)%>
                </div>                    
                <div class="cw-fe-vert cw-fe-req">
                    <%= Html.LabelFor(m => m.ConfirmPassword) %>
                </div>
                <div class="cw-fe-vert">
                    <%= Html.PasswordFor(m => m.ConfirmPassword)%>
                    <%= Html.ValidationMessageFor(m => m.ConfirmPassword)%>
                </div>                    
                <p>
                    <button type="submit" class="cw-button cw-simple cw-blue">
                    <span class="ok">Register</span>
                    </button>
                </p>
            </fieldset>
    <% } %>
    
</div>    
</asp:Content>
