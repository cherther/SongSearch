<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.UserViewModel>" %>
<%
    var user = Model.MyUsers.First();
%>
<div>
    <strong><%= user.FullName() %></strong>
</div>
<%if (Model.IsThisUser){%>
    <div>This is you! You can <%=Html.ActionLink("edit your Profile", "UpdateProfile", "Account") %>.</div>
<%}%>
<%=Html.Hidden("userid", user.UserId) %>
<div>&nbsp;</div>
<label><em>System Role</em></label>
<div>
	<%foreach (var role in Model.Roles) {%>
    <%
        string roleName = ((SongSearch.Web.Roles)role).ToString();
        string roleClass = role == user.RoleId ? "cw-green" : "cw-black"; 
		%>
		<a id="<%=String.Format("r-{0}", role) %>" class="cw-tag-box cw-role-edit cw-button cw-simple cw-small <%= roleClass%>"><%: roleName%> </a>
	<%} %>
</div>
<div>&nbsp;</div>
<div>
    <label>User Name:</label>
     <div><%= user.UserName%></div>
</div>
<div>&nbsp;</div>
<div>
    <label>Registered On:</label>
     <div><%= user.RegisteredOn.ToShortDateString()%></div>
</div>