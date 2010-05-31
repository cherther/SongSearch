<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ContentViewModel>" %>
<%
    var content = Model.Content;
%>
<div id="sm2-container"></div>
<table id="cw-tbl-media-player">
<tr>
	<td colspan="3">
		<div id="cw-media-statusbar"> 
			<div id="cw-media-loaded">
				<div id="cw-media-position"></div> 
			</div> 
		</div> 
	</td>
</tr>
<tr>
	<td width="55%">
	<%if (content.HasMediaFullVersion != null) { %>
	<%var url = Url.Action("Stream", "Media", new { id = content.ContentId, version = MediaVersion.FullSong }); %>
	<a href="<%: url %>" id = "cw-play-full"
		class = "cw-media-play-link cw-button cw-small cw-simple cw-green"><span class="b-play">Full Song</span></a>
	<%} %>
	<%if (content.HasMediaPreviewVersion != null) { %>
	<%	var url = Url.Action("Stream", "Media", new { id = content.ContentId, version = MediaVersion.Preview }); %>
    	<a href="<%: url %>" id = "cw-play-preview"
		class = "cw-media-play-link cw-button cw-small cw-simple cw-green"><span class="b-play">:30</span></a>
	<%} %>
	</td>
	<td width="20%" align="right">
		<div id="cw-media-player-volume"></div>				
	</td>
	<td align="right" width="25%" style="font-size: 10px;">
		<span id="cw-media-player-time">0:00</span>&nbsp;/&nbsp;
		<span id="cw-media-player-length">0:00</span>
	</td>

</tr>

</table>
<script type="text/javascript">
	$(function () {
		// setup master volume
		var vol = _currentVolume != null && _currentVolume >= 0 ? _currentVolume : 60;

		$("#cw-media-player-volume").slider({
			value: vol,
			orientation: "horizontal",
			range: "min",
			animate: true,
			slide: function (evt, ui) {
				changeVolume(ui.value); 
				}
		});
	});
	</script>