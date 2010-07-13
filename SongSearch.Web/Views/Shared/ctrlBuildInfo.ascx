<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div class="text-small">
	<div>Version: <% = SongSearch.Web.VersionInfo.BuildVersion()%> - 
	Build Date: <% = SongSearch.Web.VersionInfo.BuildTime()%></div> 
	<div>developed by <a href="http://www.codewerks.de/" title="They're awesome!">codewerks.de</a>
	</div>
</div>