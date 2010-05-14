<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>
    <%using (Html.BeginForm("Results", "Search", FormMethod.Get)) { %>
    <p>Title:
        <%=Html.Hidden("s[0].P", "1")%>
        <%=Html.Hidden("s[0].T", "1")%>
        <%=Html.TextBox("s[0].V") %>
    </p>
    <p>Artist:
        <%=Html.Hidden("s[1].P", "2")%>
        <%=Html.Hidden("s[1].T", "1")%>
        <%=Html.TextBox("s[1].V") %>
    </p>
    <p>Record Label:
        <%=Html.Hidden("s[2].P", "11")%>
        <%=Html.Hidden("s[2].T", "1")%>
        <%=Html.TextBox("s[2].V") %>
    </p>
    <p>Year:
        <%=Html.Hidden("s[3].P", "8")%>
        <%=Html.Hidden("s[3].T", "3")%>
        <%=Html.TextBox("s[3].V") %> to 
        <%=Html.TextBox("s[3].V") %>
    </p>
    <input type="submit" value="Search" />
    <%} %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SuvNavContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Javascripts" runat="server">
</asp:Content>
