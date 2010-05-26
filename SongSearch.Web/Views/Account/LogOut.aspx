<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.ViewModel>" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Log Out
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">

<div id="content" class="cw-outl cw-padded">
	<h3>Logging Out...</h3>
	<p style="text-align: center; padding: 0.5em 0 0.5em 0;"><img src="../../Content/loader.gif" alt="Loggings Out..." />
    </p>
</div>
</asp:Content>