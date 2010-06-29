<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ViewModel>" %>
<%

	IDictionary<string, string[]> menu = new Dictionary<string, string[]>();

	var navLocation = Model == null ? "" : Model.NavigationLocation.FirstOrDefault();
	navLocation = String.IsNullOrEmpty(navLocation) ? ViewData["NavigationLocation"] as string : navLocation;

	menu.Add("Home", new string[3] { "Index", "Home", "Home" });
	menu.Add("Search", new string[3] { "Index", "Search", "Search" });
	if (Page.User.Identity.IsAuthenticated)
	{
		//var cartCount = ViewData["MyActiveCartContentsCount"] as string ?? "";
		//var cartMnuText = String.Concat("Song Cart", cartCount != null ? String.Format("({0})", cartCount) : "");
		menu.Add("Cart", new string[3] { "Index", "Cart", "Song Cart" });
	}
	if (Page.User.UserIsAnyAdmin())
	{
		menu.Add("Admin", new string[3] { "Index", "UserManagement", "Admin" });
	}
	if (!Page.User.Identity.IsAuthenticated)
	{
		menu.Add("Register", new string[3] { "Register", "Account", "Register" });
	}
	menu.Add("Help", new string[3] { "Index", "Help", "Help" });
	menu.Add("Contact", new string[3] { "Contact", "Home", "Contact Us" });   
	%>
	
		<ul id="main-menu" class="cw-menu cw-menu-main cw-rounded-corners-top">
		<%
		foreach(var menuItem in menu)
		{
			string[] values = menuItem.Value;        
			string menuId = String.Format("menu-{0}", menuItem.Key.ToLower());
				%>
			<li id="<%=menuId %>" class="cw-rounded-corners-top <%= menuItem.Key == navLocation ? "current" : ""%>">
			<%if (menuItem.Key == "Cart") { %>
			<% Html.RenderAction(MVC.Cart.CartCount()); %>
			<%} else if (menuItem.Key == "Help") { %>
			<a href="#"  class="cw-rounded-corners-top" onclick="UserVoice.Popin.show(uservoiceOptions); return false;"><%: values[2]%></a>
			<%} else { %>
			<%= Html.ActionLink(values[2], values[0], values[1], null, new { @class = "cw-rounded-corners-top" })%>
			<%} %>
			</li>
			<%
				//}
			} %>
		</ul>
