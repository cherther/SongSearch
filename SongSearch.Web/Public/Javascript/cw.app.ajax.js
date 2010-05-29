//-----------------------------------------------------------------------------------
// Search Results
//-----------------------------------------------------------------------------------
function getContentDetailAjax(url, link) {
    
    wait();

    $.ajax({
        url: url,
        type: 'GET',
        cache: false,
        dataType: 'html',
        success: function (data, status, xhr) {
            if (status == 'error') {
                flash('error', xhr.status + ' ' + xhr.statusText);
            }
            else {

                showContentPanelCallback(data, link);
                unwait(); //detailsPanel);
            }
        },
        error: function (xhr, status, error) {
            flash('error', xhr.status + ' ' + xhr.statusText);
        }
    });

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

function addToCartAjax(url) {

    $.ajax({
        url: url,
        type: 'POST',
        cache: false,
        dataType: 'json',
        success: function (data, status, xhr) {
            if (status == 'error') {
                flash('error', xhr.status + ' ' + xhr.statusText);
            }
            else {
                flash('info', 'Added to Cart');
                updateCartCountAjax();
            }
        },
        error: function (xhr, status, error) {
            flash('error', xhr.status + ' ' + xhr.statusText);
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
//		        flash('error', xhr.status + ' ' + xhr.statusText);
//		    }
//		    else {
//		        flash('info', 'Removed from Cart');
//		        updateCartCountAjax();
//		    }
//		},
//		error: function (xhr, status, error) {
//		    flash('error', xhr.status + ' ' + xhr.statusText);
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
                flash('error', xhr.status + ' ' + xhr.statusText);
            }
            else {

                showUserDetailPanel(data);
                unwait();
            }
        },
        error: function (xhr, status, error) {
            flash('error', xhr.status + ' ' + xhr.statusText);
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
                flash('error', xhr.status + ' ' + xhr.statusText);
            }
            else {
                toggleTagBoxSelection(link, 'cw-green', true)
                flash('info', 'User role updated');
            }
        },
        error: function (xhr, status, error) {
            flash('error', xhr.status + " " + xhr.statusText);
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
                flash('error', xhr.status + ' ' + xhr.statusText);
            }
            else {
                getUserDetailAjax(detUrl);
                unwait();
            }
        },
        error: function (xhr, status, error) {
            flash('error', xhr.status + " " + xhr.statusText);
        }
    }
	);

}