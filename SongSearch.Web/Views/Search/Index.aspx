<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.SearchViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<table>
    <tr>
    <td align="left" width="200" id="cw-search-options-panel">
        <% Html.RenderPartial("ctrlSearchOptions"); %>
    </td>
    <td>
        <h3>Search Tips</h3>
        <div>&nbsp;</div>
        <ul>
            <li>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</li>
            <li>Ut tincidunt erat ac diam molestie a vehicula libero ullamcorper.</li>
            <li>Integer aliquam sagittis orci, sit amet scelerisque dolor mattis vitae.</li>
        </ul>
        <div>&nbsp;</div>
        <ul>
            <li>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</li>
            <li>Ut tincidunt erat ac diam molestie a vehicula libero ullamcorper.</li>
            <li>Integer aliquam sagittis orci, sit amet scelerisque dolor mattis vitae.</li>
        </ul>
        <div>&nbsp;</div>
        <ul>
            <li>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</li>
            <li>Ut tincidunt erat ac diam molestie a vehicula libero ullamcorper.</li>
            <li>Integer aliquam sagittis orci, sit amet scelerisque dolor mattis vitae.</li>
        </ul>
        <div>&nbsp;</div>
        <ul>
            <li>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</li>
            <li>Ut tincidunt erat ac diam molestie a vehicula libero ullamcorper.</li>
            <li>Integer aliquam sagittis orci, sit amet scelerisque dolor mattis vitae.</li>
        </ul>
        <div>&nbsp;</div>
        <ul>
            <li>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</li>
            <li>Ut tincidunt erat ac diam molestie a vehicula libero ullamcorper.</li>
            <li>Integer aliquam sagittis orci, sit amet scelerisque dolor mattis vitae.</li>
        </ul>
    </td>
    </tr>
</table>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SuvNavContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Javascripts" runat="server">
</asp:Content>
