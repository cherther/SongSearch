$(document).ready(function () {

     //-----------------------------------------------------------------------------------
    // AutoComplete
    //-----------------------------------------------------------------------------------
    var aCache = {};
    $(".cw-autocomplete").autocomplete(
        {
            //source: "/Search/AutoComplete?f=" + this.rel,//["John", "Johnny", "Jon", "Joe" ],
            source: function(request, response) {
                var field = $(this)[0].element[0].alt; //little hack to store an extra field on the input elem, rel does not work cross-browser for input tags
                if (field){
				    if (aCache.term == request.term && aCache.field == field && aCache.content) {
					    response(aCache.content);
					    return;
				    }
				    if (new RegExp(aCache.term).test(request.term) && aCache.content && aCache.content.length < 13) {
					    response($.ui.autocomplete.filter(aCache.content, request.term));
					    return;
				    }

                    var url = "/Search/AutoComplete?f=";
				    $.ajax({
					    url: url + field,
					    dataType: "json",
					    data: request,
					    success: function(data) {
                            aCache.field = field;
						    aCache.term = request.term;
						    aCache.content = data;
						    response(data);
					    }
				    });
                }
		    },
            minLength: 2,
        }
    );

    //-----------------------------------------------------------------------------------
    // Search option panel
    //-----------------------------------------------------------------------------------
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
    // Seatch results
    //-----------------------------------------------------------------------------------
    
    //***********************************************
    //  Content detail link
    //***********************************************
    $('.cw-content-detail-link').live('click',
    function (evt) {

        evt.preventDefault();

        var link = $(this);
        showContentPanel(link);
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
            //var id = link[0].rel;
            addToCartAjax(link[0]);
            
			link.text('In Cart');
			link.attr('title', 'In Cart');
			link.attr('href', '/Cart');
            link.removeClass('cw-cart-add-link');
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
            mediaPlay(url);
            togglePlayButton('#' + this.id);
        }
    );


    //-----------------------------------------------------------------------------------
    // Cart Grid
    //-----------------------------------------------------------------------------------
    //***********************************************
    // Show/Hide cart contents link
    //***********************************************
    $('.cw-tbl-carts-contents').click(
        function (evt) {

            evt.preventDefault();
           
			var cartId = $(this)[0].id.replace('s-', '');
			//alert(cartId);
			$('#c-' + cartId).toggle();
			$(this).toggleClass('cw-tbl-carts-contents-show');
			$(this).toggleClass('cw-tbl-carts-contents-hide');
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


   
}
);
