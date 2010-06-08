<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UpdateCache
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Update Cache</h2>
	<div>
    <%using (Html.BeginForm())
      { %>
      <p>&nbsp;</p>
      <p>
      Cache was last updated on: <%=SongSearch.Web.Services.CacheService.LastUpdated %>. To update all cached properties now, click the button below.
      </p>
      <p>&nbsp;</p>
      <button type="submit" class="sexybutton sexysimple sexyblue">
      <span class="sync">Update Cache</span>
      </button>
      <p>&nbsp;</p>
    <%} %>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SuvNavContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Javascripts" runat="server">
</asp:Content>
