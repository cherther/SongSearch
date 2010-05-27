// Helpers
if (!Array.prototype.indexOf) Array.prototype.indexOf = function (b) { for (var a = 0; a < this.length; a++) if (this[a] === b) return a; return -1 };
if (!Array.prototype.contains) Array.prototype.contains = function (b) { for (var a = 0; a < this.length; a++) if (this[a] === b) return true; return false };
if (!String.prototype.contains) String.prototype.contains = function (s) { if (this.indexOf(s) != -1) return true; return false; };
if (!String.prototype.swap) String.prototype.swap = function (a, b) { if (this.contains(a)) { return this.replace(a, b); } else { return this.replace(b, a); } };

function Rnd() { return Math.floor(Math.random() * 100001).toString(); }

var waiting = false;
function wait(elem, message) {

    try {
        if (!waiting) {
            //$("body").css("cursor", "wait");
            elem = elem == null ? $("body") : elem;
            message = message == null ? "Loading" : message;
            elem.css("cursor", "wait");
            //elem.block({ message: '<h3><img src="/public/images/loading.gif" />' + message + '...</h3>' });
            waiting = true;
        }
    }
    catch (ex) {
    }
}
function unwait(elem) {
    if (waiting) {
        elem = elem == null ? $("body") : elem;
        //elem.unblock();
        elem.css("cursor", "default");
        //elem.css("cursor", "pointer");
    }
    waiting = false;
}

function flash(type, msg) {

    //var fade = type == 'info';
    var flash = $('#flash');
    flash.html(msg).removeClass('').addClass(type);
    flash.slideDown('slow');
    if (type == 'info') {
        flash.delay('1200').fadeOut('slow'); 
    }
    flash.click(function () 
        {
            flash.toggle('highlight') 
        });
}

function toggleTagBoxSelection(box, selectedClass, toggle) {
    box.siblings().removeClass(selectedClass);
    if (toggle) {
        box.addClass(selectedClass);
    }
}

// UI functions
// UI element constants
var contentDetailPanelId = '#cw-content-detail-panel';
var searchOptionsPanelId = '#cw-search-options-panel';
var tagBoxSelectedClass = 'cw-blue';
var inputSelectedClass = 'cw-input-highlight';

var isContentDetailShowing;
var lastContentDetailLinkClicked;

//-----------------------------------------------------------------------------------
// clear form
//-----------------------------------------------------------------------------------
function clearSearchForm(form) {

    clearForm(form);
    $('.cw-search-tag').removeClass(tagBoxSelectedClass);
    $('.cw-form-value').removeClass(inputSelectedClass);
}

function clearForm(form) {
    // iterate over all of the inputs for the form
    // element that was passed in
    $(':input.cw-form-value', form).each(function () {
        var type = this.type;
        var tag = this.tagName.toLowerCase(); // normalize case

        // it's ok to reset the value attr of text inputs,
        // password inputs, and textareas
        if (type == 'text' || type == 'password' || tag == 'textarea' || type == 'hidden') {
            this.value = "";
        }
        // checkboxes and radios need to have their checked state cleared
        // but should *not* have their 'value' changed
        else if (type == 'checkbox' || type == 'radio')
            this.checked = false;
        // select elements need to have their 'selectedIndex' property set to -1
        // (this works for both single and multiple select elements)
        else if (tag == 'select')
            this.selectedIndex = -1;
    });
};

function setSelectedSearchTagValue(link) {

    var id = link[0].id;

    var valField = '#' + id.substring(0, id.indexOf('-'));
    var tagId = id.substring(id.indexOf('-') + 1);
    var vals = $(valField) != null ? $(valField).val().split(';') : null;

    var pos = vals.indexOf(tagId)
    if (pos > -1) {
        vals.splice(pos, 1);
    }
    else {
        vals.push(tagId);
    }

    $(valField).val(vals.join(';'));
    link.toggleClass(tagBoxSelectedClass);
}

//-----------------------------------------------------------------------------------
//  content detail panel
//-----------------------------------------------------------------------------------
function showContentPanel(link) {

    if (link[0] != lastContentDetailLinkClicked) {

        var url = link[0].href;

        getContentDetailAjax(url, link);

    } else {
        closeContentPanel();
        lastContentDetailLinkClicked = null;
    }
    
}
function showContentPanelCallback(data, trigger) {

    closeContentPanel();

    var parentRow = trigger.closest('tr');
    var numberofCells = 6;// parentRow.children('td').length;
    var detailPanelRow = $('<tr id="cw-content-detail-row"><td colspan="' + numberofCells + '">' + data);
//style="display: none" 
    parentRow.addClass('cw-row-selected');
    parentRow.after(detailPanelRow);
    
    // cw-content-detail-tabs: calls jquery ui tabs widgets
    $('#cw-content-detail-tabs').tabs();

//    detailPanelRow.slideDown();
//    detailPanelRow.css('display', ' table-row');

    isContentDetailShowing = true;
    lastContentDetailLinkClicked = trigger[0];
    
}

function closeContentPanel() {

    if (isContentDetailShowing) {
        var contentDetailRow = $('#cw-content-detail-row'); // $('.hello').remove();
        if (contentDetailRow.length > 0) { contentDetailRow.remove(); }
        $('.cw-row-selected').removeClass('cw-row-selected');
        isContentDetailShowing = false;
    }
}


//-----------------------------------------------------------------------------------
// User Management
//-----------------------------------------------------------------------------------
var isUserDetailShowing = false;

var isCatalogDetailShowing = false;
var catalogDetailId = '#cw-catalog-detail';
var userDetailId = '#cw-user-detail';
function showUserDetailPanel(data) {
    //var url = '/User/Userdetail/' + userId;
    
    var userDetail = $(userDetailId);
    userDetail.html(data);
    if (!isUserDetailShowing) {
        userDetail.show(); //fadeIn('slow');
        isUserDetailShowing = true;
    }

}