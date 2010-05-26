function getContentDetail(url, trigger) {
    
    wait(null, null);

    $.ajax({
        url: url,
        type: 'GET',
        cache: false,
        dataType: 'html',
        success: function (data, status, xhr) {
            if (status == "error") {
                showErrorNotice(xhr.status + " " + xhr.statusText);
            }
            else {

                showContentPanelCallback(data, trigger);
                unwait(null); //detailsPanel);
            }
        },
        error: function (xhr, status, error) {
            showErrorNotice(xhr.status + " " + xhr.statusText);
        }
    });

}

//-----------------------------------------------------------------------------------
// Song Cart
//-----------------------------------------------------------------------------------
function updateCartCount() {

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

function addToCart(url) {

	$.ajax({
	    url: url,
	    type: 'POST',
		cache: false,
		dataType: 'json',
		success: function (data, status, xhr) {
		    if (status == "error") {
		        showErrorNotice(xhr.status + " " + xhr.statusText);
		    }
		    else {
		        updateCartCount();
		    }
		},
		error: function (xhr, status, error) {
		    showErrorNotice(xhr.status + " " + xhr.statusText);
		}
	});

}


function removeFromCart(url) {
   
	$.ajax({
		url: url,
		type: 'POST',
		cache: false,
		dataType: 'json',
		success: function (data, status, xhr) {
		    if (status == "error") {
		        showErrorNotice(xhr.status + " " + xhr.statusText);
		    }
		    else {
		        updateCartCount();
		    }
		},
		error: function (xhr, status, error) {
		    showErrorNotice(xhr.status + " " + xhr.statusText);
		}
	});

}

//-----------------------------------------------------------------------------------
// User Management
//-----------------------------------------------------------------------------------
function getUserDetail(url) {
    //var url = '/User/Userdetail/' + userId;
    $.ajax({
        url: url,
        cache: false,
        dataType: 'html',
        success: function (data, status, xhr) {
            if (status == "error") {
                showErrorNotice(xhr.status + " " + xhr.statusText);
            }
            else {
                showUserDetailPanel(data);
            }
        },
        error: function (xhr, status, error) {
            showErrorNotice(xhr.status + " " + xhr.statusText);
        }
    }
            );

}