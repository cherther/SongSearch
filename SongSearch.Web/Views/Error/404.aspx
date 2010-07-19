<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.ViewModel>" %>
<asp:Content ID="errorTitle" ContentPlaceHolderID="TitleContent" runat="server">
	Error
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
	<div id="content" class="cw-outl cw-padded cw-rounded-corners-bottom">
		<div>&nbsp;</div>

		<div>
			<img src="../../Public/Images/error_404.jpg" alt="This is a Dead End"/>
		</div>
		<div>&nbsp;</div>
	</div>
</asp:Content>