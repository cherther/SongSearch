<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SongSearch.Web.SearchViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Results
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% Html.RenderPartial("ctrlSearchMenu"); %>
    
<%  var props = Model.SearchMenuProperties;
    var results = Model.SearchResults;
    var request = Request.RawUrl.Replace(String.Format("&p={0}", results.PageIndex), "");
    var sortUrl = Model.SortPropertyId.HasValue ?
        String.Format("&s={0}&o={1}", Model.SortPropertyId.Value, (int)Model.SortType) : "";
     %>
    <h2>Results</h2>
    <p>Showing <%= results.Count() %> of <%= results.TotalCount %> total found.</p>
    <p>Page <%= results.CurrentPage %> of <%= results.TotalPages%> total.&nbsp;
    <%if (results.HasPreviousPage) {%>
        [<a href="<%=request%>&p=<%=results.LastPage%><%= sortUrl%>">Prev</a>]
    <%} %>
    <%if (results.HasNextPage) {%>
        [<a href="<%=request%>&p=<%=results.NextPage%><%= sortUrl%>">Next</a>]
    <%} %>
    &nbsp;Go to:&nbsp; 
    <%for (var i = 0; i < results.TotalPages; i++) { %>
        <%if (i == results.PageIndex) {%>
            <strong><%=i+1%></strong>
        <%} %>
        <%else { %>
            <a href="<%=request%>&p=<%=i%><%= sortUrl%>"><%=i+1%></a>
        <%} %>
        &nbsp;|&nbsp;
    <%} %>
    
    </p>
    <%
        
        var tableHeader = new string[] { "ContentId", "Title", "Artist", "PopCharts", "CountryCharts", "ReleaseYear", "HasMediaPreviewVersion" };
        var headerUrl = request.Replace(String.Format("&s={0}&o={1}", Model.SortPropertyId, (int)Model.SortType), "");
        
    %>
    <table>
        <tr>
        <%foreach(var col in tableHeader){ %>
            <th>
                <%
                    var prop = props.Where(p => p.PropertyCode.Equals(col)).SingleOrDefault();
                    if (prop != null){
                        var sp = prop.PropertyId;
                        var ord = Model.SortPropertyId.GetValueOrDefault().Equals(sp) ? (int)Model.SortType.Flip() : (int)SortType.Ascending;
                        var colUrl = String.Format("{0}&s={1}&o={2}", headerUrl, sp, ord);
                    %>
                    <a href="<%=colUrl%>"><%: prop.PropertyName%></a>
                    <%} else { %>
                    <%: col%>
                    <%}%>
            </th>
            <%} %>
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

