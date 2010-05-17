<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.SearchViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Results
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% Html.RenderPartial("ctrlSearchMenu"); %>
    
<%
    var results = Model.SearchResults;
    
     %>
    <h2>Results</h2>
    <p>Showing <%= results.Count() %> of <%= results.TotalCount %> total found.</p>
    <p>Page <%= results.PageIndex+1 %> of <%= results.TotalPages%> total.</p>
    <table>
        <tr>
            <th>
                ContentId
            </th>
            <th>
                Title
            </th>
            <th>
                Artist
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
                HasMediaPreviewVersion
            </th>
        </tr>

    <% foreach (var item in results) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink(item.ContentId.ToString(), "Details", new { id=item.ContentId })%> |
            </td>
            <td>
                <%: item.Title %>
            </td>
            <td>
                <%: item.Artist %>
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
                <%: item.HasMediaPreviewVersion %>
            </td>
        </tr>
    
    <% } %>

    </table>

    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SuvNavContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Javascripts" runat="server">
</asp:Content>

