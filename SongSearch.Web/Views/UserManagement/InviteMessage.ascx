<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.InviteViewModel>" %>
<%
	var baseUrl = Model.BaseUrl;
	var sender = Model.InvitingUser;
	var recipient = Model.Recipient;
	var url = String.Format(@"{0}/{1}?em={2}", Model.InviteUrl, Model.InviteId, Model.Recipient);
   
 %>
<html>
<head>
<title><%: Html.SiteProfile().CompanyName %> - Registration Invitation</title>
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

	<p>I'd like to invite you to register at <a href="<%: url %>"><%: Html.SiteProfile().CompanyName %></a>.</p>
	<p>Please <a href="<%: url %>"><strong style="font-size: 1.6em; font-variant: small-caps">register</strong></a> using your e-mail address <a href="<%: url %>"><%: recipient%></a>.</p>
	<p>&nbsp;</p>	
	<p>Thanks, and I'm looking forward to working with you!</p>
	<p>&nbsp;</p>	
	<p><strong><em><%: String.Format("{0} <{1}>", sender.FullName(), sender.UserName) %></em></strong></p>
	<p>&nbsp;</p>
	<hr />
	<p>If the 'register' link above is not clickable in your email program, please copy & paste this address into your browser's location bar:</p>
	<p><a href="<%: url %>"><%: url %></a></p>
</div>
</body>
</html>