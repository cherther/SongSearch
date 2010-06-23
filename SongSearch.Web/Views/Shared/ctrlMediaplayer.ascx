<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SongSearch.Web.ContentViewModel>" %>
<%
	var content = Model.Content;
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
				<%var url = Url.Action(MVC.Media.Stream(content.ContentId, MediaVersion.FullSong)); %>
					<a href="<%: url %>" id="cw-play-full"
					class = "cw-media-play-link cw-button cw-small cw-simple cw-green" title="Play/Pause"><span class="b-play">Full Song</span></a>
				<%} %>
				<%if (content.HasMediaPreviewVersion) { %>
				<%var url = Url.Action(MVC.Media.Stream(content.ContentId, MediaVersion.Preview)); %>
					<a href="<%: url %>" id = "cw-play-preview"
					class = "cw-media-play-link cw-button cw-small cw-simple cw-green" title="Play/Pause"><span class="b-play">:30</span></a>
				<%} %>
				<%if (content.HasMediaPreviewVersion || content.HasMediaFullVersion) { %>
					<button id="cw-play-repeat" class="cw-media-repeat-link cw-button cw-small cw-simple" title="Repeat" disabled="disabled"><span class="b-undo" >Repeat</span></button>
				<%} %>
			</div>
			<div class="one column cw-media-controls">
				<div id="cw-media-player-volume"></div>
			</div>
			<div class="one column  cw-media-controls text-right text-small-fixed">
				<span id="cw-media-player-time">0:00</span>&nbsp;/&nbsp;
				<span id="cw-media-player-length">0:00</span>
			</div>

	</div>
</div>
