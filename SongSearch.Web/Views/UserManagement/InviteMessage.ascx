<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.InviteViewModel>" %>
<%
	var baseUrl = Model.BaseUrl;
	var sender = Model.Sender;
	var recipient = Model.Recipient;
	var url = String.Format(@"{0}/{1}?em={2}", Model.InviteUrl, Model.InviteId, Model.Recipient);
   
 %>
<html>
<head>
<title><%: Model.SiteProfile.CompanyName %> - Registration Invitation</title>
</head>
<body>
<div id="content" style="
		font-size: 81.25%; 
		line-height: 1.2; 
		font-family: 'Lucida Grande', 'Lucida Sans Unicode', Verdana, Helvetica, Arial, sans-serif;
		border: 1px solid #A6C9E2;
		padding:0.8em;
		border-bottom-left-radius: 5px;
		border-bottom-right-radius: 5px;
		-moz-border-radius-border-bottomleft: 5px;
		-moz-border-radius-border-bottomright: 5px;
		-webkit-border-border-bottom-left-radius: 5px;
		-webkit-border-border-bottom-right-radius: 5px;">
	<h3>Hello,</h3>

	<p>I'd like to invite you to register for <strong><%: Model.SiteProfile.CompanyName %></strong>.</p>
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