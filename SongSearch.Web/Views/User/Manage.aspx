<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.UserViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Invite
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SuvNavContent" runat="server">
<%
	IDictionary<string, string[]> menu = new Dictionary<string, string[]>();

    menu.Add("Invite", new string[4] { "Invite", "User", "Admin", "" });
    menu.Add("Manage Users", new string[4] { "Manage", "User", "Admin", "current" });
    menu.Add("Manage Catalogs", new string[4] { "Index", "Catalog", "Admin", "" });
	
	Html.RenderPartial("ctrlSubMenu", menu);
		
%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" class="cw-outl cw-padded">
    <h2><%: Model.PageTitle %></h2>

	<%if (Model != null) {
        var users = Model.MyUsers;
        var invites = Model.MyInvites;
        users = users.OrderBy(u => u.ParentUserId).ThenBy(u => u.UserName).ToList();
    %>
    <div>&nbsp;</div>
    <h3>Users</h3>
    <div>&nbsp;</div>
    <table>
	<tr>
		<td style="vertical-align: top">
            <div class = "cw-outl cw-padded" style="overflow:auto ; height: 300px;">
                <table id="user-list" class="cw-tbl-usr">
                    <% Html.RenderPartial("ctrlUserList", users ); %>
                </table>
            </div>
            <div>&nbsp;</div>
            <h3>Invites</h3><%= Html.ActionLink("Invite New", "Invite", "User") %>
            <div>&nbsp;</div>
            <div class = "cw-outl cw-padded" style="overflow: auto; height: 150px;">
                <table id="invite-list" class="cw-tbl-usr">
                <%
                foreach (var invite in invites)
                {
                %>
                    <tr>
                        <td><%= invite.InvitationEmailAddress %></td>           
                    </tr>
                <%  
                }        
                %>
                </table>
            </div>			
		</td>
		<td style="vertical-align: top">
			<div id="cw-user-detail" class="cw-outl cw-padded" style="display: none">
			</div>
		</td>
	</tr>
	</table>
    <%} %>
</div>
</asp:Content>