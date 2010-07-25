// Helpers
if (!Array.prototype.indexOf) Array.prototype.indexOf = function (b) { for (var a = 0; a < this.length; a++) if (this[a] === b) return a; return -1 };
if (!Array.prototype.contains) Array.prototype.contains = function (b) { for (var a = 0; a < this.length; a++) if (this[a] === b) return true; return false };
if (!String.prototype.contains) String.prototype.contains = function (s) { return this.indexOf(s) != -1; };
//if (!String.prototype.swap) String.prototype.swap = function (a, b) { if (this.contains(a)) { return this.replace(a, b); } else { return this.replace(b, a); } };
if (!String.prototype.swap) String.prototype.swap = function (a, b) { return this.contains(a) ? this.replace(a, b) : this.replace(b, a); };

//***********************************************
//  Constants
//***********************************************
var _debug = false; // 
var _maxCart = 100;
//***********************************************
//  Messages
//***********************************************
var _cartOverMaxMsg = 'Sorry, you can only add up to ' + _maxCart + ' items to your cart.';
var _cartAddErrorMsg = 'There was a problem adding the item(s) to your cart. Please make sure you have less than 100 items in your cart.'

//***********************************************
//  Rnd
//***********************************************
function Rnd() {return Math.floor(Math.random() * 100001).toString(); }

var waiting = false;
//***********************************************
//  wait
//***********************************************
function wait(elem, message) {

	try {
		if (!waiting){
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

//***********************************************
//  unwait
//***********************************************
function unwait(elem) {
	if (waiting) {
		elem = elem == null ? $("body") : elem;
		//elem.unblock();
		elem.css("cursor", "default");
		//elem.css("cursor", "pointer");
	}
	waiting = false;
}

//***********************************************
//  flash
//***********************************************
function debug(msg) {
	if (_debug) {
		feedback('debug', msg);
	}
}
function feedback(type, msg) {
	unwait();

	var title = 'Message';
	var img = '/public/images/icons/silk/information.png';
	var duration = 3000;

	switch (type) {
		case "error":
			title = 'Error';
			img = '/public/images/icons/silk/error.png';
			duration = 8000;
			break;
		case "debug":
			title = 'Debugging';
			img = '/public/images/icons/silk/plugin.png';
			duration = 8000;
			break;
//		default:
//			title = 'Message';
//			img = '/public/images/icons/silk/information.png';
//			duration = 3000;
	}
	//	
	//	$.feedbackBar(msg, { autoClose: autoClose, wrapperClass: wrapperClass});

	//	var fb = $('.' + wrapperClass);
	//	fb.click(function () { fb.toggle('highlight') });
	$.gritter.add({
		// (string | mandatory) the heading of the notification
		title: title,
		time: duration,
		image: img,
		// (string | mandatory) the text inside the notification
		text: msg
	});
}

function pluralize(item, numberofItems) {
	return numberofItems == 1 ? item : item + 's';
}
function setupTooltips() {
	$('[title]').tipTip({ delay: 800 });  //tooltip({ predelay: 800, effect: 'fade', opacity: 0.7 });
	//$('.cw-tooltip').tipTip({ delay: 800 }); //tooltip();
}

//***********************************************
//  toggleTagBoxSelection
//***********************************************
function toggleTagBoxSelection(box, selectedClass, toggle) {
	box.siblings().removeClass(selectedClass);
	if (toggle) {
		box.addClass(selectedClass);
	}
}


function millSecsToTimeCode(millSec, returnAsString) {
	// convert milliseconds to mm:ss, return as object literal or string
	var secs = Math.floor(millSec / 1000);
	var min = Math.floor(secs / 60);
	var sec = secs - (min * 60);
	// if (min == 0 && sec == 0) return null; // return 0:00 as null
	return (returnAsString ? (min + ':' + (sec < 10 ? '0' + sec : sec)) : { 'min': min, 'sec': sec });
}

function toFileSizeDescription(fileSize) {
	return parseInt((fileSize / (1024 * 1024))).toString() + " MB";
}

function toYesNo(yes) {
	return yes ? "Yes" : "No";
}

// UI functions
// UI element constants
var contentDetailPanelId = '#cw-content-detail-panel';
var searchOptionsPanelId = '#cw-search-options-panel';
var tagBoxSelectedClass = 'cw-blue';
var inputSelectedClass = 'cw-input-highlight';

var isContentDetailShowing;
var isContentViewMode;
var isContentEditMode;
var isContentSaveMode;

var lastContentDetailLinkClicked;
var lastContentEditLinkClicked;

var _currentContentDetailTab;

//-----------------------------------------------------------------------------------
//
//-----------------------------------------------------------------------------------

//***********************************************
//  setupAutoComplete
//***********************************************
var aCache = {};
function setupAutoComplete() {

	$(".cw-autocomplete").autocomplete(
		{
			//source: "/Search/AutoComplete?f=" + this.rel,//["John", "Johnny", "Jon", "Joe" ],
			source: function (request, response) {

				var field = $(this)[0].element[0].alt; //little hack to store an extra field on the input elem, rel does not work cross-browser for input tags
				if (field) {

					try {
						if (aCache.term == request.term && aCache.field == field && aCache.content) {
							response(aCache.content);
							return;
						}
						if (new RegExp(aCache.term).test(request.term) && aCache.content && aCache.content.length < 13) {
							response($.ui.autocomplete.filter(aCache.content, request.term));
							return;
						}
					}
					catch (ex) {

					}
					//catch() {}

					var url = "/Search/AutoComplete?f=";
					$.ajax({
						url: url + field,
						dataType: "json",
						data: request,
						success: function (data) {
							aCache.field = field;
							aCache.term = request.term;
							aCache.content = data;
							response(data);
						}
					});
				}

			},
			minLength: 2
		}
	);
}

//***********************************************
//  clearSearchForm
//***********************************************
function clearSearchForm(form) {

	clearForm(form);
	$('.cw-tagbox-search').removeClass(tagBoxSelectedClass);
	$('.cw-form-value').removeClass(inputSelectedClass);
}

//***********************************************
//  clearForm
//***********************************************
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

//***********************************************
//  setSelectedSearchTagValue
//***********************************************
function setSelectedSearchTagValue(link) {

	var id = link[0].id;

	var valField = '#' + link[0].rev; // + id.substring(0, id.indexOf('-'));
	var tagId = link[0].rel; // id.substring(id.indexOf('-') + 1);
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

//***********************************************
//  toggleRowCheckboxes
//***********************************************
function toggleRowCheckboxes(trigger) {
	var checkboxes = $('.' + trigger.id + ':enabled');
	if ($(trigger).is(':checked')) {
		checkboxes.attr('checked', 'checked');
	} else {
		checkboxes.removeAttr('checked');
	}
}

//***********************************************
//  toggleRowCheckboxes
//***********************************************
function strikeoutRowCheckboxes() {
	var checkboxes = $('.cw-row-checkbox:checked');
	checkboxes.attr('disabled', 'disabled');
	var cells = checkboxes.parent('td').siblings();
	cells.css('text-decoration', 'line-through');
	cells.attr('disabled', 'true');
	cells.children('a').attr('disabled', 'true');
}
function removeCheckedRows() {
	var checkboxes = $('.cw-row-checkbox-delete:checked'); // $('.cw-row-checkbox:checked');
	checkboxes.closest('tr').remove();

}
function isTableBodyEmpty(table) {
	var $table = $(table + ' > TBODY > TR');
	return ($table.length == 0);
}

function toggleContentsTable(link) {
	if (link.length > 0) {
		link.text(link.text().swap('Show', 'Hide'));
	}
	$('.cw-tbl-catalog-contents').toggle();
	closeContentPanel();

}

function hideEmptyContentsTable() {

	var table = $('.cw-tbl-catalog-contents');
	var link = $('#cw-catalog-contents-show-link');

	if (isTableBodyEmpty('.cw-tbl-catalog-contents')) {
		table.hide();
		link.hide();
		$('#cw-catalog-contents-msg').show();
	}
}

//***********************************************
//  updateAddToCartAllButtontext
//***********************************************
function updateAddToCartAllButtontext(count) {
	var text = count > 0 ? 'Add (' + count + ')' : 'Add';
	$('#cw-add-all-to-cart span').text(text);
}

function setUpMediaFileDialog(link) {
	var dialog = $("#upload-form");
	var contentId = link[0].rel;
	var title = link[0].rev;

	$('#contentId').val(contentId);
	$('#uploadTitle').text(title);

	if (dialog) {
		dialog.dialog("destroy");
		var template = dialog.html();
		dialog.dialog({
			autoOpen: false,
			height: 300,
			width: 350,
			modal: true,
			title: 'Select & upload new media files', //title,
			buttons: {
				'Upload': function () {

					saveMediaFilesAjax('#saveMediaFilesForm', function () {
						feedback('info', 'Files uploaded');
						resetMediaFileDialog(template);
						$(this).dialog('close');
					});

				},
				Cancel: function () {
					resetMediaFileDialog(template);
					$(this).dialog('close');
				}
			},
			close: function () {
				resetMediaFileDialog(template);
			}
		});
		dialog.dialog('open');
		resetUploaders();
		setupMediaUploader('previewVersionUploadContainer', 'previewVersionUpload', 'previewVersionFilelist', 'Preview', 0);
		setupMediaUploader('fullVersionUploadContainer', 'fullVersionUpload', 'fullVersionFilelist', 'FullSong', 1);
	}
}
function resetMediaFileDialog(template) {
	var dialog = $("#upload-form");
	if (dialog) {
		dialog.dialog("destroy");
		dialog.html(template);
	}
}
//-----------------------------------------------------------------------------------
//  content detail panel
//-----------------------------------------------------------------------------------
//***********************************************
//  showContentPanel
//***********************************************
function showContentPanel(link) {

	var contentLink = link[0];
	if (!link.is(':disabled')) {
		isContentEditMode = contentLink.rel == "Edit";
		isContentSaveMode = contentLink.rel == "Save";
		isContentViewMode = !isContentEditMode && !isContentSaveMode;

		if (isContentViewMode) {
			mediaStop();
		}
		var sameContent = lastContentDetailLinkClicked != null ? link[0] == lastContentDetailLinkClicked[0] : false;

		if (!sameContent || !isContentViewMode) {

			var url = link[0].href;
			
			getContentDetailAjax(url, link);

		} else {

			closeContentPanel();

			lastContentDetailLinkClicked = null;
		}
	}
}

//***********************************************
//  showContentPanel
//***********************************************
function saveContentPanel(link, altLink) {

	var contentLink = link[0];
	isContentEditMode = contentLink.rel == "Edit";
	isContentSaveMode = contentLink.rel == "Save";
	isContentViewMode = !isContentEditMode && !isContentSaveMode;


	// save the form data
	if (isContentSaveMode) {
		submitContentFormAjax('#cw-content-editor', link, altLink);
	} else {

		closeContentPanel();

		lastContentDetailLinkClicked = null;
	}

}
//***********************************************
//  showContentPanelCallback
//      called from ajax success call
//***********************************************
function showContentPanelCallback(data, link) {

	if (isContentViewMode && isContentDetailShowing) { closeContentPanel(); }

	var parentRow = link.closest('tr');
	var numberofCells = 6; // parentRow.children('td').length;
	var detailPanelRow = $('<tr id="cw-content-detail-row"><td colspan="' + numberofCells + '">' + data);
	parentRow.addClass('cw-row-selected');
	parentRow.after(detailPanelRow);
	unwait();

	// cw-content-detail-tabs: calls jquery ui tabs widgets
	setupContentPanelUIControls();

	var mediaUrl = link[0].rel;
	if (mediaUrl) {
		mediaPlay(mediaUrl, '#cw-play-full');
		//togglePlayButton();
	}

	isContentDetailShowing = true;
	lastContentDetailLinkClicked = link;

}

//***********************************************
//  showContentPanelCallback
//      called from ajax success call
//***********************************************
function showContentPanelCallbackEdit(data, link, altLink) {

	if (isContentSaveMode) { feedback('info', 'Item Saved'); }
	lastContentEditLinkClicked = isContentSaveMode ? null : link;

	if (altLink) {
		unwait();
		showContentPanel(altLink);
	}
	else {
		var detailPanel = $('#cw-content-detail-data');
		detailPanel.html(data);
		unwait();
		// cw-content-detail-tabs: calls jquery ui tabs widgets
		setupContentPanelUIControls();

		link.text(link.text().swap('Edit', 'Save'));
		link.toggleClass('cw-gray').toggleClass('cw-red');
		link.toggleClass('cw-content-edit-link').toggleClass('cw-content-save-link');
		link[0].href = link[0].href.swap('Edit', 'Save');
		link[0].rel = link[0].rel.swap('Edit', 'Save');

		isContentDetailShowing = true;

	}
}

function setupContentPanelUIControls() {
	//$('.cw-content-detail-menu').button();
	var $tabs = $('#cw-content-detail-tabs');
	$tabs.tabs(
		{
			select: function (evt, ui) {
				_currentContentDetailTab = ui.panel.id;
			}
		}
	);

	if (_currentContentDetailTab != null) {
		$tabs.tabs('select', '#' + _currentContentDetailTab);
	}

	//$('.cw-tag-checkbox').button();
	setupVolumeSlider();
	setupAutoComplete();
}

function deleteContentRight(link) {

	var parentRow = link.closest('tr');
	var modelAction = link.prev('.cw-model-action');
	modelAction.val(2); // ModelAction=Delete
	parentRow.hide().next().hide();

}

function deleteTagCallback(data, link) {
	var tagLocatorClass = link[0].rel;
	$('.' + tagLocatorClass).hide();
	link.hide();
}

//***********************************************
//  closeContentPanel
//***********************************************
function closeContentPanel() {

	if (isContentDetailShowing) {
		var contentDetailRow = $('#cw-content-detail-row'); // $('.hello').remove();
		if (contentDetailRow.length > 0) { contentDetailRow.remove(); }
		$('.cw-row-selected').removeClass('cw-row-selected');
		isContentDetailShowing = false;
		_currentContentDetailTab = null;
		mediaStop();
	}
}

//-----------------------------------------------------------------------------------
//  Media Player buttons
//-----------------------------------------------------------------------------------
var _lastMediaButtonPressed;


function mediaPlay(url, id) {
	soundPlay(url, false);
	togglePlayButton(id);
	$('.cw-media-repeat-link').removeAttr('disabled');
	$('.cw-media-skip-link').removeAttr('disabled');
	_lastMediaButtonPressed = id;
}

function mediaRepeat() {
	if (soundPlayRepeat() != sm_ps_playing) {
		togglePlayButton(_lastMediaButtonPressed);
	}
}


function mediaFastForward() {
	fastForward();
}
function mediaRewind() {
	rewind();
}
function mediaCue(range) {
	var start = range.length > 0 ? range[0] : 45 * 1000;
	cue(start);
}
function enableRewind(yes) {
	if (yes) {
		$('.cw-media-rew-link').removeAttr('disabled');
	} else {
		$('.cw-media-rew-link').attr('disabled', true);
	}
}
function enableFastForward(yes) {
	if (yes) {
		$('.cw-media-ffwd-link').removeAttr('disabled');
	} else {
		$('.cw-media-ffwd-link').attr('disabled', true);
	}
}
//***********************************************
//  togglePlayButton
//***********************************************
function togglePlayButton(id) {
	togglePlayButtons(id, true);
}

//***********************************************
//  togglePlayButtons
//***********************************************
function togglePlayButtons(id, toggle) {

	var readyClass = 'b-play';
	var playingClass = 'b-pause';
	var readyColorClass = 'cw-gray';
	var playingColorClass = 'cw-green';

	var button = $(id);
	var icon = button.children('span');

	if (!toggle) {
		// we're only concerned with resetting this link
		if (button.hasClass(playingColorClass)) {
			button.removeClass(playingColorClass).addClass(readyColorClass); //switchClass(playingColorClass, readyColorClass); //
			icon.removeClass(playingClass).addClass(readyClass);
		}

	} else {

		button.toggleClass(readyColorClass);
		button.toggleClass(playingColorClass);
		icon.toggleClass(playingClass);
		icon.toggleClass(readyClass);

		var otherButton = button.siblings('.cw-media-play-link');

		if (otherButton.length > 0) {
			//recurse
			togglePlayButtons('#' + otherButton[0].id, false);
		}
	}
}

//***********************************************
//  toggleAllPlayButtons: resets all buttons
//***********************************************
function toggleAllPlayButtons() {

	var buttons = $('.cw-media-play-link');

	buttons.each(
		function (x) {
			var button = buttons[x];
			togglePlayButtons('#' + button.id, false);
		}
	);
}

//***********************************************
//  setCurrentMediaTime: sets the elapsed time value
//***********************************************
function setCurrentMediaTime(length) {

	var totalTime = millSecsToTimeCode(length, true);
	$('#cw-media-player-time').html(totalTime);

}

function setCurrentPosition(position, duration) {
	duration = duration == 0 ? 1000 : duration;
	$('#cw-media-position').width((((position / duration) * 100) + '%'));
}
function setCurrentLoadPercentage(loaded, total) {
	total = total == 0 ? 1000 : total;
	$('#cw-media-loaded').width((((loaded / total) * 100) + '%'));
}
//***********************************************
//  toggleAllPlayButtons: sets the total duration value
//***********************************************
function setTotalMediaLength(length) {

	var totalTime = millSecsToTimeCode(length, true);
	$('#cw-media-player-length').html(totalTime);
}

function setupVolumeSlider() {
	// setup master volume
	var vol = _currentVolume != null && _currentVolume >= 0 ? _currentVolume : 60;

	$("#cw-media-player-volume").slider({
		value: vol,
		orientation: "horizontal",
		range: "min",
		animate: true,
		slide: function (evt, ui) { changeVolume(ui.value); }
	}
	);
}
//-----------------------------------------------------------------------------------
// User Management
//-----------------------------------------------------------------------------------
var isUserDetailShowing = false;

var isCatalogDetailShowing = false;
var catalogDetailId = '#cw-catalog-detail';
var userDetailId = '#cw-user-detail';

//***********************************************
//  showUserDetailPanel
//***********************************************
function showUserDetailPanel(data) {
	//var url = '/User/Userdetail/' + userId;

	var userDetail = $(userDetailId);
	userDetail.html(data);
	if (!isUserDetailShowing) {
		userDetail.show(); //fadeIn('slow');
		isUserDetailShowing = true;
	}

}

//***********************************************
//  showUserDetailPanel
//***********************************************
function showCatalogDetailPanel(data) {
	//var url = '/User/Userdetail/' + userId;

	var catalogDetail = $(catalogDetailId);
	catalogDetail.html(data);
	if (!isCatalogDetailShowing) {
		catalogDetail.show(); //fadeIn('slow');
		isCatalogDetailShowing = true;
	}

}