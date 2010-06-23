<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.InviteViewModel>" %>
<%
    var baseUrl = Model.BaseUrl;
    var sender = Model.Sender;
    var recipient = Model.Recipient;
    var url = String.Format(@"{0}/{1}?em={2}", Model.InviteUrl, Model.InviteId, Model.Recipient);
   
 %>
<html>
<head>
<title>Ford Music Services Registration Invitation</title>
<link type="text/css" href="<%=baseUrl%>/public/css/jquery-ui.css" rel="stylesheet" media="screen"/>	
<link type="text/css" href="<%=baseUrl%>/public/css/cw.app.css" rel="stylesheet" media="screen" />
</head>
<body>
<div id="content" class="cw-outl cw-padded cw-rounded-corners-bottom">
    <h3>Hello,</h3>

    <p>I'd like to invite you to register for <strong>Ford Music Services SongSearch System</strong>.</p>
    <p>Please <a href="<%= url %>">register</a> using your e-mail address <strong><%= recipient%></strong>.</p>
    <p>&nbsp;</p>
    <p>If the 'register' link above is not clickable in your email program, please copy & paste this address into your browser's location bar:</p>
    <p><%= url %></p>

    <p>Alternatively, you can follow these steps:</p>
        <ol>
            <li>
                Type <strong><%=baseUrl%></strong> into the url/location bar of your web browser
            </li>
            <li>
                Click on the <strong>Register</strong> button
            </li>
            <li>
                Copy & paste this email address into the E-mail address field: <strong><%= recipient%></strong>
            </li>
            <li>
                Copy & paste this invitation code into the Invitation Code field: <strong><%= Model.InviteId%></strong>
            </li>
        </ol>
    <p>Thanks, and I'm looking forward to working with you!</p>

    <p><em><%=sender%></em></p>
</div>
</body>
</html>