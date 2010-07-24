<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.ErrorViewModel>" %>
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

	<%if (User.User().ShowDebugInfo.GetValueOrDefault()){ %>
	<hr />
	<div>
	
		<h3>Some technical error detail:</h3>
		<p>
		We got a <strong><%: Model.Error.Exception != null ? Model.Error.Exception.GetType().Name : ""%></strong> in <strong><%:Model.Error.ControllerName%></strong>.<strong><%:Model.Error.ActionName%></strong> with a message:
		</p>
		<p>
		<%:Model.Error.Exception != null ? Model.Error.Exception.Message : ""%>
		</p>
	</div>
	<%} %>
	</div>
</asp:Content>