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
                fire('error', xhr.status + ' ' + xhr.statusText);
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
            fire('error', xhr.status + ' ' + xhr.statusText);
        }
    });

}

//***********************************************
//  submitContentFormAjax
//***********************************************
function submitContentFormAjax(formId, link, altLink) {
    var form = $(formId);
    var returnData = altLink == null;

    var options = {
        beforeSubmit: function () { wait(); },
        data: { returnData: returnData },
        success: function (data, status, xhr) {
            if (status == 'error') {
                unwait();
                fire('error', xhr.status + ' ' + xhr.statusText);
            } else {
                showContentPanelCallbackEdit(data, link, altLink);
            }
        },
        error:
			function (xhr, status, error) {
			    unwait();
			    fire(err);

			}
    };

    $(form).ajaxSubmit(options);

}

//-----------------------------------------------------------------------------------
// Song Cart
//-----------------------------------------------------------------------------------
function updateCartCountAjax() {

    var menuCartLink = $('#menu-cart a')
    var url = menuCartLink.attr('href');
    url = url + '/CartCount?' + Rnd();
    //alert(url);

    $.getJSON(
		    url,
		    '',
		    function (result) {
		        //var result = "count is \"<strong>" + json + "\"";
		        //alert("Result: " + result);
		        var count;
		        result == null ? count = 0 : count = result;
		        menuCartLink.text('Song Cart (' + count + ')');
		    }
	    );
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
                fire('error', xhr.status + ' ' + xhr.statusText);
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


                //fire('info', 'Added to Cart');
                updateCartCountAjax();
                unwait();
            }
        },
        error: function (xhr, status, error) {
            fire('error', xhr.status + ' ' + xhr.statusText);
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
//		        fire('error', xhr.status + ' ' + xhr.statusText);
//		    }
//		    else {
//		        fire('info', 'Removed from Cart');
//		        updateCartCountAjax();
//		    }
//		},
//		error: function (xhr, status, error) {
//		    fire('error', xhr.status + ' ' + xhr.statusText);
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
                fire('error', xhr.status + ' ' + xhr.statusText);
            }
            else {

                showUserDetailPanel(data);
                unwait();
            }
        },
        error: function (xhr, status, error) {
            fire('error', xhr.status + ' ' + xhr.statusText);
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
                fire('error', xhr.status + ' ' + xhr.statusText);
            }
            else {
                toggleTagBoxSelection(link, 'cw-green', true)
                fire('info', 'User role updated');
            }
        },
        error: function (xhr, status, error) {
            fire('error', xhr.status + " " + xhr.statusText);
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
                fire('error', xhr.status + ' ' + xhr.statusText);
            }
            else {
                getUserDetailAjax(detUrl);
                unwait();
            }
        },
        error: function (xhr, status, error) {
            fire('error', xhr.status + " " + xhr.statusText);
        }
    }
	);

}