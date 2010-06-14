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
				link.fadeTo('slow', 0.2, function () {
					link.text('In Cart');
					link.attr('title', 'In Cart');
					link.attr('href', '/Cart');
					link.fadeTo('slow', 1);
					link.removeClass('cw-cart-add-link');
					//setupContentPanelUIControls();
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
				unwait(); feedback('error', xhr.status + ' ' + xhr.statusText);
			} else {
				feedback('info', data + ' item(s) added to your song cart');
				updateCartCount();
				unwait();
			}
		},
		error: function (xhr, status, error) {
			unwait(); feedback('error', 'There was a problem adding the item(s) to your cart. Please make sure you have less than 100 items in your cart.');
		}
	}
	);

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
				feedback('info', 'User role updated');
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
			}
		},
		error: function (xhr, status, error) {
			feedback('error', xhr.status + " " + xhr.statusText);
		}
	}
	);

}