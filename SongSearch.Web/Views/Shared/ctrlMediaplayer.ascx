<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ContentViewModel>" %>
<%
	var content = Model.Content;
	var fullSong = Url.Action(MVC.Media.Stream(content.ContentId, MediaVersion.FullSong));
	var preview = Url.Action(MVC.Media.Stream(content.ContentId, MediaVersion.Preview)); 
	var excerptStart = content.MediaExcerptStart.HasValue ? content.MediaExcerptStart : 0;
	var excerptEnd = content.MediaExcerptEnd.HasValue ? content.MediaExcerptEnd : excerptStart + 30 * 1000;

	var excerptRange = String.Format("{0}:{1}", excerptStart, excerptEnd);
	var startText = ((decimal)content.MediaExcerptStart.GetValueOrDefault()).MillSecsToTimeCode();
%>
<div id="sm2-container"></div>
<%//id="cw-tbl-media-player" %>
<div id="cw-media-player" class="cw-rounded-corners">
	<div class="six_column section">
	
			<div class="six column cw-media-controls">
				<div id="cw-media-statusbar"> 
					<div id="cw-media-loaded">
						<div id="cw-media-position"></div> 
					</div> 
				</div> 
			</div>
	</div>
	<div class="six_column section">
			<div class="four column cw-media-controls">
				<%if (content.HasMediaFullVersion) { %>
					<a href="<%: fullSong %>" id="cw-play-full"
					class = "cw-media-play-link cw-button cw-small cw-simple cw-gray" title="Play the full song"><span class="b-play">Full Song</span></a>
				<%} %>
				<%if (content.HasMediaPreviewVersion) { %>
					<a href="<%: preview %>" id = "cw-play-preview"
					class = "cw-media-play-link cw-button cw-small cw-simple cw-gray" title="Play a short excerpt"><span class="b-play">:30</span></a>
				<%} else if (content.MediaExcerptStart.HasValue) { %>
					<a href="<%: fullSong %>" id = "cw-play-cue"
					class = "cw-media-cue-link cw-button cw-small cw-simple cw-gray" rel="<%: excerptRange %>" title="Play a cue from <%: startText %>"><span class="b-reload">Cue</span></a>
				<%} %>
				<%if (content.HasMediaPreviewVersion || content.HasMediaFullVersion) { %>
					<button id="cw-play-rew" class="cw-media-rew-link cw-button cw-small cw-simple cw-gray" title="Rewind" disabled="disabled"><span class="b-rewind" >Rev</span></button>
				<%} %>
				<%if (content.HasMediaPreviewVersion || content.HasMediaFullVersion) { %>
					<button id="cw-play-ffwd" class="cw-media-ffwd-link cw-button cw-small cw-simple cw-gray" title="Fast Forward" disabled="disabled"><span class="b-forward after" >Fwd</span></button>
				<%} %>
				<%if (content.HasMediaPreviewVersion || content.HasMediaFullVersion) { %>
					<button id="cw-play-repeat" class="cw-media-repeat-link cw-button cw-small cw-simple cw-gray" title="Repeat" disabled="disabled"><span class="b-undo" >Repeat</span></button>
				<%} %>
				
			</div>
			<div class="one column cw-media-controls">
				<div id="cw-media-player-volume" title="Slide to change the volume"></div>
			</div>
			<div class="one column  cw-media-controls text-right text-small-fixed">
				<span id="cw-media-player-time">0:00</span>&nbsp;/&nbsp;
				<span id="cw-media-player-length">0:00</span>
			</div>

	</div>
</div>
