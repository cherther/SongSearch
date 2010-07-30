﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--<div id="login-panel">--%>
<% //Html.EnableClientValidation(); %>
<div id="topnav" class="topnav">
<%if (Request.IsAuthenticated) 
  {%>
Hello, <strong><%:Html.Friendly() %></strong>. <%: Html.ActionLink("My Profile", MVC.Account.UpdateProfile(), new { title = "Update your user profile" })%> | <%: Html.ActionLink("Log Out", MVC.Account.LogOut(), new { title = "Log out and end your session" })%>
<%
  }
  else 
  {
%>
Already registered? <%:Html.ActionLink("Log in", MVC.Account.LogIn(), new { @class = "signin", title = "Log in" })%>
<%
  }
%>
</div>
  <fieldset id="signin_menu">
	<% using (Html.BeginForm(MVC.Account.LogIn(), FormMethod.Post)) { %>
	   <%:Html.AntiForgeryToken() %>
	  <p>
	   <label for="Email">E-mail</label>
	  <input id="Email" name="Email" value="" title="Email" tabindex="4" type="text">
	  </p>
	  <p>
		<label for="password">Password</label>
		<input id="password" name="password" value="" title="password" tabindex="5" type="password">
	  </p>
	  <p class="remember">
		<input id="signin_submit" value="Log in" tabindex="6" type="submit" class="cw-button cw-simple cw-small cw-blue">
		<input id="RememberMe" name="RememberMe" value="true" tabindex="7" type="checkbox">
		<label for="RememberMe">Remember me</label>
	  </p>
	  <p class="forgot"> <%: Html.ActionLink("Forgot your password?", MVC.Account.ResetPassword(), new { title = "Did you forget your password? We can help" })%></p>
   <%} %>
  </fieldset>

  
  
<%--</div>--%>