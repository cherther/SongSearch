<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.UpdateProfileModel>" %>

<asp:Content ID="updateProfileTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Update Profile
</asp:Content>
<asp:Content id="subNav" ContentPlaceHolderID="SuvNavContent" runat="server">
<%
    IDictionary<string, string[]> menu = new Dictionary<string, string[]>();

    menu.Add("My Profile", new string[4] { "UpdateProfile", "Account", "", "current" });
    menu.Add("Change Password", new string[4] { "ChangePassword", "Account", "", "" });
    
    menu.Add("Log Out", new string[4] { "LogOut", "Account", "", "" });

//    menu.Add("My Playlists", new string[4] { "Index", "List", "", "" });
//    menu.Add("Add New", new string[4] { "Create", "Song", "", "" });
    
    Html.RenderPartial("ctrlSubMenu", menu);
        
    %>
    
</asp:Content>
<asp:Content ID="updateProfileContent" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class = "cw-outl">

    <h3>Update Profile</h3>
    <p>
        Use the form below to update your user profile. 
    </p>
    <%//= Html.ValidationSummary("Profile update was unsuccessful. Please correct the errors and try again.")%>
    
    <% using (Html.BeginForm("UpdateProfile", "Account", null, FormMethod.Post, new { @class = "cw-form-small" }))
       {%>
       <%=Html.AntiForgeryToken() %>
       <% Html.EnableClientValidation(); %>
            <fieldset>
                <legend>Profile Information</legend>
                <div class="cw-fe-vert">
                    <%= Html.LabelFor(m => m.Email) %>
                </div>
                <div class="cw-fe-vert cw-fe-req">
                    <%= Html.Encode(Model.Email)%>
                </div>
                <div class="cw-fe-vert">
                    <%= Html.LabelFor(m => m.FirstName) %>
                </div>
                <div class="cw-fe-vert">
                    <%= Html.TextBoxFor(m => m.FirstName)%>
                    <%= Html.ValidationMessageFor(m => m.FirstName)%>
                </div>
                <div class="cw-fe-vert">
                    <%= Html.LabelFor(m => m.LastName) %>
                </div>
                <div class="cw-fe-vert">
                    <%= Html.TextBoxFor(m => m.LastName)%>
                    <%= Html.ValidationMessageFor(m => m.LastName)%>
                </div>
                <%if (Model.ShowSignatureField)
                  {%>
                <div class="cw-fe-vert">
                    <%= Html.LabelFor(m => m.Signature)%>
                </div>
                <div class="cw-fe-vert">
                    <%= Html.TextBoxFor(m => m.Signature, new { size = "65", maxlength = "80" })%>
                    <%= Html.ValidationMessageFor(m => m.Signature)%>
                </div>
                <%} %>
                
                 <p>&nbsp;</p>
                <p>
                    <button type="submit" class="cw-button cw-simple cw-blue">
                    <span class="save">Update Profile</span>
                    </button>
                </p>
            </fieldset>
    <% } %>
</div>    
</asp:Content>
