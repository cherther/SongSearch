<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ContentViewModel>" %>
<%
    var content = Model.Content;
	var fullSong = content.HasMediaFullVersion ? Url.Action("Stream", "Media", new { id = content.ContentId, version = MediaVersion.FullSong }) : null;
	var preview = content.HasMediaPreviewVersion ? Url.Action("Stream", "Media", new { id = content.ContentId, version = MediaVersion.Preview }) : null;
%>
<table id="cw-tbl-media-player">
<tr>
	<td colspan="2">
	Progress Bar
	</td>
</tr>
<tr>
	<td>
	<%if (fullSong != null) { %>
	<a href="<%: fullSong %>" id = "cw-play-full"
		class = "cw-media-play-link cw-button cw-small cw-simple cw-green"><span class="b-play">Full Song</span></a>
	<%} %>
	<%if (preview != null) { %>
    	<a href="<%: preview %>" id = "cw-play-preview"
		class = "cw-media-play-link cw-button cw-small cw-simple cw-green"><span class="b-play">:30</span></a>
	<%} %>
	</td>
	<td align="right">
	0:00 / 0:00
	</td>

</tr>

</table>
