<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<p style="text-align: right">
Version: <% = SongSearch.Web.VersionInfo.BuildVersion()%> - 
Build Date: <% = SongSearch.Web.VersionInfo.BuildTime()%>
</p> 