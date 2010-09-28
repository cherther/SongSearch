$(document).ready(function () {

	//-----------------------------------------------------------------------------------
	// AutoComplete
	//-----------------------------------------------------------------------------------
	setupAutoComplete();
	setupTooltips();
	
	$.prettyLoader({loader: '/public/images/prettyLoader/ajax-loader.gif'});
	//-----------------------------------------------------------------------------------
	// Search option panel
	//-----------------------------------------------------------------------------------

	//$('.cw-form-value-button').button();
	//-----------------------------------
	// Twitter sign-in
	//-----------------------------------
	$(".signin").click(function (evt) {
		evt.preventDefault();
		$("fieldset#signin_menu").toggle();
		$(".signin").toggleClass("menu-open");
	});

	$("fieldset#signin_menu").mouseup(function () {
		return false
	});

	$(document).mouseup(function (evt) {
		if ($(evt.target).parent("a.signin").length == 0) {
			$(".signin").removeClass("menu-open");
			$("fieldset#signin_menu").hide();
		}
	});

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
	$('.cw-select-all-items-check').live('click',
		function (evt) {
			toggleRowCheckboxes(this);
		}
	);

	$('#cw-remove-all-from-cart').click(
		function (evt) {
			evt.preventDefault();
			var checkboxes = $('.cw-row-checkbox-cart-remove:checked');
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

			$("#dialog-confirm-save-changes").dialog({
				resizable: false,
				height: 200,
				width: 360,
				modal: true,
				title: 'Save Changes?',
				buttons: {
					'Save Changes': function () {

						saveContentPanel(lastContentEditLinkClicked, link);

						$(this).dialog('close');
					},
					Cancel: function () {
						showContentPanel(link);
						$(this).dialog('close');
					}
				}
			});

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

	$('.cw-delete-tag-link').live('click',
		function (evt) {

			evt.preventDefault();

			var link = $(this);
			deleteTagAjax(link);
		}
	);

	//Content_IsControlledAllIn
	//	var _rightsTypes = { Master: 1, Comp: 2 };

	//	$('#Content_IsControlledAllIn').live('click',
	//		function (evt) {
	//			var allin = $(this).is(':checked');
	//			var defaultRightsHolder = $('#DefaultRightsHolderName').val();
	//			var defaultTerritoryId = $('#DefaultTerritoryId').val();

	//			if (defaultRightsHolder != null && defaultTerritoryId != null) {
	//				var comp = $('#rights_0__RightsHolderName');
	//				if (comp.val().length == 0) {
	//					comp.val(defaultRightsHolder);
	//					//RightsTypeId
	//					$('#rights_0__RightsTypeId').val(_rightsTypes.Comp);
	//					//RightsHolderShare
	//					$('#rights_0__RightsHolderShare').val('100%');
	//				}
	//				var master = $('#rights_1__RightsHolderName');
	//				if (master.length == 0 || master.val().length == 0) {
	//					master.val(defaultRightsHolder);
	//					//RightsTypeId
	//					$('#rights_1__RightsTypeId').val(_rightsTypes.Master);
	//					$('#rights_1__RightsHolderShare').val('100%');

	//				}

	//			}
	//		}
	//	);

	//	//cw-add-right-link
	//	$('.cw-add-right-link').live('click',
	//		function (evt) {

	//		}
	//	);
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

	$('#cw-write-id3-link').live('click',
		function (evt) {

			evt.preventDefault();
			var link = $(this);
			writeID3Ajax(link);
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
	//cw-play-preview

	$('.cw-media-repeat-link').live('click',
		function (evt) {

			evt.preventDefault();
			mediaRepeat();
		}
	);

	$('.cw-media-rew-link').live('click',
			function (evt) {

				evt.preventDefault();
				mediaRewind();
			}
		);
	$('.cw-media-ffwd-link').live('click',
		function (evt) {

			evt.preventDefault();
			mediaFastForward();
		}
	);

	$('.cw-media-cue-link').live('click',
		function (evt) {
			var range = this.rel.split(':');
			evt.preventDefault();
			mediaCue(range);
		}
	);
	//-----------------------------------------------------------------------------------
	// Cart Grid
	//-----------------------------------------------------------------------------------


	//$('.cw-tbl-data').sortable({
	//		placeholder: 'ui-state-highlight'
	//	});
	//$('.cw-tbl-data').disableSelection();


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
	$("input[name='cw-system-access']").live('click',
		function (evt) {
			var link = $(this);
			if (link.is(':checked')) {

				setSystemAdminAccessAjax(link);
			}
		}
	);

	//	$('.cw-role-edit').live('click',
	//		function (evt) {

	//			evt.preventDefault();
	//			var link = $(this);
	//			

	//			//setSystemAdminAccessAjax(link);


	//		}
	//	);

	$('#cw-user-delete-link').live('click',
		function (evt) {
			evt.preventDefault();
			$("#dialog-confirm-user-delete").dialog({
				resizable: false,
				height: 280,
				width: 360,
				modal: true,
				title: 'Delete User?',
				buttons: {
					'Delete User': function () {
						$('#cw-user-delete-form').submit();
						$(this).dialog('close');
					},
					Cancel: function () {
						evt.preventDefault();
						$(this).dialog('close');
					}
				}
			});
		}
	);
	$('#cw-user-takeowner-link').live('click',
		function (evt) {
			evt.preventDefault();
			$("#dialog-confirm-user-takeowner").dialog({
				resizable: false,
				height: 280,
				width: 360,
				modal: true,
				title: 'Take Ownership?',
				buttons: {
					'Take Ownership': function () {
						$('#cw-user-takeowner-form').submit();
						$(this).dialog('close');
					},
					Cancel: function () {
						evt.preventDefault();
						$(this).dialog('close');
					}
				}
			});
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


	//***********************************************
	// Catalog delete link
	//***********************************************
	$('#cw-catalog-delete-link').live('click',
		function (evt) {

			if (!confirm('Are you sure you want to delete this catalog and all of its songs?')) {
				evt.preventDefault();
			}
		}
	);

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
			var listing = link.parents('.cw-catalog-listing'); //???
			$('.cw-catalog-listing').removeClass('cw-selected');
			listing.addClass('cw-selected');
			closeContentPanel();
			getCatalogDetailAjax(url);

		}
	);

	$('#cw-catalog-contents-show-link').live('click',
		function (evt) {

			evt.preventDefault();
			toggleContentsTable($(this))

		}
	);

	$('#cw-delete-multiple-content').live('click',
		function (evt) {
			evt.preventDefault();
			var link = $(this);
			var checkboxes = $('.cw-row-checkbox-delete:checked');
			if (checkboxes.length > 0) {
				$("#dialog-confirm-song-delete").dialog({
					resizable: false,
					height: 200,
					width: 360,
					modal: true,
					title: 'Delete Songs?',
					buttons: {
						'Delete Songs': function () {

							var items = new Array();
							checkboxes.each(function (i) {
								var id = checkboxes[i].value;
								items.push(id);
							});
							checkboxes.attr('disabled', 'disabled');
							deleteContentMultipleAjax(link, items);


							$(this).dialog('close');
						},
						Cancel: function () {
							$(this).dialog('close');
						}
					}
				});
			}
		}
	);

	//cw-media-upload-link
	$('.cw-media-upload-link').live('click',
		function (evt) {
			evt.preventDefault();
			var link = $(this);
			closeContentPanel();
			setUpMediaFileDialog(link);

		}
	);
}
);
