<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ViewModel>" %>
<h1 style="display:inline">
    <a href="/" class="cw-logo">
    <% if (Html.SiteProfile().HasProfileLogo) {%>
        <img src="<%: Html.SiteProfile().SiteProfileLogoUrl() %>" height="70" title="<%: Html.SiteProfile().CompanyName %>" alt="<%: Html.SiteProfile().CompanyName %>" />
    <%} else { %>
    <%: Html.SiteProfile().CompanyName%>
    <%} %>
    </a>
</h1>&nbsp;
<span style="display:inline; font-weight: bold;color:Gray" class="text-medium cw-small-cap">Beta</span>
<% if (!Html.SiteProfile().HasProfileLogo) {%>
    <h3 class="cw-logo-sub">Your professional music licensing resource</h3>
<%}%>

