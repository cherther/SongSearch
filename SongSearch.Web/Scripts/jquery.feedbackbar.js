/*

Copyright 2010 Mark Ursino
http://code.ursino.info/feedbackbar/

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.

*/

(function($){

	$.fn.feedbackBar = function(o){
		$.feedbackBar(o);
	}

	$.feedbackBar = function(msg, o){
		var o = o,
			msg = msg,
			defaults = {
				wrapperClass : "feedback-bar",
				innerClass : "feedback-bar-content",
				durationIn : "slow",
				durationOut : "slow",
				autoClose : false,
				manualCloseSelector : ".close",
				delay : false,
				closeCallback : false
			},
			settings = $.extend(defaults, o);
		
		if(settings.delay) {
			setTimeout(addBar, settings.delay)
		} else {
			addBar();
		}
		
		function addBar() {
			if( $("." + settings.wrapperClass).length === 0 ) {
				var outer = $("<div />"),
					inner = $("<div />")
						.addClass(settings.innerClass)
						.html(msg);
				
				outer
					.addClass(settings.wrapperClass)
					.css({
						position : "absolute",
						top : "0",
						left : "0",
						width : "100%"
					})
					.append(inner)
					.hide()
					.appendTo("body");
				
				function slideUp(e) {
					outer.slideUp(settings.durationOut, function(){
						$(this).remove();
						if(settings.closeCallback) {
							settings.closeCallback();
						}
					});
					return false;
				}
				
				outer.slideDown(settings.durationIn, function(){
					if(settings.autoClose) {
						setTimeout(slideUp, settings.autoClose)
					} else {
						$(settings.manualCloseSelector).click(slideUp)
					}
				});
			}
		}
	};

})(jQuery);