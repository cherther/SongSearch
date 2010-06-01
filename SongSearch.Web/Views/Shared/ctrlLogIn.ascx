<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--<div id="login-panel">--%>
<div id="topnav" class="topnav">
[<%: App.Environment %>]&nbsp;
<%if (Request.IsAuthenticated) 
  {%>
Hello, <strong><%=Html.Friendly() %></strong>. <%= Html.ActionLink("Your Profile", "UpdateProfile", "Account")%> | <%= Html.ActionLink("Log Out", "LogOut", "Account") %>
<%
  }
  else 
  {
%>
Already registered? <%=Html.ActionLink("Log in", "LogIn", "Account")  %>
<%
  }
%>
</div>
  <fieldset id="signin_menu">
    <% using (Html.BeginForm("LogIn", "Account", FormMethod.Post))
       { %>
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
      <p class="forgot"> <%= Html.ActionLink("Forgot your password?", "ResetPassword", "Account")%></p>
   <%} %>
  </fieldset>

  
  
<%--</div>--%>