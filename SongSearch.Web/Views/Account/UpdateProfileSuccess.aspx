<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.ViewModel>" %>

<asp:Content ID="updateProfileTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Update Profile
</asp:Content>
<asp:Content id="subNav" ContentPlaceHolderID="SuvNavContent" runat="server">
<%
    IDictionary<string, string[]> menu = new Dictionary<string, string[]>();

    menu.Add("Your Profile", new string[4] { "UpdateProfile", "Account", "", "current" });
    menu.Add("Change Password", new string[4] { "ChangePassword", "Account", "", "" });
    
    menu.Add("Log Out", new string[4] { "LogOut", "Account", "", "" });

//    menu.Add("My Playlists", new string[4] { "Index", "List", "", "" });
//    menu.Add("Add New", new string[4] { "Create", "Song", "", "" });
    
    Html.RenderPartial("ctrlSubMenu", menu);
        
    %>
  </asp:Content>
<asp:Content ID="updateProfileSuccessContent" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded">
    <h2>Update Profile</h2>
    <p>
        Your profile has been updated.
    </p>
    </div>
</asp:Content>
