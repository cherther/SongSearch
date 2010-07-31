(function (b) {
    function s() { b.fn.ajaxSubmit.debug && window.console && window.console.log && window.console.log("[jquery.form] " + Array.prototype.join.call(arguments, "")) } b.fn.ajaxSubmit = function (a) {
        function e() {
            function o() {
                if (!z++) {
                    p.detachEvent ? p.detachEvent("onload", o) : p.removeEventListener("load", o, false); var q = true; try {
                        if (A) throw "timeout"; var n, m; m = p.contentWindow ? p.contentWindow.document : p.contentDocument ? p.contentDocument : p.document; var t = h.dataType == "xml" || m.XMLDocument || b.isXMLDoc(m); s("isXml=" +
t); if (!t && (m.body == null || m.body.innerHTML == "")) { if (--E) { z = 0; setTimeout(o, 100); return } s("Could not access iframe DOM after 50 tries."); return } i.responseText = m.body ? m.body.innerHTML : null; i.responseXML = m.XMLDocument ? m.XMLDocument : m; i.getResponseHeader = function (F) { return { "content-type": h.dataType}[F] }; if (h.dataType == "json" || h.dataType == "script") { var B = m.getElementsByTagName("textarea")[0]; if (B) i.responseText = B.value; else { var C = m.getElementsByTagName("pre")[0]; if (C) i.responseText = C.innerHTML } } else if (h.dataType ==
"xml" && !i.responseXML && i.responseText != null) i.responseXML = x(i.responseText); n = b.httpData(i, h.dataType)
                    } catch (G) { q = false; b.handleError(h, i, "error", G) } if (q) { h.success(n, "success"); u && b.event.trigger("ajaxSuccess", [i, h]) } u && b.event.trigger("ajaxComplete", [i, h]); u && ! --b.active && b.event.trigger("ajaxStop"); if (h.complete) h.complete(i, q ? "success" : "error"); setTimeout(function () { v.remove(); i.responseXML = null }, 100)
                } 
            } function x(q, n) {
                if (window.ActiveXObject) {
                    n = new ActiveXObject("Microsoft.XMLDOM"); n.async = "false";
                    n.loadXML(q)
                } else n = (new DOMParser).parseFromString(q, "text/xml"); return n && n.documentElement && n.documentElement.tagName != "parsererror" ? n : null
            } var j = g[0]; if (b(":input[name=submit]", j).length) alert('Error: Form elements must not be named "submit".'); else {
                var h = b.extend({}, b.ajaxSettings, a), r = b.extend(true, {}, b.extend(true, {}, b.ajaxSettings), h), y = "jqFormIO" + (new Date).getTime(), v = b('<iframe id="' + y + '" name="' + y + '" src="' + h.iframeSrc + '" />'), p = v[0]; v.css({ position: "absolute", top: "-1000px", left: "-1000px" });
                var i = { aborted: 0, responseText: null, responseXML: null, status: 0, statusText: "n/a", getAllResponseHeaders: function () { }, getResponseHeader: function () { }, setRequestHeader: function () { }, abort: function () { this.aborted = 1; v.attr("src", h.iframeSrc) } }, u = h.global; u && !b.active++ && b.event.trigger("ajaxStart"); u && b.event.trigger("ajaxSend", [i, h]); if (r.beforeSend && r.beforeSend(i, r) === false) r.global && b.active--; else if (!i.aborted) {
                    var z = 0, A = 0; if (r = j.clk) {
                        var D = r.name; if (D && !r.disabled) {
                            a.extraData = a.extraData || {}; a.extraData[D] =
r.value; if (r.type == "image") { a.extraData[name + ".x"] = j.clk_x; a.extraData[name + ".y"] = j.clk_y } 
                        } 
                    } setTimeout(function () {
                        var q = g.attr("target"), n = g.attr("action"); j.setAttribute("target", y); j.getAttribute("method") != "POST" && j.setAttribute("method", "POST"); j.getAttribute("action") != h.url && j.setAttribute("action", h.url); a.skipEncodingOverride || g.attr({ encoding: "multipart/form-data", enctype: "multipart/form-data" }); h.timeout && setTimeout(function () { A = true; o() }, h.timeout); var m = []; try {
                            if (a.extraData) for (var t in a.extraData) m.push(b('<input type="hidden" name="' +
t + '" value="' + a.extraData[t] + '" />').appendTo(j)[0]); v.appendTo("body"); p.attachEvent ? p.attachEvent("onload", o) : p.addEventListener("load", o, false); j.submit()
                        } finally { j.setAttribute("action", n); q ? j.setAttribute("target", q) : g.removeAttr("target"); b(m).remove() } 
                    }, 10); var E = 50
                } 
            } 
        } if (!this.length) { s("ajaxSubmit: skipping submit process - no element selected"); return this } if (typeof a == "function") a = { success: a }; var d = b.trim(this.attr("action")); if (d) d = (d.match(/^([^#]+)/) || [])[1]; d = d || window.location.href ||
""; a = b.extend({ url: d, type: this.attr("method") || "GET", iframeSrc: /^https/i.test(window.location.href || "") ? "javascript:false" : "about:blank" }, a || {}); d = {}; this.trigger("form-pre-serialize", [this, a, d]); if (d.veto) { s("ajaxSubmit: submit vetoed via form-pre-serialize trigger"); return this } if (a.beforeSerialize && a.beforeSerialize(this, a) === false) { s("ajaxSubmit: submit aborted via beforeSerialize callback"); return this } var f = this.formToArray(a.semantic); if (a.data) {
            a.extraData = a.data; for (var c in a.data) if (a.data[c] instanceof
Array) for (var l in a.data[c]) f.push({ name: c, value: a.data[c][l] }); else f.push({ name: c, value: a.data[c] })
        } if (a.beforeSubmit && a.beforeSubmit(f, this, a) === false) { s("ajaxSubmit: submit aborted via beforeSubmit callback"); return this } this.trigger("form-submit-validate", [f, this, a, d]); if (d.veto) { s("ajaxSubmit: submit vetoed via form-submit-validate trigger"); return this } c = b.param(f); if (a.type.toUpperCase() == "GET") { a.url += (a.url.indexOf("?") >= 0 ? "&" : "?") + c; a.data = null } else a.data = c; var g = this, k = []; a.resetForm &&
k.push(function () { g.resetForm() }); a.clearForm && k.push(function () { g.clearForm() }); if (!a.dataType && a.target) { var w = a.success || function () { }; k.push(function (o) { b(a.target).html(o).each(w, arguments) }) } else a.success && k.push(a.success); a.success = function (o, x) { for (var j = 0, h = k.length; j < h; j++) k[j].apply(a, [o, x, g]) }; c = b("input:file", this).fieldValue(); l = false; for (d = 0; d < c.length; d++) if (c[d]) l = true; if (c.length && a.iframe !== false || a.iframe || l || 0) a.closeKeepAlive ? b.get(a.closeKeepAlive, e) : e(); else b.ajax(a); this.trigger("form-submit-notify",
[this, a]); return this
    }; b.fn.ajaxForm = function (a) {
        return this.ajaxFormUnbind().bind("submit.form-plugin", function () { b(this).ajaxSubmit(a); return false }).bind("click.form-plugin", function (e) {
            var d = e.target, f = b(d); if (!f.is(":submit,input:image")) { d = f.closest(":submit"); if (d.length == 0) return; d = d[0] } var c = this; c.clk = d; if (d.type == "image") if (e.offsetX != undefined) { c.clk_x = e.offsetX; c.clk_y = e.offsetY } else if (typeof b.fn.offset == "function") { f = f.offset(); c.clk_x = e.pageX - f.left; c.clk_y = e.pageY - f.top } else {
                c.clk_x =
e.pageX - d.offsetLeft; c.clk_y = e.pageY - d.offsetTop
            } setTimeout(function () { c.clk = c.clk_x = c.clk_y = null }, 100)
        })
    }; b.fn.ajaxFormUnbind = function () { return this.unbind("submit.form-plugin click.form-plugin") }; b.fn.formToArray = function (a) {
        var e = []; if (this.length == 0) return e; var d = this[0], f = a ? d.getElementsByTagName("*") : d.elements; if (!f) return e; for (var c = 0, l = f.length; c < l; c++) {
            var g = f[c], k = g.name; if (k) if (a && d.clk && g.type == "image") {
                if (!g.disabled && d.clk == g) {
                    e.push({ name: k, value: b(g).val() }); e.push({ name: k + ".x",
                        value: d.clk_x
                    }, { name: k + ".y", value: d.clk_y })
                } 
            } else if ((g = b.fieldValue(g, true)) && g.constructor == Array) for (var w = 0, o = g.length; w < o; w++) e.push({ name: k, value: g[w] }); else g !== null && typeof g != "undefined" && e.push({ name: k, value: g })
        } if (!a && d.clk) { a = b(d.clk); f = a[0]; if ((k = f.name) && !f.disabled && f.type == "image") { e.push({ name: k, value: a.val() }); e.push({ name: k + ".x", value: d.clk_x }, { name: k + ".y", value: d.clk_y }) } } return e
    }; b.fn.formSerialize = function (a) { return b.param(this.formToArray(a)) }; b.fn.fieldSerialize = function (a) {
        var e =
[]; this.each(function () { var d = this.name; if (d) { var f = b.fieldValue(this, a); if (f && f.constructor == Array) for (var c = 0, l = f.length; c < l; c++) e.push({ name: d, value: f[c] }); else f !== null && typeof f != "undefined" && e.push({ name: this.name, value: f }) } }); return b.param(e)
    }; b.fn.fieldValue = function (a) { for (var e = [], d = 0, f = this.length; d < f; d++) { var c = b.fieldValue(this[d], a); c === null || typeof c == "undefined" || c.constructor == Array && !c.length || (c.constructor == Array ? b.merge(e, c) : e.push(c)) } return e }; b.fieldValue = function (a, e) {
        var d =
a.name, f = a.type, c = a.tagName.toLowerCase(); if (typeof e == "undefined") e = true; if (e && (!d || a.disabled || f == "reset" || f == "button" || (f == "checkbox" || f == "radio") && !a.checked || (f == "submit" || f == "image") && a.form && a.form.clk != a || c == "select" && a.selectedIndex == -1)) return null; if (c == "select") {
            c = a.selectedIndex; if (c < 0) return null; e = []; a = a.options; d = (f = f == "select-one") ? c + 1 : a.length; for (c = f ? c : 0; c < d; c++) {
                var l = a[c]; if (l.selected) {
                    var g = l.value; g || (g = l.attributes && l.attributes.value && !l.attributes.value.specified ? l.text :
l.value); if (f) return g; e.push(g)
                } 
            } return e
        } return a.value
    }; b.fn.clearForm = function () { return this.each(function () { b("input,select,textarea", this).clearFields() }) }; b.fn.clearFields = b.fn.clearInputs = function () { return this.each(function () { var a = this.type, e = this.tagName.toLowerCase(); if (a == "text" || a == "password" || e == "textarea") this.value = ""; else if (a == "checkbox" || a == "radio") this.checked = false; else if (e == "select") this.selectedIndex = -1 }) }; b.fn.resetForm = function () {
        return this.each(function () {
            if (typeof this.reset ==
"function" || typeof this.reset == "object" && !this.reset.nodeType) this.reset()
        })
    }; b.fn.enable = function (a) { if (a == undefined) a = true; return this.each(function () { this.disabled = !a }) }; b.fn.selected = function (a) { if (a == undefined) a = true; return this.each(function () { var e = this.type; if (e == "checkbox" || e == "radio") this.checked = a; else if (this.tagName.toLowerCase() == "option") { e = b(this).parent("select"); a && e[0] && e[0].type == "select-one" && e.find("option").selected(false); this.selected = a } }) } 
})(jQuery);
/*!
 * jQuery blockUI plugin
 * Version 2.28 (02-DEC-2009)
 * @requires jQuery v1.2.3 or later
 *
 * Examples at: http://malsup.com/jquery/block/
 * Copyright (c) 2007-2008 M. Alsup
 * Dual licensed under the MIT and GPL licenses:
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.gnu.org/licenses/gpl.html
 *
 * Thanks to Amir-Hossein Sobhi for some excellent contributions!
 */

;(function($) {

if (/1\.(0|1|2)\.(0|1|2)/.test($.fn.jquery) || /^1.1/.test($.fn.jquery)) {
	alert('blockUI requires jQuery v1.2.3 or later!  You are using v' + $.fn.jquery);
	return;
}

$.fn._fadeIn = $.fn.fadeIn;

// this bit is to ensure we don't call setExpression when we shouldn't (with extra muscle to handle
// retarded userAgent strings on Vista)
var mode = document.documentMode || 0;
var setExpr = $.browser.msie && (($.browser.version < 8 && !mode) || mode < 8);
var ie6 = $.browser.msie && /MSIE 6.0/.test(navigator.userAgent) && !mode;

// global $ methods for blocking/unblocking the entire page
$.blockUI   = function(opts) { install(window, opts); };
$.unblockUI = function(opts) { remove(window, opts); };

// convenience method for quick growl-like notifications  (http://www.google.com/search?q=growl)
$.growlUI = function(title, message, timeout, onClose) {
	var $m = $('<div class="growlUI"></div>');
	if (title) $m.append('<h1>'+title+'</h1>');
	if (message) $m.append('<h2>'+message+'</h2>');
	if (timeout == undefined) timeout = 3000;
	$.blockUI({
		message: $m, fadeIn: 700, fadeOut: 1000, centerY: false,
		timeout: timeout, showOverlay: false,
		onUnblock: onClose, 
		css: $.blockUI.defaults.growlCSS
	});
};

// plugin method for blocking element content
$.fn.block = function(opts) {
	return this.unblock({ fadeOut: 0 }).each(function() {
		if ($.css(this,'position') == 'static')
			this.style.position = 'relative';
		if ($.browser.msie)
			this.style.zoom = 1; // force 'hasLayout'
		install(this, opts);
	});
};

// plugin method for unblocking element content
$.fn.unblock = function(opts) {
	return this.each(function() {
		remove(this, opts);
	});
};

$.blockUI.version = 2.28; // 2nd generation blocking at no extra cost!

// override these in your code to change the default behavior and style
$.blockUI.defaults = {
	// message displayed when blocking (use null for no message)
	message:  '<h1>Please wait...</h1>',

	title: null,	  // title string; only used when theme == true
	draggable: true,  // only used when theme == true (requires jquery-ui.js to be loaded)
	
	theme: false, // set to true to use with jQuery UI themes
	
	// styles for the message when blocking; if you wish to disable
	// these and use an external stylesheet then do this in your code:
	// $.blockUI.defaults.css = {};
	css: {
		padding:	0,
		margin:		0,
		width:		'30%',
		top:		'40%',
		left:		'35%',
		textAlign:	'center',
		color:		'#000',
		border:		'3px solid #aaa',
		backgroundColor:'#fff',
		cursor:		'wait'
	},
	
	// minimal style set used when themes are used
	themedCSS: {
		width:	'30%',
		top:	'40%',
		left:	'35%'
	},

	// styles for the overlay
	overlayCSS:  {
		backgroundColor: '#000',
		opacity:	  	 0.6,
		cursor:		  	 'wait'
	},

	// styles applied when using $.growlUI
	growlCSS: {
		width:  	'350px',
		top:		'10px',
		left:   	'',
		right:  	'10px',
		border: 	'none',
		padding:	'5px',
		opacity:	0.6,
		cursor: 	'default',
		color:		'#fff',
		backgroundColor: '#000',
		'-webkit-border-radius': '10px',
		'-moz-border-radius':	 '10px'
	},
	
	// IE issues: 'about:blank' fails on HTTPS and javascript:false is s-l-o-w
	// (hat tip to Jorge H. N. de Vasconcelos)
	iframeSrc: /^https/i.test(window.location.href || '') ? 'javascript:false' : 'about:blank',

	// force usage of iframe in non-IE browsers (handy for blocking applets)
	forceIframe: false,

	// z-index for the blocking overlay
	baseZ: 1000,

	// set these to true to have the message automatically centered
	centerX: true, // <-- only effects element blocking (page block controlled via css above)
	centerY: true,

	// allow body element to be stetched in ie6; this makes blocking look better
	// on "short" pages.  disable if you wish to prevent changes to the body height
	allowBodyStretch: true,

	// enable if you want key and mouse events to be disabled for content that is blocked
	bindEvents: true,

	// be default blockUI will supress tab navigation from leaving blocking content
	// (if bindEvents is true)
	constrainTabKey: true,

	// fadeIn time in millis; set to 0 to disable fadeIn on block
	fadeIn:  200,

	// fadeOut time in millis; set to 0 to disable fadeOut on unblock
	fadeOut:  400,

	// time in millis to wait before auto-unblocking; set to 0 to disable auto-unblock
	timeout: 0,

	// disable if you don't want to show the overlay
	showOverlay: true,

	// if true, focus will be placed in the first available input field when
	// page blocking
	focusInput: true,

	// suppresses the use of overlay styles on FF/Linux (due to performance issues with opacity)
	applyPlatformOpacityRules: true,

	// callback method invoked when unblocking has completed; the callback is
	// passed the element that has been unblocked (which is the window object for page
	// blocks) and the options that were passed to the unblock call:
	//	 onUnblock(element, options)
	onUnblock: null,

	// don't ask; if you really must know: http://groups.google.com/group/jquery-en/browse_thread/thread/36640a8730503595/2f6a79a77a78e493#2f6a79a77a78e493
	quirksmodeOffsetHack: 4
};

// private data and functions follow...

var pageBlock = null;
var pageBlockEls = [];

function install(el, opts) {
	var full = (el == window);
	var msg = opts && opts.message !== undefined ? opts.message : undefined;
	opts = $.extend({}, $.blockUI.defaults, opts || {});
	opts.overlayCSS = $.extend({}, $.blockUI.defaults.overlayCSS, opts.overlayCSS || {});
	var css = $.extend({}, $.blockUI.defaults.css, opts.css || {});
	var themedCSS = $.extend({}, $.blockUI.defaults.themedCSS, opts.themedCSS || {});
	msg = msg === undefined ? opts.message : msg;

	// remove the current block (if there is one)
	if (full && pageBlock)
		remove(window, {fadeOut:0});

	// if an existing element is being used as the blocking content then we capture
	// its current place in the DOM (and current display style) so we can restore
	// it when we unblock
	if (msg && typeof msg != 'string' && (msg.parentNode || msg.jquery)) {
		var node = msg.jquery ? msg[0] : msg;
		var data = {};
		$(el).data('blockUI.history', data);
		data.el = node;
		data.parent = node.parentNode;
		data.display = node.style.display;
		data.position = node.style.position;
		if (data.parent)
			data.parent.removeChild(node);
	}

	var z = opts.baseZ;

	// blockUI uses 3 layers for blocking, for simplicity they are all used on every platform;
	// layer1 is the iframe layer which is used to supress bleed through of underlying content
	// layer2 is the overlay layer which has opacity and a wait cursor (by default)
	// layer3 is the message content that is displayed while blocking

	var lyr1 = ($.browser.msie || opts.forceIframe) 
		? $('<iframe class="blockUI" style="z-index:'+ (z++) +';display:none;border:none;margin:0;padding:0;position:absolute;width:100%;height:100%;top:0;left:0" src="'+opts.iframeSrc+'"></iframe>')
		: $('<div class="blockUI" style="display:none"></div>');
	var lyr2 = $('<div class="blockUI blockOverlay" style="z-index:'+ (z++) +';display:none;border:none;margin:0;padding:0;width:100%;height:100%;top:0;left:0"></div>');
	
	var lyr3;
	if (opts.theme && full) {
		var s = '<div class="blockUI blockMsg blockPage ui-dialog ui-widget ui-corner-all" style="z-index:'+z+';display:none;position:fixed">' +
					'<div class="ui-widget-header ui-dialog-titlebar blockTitle">'+(opts.title || '&nbsp;')+'</div>' +
					'<div class="ui-widget-content ui-dialog-content"></div>' +
				'</div>';
		lyr3 = $(s);
	}
	else {
		lyr3 = full ? $('<div class="blockUI blockMsg blockPage" style="z-index:'+z+';display:none;position:fixed"></div>')
					: $('<div class="blockUI blockMsg blockElement" style="z-index:'+z+';display:none;position:absolute"></div>');
	}						   

	// if we have a message, style it
	if (msg) {
		if (opts.theme) {
			lyr3.css(themedCSS);
			lyr3.addClass('ui-widget-content');
		}
		else 
			lyr3.css(css);
	}

	// style the overlay
	if (!opts.applyPlatformOpacityRules || !($.browser.mozilla && /Linux/.test(navigator.platform)))
		lyr2.css(opts.overlayCSS);
	lyr2.css('position', full ? 'fixed' : 'absolute');

	// make iframe layer transparent in IE
	if ($.browser.msie || opts.forceIframe)
		lyr1.css('opacity',0.0);

	$([lyr1[0],lyr2[0],lyr3[0]]).appendTo(full ? 'body' : el);
	
	if (opts.theme && opts.draggable && $.fn.draggable) {
		lyr3.draggable({
			handle: '.ui-dialog-titlebar',
			cancel: 'li'
		});
	}

	// ie7 must use absolute positioning in quirks mode and to account for activex issues (when scrolling)
	var expr = setExpr && (!$.boxModel || $('object,embed', full ? null : el).length > 0);
	if (ie6 || expr) {
		// give body 100% height
		if (full && opts.allowBodyStretch && $.boxModel)
			$('html,body').css('height','100%');

		// fix ie6 issue when blocked element has a border width
		if ((ie6 || !$.boxModel) && !full) {
			var t = sz(el,'borderTopWidth'), l = sz(el,'borderLeftWidth');
			var fixT = t ? '(0 - '+t+')' : 0;
			var fixL = l ? '(0 - '+l+')' : 0;
		}

		// simulate fixed position
		$.each([lyr1,lyr2,lyr3], function(i,o) {
			var s = o[0].style;
			s.position = 'absolute';
			if (i < 2) {
				full ? s.setExpression('height','Math.max(document.body.scrollHeight, document.body.offsetHeight) - (jQuery.boxModel?0:'+opts.quirksmodeOffsetHack+') + "px"')
					 : s.setExpression('height','this.parentNode.offsetHeight + "px"');
				full ? s.setExpression('width','jQuery.boxModel && document.documentElement.clientWidth || document.body.clientWidth + "px"')
					 : s.setExpression('width','this.parentNode.offsetWidth + "px"');
				if (fixL) s.setExpression('left', fixL);
				if (fixT) s.setExpression('top', fixT);
			}
			else if (opts.centerY) {
				if (full) s.setExpression('top','(document.documentElement.clientHeight || document.body.clientHeight) / 2 - (this.offsetHeight / 2) + (blah = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop) + "px"');
				s.marginTop = 0;
			}
			else if (!opts.centerY && full) {
				var top = (opts.css && opts.css.top) ? parseInt(opts.css.top) : 0;
				var expression = '((document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop) + '+top+') + "px"';
				s.setExpression('top',expression);
			}
		});
	}

	// show the message
	if (msg) {
		if (opts.theme)
			lyr3.find('.ui-widget-content').append(msg);
		else
			lyr3.append(msg);
		if (msg.jquery || msg.nodeType)
			$(msg).show();
	}

	if (($.browser.msie || opts.forceIframe) && opts.showOverlay)
		lyr1.show(); // opacity is zero
	if (opts.fadeIn) {
		if (opts.showOverlay)
			lyr2._fadeIn(opts.fadeIn);
		if (msg)
			lyr3.fadeIn(opts.fadeIn);
	}
	else {
		if (opts.showOverlay)
			lyr2.show();
		if (msg)
			lyr3.show();
	}

	// bind key and mouse events
	bind(1, el, opts);

	if (full) {
		pageBlock = lyr3[0];
		pageBlockEls = $(':input:enabled:visible',pageBlock);
		if (opts.focusInput)
			setTimeout(focus, 20);
	}
	else
		center(lyr3[0], opts.centerX, opts.centerY);

	if (opts.timeout) {
		// auto-unblock
		var to = setTimeout(function() {
			full ? $.unblockUI(opts) : $(el).unblock(opts);
		}, opts.timeout);
		$(el).data('blockUI.timeout', to);
	}
};

// remove the block
function remove(el, opts) {
	var full = (el == window);
	var $el = $(el);
	var data = $el.data('blockUI.history');
	var to = $el.data('blockUI.timeout');
	if (to) {
		clearTimeout(to);
		$el.removeData('blockUI.timeout');
	}
	opts = $.extend({}, $.blockUI.defaults, opts || {});
	bind(0, el, opts); // unbind events
	
	var els;
	if (full) // crazy selector to handle odd field errors in ie6/7
		els = $('body').children().filter('.blockUI').add('body > .blockUI');
	else
		els = $('.blockUI', el);

	if (full)
		pageBlock = pageBlockEls = null;

	if (opts.fadeOut) {
		els.fadeOut(opts.fadeOut);
		setTimeout(function() { reset(els,data,opts,el); }, opts.fadeOut);
	}
	else
		reset(els, data, opts, el);
};

// move blocking element back into the DOM where it started
function reset(els,data,opts,el) {
	els.each(function(i,o) {
		// remove via DOM calls so we don't lose event handlers
		if (this.parentNode)
			this.parentNode.removeChild(this);
	});

	if (data && data.el) {
		data.el.style.display = data.display;
		data.el.style.position = data.position;
		if (data.parent)
			data.parent.appendChild(data.el);
		$(el).removeData('blockUI.history');
	}

	if (typeof opts.onUnblock == 'function')
		opts.onUnblock(el,opts);
};

// bind/unbind the handler
function bind(b, el, opts) {
	var full = el == window, $el = $(el);

	// don't bother unbinding if there is nothing to unbind
	if (!b && (full && !pageBlock || !full && !$el.data('blockUI.isBlocked')))
		return;
	if (!full)
		$el.data('blockUI.isBlocked', b);

	// don't bind events when overlay is not in use or if bindEvents is false
	if (!opts.bindEvents || (b && !opts.showOverlay)) 
		return;

	// bind anchors and inputs for mouse and key events
	var events = 'mousedown mouseup keydown keypress';
	b ? $(document).bind(events, opts, handler) : $(document).unbind(events, handler);

// former impl...
//	   var $e = $('a,:input');
//	   b ? $e.bind(events, opts, handler) : $e.unbind(events, handler);
};

// event handler to suppress keyboard/mouse events when blocking
function handler(e) {
	// allow tab navigation (conditionally)
	if (e.keyCode && e.keyCode == 9) {
		if (pageBlock && e.data.constrainTabKey) {
			var els = pageBlockEls;
			var fwd = !e.shiftKey && e.target == els[els.length-1];
			var back = e.shiftKey && e.target == els[0];
			if (fwd || back) {
				setTimeout(function(){focus(back)},10);
				return false;
			}
		}
	}
	// allow events within the message content
	if ($(e.target).parents('div.blockMsg').length > 0)
		return true;

	// allow events for content that is not being blocked
	return $(e.target).parents().children().filter('div.blockUI').length == 0;
};

function focus(back) {
	if (!pageBlockEls)
		return;
	var e = pageBlockEls[back===true ? pageBlockEls.length-1 : 0];
	if (e)
		e.focus();
};

function center(el, x, y) {
	var p = el.parentNode, s = el.style;
	var l = ((p.offsetWidth - el.offsetWidth)/2) - sz(p,'borderLeftWidth');
	var t = ((p.offsetHeight - el.offsetHeight)/2) - sz(p,'borderTopWidth');
	if (x) s.left = l > 0 ? (l+'px') : '0';
	if (y) s.top  = t > 0 ? (t+'px') : '0';
};

function sz(el, p) {
	return parseInt($.css(el,p))||0;
};

})(jQuery);

(function($){$.gritter={};$.gritter.options={fade_in_speed:'medium',fade_out_speed:1000,time:6000}
$.gritter.add=function(params){try{return Gritter.add(params||{});}catch(e){var err='Gritter Error: '+e;(typeof(console)!='undefined'&&console.error)?console.error(err,params):alert(err);}}
$.gritter.remove=function(id,params){Gritter.removeSpecific(id,params||{});}
$.gritter.removeAll=function(params){Gritter.stop(params||{});}
var Gritter={fade_in_speed:'',fade_out_speed:'',time:'',_custom_timer:0,_item_count:0,_is_setup:0,_tpl_close:'<div class="gritter-close"></div>',_tpl_item:'<div id="gritter-item-[[number]]" class="gritter-item-wrapper [[item_class]]" style="display:none"><div class="gritter-top"></div><div class="gritter-item">[[image]]<div class="[[class_name]]"><span class="gritter-title">[[username]]</span><p>[[text]]</p></div><div style="clear:both"></div></div><div class="gritter-bottom"></div></div>',_tpl_wrap:'<div id="gritter-notice-wrapper"></div>',add:function(params){if(!params.title||!params.text){throw'You need to fill out the first 2 params: "title" and "text"';}
if(!this._is_setup){this._runSetup();}
var user=params.title,text=params.text,image=params.image||'',sticky=params.sticky||false,item_class=params.class_name||'',time_alive=params.time||'';this._verifyWrapper();this._item_count++;var number=this._item_count,tmp=this._tpl_item;$(['before_open','after_open','before_close','after_close']).each(function(i,val){Gritter['_'+val+'_'+number]=($.isFunction(params[val]))?params[val]:function(){}});this._custom_timer=0;if(time_alive){this._custom_timer=time_alive;}
var image_str=(image!='')?'<img src="'+image+'" class="gritter-image" />':'',class_name=(image!='')?'gritter-with-image':'gritter-without-image';tmp=this._str_replace(['[[username]]','[[text]]','[[image]]','[[number]]','[[class_name]]','[[item_class]]'],[user,text,image_str,this._item_count,class_name,item_class],tmp);this['_before_open_'+number]();$('#gritter-notice-wrapper').append(tmp);var item=$('#gritter-item-'+this._item_count);item.fadeIn(this.fade_in_speed,function(){Gritter['_after_open_'+number]($(this));});if(!sticky){this._setFadeTimer(item,number);}
$(item).bind('mouseenter mouseleave',function(event){if(event.type=='mouseenter'){if(!sticky){Gritter._restoreItemIfFading($(this),number);}}
else{if(!sticky){Gritter._setFadeTimer($(this),number);}}
Gritter._hoverState($(this),event.type);});return number;},_countRemoveWrapper:function(unique_id,e){e.remove();this['_after_close_'+unique_id](e);if($('.gritter-item-wrapper').length==0){$('#gritter-notice-wrapper').remove();}},_fade:function(e,unique_id,params,unbind_events){var params=params||{},fade=(typeof(params.fade)!='undefined')?params.fade:true;fade_out_speed=params.speed||this.fade_out_speed;this['_before_close_'+unique_id](e);if(unbind_events){e.unbind('mouseenter mouseleave');}
if(fade){e.animate({opacity:0},fade_out_speed,function(){e.animate({height:0},300,function(){Gritter._countRemoveWrapper(unique_id,e);})})}
else{this._countRemoveWrapper(unique_id,e);}},_hoverState:function(e,type){if(type=='mouseenter'){e.addClass('hover');var find_img=e.find('img');(find_img.length)?find_img.before(this._tpl_close):e.find('span').before(this._tpl_close);e.find('.gritter-close').click(function(){var unique_id=e.attr('id').split('-')[2];Gritter.removeSpecific(unique_id,{},e,true);});}
else{e.removeClass('hover');e.find('.gritter-close').remove();}},removeSpecific:function(unique_id,params,e,unbind_events){if(!e){var e=$('#gritter-item-'+unique_id);}
this._fade(e,unique_id,params||{},unbind_events);},_restoreItemIfFading:function(e,unique_id){clearTimeout(this['_int_id_'+unique_id]);e.stop().css({opacity:''});},_runSetup:function(){for(opt in $.gritter.options){this[opt]=$.gritter.options[opt];}
this._is_setup=1;},_setFadeTimer:function(e,unique_id){var timer_str=(this._custom_timer)?this._custom_timer:this.time;this['_int_id_'+unique_id]=setTimeout(function(){Gritter._fade(e,unique_id);},timer_str);},stop:function(params){var before_close=($.isFunction(params.before_close))?params.before_close:function(){};var after_close=($.isFunction(params.after_close))?params.after_close:function(){};var wrap=$('#gritter-notice-wrapper');before_close(wrap);wrap.fadeOut(function(){$(this).remove();after_close();});},_str_replace:function(search,replace,subject,count){var i=0,j=0,temp='',repl='',sl=0,fl=0,f=[].concat(search),r=[].concat(replace),s=subject,ra=r instanceof Array,sa=s instanceof Array;s=[].concat(s);if(count){this.window[count]=0;}
for(i=0,sl=s.length;i<sl;i++){if(s[i]===''){continue;}
for(j=0,fl=f.length;j<fl;j++){temp=s[i]+'';repl=ra?(r[j]!==undefined?r[j]:''):r[0];s[i]=(temp).split(f[j]).join(repl);if(count&&s[i]!==temp){this.window[count]+=(temp.length-s[i].length)/f[j].length;}}}
return sa?s:s[0];},_verifyWrapper:function(){if($('#gritter-notice-wrapper').length==0){$('body').append(this._tpl_wrap);}}}})(jQuery);
/*!
   SoundManager 2: Javascript Sound for the Web
   --------------------------------------------
   http://schillmania.com/projects/soundmanager2/

   Copyright (c) 2007, Scott Schiller. All rights reserved.
   Code provided under the BSD License:
   http://schillmania.com/projects/soundmanager2/license.txt

   V2.95b.20100323
*/

(function(g){function n(o,p){this.flashVersion=8;this.debugFlash=this.debugMode=false;this.useConsole=true;this.waitForWindowLoad=this.consoleOnly=false;this.nullURL="null.mp3";this.allowPolling=true;this.useMovieStar=this.useFastPolling=false;this.bgColor="#ffffff";this.useHighPerformance=false;this.flashLoadTimeout=1E3;this.wmode=null;this.allowFullScreen=true;this.allowScriptAccess="always";this.useFlashBlock=false;this.defaultOptions={autoLoad:false,stream:true,autoPlay:false,onid3:null,onload:null, whileloading:null,onplay:null,onpause:null,onresume:null,whileplaying:null,onstop:null,onfinish:null,onbeforefinish:null,onbeforefinishtime:5E3,onbeforefinishcomplete:null,onjustbeforefinish:null,onjustbeforefinishtime:200,multiShot:true,multiShotEvents:false,position:null,pan:0,volume:100};this.flash9Options={isMovieStar:null,usePeakData:false,useWaveformData:false,useEQData:false,onbufferchange:null,ondataerror:null};this.movieStarOptions={onmetadata:null,useVideo:false,bufferTime:2};var l,a=this, j,m;this.version=null;this.versionNumber="V2.95b.20100323";this.movieURL=null;this.url=o||null;this.altURL=null;this.enabled=this.swfLoaded=false;this.o=null;this.movieID="sm2-container";this.id=p||"sm2movie";this.swfCSS={swfDefault:"movieContainer",swfError:"swf_error",swfTimedout:"swf_timedout",swfUnblocked:"swf_unblocked",sm2Debug:"sm2_debug",highPerf:"high_performance",flashDebug:"flash_debug"};this.oMC=null;this.sounds={};this.soundIDs=[];this.isFullScreen=this.muted=false;this.isIE=navigator.userAgent.match(/MSIE/i); this.isSafari=navigator.userAgent.match(/safari/i);this.debugID="soundmanager-debug";this.debugURLParam=/([#?&])debug=1/i;this.didFlashBlock=this.specialWmodeCase=false;this._onready=[];this._debugOpen=true;this._windowLoaded=this._disabled=this._didInit=this._appendSuccess=this._didAppend=false;this._hasConsole=typeof console!=="undefined"&&typeof console.log!=="undefined";this._debugLevels=["log","info","warn","error"];this._defaultFlashVersion=8;this._oRemovedHTML=this._oRemoved=null;j=function(c){return document.getElementById(c)}; this.filePattern=null;this.filePatterns={flash8:/\.mp3(\?\.*)?$/i,flash9:/\.mp3(\?\.*)?$/i};this.baseMimeTypes=/^audio\/(?:x-)?(?:mp(?:eg|3))\s*;?/i;this.netStreamMimeTypes=/^audio\/(?:x-)?(?:mp(?:eg|3)|mp4a-latm|aac|speex)\s*;?/i;this.netStreamTypes=["aac","flv","mov","mp4","m4v","f4v","m4a","mp4v","3gp","3g2"];this.netStreamPattern=new RegExp("\\.("+this.netStreamTypes.join("|")+")(\\?.*)?$","i");this.mimePattern=a.baseMimeTypes;this.features={buffering:false,peakData:false,waveformData:false,eqData:false, movieStar:false};this.sandbox={type:null,types:{remote:"remote (domain-based) rules",localWithFile:"local with file access (no internet access)",localWithNetwork:"local with network (internet access only, no local access)",localTrusted:"local, trusted (local+internet access)"},description:null,noRemote:null,noLocal:null};this._setVersionInfo=function(){if(a.flashVersion!==8&&a.flashVersion!==9){alert(a._str("badFV",a.flashVersion,a._defaultFlashVersion));a.flashVersion=a._defaultFlashVersion}a.version= a.versionNumber+(a.flashVersion===9?" (AS3/Flash 9)":" (AS2/Flash 8)");if(a.flashVersion>8){a.defaultOptions=a._mergeObjects(a.defaultOptions,a.flash9Options);a.features.buffering=true}if(a.flashVersion>8&&a.useMovieStar){a.defaultOptions=a._mergeObjects(a.defaultOptions,a.movieStarOptions);a.filePatterns.flash9=new RegExp("\\.(mp3|"+a.netStreamTypes.join("|")+")(\\?.*)?$","i");a.mimePattern=a.netStreamMimeTypes;a.features.movieStar=true}else{a.useMovieStar=false;a.features.movieStar=false}a.filePattern= a.filePatterns[a.flashVersion!==8?"flash9":"flash8"];a.movieURL=a.flashVersion===8?"soundmanager2.swf":"soundmanager2_flash9.swf";a.features.peakData=a.features.waveformData=a.features.eqData=a.flashVersion>8};this._overHTTP=document.location?document.location.protocol.match(/http/i):null;this._initPending=this._waitingforEI=false;this._tryInitOnFocus=this.isSafari&&typeof document.hasFocus==="undefined";this._isFocused=typeof document.hasFocus!=="undefined"?document.hasFocus():null;this._okToDisable= !this._tryInitOnFocus;this.useAltURL=!this._overHTTP;this.strings={};this._str=function(){var c=Array.prototype.slice.call(arguments),b=c.shift();b=a.strings&&a.strings[b]?a.strings[b]:"";var d,e;if(b&&c&&c.length){d=0;for(e=c.length;d<e;d++)b=b.replace("%s",c[d])}return b};this.supported=function(){return a._didInit&&!a._disabled};this.getMovie=function(c){return a.isIE?g[c]:a.isSafari?j(c)||document[c]:j(c)};this.loadFromXML=function(c){try{a.o._loadFromXML(c)}catch(b){a._failSafely();return true}}; this.createSound=function(c){var b=null;b=null;if(!a._didInit)throw a._complain("soundManager.createSound(): "+a._str("notReady"),arguments.callee.caller);if(arguments.length===2)c={id:arguments[0],url:arguments[1]};b=b=a._mergeObjects(c);if(a._idCheck(b.id,true))return a.sounds[b.id];if(a.flashVersion>8&&a.useMovieStar){if(b.isMovieStar===null)b.isMovieStar=b.url.match(a.netStreamPattern)?true:false;if(b.isMovieStar&&b.usePeakData)b.usePeakData=false}a.sounds[b.id]=new l(b);a.soundIDs[a.soundIDs.length]= b.id;a.flashVersion===8?a.o._createSound(b.id,b.onjustbeforefinishtime):a.o._createSound(b.id,b.url,b.onjustbeforefinishtime,b.usePeakData,b.useWaveformData,b.useEQData,b.isMovieStar,b.isMovieStar?b.useVideo:false,b.isMovieStar?b.bufferTime:false);if(b.autoLoad||b.autoPlay)a.sounds[b.id]&&a.sounds[b.id].load(b);b.autoPlay&&a.sounds[b.id].play();return a.sounds[b.id]};this.createVideo=function(c){if(arguments.length===2)c={id:arguments[0],url:arguments[1]};if(a.flashVersion>=9){c.isMovieStar=true; c.useVideo=true}else return false;return a.createSound(c)};this.destroyVideo=this.destroySound=function(c,b){if(!a._idCheck(c))return false;for(var d=0;d<a.soundIDs.length;d++)a.soundIDs[d]===c&&a.soundIDs.splice(d,1);a.sounds[c].unload();b||a.sounds[c].destruct();delete a.sounds[c]};this.load=function(c,b){if(!a._idCheck(c))return false;a.sounds[c].load(b)};this.unload=function(c){if(!a._idCheck(c))return false;a.sounds[c].unload()};this.start=this.play=function(c,b){if(!a._didInit)throw a._complain("soundManager.play(): "+ a._str("notReady"),arguments.callee.caller);if(!a._idCheck(c)){b instanceof Object||(b={url:b});if(b&&b.url){b.id=c;return a.createSound(b).play()}else return false}a.sounds[c].play(b)};this.setPosition=function(c,b){if(!a._idCheck(c))return false;a.sounds[c].setPosition(b)};this.stop=function(c){if(!a._idCheck(c))return false;a.sounds[c].stop()};this.stopAll=function(){for(var c in a.sounds)a.sounds[c]instanceof l&&a.sounds[c].stop()};this.pause=function(c){if(!a._idCheck(c))return false;a.sounds[c].pause()}; this.pauseAll=function(){for(var c=a.soundIDs.length;c--;)a.sounds[a.soundIDs[c]].pause()};this.resume=function(c){if(!a._idCheck(c))return false;a.sounds[c].resume()};this.resumeAll=function(){for(var c=a.soundIDs.length;c--;)a.sounds[a.soundIDs[c]].resume()};this.togglePause=function(c){if(!a._idCheck(c))return false;a.sounds[c].togglePause()};this.setPan=function(c,b){if(!a._idCheck(c))return false;a.sounds[c].setPan(b)};this.setVolume=function(c,b){if(!a._idCheck(c))return false;a.sounds[c].setVolume(b)}; this.mute=function(c){var b=0;if(typeof c!=="string")c=null;if(c){if(!a._idCheck(c))return false;a.sounds[c].mute()}else{for(b=a.soundIDs.length;b--;)a.sounds[a.soundIDs[b]].mute();a.muted=true}};this.muteAll=function(){a.mute()};this.unmute=function(c){if(typeof c!=="string")c=null;if(c){if(!a._idCheck(c))return false;a.sounds[c].unmute()}else{for(c=a.soundIDs.length;c--;)a.sounds[a.soundIDs[c]].unmute();a.muted=false}};this.unmuteAll=function(){a.unmute()};this.toggleMute=function(c){if(!a._idCheck(c))return false; a.sounds[c].toggleMute()};this.getMemoryUse=function(){if(a.flashVersion===8)return 0;if(a.o)return parseInt(a.o._getMemoryUse(),10)};this.disable=function(c){if(typeof c==="undefined")c=false;if(a._disabled)return false;a._disabled=true;for(var b=a.soundIDs.length;b--;)a._disableObject(a.sounds[a.soundIDs[b]]);a.initComplete(c)};this.canPlayMIME=function(c){return c?c.match(a.mimePattern)?true:false:null};this.canPlayURL=function(c){return c?c.match(a.filePattern)?true:false:null};this.canPlayLink= function(c){if(typeof c.type!=="undefined"&&c.type)if(a.canPlayMIME(c.type))return true;return a.canPlayURL(c.href)};this.getSoundById=function(c){if(!c)throw new Error("SoundManager.getSoundById(): sID is null/undefined");return a.sounds[c]};this.onready=function(c,b){if(c&&c instanceof Function){b||(b=g);a._addOnReady(c,b);a._processOnReady();return true}else throw a._str("needFunction");};this.oninitmovie=function(){};this.onload=function(){};this.onerror=function(){};this._idCheck=this.getSoundById; this._complain=function(c,b){if(!b)return new Error("Error: "+c);typeof console!=="undefined"&&typeof console.trace!=="undefined"&&console.trace();c="Error: "+c+". \nCaller: "+b.toString();return new Error(c)};m=function(){return false};m._protected=true;this._disableObject=function(c){for(var b in c)if(typeof c[b]==="function"&&typeof c[b]._protected==="undefined")c[b]=m};this._failSafely=function(c){if(typeof c==="undefined")c=false;if(!a._disabled||c)a.disable(c)};this._normalizeMovieURL=function(c){var b= null;if(c)if(c.match(/\.swf(\?\.*)?$/i)){if(b=c.substr(c.toLowerCase().lastIndexOf(".swf?")+4))return c}else if(c.lastIndexOf("/")!==c.length-1)c+="/";return(c&&c.lastIndexOf("/")!==-1?c.substr(0,c.lastIndexOf("/")+1):"./")+a.movieURL};this._getDocument=function(){return document.body?document.body:document.documentElement?document.documentElement:document.getElementsByTagName("div")[0]};this._getDocument._protected=true;this._setPolling=function(c,b){if(!a.o||!a.allowPolling)return false;a.o._setPolling(c, b)};this._createMovie=function(c,b){var d=b?b:a.url;b=a.altURL?a.altURL:d;var e,f,i,h;if(a._didAppend&&a._appendSuccess)return false;a._didAppend=true;a._setVersionInfo();a.url=a._normalizeMovieURL(a._overHTTP?d:b);b=a.url;if(a.useHighPerformance&&a.useMovieStar&&a.defaultOptions.useVideo===true)a.useHighPerformance=false;a.wmode=!a.wmode&&a.useHighPerformance&&!a.useMovieStar?"transparent":a.wmode;if(a.wmode!==null&&!a.isIE&&!a.useHighPerformance&&navigator.platform.match(/win32/i)){a.specialWmodeCase= true;a.wmode=null}if(a.flashVersion===8)a.allowFullScreen=false;e={name:c,id:c,src:b,width:"100%",height:"100%",quality:"high",allowScriptAccess:a.allowScriptAccess,bgcolor:a.bgColor,pluginspage:"http://www.macromedia.com/go/getflashplayer",type:"application/x-shockwave-flash",wmode:a.wmode,allowfullscreen:a.allowFullScreen?"true":"false"};if(a.debugFlash)e.FlashVars="debug=1";a.wmode||delete e.wmode;if(a.isIE){d=document.createElement("div");i='<object id="'+c+'" data="'+b+'" type="'+e.type+'" width="'+ e.width+'" height="'+e.height+'"><param name="movie" value="'+b+'" /><param name="AllowScriptAccess" value="'+a.allowScriptAccess+'" /><param name="quality" value="'+e.quality+'" />'+(a.wmode?'<param name="wmode" value="'+a.wmode+'" /> ':"")+'<param name="bgcolor" value="'+a.bgColor+'" /><param name="allowFullScreen" value="'+e.allowFullScreen+'" />'+(a.debugFlash?'<param name="FlashVars" value="'+e.FlashVars+'" />':"")+"<!-- --\></object>"}else{d=document.createElement("embed");for(f in e)e.hasOwnProperty(f)&& d.setAttribute(f,e[f])}if(a.debugMode){h=document.createElement("div");h.id=a.debugID+"-toggle";c={position:"fixed",bottom:"0px",right:"0px",width:"1.2em",height:"1.2em",lineHeight:"1.2em",margin:"2px",textAlign:"center",border:"1px solid #999",cursor:"pointer",background:"#fff",color:"#333",zIndex:10001};h.appendChild(document.createTextNode("-"));h.onclick=a._toggleDebug;h.title="Toggle SM2 debug console";if(navigator.userAgent.match(/msie 6/i)){h.style.position="absolute";h.style.cursor="hand"}for(f in c)if(c.hasOwnProperty(f))h.style[f]= c[f]}c=a.getSWFCSS();if(f=a._getDocument()){a.oMC=j(a.movieID)?j(a.movieID):document.createElement("div");if(a.oMC.id){b=a.oMC.className;a.oMC.className=(b?b+" ":a.swfCSS.swfDefault)+(c?" "+c:"");a.oMC.appendChild(d);if(a.isIE){c=a.oMC.appendChild(document.createElement("div"));c.className="sm2-object-box";c.innerHTML=i}a._appendSuccess=true}else{a.oMC.id=a.movieID;a.oMC.className=a.swfCSS.swfDefault+" "+c;c=b=null;a.useFlashBlock||(b=a.useHighPerformance?{position:"fixed",width:"6px",height:"6px", bottom:"0px",left:"0px",overflow:"hidden"}:{position:"absolute",width:"6px",height:"6px",top:"-9999px",left:"-9999px"});e=null;if(!a.debugFlash)for(e in b)if(b.hasOwnProperty(e))a.oMC.style[e]=b[e];try{a.isIE||a.oMC.appendChild(d);f.appendChild(a.oMC);if(a.isIE){c=a.oMC.appendChild(document.createElement("div"));c.className="sm2-object-box";c.innerHTML=i}a._appendSuccess=true}catch(q){throw new Error(a._str("appXHTML"));}}if(a.debugMode&&!j(a.debugID)&&(!a._hasConsole||!a.useConsole||a.useConsole&& a._hasConsole&&!a.consoleOnly)){i=document.createElement("div");i.id=a.debugID;i.style.display=a.debugMode?"block":"none";if(a.debugMode&&!j(h.id)){try{f.appendChild(h)}catch(r){throw new Error(a._str("appXHTML"));}f.appendChild(i)}}}};this._writeDebug=function(){};this._writeDebug._protected=true;this._wdCount=0;this._wdCount._protected=true;this._wD=this._writeDebug;this._wDS=function(){};this._wDS._protected=true;this._wDAlert=function(){};this._toggleDebug=function(){};this._toggleDebug._protected= true;this._debug=function(){for(var c=0,b=a.soundIDs.length;c<b;c++)a.sounds[a.soundIDs[c]]._debug()};this._debugTS=function(){};this._debugTS._protected=true;this._mergeObjects=function(c,b){var d={},e,f;for(e in c)if(c.hasOwnProperty(e))d[e]=c[e];c=typeof b==="undefined"?a.defaultOptions:b;for(f in c)if(c.hasOwnProperty(f)&&typeof d[f]==="undefined")d[f]=c[f];return d};this.go=this.createMovie=function(c){if(c)a.url=c;a._initMovie()};this._initMovie=function(){if(a.o)return false;a.o=a.getMovie(a.id); if(!a.o){if(a.oRemoved){if(a.isIE)a.oMC.innerHTML=a.oRemovedHTML;else a.oMC.appendChild(a.oRemoved);a.oRemoved=null;a._didAppend=true}else a._createMovie(a.id,a.url);a.o=a.getMovie(a.id)}typeof a.oninitmovie==="function"&&setTimeout(a.oninitmovie,1)};this.waitForExternalInterface=function(){if(a._waitingForEI)return false;a._waitingForEI=true;if(a._tryInitOnFocus&&!a._isFocused)return false;var c;setTimeout(function(){c=a.getMoviePercent();if(!a._didInit&&a._okToDisable)if(c)a.flashLoadTimeout!== 0&&a._failSafely(true);else if(a.useFlashBlock||a.flashLoadTimeout===0)a.useFlashBlock&&a.flashBlockHandler();else a._failSafely(true)},a.flashLoadTimeout)};this.getSWFCSS=function(){var c=[];a.debugMode&&c.push(a.swfCSS.sm2Debug);a.debugFlash&&c.push(a.swfCSS.flashDebug);a.useHighPerformance&&c.push(a.swfCSS.highPerf);return c.join(" ")};this.flashBlockHandler=function(){var c=a.getMoviePercent();if(a.supported())a.oMC.className=a.getSWFCSS()+" "+a.swfCSS.swfDefault+(" "+a.swfCSS.swfUnblocked);else{a.oMC.className= a.getSWFCSS()+" "+a.swfCSS.swfDefault+" "+(!c?a.swfCSS.swfTimedout:a.swfCSS.swfError);a._processOnReady(true);a.onerror instanceof Function&&a.onerror.apply(g);a.didFlashBlock=true}};this.getMoviePercent=function(){return a.o&&typeof a.o.PercentLoaded!=="undefined"?a.o.PercentLoaded():null};this.handleFocus=function(){if(a._isFocused||!a._tryInitOnFocus)return true;a._okToDisable=true;a._isFocused=true;a._tryInitOnFocus&&g.removeEventListener("mousemove",a.handleFocus,false);a._waitingForEI=false; setTimeout(a.waitForExternalInterface,500);if(g.removeEventListener)g.removeEventListener("focus",a.handleFocus,false);else g.detachEvent&&g.detachEvent("onfocus",a.handleFocus)};this.initComplete=function(c){if(a._didInit)return false;if(!(a.useFlashBlock&&a.flashLoadTimeout&&!a.getMoviePercent()))a._didInit=true;if(a._disabled||c){if(a.useFlashBlock)a.oMC.className=a.getSWFCSS()+" "+(!a.getMoviePercent()?a.swfCSS.swfTimedout:a.swfCSS.swfError);a._processOnReady();a.onerror instanceof Function&& a.onerror.apply(g);return false}if(a.waitForWindowLoad&&!a._windowLoaded){if(g.addEventListener)g.addEventListener("load",a._initUserOnload,false);else g.attachEvent&&g.attachEvent("onload",a._initUserOnload);return false}else a._initUserOnload()};this._addOnReady=function(c,b){a._onready.push({method:c,scope:b||null,fired:false})};this._processOnReady=function(c){if(!a._didInit&&!c)return false;c={success:c?a.supported():!a._disabled};var b=[],d,e,f=!a.useFlashBlock||a.useFlashBlock&&!a.supported(); d=0;for(e=a._onready.length;d<e;d++)a._onready[d].fired!==true&&b.push(a._onready[d]);if(b.length){d=0;for(e=b.length;d<e;d++){b[d].scope?b[d].method.apply(b[d].scope,[c]):b[d].method(c);if(!f)b[d].fired=true}}};this._initUserOnload=function(){g.setTimeout(function(){a.useFlashBlock&&a.flashBlockHandler();a._processOnReady();a.onload.apply(g)})};this.init=function(){a._initMovie();if(a._didInit)return false;if(g.removeEventListener)g.removeEventListener("load",a.beginDelayedInit,false);else g.detachEvent&& g.detachEvent("onload",a.beginDelayedInit);try{a.o._externalInterfaceTest(false);if(a.allowPolling)a._setPolling(true,a.useFastPolling?true:false);a.debugMode||a.o._disableDebug();a.enabled=true}catch(c){a._failSafely(true);a.initComplete();return false}a.initComplete()};this.beginDelayedInit=function(){a._windowLoaded=true;setTimeout(a.waitForExternalInterface,500);setTimeout(a.beginInit,20)};this.beginInit=function(){if(a._initPending)return false;a.createMovie();a._initMovie();return a._initPending= true};this.domContentLoaded=function(){document.removeEventListener&&document.removeEventListener("DOMContentLoaded",a.domContentLoaded,false);a.go()};this._externalInterfaceOK=function(){if(a.swfLoaded)return false;(new Date).getTime();a.swfLoaded=true;a._tryInitOnFocus=false;a.isIE?setTimeout(a.init,100):a.init()};this._setSandboxType=function(c){var b=a.sandbox;b.type=c;b.description=b.types[typeof b.types[c]!=="undefined"?c:"unknown"];if(b.type==="localWithFile"){b.noRemote=true;b.noLocal=false}else if(b.type=== "localWithNetwork"){b.noRemote=false;b.noLocal=true}else if(b.type==="localTrusted"){b.noRemote=false;b.noLocal=false}};this.reboot=function(){for(var c=a.soundIDs.length;c--;)a.sounds[a.soundIDs[c]].destruct();try{if(a.isIE)a.oRemovedHTML=a.o.innerHTML;a.oRemoved=a.o.parentNode.removeChild(a.o)}catch(b){}a.oRemovedHTML=null;a.oRemoved=null;a.enabled=false;a._didInit=false;k._waitingForEI=false;a._initPending=false;a._didAppend=false;a._appendSuccess=false;a._disabled=false;a.swfLoaded=false;a.soundIDs= {};a.sounds=[];a.o=null;for(c=a._onready.length;c--;)a._onready[c].fired=false;g.setTimeout(k.beginDelayedInit,20)};this.destruct=function(){a.disable(true)};l=function(c){var b=this;this.sID=c.id;this.url=c.url;this._iO=this.instanceOptions=this.options=a._mergeObjects(c);this.pan=this.options.pan;this.volume=this.options.volume;this._lastURL=null;this._debug=function(){if(a.debugMode){var d=null,e=[],f,i;for(d in b.options)if(b.options[d]!==null)if(b.options[d]instanceof Function){f=b.options[d].toString(); f=f.replace(/\s\s+/g," ");i=f.indexOf("{");e[e.length]=" "+d+": {"+f.substr(i+1,Math.min(Math.max(f.indexOf("\n")-1,64),64)).replace(/\n/g,"")+"... }"}else e[e.length]=" "+d+": "+b.options[d]}};this._debug();this.id3={};this.resetProperties=function(){b.bytesLoaded=null;b.bytesTotal=null;b.position=null;b.duration=null;b.durationEstimate=null;b.loaded=false;b.playState=0;b.paused=false;b.readyState=0;b.muted=false;b.didBeforeFinish=false;b.didJustBeforeFinish=false;b.isBuffering=false;b.instanceOptions= {};b.instanceCount=0;b.peakData={left:0,right:0};b.waveformData={left:[],right:[]};b.eqData=[];b.eqData.left=[];b.eqData.right=[]};b.resetProperties();this.load=function(d){if(typeof d!=="undefined"){b._iO=a._mergeObjects(d);b.instanceOptions=b._iO}else{d=b.options;b._iO=d;b.instanceOptions=b._iO;if(b._lastURL&&b._lastURL!==b.url){b._iO.url=b.url;b.url=null}}if(typeof b._iO.url==="undefined")b._iO.url=b.url;if(b._iO.url===b.url&&b.readyState!==0&&b.readyState!==2)return false;b.url=b._iO.url;b._lastURL= b._iO.url;b.loaded=false;b.readyState=1;b.playState=0;try{if(a.flashVersion===8)a.o._load(b.sID,b._iO.url,b._iO.stream,b._iO.autoPlay,b._iO.whileloading?1:0);else{a.o._load(b.sID,b._iO.url,b._iO.stream?true:false,b._iO.autoPlay?true:false);b._iO.isMovieStar&&b._iO.autoLoad&&!b._iO.autoPlay&&b.pause()}}catch(e){a.onerror();a.disable()}};this.unload=function(){if(b.readyState!==0){b.readyState!==2&&b.setPosition(0,true);a.o._unload(b.sID,a.nullURL);b.resetProperties()}};this.destruct=function(){a.o._destroySound(b.sID); a.destroySound(b.sID,true)};this.start=this.play=function(d){d||(d={});b._iO=a._mergeObjects(d,b._iO);b._iO=a._mergeObjects(b._iO,b.options);b.instanceOptions=b._iO;if(b.playState===1){d=b._iO.multiShot;if(!d)return false}if(!b.loaded)if(b.readyState===0){b._iO.autoPlay=true;b.load(b._iO)}else if(b.readyState===2)return false;if(b.paused)b.resume();else{b.playState=1;if(!b.instanceCount||a.flashVersion>8)b.instanceCount++;b.position=typeof b._iO.position!=="undefined"&&!isNaN(b._iO.position)?b._iO.position: 0;b._iO.onplay&&b._iO.onplay.apply(b);b.setVolume(b._iO.volume,true);b.setPan(b._iO.pan,true);a.o._start(b.sID,b._iO.loop||1,a.flashVersion===9?b.position:b.position/1E3)}};this.stop=function(d){if(b.playState===1){b.playState=0;b.paused=false;b._iO.onstop&&b._iO.onstop.apply(b);a.o._stop(b.sID,d);b.instanceCount=0;b._iO={}}};this.setPosition=function(d){if(typeof d==="undefined")d=0;d=Math.min(b.duration,Math.max(d,0));b._iO.position=d;a.o._setPosition(b.sID,a.flashVersion===9?b._iO.position:b._iO.position/ 1E3,b.paused||!b.playState)};this.pause=function(){if(b.paused||b.playState===0)return false;b.paused=true;a.o._pause(b.sID);b._iO.onpause&&b._iO.onpause.apply(b)};this.resume=function(){if(!b.paused||b.playState===0)return false;b.paused=false;a.o._pause(b.sID);b._iO.onresume&&b._iO.onresume.apply(b)};this.togglePause=function(){if(b.playState===0){b.play({position:a.flashVersion===9?b.position:b.position/1E3});return false}b.paused?b.resume():b.pause()};this.setPan=function(d,e){if(typeof d==="undefined")d= 0;if(typeof e==="undefined")e=false;a.o._setPan(b.sID,d);b._iO.pan=d;if(!e)b.pan=d};this.setVolume=function(d,e){if(typeof d==="undefined")d=100;if(typeof e==="undefined")e=false;a.o._setVolume(b.sID,a.muted&&!b.muted||b.muted?0:d);b._iO.volume=d;if(!e)b.volume=d};this.mute=function(){b.muted=true;a.o._setVolume(b.sID,0)};this.unmute=function(){b.muted=false;a.o._setVolume(b.sID,typeof b._iO.volume!=="undefined"?b._iO.volume:b.options.volume)};this.toggleMute=function(){b.muted?b.unmute():b.mute()}; this._whileloading=function(d,e,f){if(b._iO.isMovieStar){b.bytesLoaded=d;b.bytesTotal=e;b.duration=Math.floor(f);b.durationEstimate=b.duration}else{b.bytesLoaded=d;b.bytesTotal=e;b.duration=Math.floor(f);b.durationEstimate=parseInt(b.bytesTotal/b.bytesLoaded*b.duration,10);if(b.durationEstimate===undefined)b.durationEstimate=b.duration}b.readyState!==3&&b._iO.whileloading&&b._iO.whileloading.apply(b)};this._onid3=function(d,e){var f=[],i,h;i=0;for(h=d.length;i<h;i++)f[d[i]]=e[i];b.id3=a._mergeObjects(b.id3, f);b._iO.onid3&&b._iO.onid3.apply(b)};this._whileplaying=function(d,e,f,i,h){if(isNaN(d)||d===null)return false;if(b.playState===0&&d>0)d=0;b.position=d;if(a.flashVersion>8){if(b._iO.usePeakData&&typeof e!=="undefined"&&e)b.peakData={left:e.leftPeak,right:e.rightPeak};if(b._iO.useWaveformData&&typeof f!=="undefined"&&f)b.waveformData={left:f.split(","),right:i.split(",")};if(b._iO.useEQData)if(typeof h!=="undefined"&&h.leftEQ){d=h.leftEQ.split(",");b.eqData=d;b.eqData.left=d;if(typeof h.rightEQ!== "undefined"&&h.rightEQ)b.eqData.right=h.rightEQ.split(",")}}if(b.playState===1){b.isBuffering&&b._onbufferchange(0);b._iO.whileplaying&&b._iO.whileplaying.apply(b);b.loaded&&b._iO.onbeforefinish&&b._iO.onbeforefinishtime&&!b.didBeforeFinish&&b.duration-b.position<=b._iO.onbeforefinishtime&&b._onbeforefinish()}};this._onload=function(d){d=d===1?true:false;b.loaded=d;b.readyState=d?3:2;b._iO.onload&&b._iO.onload.apply(b)};this._onbeforefinish=function(){if(!b.didBeforeFinish){b.didBeforeFinish=true; b._iO.onbeforefinish&&b._iO.onbeforefinish.apply(b)}};this._onjustbeforefinish=function(){if(!b.didJustBeforeFinish){b.didJustBeforeFinish=true;b._iO.onjustbeforefinish&&b._iO.onjustbeforefinish.apply(b)}};this._onfinish=function(){b._iO.onbeforefinishcomplete&&b._iO.onbeforefinishcomplete.apply(b);b.didBeforeFinish=false;b.didJustBeforeFinish=false;if(b.instanceCount){b.instanceCount--;if(!b.instanceCount){b.playState=0;b.paused=false;b.instanceCount=0;b.instanceOptions={}}if(!b.instanceCount||b._iO.multiShotEvents)b._iO.onfinish&& b._iO.onfinish.apply(b)}};this._onmetadata=function(d){if(!d.width&&!d.height){d.width=320;d.height=240}b.metadata=d;b.width=d.width;b.height=d.height;b._iO.onmetadata&&b._iO.onmetadata.apply(b)};this._onbufferchange=function(d){if(b.playState===0)return false;if(d===b.isBuffering)return false;b.isBuffering=d===1?true:false;b._iO.onbufferchange&&b._iO.onbufferchange.apply(b)};this._ondataerror=function(){b.playState>0&&b._iO.ondataerror&&b._iO.ondataerror.apply(b)}};this._onfullscreenchange=function(c){a.isFullScreen= c===1?true:false;if(!a.isFullScreen)try{g.focus()}catch(b){}};if(g.addEventListener){g.addEventListener("focus",a.handleFocus,false);g.addEventListener("load",a.beginDelayedInit,false);g.addEventListener("unload",a.destruct,false);a._tryInitOnFocus&&g.addEventListener("mousemove",a.handleFocus,false)}else if(g.attachEvent){g.attachEvent("onfocus",a.handleFocus);g.attachEvent("onload",a.beginDelayedInit);g.attachEvent("unload",a.destruct)}else{k.onerror();k.disable()}document.addEventListener&&document.addEventListener("DOMContentLoaded", a.domContentLoaded,false)}var k=null;if(typeof SM2_DEFER==="undefined"||!SM2_DEFER)k=new n;g.SoundManager=n;g.soundManager=k})(window);
(function(){var c=0,h=[],j={},f={},a={"<":"lt",">":"gt","&":"amp",'"':"quot","'":"#39"},i=/[<>&\"\']/g,b;function e(){this.returnValue=false}function g(){this.cancelBubble=true}(function(k){var l=k.split(/,/),m,o,n;for(m=0;m<l.length;m+=2){n=l[m+1].split(/ /);for(o=0;o<n.length;o++){f[n[o]]=l[m]}}})("application/msword,doc dot,application/pdf,pdf,application/pgp-signature,pgp,application/postscript,ps ai eps,application/rtf,rtf,application/vnd.ms-excel,xls xlb,application/vnd.ms-powerpoint,ppt pps pot,application/zip,zip,application/x-shockwave-flash,swf swfl,application/vnd.openxmlformats,docx pptx xlsx,audio/mpeg,mpga mpega mp2 mp3,audio/x-wav,wav,image/bmp,bmp,image/gif,gif,image/jpeg,jpeg jpg jpe,image/png,png,image/svg+xml,svg svgz,image/tiff,tiff tif,text/html,htm html xhtml,text/rtf,rtf,video/mpeg,mpeg mpg mpe,video/quicktime,qt mov,video/x-flv,flv,video/vnd.rn-realvideo,rv,text/plain,asc txt text diff log,application/octet-stream,exe");var d={STOPPED:1,STARTED:2,QUEUED:1,UPLOADING:2,FAILED:4,DONE:5,GENERIC_ERROR:-100,HTTP_ERROR:-200,IO_ERROR:-300,SECURITY_ERROR:-400,INIT_ERROR:-500,FILE_SIZE_ERROR:-600,FILE_EXTENSION_ERROR:-700,mimeTypes:f,extend:function(k){d.each(arguments,function(l,m){if(m>0){d.each(l,function(o,n){k[n]=o})}});return k},cleanName:function(k){var l,m;m=[/[\300-\306]/g,"A",/[\340-\346]/g,"a",/\307/g,"C",/\347/g,"c",/[\310-\313]/g,"E",/[\350-\353]/g,"e",/[\314-\317]/g,"I",/[\354-\357]/g,"i",/\321/g,"N",/\361/g,"n",/[\322-\330]/g,"O",/[\362-\370]/g,"o",/[\331-\334]/g,"U",/[\371-\374]/g,"u"];for(l=0;l<m.length;l+=2){k=k.replace(m[l],m[l+1])}k=k.replace(/\s+/g,"_");k=k.replace(/[^a-z0-9_\-\.]+/gi,"");return k},addRuntime:function(k,l){l.name=k;h[k]=l;h.push(l);return l},guid:function(){var k=new Date().getTime().toString(32),l;for(l=0;l<5;l++){k+=Math.floor(Math.random()*65535).toString(32)}return(d.guidPrefix||"p")+k+(c++).toString(32)},buildUrl:function(l,k){var m="";d.each(k,function(o,n){m+=(m?"&":"")+encodeURIComponent(n)+"="+encodeURIComponent(o)});if(m){l+=(l.indexOf("?")>0?"&":"?")+m}return l},each:function(n,o){var m,l,k;if(n){m=n.length;if(m===b){for(l in n){if(n.hasOwnProperty(l)){if(o(n[l],l)===false){return}}}}else{for(k=0;k<m;k++){if(o(n[k],k)===false){return}}}}},formatSize:function(k){if(k===b){return d.translate("N/A")}if(k>1048576){return Math.round(k/1048576,1)+" MB"}if(k>1024){return Math.round(k/1024,1)+" KB"}return k+" b"},getPos:function(l,p){var q=0,o=0,s,r=document,m,n;l=l;p=p||r.body;function k(w){var u,v,t=0,z=0;if(w){v=w.getBoundingClientRect();u=r.compatMode==="CSS1Compat"?r.documentElement:r.body;t=v.left+u.scrollLeft;z=v.top+u.scrollTop}return{x:t,y:z}}if(l.getBoundingClientRect&&(navigator.userAgent.indexOf("MSIE")>0&&r.documentMode!==8)){m=k(l);n=k(p);return{x:m.x-n.x,y:m.y-n.y}}s=l;while(s&&s!=p&&s.nodeType){q+=s.offsetLeft||0;o+=s.offsetTop||0;s=s.offsetParent}s=l.parentNode;while(s&&s!=p&&s.nodeType){q-=s.scrollLeft||0;o-=s.scrollTop||0;s=s.parentNode}return{x:q,y:o}},getSize:function(k){return{w:k.clientWidth||k.offsetWidth,h:k.clientHeight||k.offsetHeight}},parseSize:function(k){var l;if(typeof(k)=="string"){k=/^([0-9]+)([mgk]+)$/.exec(k.toLowerCase().replace(/[^0-9mkg]/g,""));l=k[2];k=+k[1];if(l=="g"){k*=1073741824}if(l=="m"){k*=1048576}if(l=="k"){k*=1024}}return k},xmlEncode:function(k){return k?(""+k).replace(i,function(l){return a[l]?"&"+a[l]+";":l}):k},toArray:function(m){var l,k=[];for(l=0;l<m.length;l++){k[l]=m[l]}return k},addI18n:function(k){return d.extend(j,k)},translate:function(k){return j[k]||k},addEvent:function(l,k,m){if(l.attachEvent){l.attachEvent("on"+k,function(){var n=window.event;if(!n.target){n.target=n.srcElement}n.preventDefault=e;n.stopPropagation=g;m(n)})}else{if(l.addEventListener){l.addEventListener(k,m,false)}}}};d.Uploader=function(n){var l={},q,p=[],r,m;q=new d.QueueProgress();n=d.extend({chunk_size:0,max_file_size:"1gb",multi_selection:true,file_data_name:"file",filters:[]},n);function o(){var s;if(this.state==d.STARTED&&r<p.length){s=p[r++];if(s.status==d.QUEUED){this.trigger("UploadFile",s)}else{o.call(this)}}else{this.stop()}}function k(){var t,s;q.reset();for(t=0;t<p.length;t++){s=p[t];if(s.size!==b){q.size+=s.size;q.loaded+=s.loaded}else{q.size=b}if(s.status==d.DONE){q.uploaded++}else{if(s.status==d.FAILED){q.failed++}else{q.queued++}}}if(q.size===b){q.percent=p.length>0?Math.ceil(q.uploaded/p.length*100):0}else{q.bytesPerSec=Math.ceil(q.loaded/((+new Date()-m||1)/1000));q.percent=q.size>0?Math.ceil(q.loaded/q.size*100):0}}d.extend(this,{state:d.STOPPED,features:{},files:p,settings:n,total:q,id:d.guid(),init:function(){var x=this,y,u,t,w=0,v;n.page_url=n.page_url||document.location.pathname.replace(/\/[^\/]+$/g,"/");if(!/^(\w+:\/\/|\/)/.test(n.url)){n.url=n.page_url+n.url}n.chunk_size=d.parseSize(n.chunk_size);n.max_file_size=d.parseSize(n.max_file_size);x.bind("FilesAdded",function(z,C){var B,A,F=0,E,D=n.filters;if(D&&D.length){E={};d.each(D,function(G){d.each(G.extensions.split(/,/),function(H){E[H.toLowerCase()]=true})})}for(B=0;B<C.length;B++){A=C[B];A.loaded=0;A.percent=0;A.status=d.QUEUED;if(E&&!E[A.name.toLowerCase().split(".").slice(-1)]){z.trigger("Error",{code:d.FILE_EXTENSION_ERROR,message:"File extension error.",file:A});continue}if(A.size!==b&&A.size>n.max_file_size){z.trigger("Error",{code:d.FILE_SIZE_ERROR,message:"File size error.",file:A});continue}p.push(A);F++}if(F){x.trigger("QueueChanged");x.refresh()}});if(n.unique_names){x.bind("UploadFile",function(z,A){A.target_name=A.id+".tmp"})}x.bind("UploadProgress",function(z,A){if(A.status==d.QUEUED){A.status=d.UPLOADING}A.percent=A.size>0?Math.ceil(A.loaded/A.size*100):100;k()});x.bind("StateChanged",function(z){if(z.state==d.STARTED){m=(+new Date())}});x.bind("QueueChanged",k);x.bind("Error",function(z,A){if(A.file){A.file.status=d.FAILED;k();window.setTimeout(function(){o.call(x)})}});x.bind("FileUploaded",function(z,A){A.status=d.DONE;z.trigger("UploadProgress",A);o.call(x)});if(n.runtimes){u=[];v=n.runtimes.split(/\s?,\s?/);for(y=0;y<v.length;y++){if(h[v[y]]){u.push(h[v[y]])}}}else{u=h}function s(){var C=u[w++],B,z,A;if(C){B=C.getFeatures();z=x.settings.required_features;if(z){z=z.split(",");for(A=0;A<z.length;A++){if(!B[z[A]]){s();return}}}C.init(x,function(D){if(D&&D.success){x.features=B;x.trigger("Init",{runtime:C.name});x.trigger("PostInit");x.refresh()}else{s()}})}else{x.trigger("Error",{code:d.INIT_ERROR,message:"Init error."})}}s()},refresh:function(){this.trigger("Refresh")},start:function(){if(this.state!=d.STARTED){r=0;this.state=d.STARTED;this.trigger("StateChanged");o.call(this)}},stop:function(){if(this.state!=d.STOPPED){this.state=d.STOPPED;this.trigger("StateChanged")}},getFile:function(t){var s;for(s=p.length-1;s>=0;s--){if(p[s].id===t){return p[s]}}},removeFile:function(t){var s;for(s=p.length-1;s>=0;s--){if(p[s].id===t.id){return this.splice(s,1)[0]}}},splice:function(u,s){var t;t=p.splice(u,s);this.trigger("FilesRemoved",t);this.trigger("QueueChanged");return t},trigger:function(t){var v=l[t.toLowerCase()],u,s;if(v){s=Array.prototype.slice.call(arguments);s[0]=this;for(u=0;u<v.length;u++){if(v[u].func.apply(v[u].scope,s)===false){return false}}}return true},bind:function(s,u,t){var v;s=s.toLowerCase();v=l[s]||[];v.push({func:u,scope:t||this});l[s]=v},unbind:function(s,u){var v=l[s.toLowerCase()],t;if(v){for(t=v.length-1;t>=0;t--){if(v[t].func===u){v.splice(t,1)}}}}})};d.File=function(n,l,m){var k=this;k.id=n;k.name=l;k.size=m;k.loaded=0;k.percent=0;k.status=0};d.Runtime=function(){this.getFeatures=function(){};this.init=function(k,l){}};d.QueueProgress=function(){var k=this;k.size=0;k.loaded=0;k.uploaded=0;k.failed=0;k.queued=0;k.percent=0;k.bytesPerSec=0;k.reset=function(){k.size=k.loaded=k.uploaded=k.failed=k.queued=k.percent=k.bytesPerSec=0}};d.runtimes={};window.plupload=d})();(function(b){var c={};function a(i,e,k,j,d){var l,g,f,h;g=google.gears.factory.create("beta.canvas");g.decode(i);h=Math.min(e/g.width,k/g.height);if(h<1){e=Math.round(g.width*h);k=Math.round(g.height*h)}else{e=g.width;k=g.height}g.resize(e,k);return g.encode(d,{quality:j/100})}b.runtimes.Gears=b.addRuntime("gears",{getFeatures:function(){return{dragdrop:true,jpgresize:true,pngresize:true,chunks:true,progress:true,multipart:true}},init:function(g,i){var h;if(!window.google||!google.gears){return i({success:false})}try{h=google.gears.factory.create("beta.desktop")}catch(f){return i({success:false})}function d(k){var j,e,l=[],m;for(e=0;e<k.length;e++){j=k[e];m=b.guid();c[m]=j.blob;l.push(new b.File(m,j.name,j.blob.length))}g.trigger("FilesAdded",l)}g.bind("PostInit",function(){var j=g.settings,e=document.getElementById(j.drop_element);if(e){b.addEvent(e,"dragover",function(k){h.setDropEffect(k,"copy");k.preventDefault()});b.addEvent(e,"drop",function(l){var k=h.getDragData(l,"application/x-gears-files");if(k){d(k.files)}l.preventDefault()});e=0}b.addEvent(document.getElementById(j.browse_button),"click",function(o){var n=[],l,k,m;o.preventDefault();for(l=0;l<j.filters.length;l++){m=j.filters[l].extensions.split(",");for(k=0;k<m.length;k++){n.push("."+m[k])}}h.openFiles(d,{singleFile:!j.multi_selection,filter:n})})});g.bind("UploadFile",function(o,l){var q=0,p,m,n=0,k=o.settings.resize,e;m=o.settings.chunk_size;e=m>0;p=Math.ceil(l.size/m);if(!e){m=l.size;p=1}if(k&&/\.(png|jpg|jpeg)$/i.test(l.name)){c[l.id]=a(c[l.id],k.width,k.height,k.quality||90,/\.png$/i.test(l.name)?"image/png":"image/jpeg")}l.size=c[l.id].length;function j(){var u,w,s=o.settings.multipart,r=0,v={name:l.target_name||l.name};function t(y){var x,C="----pluploadboundary"+b.guid(),A="--",B="\r\n",z;if(s){u.setRequestHeader("Content-Type","multipart/form-data; boundary="+C);x=google.gears.factory.create("beta.blobbuilder");b.each(o.settings.multipart_params,function(E,D){x.append(A+C+B+'Content-Disposition: form-data; name="'+D+'"'+B+B);x.append(E+B)});x.append(A+C+B+'Content-Disposition: form-data; name="'+o.settings.file_data_name+'"; filename="'+l.name+'"'+B+"Content-Type: application/octet-stream"+B+B);x.append(y);x.append(B+A+C+A+B);z=x.getAsBlob();r=z.length-y.length;y=z}u.send(y)}if(l.status==b.DONE||l.status==b.FAILED||o.state==b.STOPPED){return}if(e){v.chunk=q;v.chunks=p}w=Math.min(m,l.size-(q*m));u=google.gears.factory.create("beta.httprequest");u.open("POST",b.buildUrl(o.settings.url,v));if(!s){u.setRequestHeader("Content-Disposition",'attachment; filename="'+l.name+'"');u.setRequestHeader("Content-Type","application/octet-stream")}b.each(o.settings.headers,function(y,x){u.setRequestHeader(x,y)});u.upload.onprogress=function(x){l.loaded=n+x.loaded-r;o.trigger("UploadProgress",l)};u.onreadystatechange=function(){var x;if(u.readyState==4){if(u.status==200){x={chunk:q,chunks:p,response:u.responseText,status:u.status};o.trigger("ChunkUploaded",l,x);if(x.cancelled){l.status=b.FAILED;return}n+=w;if(++q>=p){l.status=b.DONE;o.trigger("FileUploaded",l,{response:u.responseText,status:u.status})}else{j()}}else{o.trigger("Error",{code:b.HTTP_ERROR,message:"HTTP Error.",file:l,chunk:q,chunks:p,status:u.status})}}};if(q<p){t(c[l.id].slice(q*m,w))}}j()});i({success:true})}})})(plupload);(function(c){var a={};function b(l){var k,j=typeof l,h,e,g,f;if(j==="string"){k="\bb\tt\nn\ff\rr\"\"''\\\\";return'"'+l.replace(/([\u0080-\uFFFF\x00-\x1f\"])/g,function(n,m){var i=k.indexOf(m);if(i+1){return"\\"+k.charAt(i+1)}n=m.charCodeAt().toString(16);return"\\u"+"0000".substring(n.length)+n})+'"'}if(j=="object"){e=l.length!==h;k="";if(e){for(g=0;g<l.length;g++){if(k){k+=","}k+=b(l[g])}k="["+k+"]"}else{for(f in l){if(l.hasOwnProperty(f)){if(k){k+=","}k+=b(f)+":"+b(l[f])}}k="{"+k+"}"}return k}if(l===h){return"null"}return""+l}function d(o){var r=false,f=null,k=null,g,h,i,q,j,m=0;try{try{k=new ActiveXObject("AgControl.AgControl");if(k.IsVersionSupported(o)){r=true}k=null}catch(n){var l=navigator.plugins["Silverlight Plug-In"];if(l){g=l.description;if(g==="1.0.30226.2"){g="2.0.30226.2"}h=g.split(".");while(h.length>3){h.pop()}while(h.length<4){h.push(0)}i=o.split(".");while(i.length>4){i.pop()}do{q=parseInt(i[m],10);j=parseInt(h[m],10);m++}while(m<i.length&&q===j);if(q<=j&&!isNaN(q)){r=true}}}}catch(p){r=false}return r}c.silverlight={trigger:function(j,f){var h=a[j],g,e;if(h){e=c.toArray(arguments).slice(1);e[0]="Silverlight:"+f;setTimeout(function(){h.trigger.apply(h,e)},0)}}};c.runtimes.Silverlight=c.addRuntime("silverlight",{getFeatures:function(){return{jpgresize:true,pngresize:true,chunks:true,progress:true,multipart:true}},init:function(l,m){var k,h="",j=l.settings.filters,g,f=document.body;if(!d("2.0.31005.0")||(window.opera&&window.opera.buildNumber)){m({success:false});return}a[l.id]=l;k=document.createElement("div");k.id=l.id+"_silverlight_container";c.extend(k.style,{position:"absolute",top:"0px",background:l.settings.shim_bgcolor||"transparent",zIndex:99999,width:"100px",height:"100px",overflow:"hidden",opacity:l.settings.shim_bgcolor?"":0.01});k.className="plupload silverlight";if(l.settings.container){f=document.getElementById(l.settings.container);f.style.position="relative"}f.appendChild(k);for(g=0;g<j.length;g++){h+=(h!=""?"|":"")+j[g].title+" | *."+j[g].extensions.replace(/,/g,";*.")}k.innerHTML='<object id="'+l.id+'_silverlight" data="data:application/x-silverlight," type="application/x-silverlight-2" style="outline:none;" width="1024" height="1024"><param name="source" value="'+l.settings.silverlight_xap_url+'"/><param name="background" value="Transparent"/><param name="windowless" value="true"/><param name="initParams" value="id='+l.id+",filter="+h+'"/></object>';function e(){return document.getElementById(l.id+"_silverlight").content.Upload}l.bind("Silverlight:Init",function(){var i,n={};l.bind("Silverlight:StartSelectFiles",function(o){i=[]});l.bind("Silverlight:SelectFile",function(o,r,p,q){var s;s=c.guid();n[s]=r;n[r]=s;i.push(new c.File(s,p,q))});l.bind("Silverlight:SelectSuccessful",function(){if(i.length){l.trigger("FilesAdded",i)}});l.bind("Silverlight:UploadChunkError",function(o,r,p,s,q){l.trigger("Error",{code:c.IO_ERROR,message:"IO Error.",details:q,file:o.getFile(n[r])})});l.bind("Silverlight:UploadFileProgress",function(o,s,p,r){var q=o.getFile(n[s]);if(q.status!=c.FAILED){q.size=r;q.loaded=p;o.trigger("UploadProgress",q)}});l.bind("Refresh",function(o){var p,q,r;p=document.getElementById(o.settings.browse_button);q=c.getPos(p,document.getElementById(o.settings.container));r=c.getSize(p);c.extend(document.getElementById(o.id+"_silverlight_container").style,{top:q.y+"px",left:q.x+"px",width:r.w+"px",height:r.h+"px"})});l.bind("Silverlight:UploadChunkSuccessful",function(o,r,p,u,t){var s,q=o.getFile(n[r]);s={chunk:p,chunks:u,response:t};o.trigger("ChunkUploaded",q,s);if(q.status!=c.FAILED){e().UploadNextChunk()}if(p==u-1){q.status=c.DONE;o.trigger("FileUploaded",q,{response:t})}});l.bind("Silverlight:UploadSuccessful",function(o,r,p){var q=o.getFile(n[r]);q.status=c.DONE;o.trigger("FileUploaded",q,{response:p})});l.bind("FilesRemoved",function(o,q){var p;for(p=0;p<q.length;p++){e().RemoveFile(n[q[p].id])}});l.bind("UploadFile",function(o,q){var r=o.settings,p=r.resize||{};e().UploadFile(n[q.id],c.buildUrl(o.settings.url,{name:q.target_name||q.name}),b({chunk_size:r.chunk_size,image_width:p.width,image_height:p.height,image_quality:p.quality||90,multipart:!!r.multipart,multipart_params:r.multipart_params||{},headers:r.headers}))});m({success:true})})}})})(plupload);(function(c){var a={};function b(){var d;try{d=navigator.plugins["Shockwave Flash"];d=d.description}catch(f){try{d=new ActiveXObject("ShockwaveFlash.ShockwaveFlash").GetVariable("$version")}catch(e){d="0.0"}}d=d.match(/\d+/g);return parseFloat(d[0]+"."+d[1])}c.flash={trigger:function(f,d,e){setTimeout(function(){var j=a[f],h,g;if(j){j.trigger("Flash:"+d,e)}},0)}};c.runtimes.Flash=c.addRuntime("flash",{getFeatures:function(){return{jpgresize:true,pngresize:true,chunks:true,progress:true,multipart:true}},init:function(g,l){var k,f,h,e,m=0,d=document.body;if(b()<10){l({success:false});return}a[g.id]=g;k=document.getElementById(g.settings.browse_button);f=document.createElement("div");f.id=g.id+"_flash_container";c.extend(f.style,{position:"absolute",top:"0px",background:g.settings.shim_bgcolor||"transparent",zIndex:99999,width:"100%",height:"100%"});f.className="plupload flash";if(g.settings.container){d=document.getElementById(g.settings.container);d.style.position="relative"}d.appendChild(f);h="id="+escape(g.id);f.innerHTML='<object id="'+g.id+'_flash" width="100%" height="100%" style="outline:0" type="application/x-shockwave-flash" data="'+g.settings.flash_swf_url+'"><param name="movie" value="'+g.settings.flash_swf_url+'" /><param name="flashvars" value="'+h+'" /><param name="wmode" value="transparent" /><param name="allowscriptaccess" value="always" /></object>';function j(){return document.getElementById(g.id+"_flash")}function i(){if(m++>5000){l({success:false});return}if(!e){setTimeout(i,1)}}i();k=f=null;g.bind("Flash:Init",function(){var p={},o,n=g.settings.resize||{};e=true;j().setFileFilters(g.settings.filters,g.settings.multi_selection);g.bind("UploadFile",function(q,r){var s=q.settings;j().uploadFile(p[r.id],c.buildUrl(s.url,{name:r.target_name||r.name}),{chunk_size:s.chunk_size,width:n.width,height:n.height,quality:n.quality||90,multipart:s.multipart,multipart_params:s.multipart_params,file_data_name:s.file_data_name,format:/\.(jpg|jpeg)$/i.test(r.name)?"jpg":"png",headers:s.headers})});g.bind("Flash:UploadProcess",function(r,q){var s=r.getFile(p[q.id]);if(s.status!=c.FAILED){s.loaded=q.loaded;s.size=q.size;r.trigger("UploadProgress",s)}});g.bind("Flash:UploadChunkComplete",function(q,s){var t,r=q.getFile(p[s.id]);t={chunk:s.chunk,chunks:s.chunks,response:s.text};q.trigger("ChunkUploaded",r,t);if(r.status!=c.FAILED){j().uploadNextChunk()}if(s.chunk==s.chunks-1){r.status=c.DONE;q.trigger("FileUploaded",r,{response:s.text})}});g.bind("Flash:SelectFiles",function(q,t){var s,r,u=[],v;for(r=0;r<t.length;r++){s=t[r];v=c.guid();p[v]=s.id;p[s.id]=v;u.push(new c.File(v,s.name,s.size))}if(u.length){g.trigger("FilesAdded",u)}});g.bind("Flash:SecurityError",function(q,r){g.trigger("Error",{code:c.SECURITY_ERROR,message:"Security error.",details:r.message,file:g.getFile(p[r.id])})});g.bind("Flash:GenericError",function(q,r){g.trigger("Error",{code:c.GENERIC_ERROR,message:"Generic error.",details:r.message,file:g.getFile(p[r.id])})});g.bind("Flash:IOError",function(q,r){g.trigger("Error",{code:c.IO_ERROR,message:"IO error.",details:r.message,file:g.getFile(p[r.id])})});g.bind("QueueChanged",function(q){g.refresh()});g.bind("FilesRemoved",function(q,s){var r;for(r=0;r<s.length;r++){j().removeFile(p[s[r].id])}});g.bind("StateChanged",function(q){g.refresh()});g.bind("Refresh",function(q){var r,s,t;j().setFileFilters(g.settings.filters,g.settings.multi_selection);r=document.getElementById(q.settings.browse_button);s=c.getPos(r,document.getElementById(q.settings.container));t=c.getSize(r);c.extend(document.getElementById(q.id+"_flash_container").style,{top:s.y+"px",left:s.x+"px",width:t.w+"px",height:t.h+"px"})});l({success:true})})}})})(plupload);(function(a){a.runtimes.BrowserPlus=a.addRuntime("browserplus",{getFeatures:function(){return{dragdrop:true,jpgresize:true,pngresize:true,chunks:true,progress:true,multipart:true}},init:function(g,i){var e=window.BrowserPlus,h={},d=g.settings,c=d.resize;function f(n){var m,l,j=[],k,o;for(l=0;l<n.length;l++){k=n[l];o=a.guid();h[o]=k;j.push(new a.File(o,k.name,k.size))}if(l){g.trigger("FilesAdded",j)}}function b(){g.bind("PostInit",function(){var m,k=d.drop_element,o=g.id+"_droptarget",j=document.getElementById(k),l;function p(r,q){e.DragAndDrop.AddDropTarget({id:r},function(s){e.DragAndDrop.AttachCallbacks({id:r,hover:function(t){if(!t&&q){q()}},drop:function(t){if(q){q()}f(t)}},function(){})})}function n(){document.getElementById(o).style.top="-1000px"}if(j){if(document.attachEvent&&(/MSIE/gi).test(navigator.userAgent)){m=document.createElement("div");m.setAttribute("id",o);a.extend(m.style,{position:"absolute",top:"-1000px",background:"red",filter:"alpha(opacity=0)",opacity:0});document.body.appendChild(m);a.addEvent(j,"dragenter",function(r){var q,s;q=document.getElementById(k);s=a.getPos(q);a.extend(document.getElementById(o).style,{top:s.y+"px",left:s.x+"px",width:q.offsetWidth+"px",height:q.offsetHeight+"px"})});p(o,n)}else{p(k)}}a.addEvent(document.getElementById(d.browse_button),"click",function(v){var t=[],r,q,u=d.filters,s;v.preventDefault();for(r=0;r<u.length;r++){s=u[r].extensions.split(",");for(q=0;q<s.length;q++){t.push(a.mimeTypes[s[q]])}}e.FileBrowse.OpenBrowseDialog({mimeTypes:t},function(w){if(w.success){f(w.value)}})});j=m=null});g.bind("UploadFile",function(n,k){var m=h[k.id],j={},l=n.settings.chunk_size,o,p=[];function r(s,u){var t;if(k.status==a.FAILED){return}j.name=k.target_name||k.name;if(l){j.chunk=s;j.chunks=u}t=p.shift();e.Uploader.upload({url:a.buildUrl(n.settings.url,j),files:{file:t},cookies:document.cookies,postvars:n.settings.multipart_params,progressCallback:function(x){var w,v=0;o[s]=parseInt(x.filePercent*t.size/100,10);for(w=0;w<o.length;w++){v+=o[w]}k.loaded=v;n.trigger("UploadProgress",k)}},function(w){var v,x;if(w.success){v=w.value.statusCode;if(l){n.trigger("ChunkUploaded",k,{chunk:s,chunks:u,response:w.value.body,status:v})}if(p.length>0){r(++s,u)}else{k.status=a.DONE;n.trigger("FileUploaded",k,{response:w.value.body,status:v});if(v>=400){n.trigger("Error",{code:a.HTTP_ERROR,message:"HTTP Error.",file:k,status:v})}}}else{n.trigger("Error",{code:a.GENERIC_ERROR,message:"Generic Error.",file:k,details:w.error})}})}function q(s){k.size=s.size;if(l){e.FileAccess.chunk({file:s,chunkSize:l},function(v){if(v.success){var w=v.value,t=w.length;o=Array(t);for(var u=0;u<t;u++){o[u]=0;p.push(w[u])}r(0,t)}})}else{o=Array(1);p.push(s);r(0,1)}}if(c&&/\.(png|jpg|jpeg)$/i.test(k.name)){BrowserPlus.ImageAlter.transform({file:m,quality:c.quality||90,actions:[{scale:{maxwidth:c.width,maxheight:c.height}}]},function(s){if(s.success){q(s.value.file)}})}else{q(m)}});i({success:true})}if(e){e.init(function(k){var j=[{service:"Uploader",version:"3"},{service:"DragAndDrop",version:"1"},{service:"FileBrowse",version:"1"},{service:"FileAccess",version:"2"}];if(c){j.push({service:"ImageAlter",version:"4"})}if(k.success){e.require({services:j},function(l){if(l.success){b()}else{i()}})}else{i()}})}else{i()}}})})(plupload);(function(b){function a(i,l,j,c,k){var e,d,h,g,f;e=document.createElement("canvas");e.style.display="none";document.body.appendChild(e);d=e.getContext("2d");h=new Image();h.onload=function(){var o,m,n;f=Math.min(l/h.width,j/h.height);if(f<1){o=Math.round(h.width*f);m=Math.round(h.height*f)}else{o=h.width;m=h.height}e.width=o;e.height=m;d.drawImage(h,0,0,o,m);g=e.toDataURL(c);g=g.substring(g.indexOf("base64,")+7);g=atob(g);e.parentNode.removeChild(e);k({success:true,data:g})};h.src=i}b.runtimes.Html5=b.addRuntime("html5",{getFeatures:function(){var g,d,f,e,c;d=f=e=c=false;if(window.XMLHttpRequest){g=new XMLHttpRequest();f=!!g.upload;d=!!(g.sendAsBinary||g.upload)}if(d){e=!!(File&&File.prototype.getAsDataURL);c=!!(File&&File.prototype.slice)}return{html5:d,dragdrop:window.mozInnerScreenX!==undefined||c,jpgresize:e,pngresize:e,multipart:e,progress:f}},init:function(e,f){var c={};function d(k){var h,g,j=[],l;for(g=0;g<k.length;g++){h=k[g];l=b.guid();c[l]=h;j.push(new b.File(l,h.fileName,h.fileSize))}if(j.length){e.trigger("FilesAdded",j)}}if(!this.getFeatures().html5){f({success:false});return}e.bind("Init",function(l){var p,n=[],k,o,h=l.settings.filters,j,m,g=document.body;p=document.createElement("div");p.id=l.id+"_html5_container";for(k=0;k<h.length;k++){j=h[k].extensions.split(/,/);for(o=0;o<j.length;o++){m=b.mimeTypes[j[o]];if(m){n.push(m)}}}b.extend(p.style,{position:"absolute",background:e.settings.shim_bgcolor||"transparent",width:"100px",height:"100px",overflow:"hidden",zIndex:99999,opacity:e.settings.shim_bgcolor?"":0});p.className="plupload html5";if(e.settings.container){g=document.getElementById(e.settings.container);g.style.position="relative"}g.appendChild(p);p.innerHTML='<input id="'+e.id+'_html5" style="width:100%;" type="file" accept="'+n.join(",")+'" '+(e.settings.multi_selection?'multiple="multiple"':"")+" />";document.getElementById(e.id+"_html5").onchange=function(){d(this.files);this.value=""}});e.bind("PostInit",function(){var g=document.getElementById(e.settings.drop_element);if(g){b.addEvent(g,"dragover",function(h){h.preventDefault()});b.addEvent(g,"drop",function(i){var h=i.dataTransfer;if(h&&h.files){d(h.files)}i.preventDefault()})}});e.bind("Refresh",function(g){var h,i,j;h=document.getElementById(e.settings.browse_button);i=b.getPos(h,document.getElementById(g.settings.container));j=b.getSize(h);b.extend(document.getElementById(e.id+"_html5_container").style,{top:i.y+"px",left:i.x+"px",width:j.w+"px",height:j.h+"px"})});e.bind("UploadFile",function(g,j){var n=new XMLHttpRequest(),i=n.upload,h=g.settings.resize,m,l=0;function k(o){var s="----pluploadboundary"+b.guid(),q="--",r="\r\n",p="";if(g.settings.multipart){n.setRequestHeader("Content-Type","multipart/form-data; boundary="+s);b.each(g.settings.multipart_params,function(u,t){p+=q+s+r+'Content-Disposition: form-data; name="'+t+'"'+r+r;p+=u+r});p+=q+s+r+'Content-Disposition: form-data; name="'+g.settings.file_data_name+'"; filename="'+j.name+'"'+r+"Content-Type: application/octet-stream"+r+r+o+r+q+s+q+r;l=p.length-o.length;o=p}n.sendAsBinary(o)}if(j.status==b.DONE||j.status==b.FAILED||g.state==b.STOPPED){return}if(i){i.onprogress=function(o){j.loaded=o.loaded-l;g.trigger("UploadProgress",j)}}n.onreadystatechange=function(){var o;if(n.readyState==4){try{o=n.status}catch(p){o=0}j.status=b.DONE;j.loaded=j.size;g.trigger("UploadProgress",j);g.trigger("FileUploaded",j,{response:n.responseText,status:o});if(o>=400){g.trigger("Error",{code:b.HTTP_ERROR,message:"HTTP Error.",file:j,status:o})}}};n.open("post",b.buildUrl(g.settings.url,{name:j.target_name||j.name}),true);n.setRequestHeader("Content-Type","application/octet-stream");b.each(g.settings.headers,function(p,o){n.setRequestHeader(o,p)});m=c[j.id];if(n.sendAsBinary){if(h&&/\.(png|jpg|jpeg)$/i.test(j.name)){a(m.getAsDataURL(),h.width,h.height,/\.png$/i.test(j.name)?"image/png":"image/jpeg",function(o){if(o.success){j.size=o.data.length;k(o.data)}else{k(m.getAsBinary())}})}else{k(m.getAsBinary())}}else{n.send(m)}});f({success:true})}})})(plupload);(function(a){a.runtimes.Html4=a.addRuntime("html4",{getFeatures:function(){return{multipart:true}},init:function(f,g){var d={},c,b;function e(l){var k,j,m=[],n,h;h=l.value.replace(/\\/g,"/");h=h.substring(h.length,h.lastIndexOf("/")+1);n=a.guid();k=new a.File(n,h);d[n]=k;k.input=l;m.push(k);if(m.length){f.trigger("FilesAdded",m)}}f.bind("Init",function(p){var h,x,v,t=[],o,u,m=p.settings.filters,l,s,r=/MSIE/.test(navigator.userAgent),k="javascript",w,j=document.body,n;if(f.settings.container){j=document.getElementById(f.settings.container);j.style.position="relative"}c=(typeof p.settings.form=="string")?document.getElementById(p.settings.form):p.settings.form;if(!c){n=document.getElementById(f.settings.browse_button);for(;n;n=n.parentNode){if(n.nodeName=="FORM"){c=n}}}if(!c){c=document.createElement("form");c.style.display="inline";n=document.getElementById(f.settings.container);n.parentNode.insertBefore(c,n);c.appendChild(n)}c.setAttribute("method","post");c.setAttribute("enctype","multipart/form-data");a.each(p.settings.multipart_params,function(z,y){var i=document.createElement("input");a.extend(i,{type:"hidden",name:y,value:z});c.appendChild(i)});b=document.createElement("iframe");b.setAttribute("src",k+':""');b.setAttribute("name",p.id+"_iframe");b.setAttribute("id",p.id+"_iframe");b.style.display="none";a.addEvent(b,"load",function(B){var C=B.target,z=f.currentfile,A;try{A=C.contentWindow.document||C.contentDocument||window.frames[C.id].document}catch(y){p.trigger("Error",{code:a.SECURITY_ERROR,message:"Security error.",file:z});return}if(A.location.href=="about:blank"||!z){return}var i=A.documentElement.innerText||A.documentElement.textContent;if(i!=""){z.status=a.DONE;z.loaded=1025;z.percent=100;if(z.input){z.input.removeAttribute("name")}p.trigger("UploadProgress",z);p.trigger("FileUploaded",z,{response:i});if(c.tmpAction){c.setAttribute("action",c.tmpAction)}if(c.tmpTarget){c.setAttribute("target",c.tmpTarget)}}});c.appendChild(b);if(r){window.frames[b.id].name=b.name}x=document.createElement("div");x.id=p.id+"_iframe_container";for(o=0;o<m.length;o++){l=m[o].extensions.split(/,/);for(u=0;u<l.length;u++){s=a.mimeTypes[l[u]];if(s){t.push(s)}}}a.extend(x.style,{position:"absolute",background:"transparent",width:"100px",height:"100px",overflow:"hidden",zIndex:99999,opacity:0});w=f.settings.shim_bgcolor;if(w){a.extend(x.style,{background:w,opacity:1})}x.className="plupload_iframe";j.appendChild(x);function q(){v=document.createElement("input");v.setAttribute("type","file");v.setAttribute("accept",t.join(","));v.setAttribute("size",1);a.extend(v.style,{width:"100%",height:"100%",opacity:0});if(r){a.extend(v.style,{filter:"alpha(opacity=0)"})}a.addEvent(v,"change",function(i){var y=i.target;if(y.value){q();y.style.display="none";e(y)}});x.appendChild(v);return true}q()});f.bind("Refresh",function(h){var i,j,k;i=document.getElementById(f.settings.browse_button);j=a.getPos(i,document.getElementById(h.settings.container));k=a.getSize(i);a.extend(document.getElementById(f.id+"_iframe_container").style,{top:j.y+"px",left:j.x+"px",width:k.w+"px",height:k.h+"px"})});f.bind("UploadFile",function(h,i){if(i.status==a.DONE||i.status==a.FAILED||h.state==a.STOPPED){return}if(!i.input){i.status=a.ERROR;return}i.input.setAttribute("name",h.settings.file_data_name);c.tmpAction=c.getAttribute("action");c.setAttribute("action",a.buildUrl(h.settings.url,{name:i.target_name||i.name}));c.tmpTarget=c.getAttribute("target");c.setAttribute("target",b.name);this.currentfile=i;c.submit()});f.bind("FilesRemoved",function(h,k){var j,l;for(j=0;j<k.length;j++){l=k[j].input;if(l){l.parentNode.removeChild(l)}}});g({success:true})}})})(plupload);
(function(c){var d={};function a(e){return plupload.translate(e)||e}function b(f,e){e.contents().each(function(g,h){h=c(h);if(!h.is(".plupload")){h.remove()}});e.prepend('<div class="plupload_wrapper plupload_scroll"><div id="'+f+'_container" class="plupload_container"><div class="plupload"><div class="plupload_header"><div class="plupload_header_content"><div class="plupload_header_title">'+a("Select files")+'</div><div class="plupload_header_text">'+a("Add files to the upload queue and click the start button.")+'</div></div></div><div class="plupload_content"><div class="plupload_filelist_header"><div class="plupload_file_name">'+a("Filename")+'</div><div class="plupload_file_action">&nbsp;</div><div class="plupload_file_status"><span>'+a("Status")+'</span></div><div class="plupload_file_size">'+a("Size")+'</div><div class="plupload_clearer">&nbsp;</div></div><ul id="'+f+'_filelist" class="plupload_filelist"></ul><div class="plupload_filelist_footer"><div class="plupload_file_name"><div class="plupload_buttons"><a href="#" class="plupload_button plupload_add">'+a("Add files")+'</a><a href="#" class="plupload_button plupload_start">'+a("Start upload")+'</a></div><span class="plupload_upload_status"></span></div><div class="plupload_file_action"></div><div class="plupload_file_status"><span class="plupload_total_status">0%</span></div><div class="plupload_file_size"><span class="plupload_total_file_size">0 b</span></div><div class="plupload_progress"><div class="plupload_progress_container"><div class="plupload_progress_bar"></div></div></div><div class="plupload_clearer">&nbsp;</div></div></div></div></div><input type="hidden" id="'+f+'_count" name="'+f+'_count" value="0" /></div>')}c.fn.pluploadQueue=function(e){if(e){this.each(function(){var j,i,k;i=c(this);k=i.attr("id");if(!k){k=plupload.guid();i.attr("id",k)}j=new plupload.Uploader(c.extend({dragdrop:true,container:k},e));if(e.preinit){e.preinit(j)}d[k]=j;function h(l){var m;if(l.status==plupload.DONE){m="plupload_done"}if(l.status==plupload.FAILED){m="plupload_failed"}if(l.status==plupload.QUEUED){m="plupload_delete"}if(l.status==plupload.UPLOADING){m="plupload_uploading"}c("#"+l.id).attr("class",m).find("a").css("display","block")}function f(){c("span.plupload_total_status",i).html(j.total.percent+"%");c("div.plupload_progress_bar",i).css("width",j.total.percent+"%");c("span.plupload_upload_status",i).text("Uploaded "+j.total.uploaded+"/"+j.files.length+" files");if(j.total.uploaded==j.files.length){j.stop()}}function g(){var m=c("ul.plupload_filelist",i).html(""),n=0,l;c.each(j.files,function(p,o){l="";if(o.status==plupload.DONE){if(o.target_name){l+='<input type="hidden" name="'+k+"_"+n+'_tmpname" value="'+plupload.xmlEncode(o.target_name)+'" />'}l+='<input type="hidden" name="'+k+"_"+n+'_name" value="'+plupload.xmlEncode(o.name)+'" />';l+='<input type="hidden" name="'+k+"_"+n+'_status" value="'+(o.status==plupload.DONE?"done":"failed")+'" />';n++;c("#"+k+"_count").val(n)}m.append('<li id="'+o.id+'"><div class="plupload_file_name"><span>'+o.name+'</span></div><div class="plupload_file_action"><a href="#"></a></div><div class="plupload_file_status">'+o.percent+'%</div><div class="plupload_file_size">'+plupload.formatSize(o.size)+'</div><div class="plupload_clearer">&nbsp;</div>'+l+"</li>");h(o);c("#"+o.id+".plupload_delete a").click(function(q){c("#"+o.id).remove();j.removeFile(o);q.preventDefault()})});c("span.plupload_total_file_size",i).html(plupload.formatSize(j.total.size));if(j.total.queued===0){c("span.plupload_add_text",i).text(a("Add files."))}else{c("span.plupload_add_text",i).text(j.total.queued+" files queued.")}c("a.plupload_start",i).toggleClass("plupload_disabled",j.files.length===0);m[0].scrollTop=m[0].scrollHeight;f();if(!j.files.length&&j.features.dragdrop&&j.settings.dragdrop){c("#"+k+"_filelist").append('<li class="plupload_droptext">'+a("Drag files here.")+"</li>")}}j.bind("UploadFile",function(l,m){c("#"+m.id).addClass("plupload_current_file")});j.bind("Init",function(l,m){b(k,i);if(!e.unique_names&&e.rename){c("#"+k+"_filelist div.plupload_file_name span",i).live("click",function(s){var q=c(s.target),o,r,n,p="";o=l.getFile(q.parents("li")[0].id);n=o.name;r=/^(.+)(\.[^.]+)$/.exec(n);if(r){n=r[1];p=r[2]}q.hide().after('<input type="text" />');q.next().val(n).focus().blur(function(){q.show().next().remove()}).keydown(function(u){var t=c(this);if(u.keyCode==13){u.preventDefault();o.name=t.val()+p;q.text(o.name);t.blur()}})})}c("a.plupload_add",i).attr("id",k+"_browse");l.settings.browse_button=k+"_browse";if(l.features.dragdrop&&l.settings.dragdrop){l.settings.drop_element=k+"_filelist";c("#"+k+"_filelist").append('<li class="plupload_droptext">'+a("Drag files here.")+"</li>")}c("#"+k+"_container").attr("title","Using runtime: "+m.runtime);c("a.plupload_start",i).click(function(n){if(!c(this).hasClass("plupload_disabled")){j.start()}n.preventDefault()});c("a.plupload_stop",i).click(function(n){j.stop();n.preventDefault()});c("a.plupload_start",i).addClass("plupload_disabled")});j.init();if(e.setup){e.setup(j)}j.bind("Error",function(l,o){var m=o.file,n;if(m){n=o.message;if(o.details){n+=" ("+o.details+")"}c("#"+m.id).attr("class","plupload_failed").find("a").css("display","block").attr("title",n)}});j.bind("StateChanged",function(){if(j.state===plupload.STARTED){c("li.plupload_delete a,div.plupload_buttons",i).hide();c("span.plupload_upload_status,div.plupload_progress,a.plupload_stop",i).css("display","block");c("span.plupload_upload_status",i).text("Uploaded 0/"+j.files.length+" files")}else{c("a.plupload_stop,div.plupload_progress",i).hide();c("a.plupload_delete",i).css("display","block")}});j.bind("QueueChanged",g);j.bind("StateChanged",function(l){if(l.state==plupload.STOPPED){g()}});j.bind("FileUploaded",function(l,m){h(m)});j.bind("UploadProgress",function(l,m){c("#"+m.id+" div.plupload_file_status",i).html(m.percent+"%");h(m);f()})});return this}else{return d[c(this[0]).attr("id")]}}})(jQuery);
 /*
 * TipTip
 * Copyright 2010 Drew Wilson
 * www.drewwilson.com
 * code.drewwilson.com/entry/tiptip-jquery-plugin
 *
 * Version 1.3   -   Updated: Mar. 23, 2010
 *
 * This Plug-In will create a custom tooltip to replace the default
 * browser tooltip. It is extremely lightweight and very smart in
 * that it detects the edges of the browser window and will make sure
 * the tooltip stays within the current window size. As a result the
 * tooltip will adjust itself to be displayed above, below, to the left 
 * or to the right depending on what is necessary to stay within the
 * browser window. It is completely customizable as well via CSS.
 *
 * This TipTip jQuery plug-in is dual licensed under the MIT and GPL licenses:
 *   http://www.opensource.org/licenses/mit-license.php
 *   http://www.gnu.org/licenses/gpl.html
 */
(function($){$.fn.tipTip=function(options){var defaults={activation:"hover",keepAlive:false,maxWidth:"200px",edgeOffset:3,defaultPosition:"bottom",delay:400,fadeIn:200,fadeOut:200,attribute:"title",content:false,enter:function(){},exit:function(){}};var opts=$.extend(defaults,options);if($("#tiptip_holder").length<=0){var tiptip_holder=$('<div id="tiptip_holder" style="max-width:'+opts.maxWidth+';"></div>');var tiptip_content=$('<div id="tiptip_content"></div>');var tiptip_arrow=$('<div id="tiptip_arrow"></div>');$("body").append(tiptip_holder.html(tiptip_content).prepend(tiptip_arrow.html('<div id="tiptip_arrow_inner"></div>')))}else{var tiptip_holder=$("#tiptip_holder");var tiptip_content=$("#tiptip_content");var tiptip_arrow=$("#tiptip_arrow")}return this.each(function(){var org_elem=$(this);if(opts.content){var org_title=opts.content}else{var org_title=org_elem.attr(opts.attribute)}if(org_title!=""){if(!opts.content){org_elem.removeAttr(opts.attribute)}var timeout=false;if(opts.activation=="hover"){org_elem.hover(function(){active_tiptip()},function(){if(!opts.keepAlive){deactive_tiptip()}});if(opts.keepAlive){tiptip_holder.hover(function(){},function(){deactive_tiptip()})}}else if(opts.activation=="focus"){org_elem.focus(function(){active_tiptip()}).blur(function(){deactive_tiptip()})}else if(opts.activation=="click"){org_elem.click(function(){active_tiptip();return false}).hover(function(){},function(){if(!opts.keepAlive){deactive_tiptip()}});if(opts.keepAlive){tiptip_holder.hover(function(){},function(){deactive_tiptip()})}}function active_tiptip(){opts.enter.call(this);tiptip_content.html(org_title);tiptip_holder.hide().removeAttr("class").css("margin","0");tiptip_arrow.removeAttr("style");var top=parseInt(org_elem.offset()['top']);var left=parseInt(org_elem.offset()['left']);var org_width=parseInt(org_elem.outerWidth());var org_height=parseInt(org_elem.outerHeight());var tip_w=tiptip_holder.outerWidth();var tip_h=tiptip_holder.outerHeight();var w_compare=Math.round((org_width-tip_w)/2);var h_compare=Math.round((org_height-tip_h)/2);var marg_left=Math.round(left+w_compare);var marg_top=Math.round(top+org_height+opts.edgeOffset);var t_class="";var arrow_top="";var arrow_left=Math.round(tip_w-12)/2;if(opts.defaultPosition=="bottom"){t_class="_bottom"}else if(opts.defaultPosition=="top"){t_class="_top"}else if(opts.defaultPosition=="left"){t_class="_left"}else if(opts.defaultPosition=="right"){t_class="_right"}var right_compare=(w_compare+left)<parseInt($(window).scrollLeft());var left_compare=(tip_w+left)>parseInt($(window).width());if((right_compare&&w_compare<0)||(t_class=="_right"&&!left_compare)||(t_class=="_left"&&left<(tip_w+opts.edgeOffset+5))){t_class="_right";arrow_top=Math.round(tip_h-13)/2;arrow_left=-12;marg_left=Math.round(left+org_width+opts.edgeOffset);marg_top=Math.round(top+h_compare)}else if((left_compare&&w_compare<0)||(t_class=="_left"&&!right_compare)){t_class="_left";arrow_top=Math.round(tip_h-13)/2;arrow_left=Math.round(tip_w);marg_left=Math.round(left-(tip_w+opts.edgeOffset+5));marg_top=Math.round(top+h_compare)}var top_compare=(top+org_height+opts.edgeOffset+tip_h+8)>parseInt($(window).height()+$(window).scrollTop());var bottom_compare=((top+org_height)-(opts.edgeOffset+tip_h+8))<0;if(top_compare||(t_class=="_bottom"&&top_compare)||(t_class=="_top"&&!bottom_compare)){if(t_class=="_top"||t_class=="_bottom"){t_class="_top"}else{t_class=t_class+"_top"}arrow_top=tip_h;marg_top=Math.round(top-(tip_h+5+opts.edgeOffset))}else if(bottom_compare|(t_class=="_top"&&bottom_compare)||(t_class=="_bottom"&&!top_compare)){if(t_class=="_top"||t_class=="_bottom"){t_class="_bottom"}else{t_class=t_class+"_bottom"}arrow_top=-12;marg_top=Math.round(top+org_height+opts.edgeOffset)}if(t_class=="_right_top"||t_class=="_left_top"){marg_top=marg_top+5}else if(t_class=="_right_bottom"||t_class=="_left_bottom"){marg_top=marg_top-5}if(t_class=="_left_top"||t_class=="_left_bottom"){marg_left=marg_left+5}tiptip_arrow.css({"margin-left":arrow_left+"px","margin-top":arrow_top+"px"});tiptip_holder.css({"margin-left":marg_left+"px","margin-top":marg_top+"px"}).attr("class","tip"+t_class);if(timeout){clearTimeout(timeout)}timeout=setTimeout(function(){tiptip_holder.stop(true,true).fadeIn(opts.fadeIn)},opts.delay)}function deactive_tiptip(){opts.exit.call(this);if(timeout){clearTimeout(timeout)}tiptip_holder.fadeOut(opts.fadeOut)}}})}})(jQuery);
/* ------------------------------------------------------------------------
 * Class: prettyLoader
 * Use: A unified solution for AJAX loader
 * Author: Stephane Caron (http://www.no-margin-for-errors.com)
 * Version: 1.0.1
 * ------------------------------------------------------------------------- */

(function ($) {
	$.prettyLoader = { version: '1.0.1' }; $.prettyLoader = function (settings) {
		settings = jQuery.extend({ animation_speed: 'fast', bind_to_ajax: true, delay: false, loader: '/public/images/prettyLoader/ajax-loader.gif', offset_top: 13, offset_left: 10 }, settings); scrollPos = _getScroll(); imgLoader = new Image(); imgLoader.onerror = function () { alert('Preloader image cannot be loaded. Make sure the path is correct in the settings and that the image is reachable.'); }; imgLoader.src = settings.loader; if (settings.bind_to_ajax)
jQuery(document).ajaxStart(function(){$.prettyLoader.show()}).ajaxStop(function(){$.prettyLoader.hide()});$.prettyLoader.positionLoader=function(e){e=e?e:window.event;cur_x=(e.clientX)?e.clientX:cur_x;cur_y=(e.clientY)?e.clientY:cur_y;left_pos=cur_x+settings.offset_left+scrollPos['scrollLeft'];top_pos=cur_y+settings.offset_top+scrollPos['scrollTop'];$('.prettyLoader').css({'top':top_pos,'left':left_pos});}
$.prettyLoader.show=function(delay){if($('.prettyLoader').size()>0)return;scrollPos=_getScroll();$('<div></div>').addClass('prettyLoader').addClass('prettyLoader_'+settings.theme).appendTo('body').hide();if($.browser.msie&&$.browser.version==6)
$('.prettyLoader').addClass('pl_ie6');$('<img />').attr('src',settings.loader).appendTo('.prettyLoader');$('.prettyLoader').fadeIn(settings.animation_speed);$(document).bind('click',$.prettyLoader.positionLoader);$(document).bind('mousemove',$.prettyLoader.positionLoader);$(window).scroll(function(){scrollPos=_getScroll();$(document).triggerHandler('mousemove');});delay=(delay)?delay:settings.delay;if(delay){setTimeout(function(){$.prettyLoader.hide()},delay);}};$.prettyLoader.hide=function(){$(document).unbind('click',$.prettyLoader.positionLoader);$(document).unbind('mousemove',$.prettyLoader.positionLoader);$(window).unbind('scroll');$('.prettyLoader').fadeOut(settings.animation_speed,function(){$(this).remove();});};function _getScroll(){if(self.pageYOffset){return{scrollTop:self.pageYOffset,scrollLeft:self.pageXOffset};}else if(document.documentElement&&document.documentElement.scrollTop){return{scrollTop:document.documentElement.scrollTop,scrollLeft:document.documentElement.scrollLeft};}else if(document.body){return{scrollTop:document.body.scrollTop,scrollLeft:document.body.scrollLeft};};};return this;};})(jQuery);

