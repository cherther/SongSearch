//-----------------------------------------------------------------------------------
// Search Results
//-----------------------------------------------------------------------------------
//***********************************************
//  getContentDetailAjax
//***********************************************
function getContentDetailAjax(url, link) {
	
	wait();

	$.ajax({
		url: url,
		type: 'GET',
		cache: false,
		dataType: 'html',
		success: function (data, status, xhr) {
			if (status == 'error') {
				unwait();
				feedback('error', xhr.status + ' ' + xhr.statusText);
			}
			else {
				if (isContentViewMode) {
					showContentPanelCallback(data, link);
				} else {
					showContentPanelCallbackEdit(data, link, null);
				}
			}
		},
		error: function (xhr, status, error) {
			unwait();
			feedback('error', xhr.status + ' ' + xhr.statusText);
		}
	});

}

//***********************************************
//  submitContentFormAjax
//***********************************************
function submitContentFormAjax(formId, link, altLink) {
	var form = $(formId);
	var returnData = altLink == null;

//	var tags = $('.cw-tagbox-checkbox').map(
//		function () {
//			return $(this).val();
//		}).get().join(",");
//	
//	var fields = form.serializeArray();
//	
//	$.each(fields, function (i, field) {
//		if (field && field.name.indexOf('tags') != -1 && field.value == 'false'){
//			fields.splice(i, 1);
//		}
//	});

//	$.ajax({
//		url: form.attr('action'),
//		data: fields,
//		type: 'POST',
//		dataType: 'json',
//		success: function (data, status, xhr) {
//			if (status == 'error') {
//				unwait();
//				feedback('error', xhr.status + ' ' + xhr.statusText);
//			} else {
//				showContentPanelCallbackEdit(data, link, altLink);
//			}
//		},
//		error:
//			function (xhr, status, error) {
//				unwait();
//				feedback('error', xhr.status + ' ' + xhr.statusText);

//			}
//		}
//	);
//////	$('input:radio[name=bar]:checked')
	var options = {
		beforeSubmit: function () { wait(); },
		data: { returnData: returnData },
		success: function (data, status, xhr) {
			if (status == 'error') {
				unwait();
				feedback('error', xhr.status + ' ' + xhr.statusText);
			} else {
				showContentPanelCallbackEdit(data, link, altLink);
			}
		},
		error:
			function (xhr, status, error) {
				unwait();
				feedback('error', xhr.status + ' ' + xhr.statusText);

			}
	};

	form.ajaxSubmit(options);

}
function addContentRepresentationEditorAjax(url, modelData, link) {

	wait();

	$.ajax({
		url: url,
		data: modelData,
		type: 'GET',
		cache: false,
		dataType: 'html',
		success: function (data, status, xhr) {
			if (status == 'error') {
				unwait();
				feedback('error', xhr.status + ' ' + xhr.statusText);
			}
			else {
				addContentRepresentationEditor(data, link);
			}
		},
		error: function (xhr, status, error) {
			unwait();
			feedback('error', xhr.status + ' ' + xhr.statusText);
		}
	});

}
//-----------------------------------------------------------------------------------
// Song Cart
//-----------------------------------------------------------------------------------
function updateCartCount() {
	var count = getCartCountAjax();
	var menuCartLink = $('#menu-cart a')
	
	menuCartLink.text('Song Cart (' + count + ')');

	return count;
	
}
function getCartCountAjax() {
	var menuCartLink = $('#menu-cart a')
	var url = menuCartLink.attr('href');
	url = url + '/CartCount?' + Rnd();
	var count = 0;
	//alert(url);
	$.ajax({
		async: false,
		url: url,
		type: 'GET',
		dataType: 'json',
		success: function (result) {
			//var result = "count is \"<strong>" + json + "\"";
			//alert("Result: " + result);
			//			    result == null ? _cartCount = 0 : _cartCount = result;
			count = result != null ? result : 0;
		}
	});

	return count;
}

function addToCartAjax(link) {

	var url = link[0].href;
	wait();
	
	$.ajax({
		url: url,
		type: 'POST',
		cache: false,
		dataType: 'json',
		success: function (data, status, xhr) {
			if (status == 'error') {
				feedback('error', xhr.status + ' ' + xhr.statusText);
			}
			else {
				feedback('info', '1 item added to your song cart');
				link.fadeOut('slow', function() { 
						link.text('In Cart');
						link.attr('title', 'In Cart');
						link.attr('href', '/Cart');
						//link.fadeTo('slow', 1);
						link.removeClass('cw-cart-add-link');
						//setupContentPanelUIControls();
						link.fadeIn('highlight');//, 0.2, function () {
					}
					);


				//feedback('info', 'Added to Cart');
				updateCartCount();
				unwait();
			}
		},
		error: function (xhr, status, error) {
			feedback('error', xhr.status + ' ' + xhr.statusText);
		}
	});

}

function addToCartMultipleAjax(link, items) {

	var url = link[0].href;
	wait();
	$.ajax({
		url: url,
		type: 'POST',
		cache: false,
		dataType: 'json',
		data: { 'items': items },
		success: function (data, status, xhr) {
			if (status == 'error') {
				unwait(); feedback('error', _cartAddErrorMsg);
			} else {
				var msg = data + ' ' + pluralize('item', data)  + ' added to your song cart';
				feedback('info', msg);
				updateCartCount();
				unwait();
			}
		},
		error: function (xhr, status, error) {
			unwait(); feedback('error', _cartAddErrorMsg);
		}
	}
	);

}

function deleteTagAjax(link) {

	var url = link[0].href;
	wait();
	$.ajax({
		url: url,
		type: 'POST',
		cache: false,
		dataType: 'json',
		success: function (data, status, xhr) {
			if (status == 'error') {
				feedback('error', xhr.status + ' ' + xhr.statusText);
			}
			else {
				//callback
				deleteTagCallback(data, link)
				unwait();
			}
		},
		error: function (xhr, status, error) {
			feedback('error', xhr.status + ' ' + xhr.statusText);
		}
	});
}

function writeID3Ajax(link) {

	var url = link[0].href;
	wait();
	link.fadeOut('slow');
	$.ajax({
		url: url,
		type: 'POST',
		cache: false,
		dataType: 'json',
		success: function (data, status, xhr) {
			if (status == 'error') {
				feedback('error', xhr.status + ' ' + xhr.statusText);
			}
			else {
				var text = link.text();
				link.text('Saving...');
				link.fadeOut('slow', function () {

					link.fadeIn('highlight');
					link.text(text);
				}
					);
			}
			unwait();

		},
		error: function (xhr, status, error) {
			feedback('error', xhr.status + ' ' + xhr.statusText);
		}
	});

}
function saveMediaFilesAjax(formId, callback) {

	var form = $(formId);
	
	var options = {
		beforeSubmit: function () { wait(); },
		success: function (data, status, xhr) {
			if (status == 'error') {
				feedback('error', xhr.status + ' ' + xhr.statusText);
			}
			else {
				callback();
				//				resetMediaFileDialog(template);
				//				dialog.dialog('close');
			}
			unwait();
		},
		error:
			function (xhr, status, error) {
				unwait();
				feedback('error', xhr.status + ' ' + xhr.statusText);

			}
	};

	form.ajaxSubmit(options);

}
//-----------------------------------------------------------------------------------
// Catalog Contents Management
//-----------------------------------------------------------------------------------
function deleteContentMultipleAjax(link, items) {

	var url = link[0].href;
	wait();
	$.ajax({
		url: url,
		type: 'POST',
		cache: false,
		dataType: 'json',
		data: { 'items': items },
		success: function (data, status, xhr) {
			if (status == 'error') {
				unwait();
				feedback('error', xhr.status + ' ' + xhr.statusText);
			} else {
				feedback('info', data + ' ' + pluralize('item', data) + ' deleted');
				updateCartCount();
				closeContentPanel();
				removeCheckedRows();
				hideEmptyContentsTable();
				unwait();
			}
		},
		error:
			function (xhr, status, error) {
				unwait();
				feedback('error', xhr.status + ' ' + xhr.statusText);

			}
	});
}

//function removeFromCartAjax(url) {
//   
//	$.ajax({
//		url: url,
//		type: 'POST',
//		cache: false,
//		dataType: 'json',
//		success: function (data, status, xhr) {
//		    if (status == 'error') {
//		        feedback('error', xhr.status + ' ' + xhr.statusText);
//		    }
//		    else {
//		        feedback('info', 'Removed from Cart');
//		        updateCartCount();
//		    }
//		},
//		error: function (xhr, status, error) {
//		    feedback('error', xhr.status + ' ' + xhr.statusText);
//		}
//	});

//}

//-----------------------------------------------------------------------------------
// User Management
//-----------------------------------------------------------------------------------
function getUserDetailAjax(url) {
	//var url = '/User/Userdetail/' + userId;
	wait();
	$.ajax({
		url: url,
		cache: false,
		dataType: 'html',
		success: function (data, status, xhr) {
			if (status == 'error') {
				feedback('error', xhr.status + ' ' + xhr.statusText);
			}
			else {

				showUserDetailPanel(data);
				unwait();
			}
		},
		error: function (xhr, status, error) {
			feedback('error', xhr.status + ' ' + xhr.statusText);
		}
	}
 );
}

function getCatalogDetailAjax(url){
	//var url = '/User/Userdetail/' + userId;
	wait();
	$.ajax({
		url: url,
		cache: false,
		dataType: 'html',
		success: function (data, status, xhr) {
			if (status == 'error') {
				feedback('error', xhr.status + ' ' + xhr.statusText);
			}
			else {

				showCatalogDetailPanel(data);
				unwait();
			}
		},
		error: function (xhr, status, error) {
			feedback('error', xhr.status + ' ' + xhr.statusText);
		}
	}
 );
}


function updateRoleAjax(link) {

	var url = link[0].href;

	$.ajax({
		url: url,
		dataType: 'json',
		type: 'POST',
		cache: false,
		success: function (data, status, xhr) {
			if (status == "error") {
				feedback('error', xhr.status + ' ' + xhr.statusText);
			}
			else {
				toggleTagBoxSelection(link, 'cw-green', true)
				//feedback('info', 'User role updated');
			}
		},
		error: function (xhr, status, error) {
			feedback('error', xhr.status + " " + xhr.statusText);
		}
		}
	);

}

function setSystemAdminAccessAjax(link) {

	var url = link[0].value;

	$.ajax({
		url: url,
		dataType: 'json',
		type: 'POST',
		cache: false,
		success: function (data, status, xhr) {
			if (status == "error") {
				feedback('error', xhr.status + ' ' + xhr.statusText);
			}
			else {
//				var label = link.next('label');
//				if (link.is(':checked')) {
//					link.removeAttr('checked');
//					label.text('No');
//					label.removeClass('cw-label-red');
//				} else {
//					link.attr('checked', true);
//					label.text('Yes');
//					label.addClass('cw-label-red');
//				}

				feedback('info', 'System access updated');
			}
		},
		error: function (xhr, status, error) {
			feedback('error', xhr.status + " " + xhr.statusText);
		}
	}
	);

}

function updateUserCatRoleAjax(link) {

	wait();
	var url = link[0].href;
	var detUrl = link[0].rel;

	$.ajax({
		url: url,
		dataType: 'json',
		type: 'POST',
		cache: false,
		success: function (data, status, xhr) {
			if (status == "error") {
				feedback('error', xhr.status + ' ' + xhr.statusText);
			}
			else {
				getUserDetailAjax(detUrl);
				unwait();
				//feedback('info', 'Catalog access updated');
			}
		},
		error: function (xhr, status, error) {
			feedback('error', xhr.status + " " + xhr.statusText);
		}
	}
	);

}

function updateCatRoleAjax(link) {

	wait();
	var url = link[0].href;
	var detUrl = link[0].rel;

	$.ajax({
		url: url,
		dataType: 'json',
		type: 'POST',
		cache: false,
		success: function (data, status, xhr) {
			if (status == "error") {
				feedback('error', xhr.status + ' ' + xhr.statusText);
			}
			else {
				getCatalogDetailAjax(detUrl);
				unwait();
				//feedback('info', 'Catalog access updated');
			}
		},
		error: function (xhr, status, error) {
			feedback('error', xhr.status + " " + xhr.statusText);
		}
	}
	);

}