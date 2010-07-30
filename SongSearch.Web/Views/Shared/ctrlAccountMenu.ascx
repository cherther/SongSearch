﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
	var current = ViewData["SubMenuLocation"];
	var menu = new MenuViewModel();

	if (Page.User.Identity.IsAuthenticated) {

		menu.MenuItems.Add(new SongSearch.Web.MenuItem() {
			Name = "UpdateProfile",
			LinkDisplayName = "My Profile",
			LinkActionName = "UpdateProfile",
			LinkControllerName = "Account",
			CurrentLocation = current.ToString()
		});
		if (Page.User.User().IsPlanUser) {
			menu.MenuItems.Add(new SongSearch.Web.MenuItem() {
				Name = "Plan",
				LinkDisplayName = "My Plan",
				LinkActionName = "Plan",
				LinkControllerName = "Account",
				CurrentLocation = current.ToString()
			});
		}
		
		menu.MenuItems.Add(new SongSearch.Web.MenuItem() {
			Name = "ChangePassword",
			LinkDisplayName = "Change Password",
			LinkActionName = "ChangePassword",
			LinkControllerName = "Account",
			CurrentLocation = current.ToString()
		});
		

		menu.MenuItems.Add(new SongSearch.Web.MenuItem() {
			Name = "LogOut",
			LinkDisplayName = "Log Out",
			LinkActionName = "LogOut",
			LinkControllerName = "Account",
			CurrentLocation = current.ToString()
		});

	} else {

		menu.MenuItems.Add(new SongSearch.Web.MenuItem() {
			Name = "Register",
			LinkDisplayName = "Register",
			LinkActionName = "Register",
			LinkControllerName = "Account",
			CurrentLocation = current.ToString()
		});
		menu.MenuItems.Add(new SongSearch.Web.MenuItem() {
			Name = "ResetPassword",
			LinkDisplayName = "Reset Password",
			LinkActionName = "ResetPassword",
			LinkControllerName = "Account",
			CurrentLocation = current.ToString()
		});
	}



	Html.RenderPartial(MVC.Shared.Views.ctrlSubMenu, menu);
		
%>