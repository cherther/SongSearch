<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
	var current = ViewData["SubMenuLocation"];
	var menu = new MenuViewModel();

	menu.MenuItems.Add(new SongSearch.Web.MenuItem() {
		Name = "UserManagement",
		LinkDisplayName = "User Management",
		LinkActionName = "Index",
		LinkControllerName = "UserManagement",
		CurrentLocation = current.ToString(),
		IsAdmin = true
	});
	menu.MenuItems.Add(new SongSearch.Web.MenuItem() {
		Name = "CatalogManagement",
		LinkDisplayName = "Catalog Management",
		LinkActionName = "Index",
		LinkControllerName = "CatalogManagement",
		CurrentLocation = current.ToString(),
		IsAdmin = true
	}); 
	menu.MenuItems.Add(new SongSearch.Web.MenuItem() {
		Name = "CatalogUpload",
		LinkDisplayName = "Catalog Upload",
		LinkActionName = "Upload",
		LinkControllerName = "CatalogUpload",
		CurrentLocation = current.ToString(),
		IsAdmin = true
	});
	menu.MenuItems.Add(new SongSearch.Web.MenuItem() {
		Name = "Invite",
		LinkDisplayName = "Invite",
		LinkActionName = "Invite",
		LinkControllerName = "UserManagement",
		CurrentLocation = current.ToString(),
		IsAdmin = true
	});

	
	Html.RenderPartial(MVC.Shared.Views.ctrlSubMenu, menu);
		
%>
