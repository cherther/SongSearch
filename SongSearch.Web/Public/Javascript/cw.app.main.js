// Helpers
if (!Array.prototype.indexOf) Array.prototype.indexOf = function (b) { for (var a = 0; a < this.length; a++) if (this[a] === b) return a; return -1 };
if (!Array.prototype.contains) Array.prototype.contains = function (b) { for (var a = 0; a < this.length; a++) if (this[a] === b) return true; return false };
if (!String.prototype.contains) String.prototype.contains = function (s) { if (this.indexOf(s) != -1) return true; return false; };
if (!String.prototype.swap) String.prototype.swap = function (a, b) { if (this.contains(a)) { return this.replace(a, b); } else { return this.replace(b, a); } };

function Rnd() { return Math.floor(Math.random() * 100001).toString(); }

function wait(elem, message) {

    try {
        //$("body").css("cursor", "wait");
        elem = elem == null ? $("body") : elem;
        message = message == null ? "Loading" : message;
        elem.css("cursor", "wait");
        //elem.block({ message: '<h3><img src="/public/images/loading.gif" />' + message + '...</h3>' });
        
    }
    catch (ex) {
    }
}
function unwait(elem) {
    elem = elem == null ? $("body") : elem;
    //elem.unblock();
    elem.css("cursor", "default");
    //elem.css("cursor", "pointer");
}

// UI functions
// UI element constants
var contentDetailPanelId = '#cw-content-detail-panel';
var searchOptionsPanelId = '#cw-search-options-panel';

var isContentDetailShowing;
var lastContentDetailLinkClicked;

function hideSearchOptions() {
    $(searchOptionsPanelId).hide();
}

function showContentPanel(link) {

    if (link[0] != lastContentDetailLinkClicked) {

        var url = link[0].href;

        getContentDetail(url, link);

    } else {
        closeContentPanel();
        lastContentDetailLinkClicked = null;
    }
    
}
function showContentPanelCallback(data, trigger) {

    closeContentPanel();

    var parentRow = trigger.closest('tr');
    var numberofCells = 6;// parentRow.children('td').length;
    var detailPanelRow = $('<tr style="display: none" id="cw-content-detail-row"><td colspan="' + numberofCells + '">' + data);

    parentRow.addClass('cw-row-selected');
    parentRow.after(detailPanelRow);
    detailPanelRow.slideDown();

    isContentDetailShowing = true;
    lastContentDetailLinkClicked = trigger[0];
    
//    var contentDetailPanel = $(contentDetailPanelId);

//    contentDetailPanel.html(data);

//    if (!isContentDetailShowing) {

//        contentDetailPanel.show();

//        isContentDetailShowing = true;

//        hideSearchOptions();
//    }
}

function closeContentPanel() {

    if (isContentDetailShowing) {
        var contentDetailRow = $('#cw-content-detail-row'); // $('.hello').remove();
        if (contentDetailRow.length > 0) { contentDetailRow.remove(); }
        $('.cw-row-selected').removeClass('cw-row-selected');
        isContentDetailShowing = false;
    }
}