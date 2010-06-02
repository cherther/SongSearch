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