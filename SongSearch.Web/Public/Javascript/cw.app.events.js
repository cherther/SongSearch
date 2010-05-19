$(document).ready(function () {


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
