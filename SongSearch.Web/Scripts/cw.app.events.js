﻿$(document).ready(function () {

    //-----------------------------------------------------------------------------------
    // AutoComplete
    //-----------------------------------------------------------------------------------
    setupAutoComplete();

    //-----------------------------------------------------------------------------------
    // Search option panel
    //-----------------------------------------------------------------------------------

    //$('.cw-form-value-button').button();
    //***********************************************
    //  Form Reset
    //***********************************************
    $('.cw-reset-form').click(
			function (evt) {
			    evt.preventDefault();
			    clearSearchForm($(this).parents('form'));

			}
		);

    //***********************************************
    //  Tag Box Click
    //***********************************************
    $('.cw-tagbox-search').click(
		function (evt) {
		    evt.preventDefault();

		    var link = $(this);
		    setSelectedSearchTagValue(link);

		    // alert($('#' + propId).val());
		}
	);

    //***********************************************
    //  Tag CheckBox Click
    //***********************************************
    $('.cw-tagbox-label-edit').live('click',
		function (evt) {
		    $(this).toggleClass('cw-blue');
		}
	);
    //***********************************************
    //  Tag Box More Choices Click
    //***********************************************
    $('.cw-tags-more-link').click(
		function (evt) {
		    evt.preventDefault();

		    var link = $(this);
		    var moretags = link.siblings('.cw-more-tags');
		    moretags.toggleClass('cw-optional');
		    link.text(link.text().swap('more', 'less'));

		    // alert($('#' + propId).val());
		}
	);

    //-----------------------------------------------------------------------------------
    // Search results
    //-----------------------------------------------------------------------------------


    $('#cw-select-all-items-check').click(
		function (evt) {
		    var checkboxes = $('.add-to-cart-checkbox');
		    if ($(this).is(':checked')) {
		        var cartCount = getCartCountAjax();
		        var unchecked = checkboxes.filter(':not(:checked) :not(:disabled)');
		        if (unchecked.length + cartCount > 100) {
		            feedback('error', _cartOverMaxMsg);
		        } else {
		            unchecked.attr('checked', 'checked');
		            unchecked.addClass('clicked');
		        }

		    } else {
		        var clicked = checkboxes.filter('.clicked'); // filter(':checked :not(:disabled)');
		        clicked.filter('.clicked').removeAttr('checked');
		        clicked.removeClass('clicked');

		    }

		    updateAddToCartAllButtontext($('.clicked').length);
		}
	);
    //
    $('#cw-select-all-cart-items-check').click(
		function (evt) {
		    var checkboxes = $('.remove-from-cart-checkbox');
		    if ($(this).is(':checked')) {
		        checkboxes.attr('checked', 'checked');
		    } else {
		        checkboxes.removeAttr('checked');
		    }
		}
	);

    $('#cw-remove-all-from-cart').click(
		function (evt) {
		    evt.preventDefault();
		    var checkboxes = $('.remove-from-cart-checkbox:checked');
		    if (checkboxes.length > 0) {
		        var form = $('#cw-cart-form');
		        form.attr('action', '/Cart/RemoveMultiple');
		        form.submit();
		    }
		}

	);

    $('#cw-add-all-to-cart').click(
		function (evt) {
		    evt.preventDefault();
		    var checkboxes = $('.add-to-cart-checkbox').filter('.clicked');
		    if (checkboxes.length > 0) {
		        var cartCount = getCartCountAjax();

		        if (checkboxes.length + cartCount > 100) {
		            feedback('error', _cartOverMaxMsg);
		        } else {

		            var items = new Array();
		            checkboxes.each(function (i) {
		                var id = checkboxes[i].id;
		                items.push(id);
		            }
				);
		            checkboxes.attr('disabled', 'disabled');
		            checkboxes.removeClass('clicked');
		            updateAddToCartAllButtontext(0)
		            addToCartMultipleAjax($(this), items);

		        }
		    }
		}
	);

    $('.add-to-cart-checkbox').click(
		function (evt) {

		    //		    var checkboxes = $('.add-to-cart-checkbox:checked');
		    $(this).toggleClass('clicked');
		    var count = $('.clicked').length;
		    var cartCount = getCartCountAjax();
		    if (count + cartCount > 100) {
		        feedback('error', _cartOverMaxMsg);
		        $(this).removeClass('clicked');
		        evt.preventDefault();
		    } else {
		        updateAddToCartAllButtontext(count);
		    }
		}
	);
    //

    //***********************************************
    //  Content detail link
    //***********************************************
    $('.cw-content-detail-link').live('click',
	function (evt) {

	    evt.preventDefault();
	    var link = $(this);

	    if (isContentEditMode) {
	        if (window.confirm('You\'re in Edit mode. Do you want to SAVE changes you\'ve made?')) {
	            // save stuff, then showContentPanel, maybe pass in as a delegate? for now:
	            saveContentPanel(lastContentEditLinkClicked, link);
	        } else {
	            feedback('warning', 'Save cancelled...');
	            showContentPanel(link);
	        }

	    } else {
	        showContentPanel(link);
	    }
	}
	);

    //***********************************************
    //  Content edit link
    //***********************************************
    $('.cw-content-edit-link').live('click',
	function (evt) {

	    evt.preventDefault();

	    var link = $(this);
	    showContentPanel(link);
	}
	);

    $('.cw-delete-right-link').live('click',
	function (evt) {

	    evt.preventDefault();

	    var link = $(this);
	    deleteContentRight(link);
	}
	);

    //***********************************************
    //  Content save link
    //***********************************************
    $('.cw-content-save-link').live('click',
	function (evt) {

	    evt.preventDefault();

	    var link = $(this);

	    saveContentPanel(link, null);

	}
	);
    //-----------------------------------------------------------------------------------
    // Content detail panel
    //-----------------------------------------------------------------------------------
    //***********************************************
    //  Close button click
    //***********************************************
    $('#cw-detail-close-link').live('click',
		function (evt) {
		    closeContentPanel();
		}
	);


    //-----------------------------------------------------------
    // Content detail menu
    //-----------------------------------------------------------
    //***********************************************
    //  Add-to-cart link click
    //***********************************************
    $('.cw-cart-add-link').live('click',
		function (evt) {

		    evt.preventDefault();

		    var link = $(this);
		    var cartCount = getCartCountAjax();

		    if (cartCount + 1 > 100) {
		        feedback('error', _cartOverMaxMsg);
		    } else {
		        //var id = link[0].rel;
		        addToCartAjax(link);
		    }
		}
	);

    //-----------------------------------------------------------------------------------
    //Media Player 
    //-----------------------------------------------------------------------------------


    //***********************************************
    //  Media Play
    //***********************************************
    $('.cw-media-play-link').live('click',
		function (evt) {

		    evt.preventDefault();

		    var url = this.href;
		    //var id = link[0].rel;
		    mediaPlay(url, '#' + this.id);
		}
	);

    $('.cw-media-repeat-link').live('click',
		function (evt) {

		    evt.preventDefault();
		    mediaRepeat();
		}
	);

    //-----------------------------------------------------------------------------------
    // Cart Grid
    //-----------------------------------------------------------------------------------
    //***********************************************
    // Show/Hide cart contents link
    //***********************************************
    $('.cw-carts-contents').click(
		function (evt) {

		    evt.preventDefault();

		    var cartId = $(this)[0].id.replace('s-', '');
		    //alert(cartId);
		    $('#c-' + cartId).toggle();
		    $(this).toggleClass('cw-carts-contents-show');
		    $(this).toggleClass('cw-carts-contents-hide');
		}
	);

    //-----------------------------------------------------------------------------------
    // User Management List
    //-----------------------------------------------------------------------------------
    //***********************************************
    // User detail link
    //***********************************************
    $('.cw-user-detail-link').live('click',
		function (evt) {

		    evt.preventDefault();
		    var link = $(this);
		    var url = link[0].href;
		    var listing = link.parents('.cw-user-listing');
		    $('.cw-user-listing').removeClass('cw-selected');
		    listing.addClass('cw-selected');

		    getUserDetailAjax(url);

		}
	);

    //***********************************************
    // User role edit link
    //***********************************************
    $('.cw-role-edit').live('click',
		function (evt) {

		    evt.preventDefault();
		    var link = $(this);

		    updateRoleAjax(link, 'cw-green');


		}
	);

    //***********************************************
    // Catalog role edit link
    //***********************************************
    $('.cw-usrcat-role-edit').live('click',
		function (evt) {

		    evt.preventDefault();
		    var link = $(this);

		    updateUserCatRoleAjax(link);


		}
	);

    //***********************************************
    // Catalog role edit all link
    //***********************************************
    $('.cw-usrcat-role-edit-all').live('click',
		function (evt) {

		    evt.preventDefault();
		    var link = $(this);

		    updateUserCatRoleAjax(link);


		}
	);





    //***********************************************
    //  Catalog role edit link
    //***********************************************
    $('.cw-cat-role-edit').live('click',
	function (evt) {

	    evt.preventDefault();
	    var link = $(this);

	    updateCatRoleAjax(link);


	}
	);

    //***********************************************
    // Catalog role edit all link
    //***********************************************
    $('.cw-cat-role-edit-all').live('click',
	function (evt) {

	    evt.preventDefault();
	    var link = $(this);

	    updateCatRoleAjax(link);


	}
	);

    //

    //-----------------------------------------------------------------------------------
    // Catalog Management List
    //-----------------------------------------------------------------------------------
    //***********************************************
    // Catalog detail link
    //***********************************************
    $('.cw-catalog-detail-link').live('click',
		function (evt) {

		    evt.preventDefault();
		    var link = $(this);
		    var url = link[0].href;
		    var listing = link.parents('.cw-catalog-listing');
		    $('.cw-catalog-listing').removeClass('cw-selected');
		    listing.addClass('cw-selected');

		    getCatalogDetailAjax(url);

		}
	);
}
);
