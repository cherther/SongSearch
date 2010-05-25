<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.InviteViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Invite
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SuvNavContent" runat="server">
<%
	IDictionary<string, string[]> menu = new Dictionary<string, string[]>();

    menu.Add("Invite", new string[4] { "Invite", "User", "Admin", "current" });
    menu.Add("Manage Users", new string[4] { "Index", "User", "Admin", "" });
    menu.Add("Manage Catalogs", new string[4] { "Index", "Catalog", "Admin", "" });
	
	Html.RenderPartial("ctrlSubMenu", menu);
		
%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Invite New Users</h2>

	<p>Please enter one or many e-mail addresses (separated by commas).</p> 
	<p>After you click 'Send', an e-mail invitation will be sent to each address with a personalized invitation code and instructions on how to complete the registration process.</p>
	<p> This invitation code, along with their e-mail address will allow the recipient to register on the site using a password of their choice.</p>
	
	<% using (Html.BeginForm("Invite", "User", null, FormMethod.Post, new { @class = "cw-form-small" }))
	   {
		   //string baseUrl = Html.ActionLink("register", "Register").ToString();
		   %>
       <%=Html.AntiForgeryToken() %>
	 <fieldset id="invite">
		<legend>
		   Invite
		</legend>           
	   <div><%=Html.Label("From:")%>&nbsp;<%=Html.Encode(String.Format("{0} <{1}>", Html.Friendly(), User.Identity.Name))%></div>
	   <div><%=Html.Label("To:")%></div>
	   <div><%=Html.TextArea("Recipient", new { cols = "50", rows = 10 })%></div>
	   <div>
		<button id="invite-send" type="submit" title="Send" class="cw-button cw-simple cw-blue">
		<span class="b-email">Send</span>
		</button>
	   </div>
	   
	 </fieldset>  
	<%} %>

</asp:Content>