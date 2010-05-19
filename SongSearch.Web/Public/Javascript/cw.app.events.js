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
