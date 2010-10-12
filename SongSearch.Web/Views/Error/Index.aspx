﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Error.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>
<asp:Content ID="errorTitle" ContentPlaceHolderID="TitleContent" runat="server">
	Error
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
	<div id="content" class="cw-outl cw-padded cw-rounded-corners-bottom">
	<h2>Oops!</h2>
	<div>&nbsp;</div>

	<div>
		 We're sorry, something bad happened. We're looking into it.
	</div>
	<div>&nbsp;</div>
		<div>Let's <a href="/">start over</a>...!</div>
	<div>&nbsp;</div>

		<div>
			<img src="../../Public/Images/error-500.jpg" alt="Ooops!"/>
		</div>
		<div>&nbsp;</div>
	</div>
</asp:Content>