<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ViewModel>" %>
<div class="text-small">
	<div>developed by <a href="http://www.codewerks.de/" title="They're awesome!">codewerks.de</a>
	</div>
	<div>[<%: App.Environment %>] - [<%: App.Version %>] - Build Date: <% = SongSearch.Web.VersionInfo.BuildDate()%></div> 

</div>