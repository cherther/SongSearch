function getContentDetail(url, trigger) {
    
    wait(null, null);

    $.ajax({
        url: url,
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