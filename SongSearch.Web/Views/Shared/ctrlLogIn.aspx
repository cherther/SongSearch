<%@ Page Language="C#"  Inherits="System.Web.Mvc.ViewPage" %>
<%--<div id="login-panel">--%>
<div id="topnav" class="topnav">
<%if (Request.IsAuthenticated) 
  {%>
Hello, <strong><%=Html.Friendly() %></strong>! <%= Html.ActionLink("My Account", "UpdateProfile", "Account") %> | <%= Html.ActionLink("Log Out", "LogOut", "Account") %>
<%
  }
  else 
  {
%>
Already registered? <a href="/account/login" class="signin"><span>Log in</span></a> 
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
        <input id="signin_submit" value="Log in" tabindex="6" type="submit" class="sexybutton sexysimple sexysmall sexyblue">
        <input id="RememberMe" name="RememberMe" value="true" tabindex="7" type="checkbox">
        <label for="RememberMe">Remember me</label>
      </p>
      <p class="forgot"> <%= Html.ActionLink("Forgot your password?", "ResetPassword", "Account")%></p>
   <%} %>
  </fieldset>

  
  
<%--</div>--%>