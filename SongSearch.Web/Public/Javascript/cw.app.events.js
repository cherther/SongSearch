$(document).ready(function () {

    //-----------------------------------------------------------------------------------
    // form.reset
    //-----------------------------------------------------------------------------------
    $('.cw-reset-form').click(
			function (evt) {
			    evt.preventDefault();
			    clearSearchForm($(this).parents('form'));

			}
		);

    $('.cw-tagbox-search').click(
        function (evt) {
            evt.preventDefault();

            var link = $(this);
            setSelectedSearchTagValue(link);

            // alert($('#' + propId).val());
        }
    );

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
    // cw-detail-close-link
    //-----------------------------------------------------------------------------------
    $('#cw-detail-close-link').live('click',
        function (evt) {
            closeContentPanel();
        }
    );
    //-----------------------------------------------------------------------------------
    // cw-content-detail-link
    //-----------------------------------------------------------------------------------
    $('.cw-content-detail-link').live('click',
    function (evt) {

        evt.preventDefault();

        var link = $(this);
        showContentPanel(link);
    }
    );


}
);
