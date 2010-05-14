<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SongSearch.Web.Data.Content>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Results
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Results</h2>

    <table>
        <tr>
            <th>
                ContentId
            </th>
            <th>
                CatalogId
            </th>
 
            <th>
                Title
            </th>
            <th>
                Artist
            </th>
            <th>
                Writers
            </th>
            <th>
                PopCharts
            </th>
            <th>
                CountryCharts
            </th>
            <th>
                ReleaseYear
            </th>
            <th>
                RecordLabel
            </th>
     
            <th>
                HasMediaPreviewVersion
            </th>
            <th>
                HasMediaFullVersion
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink(item.ContentId.ToString(), "Details", new { id=item.ContentId })%> |
            </td>
            <td>
                <%: item.CatalogId %>
            </td>
           
            <td>
                <%: item.Title %>
            </td>
            <td>
                <%: item.Artist %>
            </td>
            <td>
                <%: item.Writers %>
            </td>
            <td>
                <%: item.PopCharts %>
            </td>
            <td>
                <%: item.CountryCharts %>
            </td>
            <td>
                <%: item.ReleaseYear %>
            </td>
            <td>
                <%: item.RecordLabel %>
            </td>
           
            <td>
                <%: item.HasMediaPreviewVersion %>
            </td>
            <td>
                <%: item.HasMediaFullVersion %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%: Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SuvNavContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Javascripts" runat="server">
</asp:Content>

