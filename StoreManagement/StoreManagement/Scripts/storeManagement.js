function isEmpty(str) {
    return (!str || 0 === str.length);
}

$(document).ready(function () {
  
    

});

function ajaxMethodCall(postData,ajaxUrl, successFunction) {

    $.ajax({
        type: "POST",
        url: ajaxUrl,
        data: postData,
        success: successFunction,
        error: function(jqXHR, exception) {
            if (jqXHR.status === 0) {
                console.error('Not connect.\n Verify Network.');
            } else if (jqXHR.status == 404) {
                console.error('Requested page not found. [404]');
            } else if (jqXHR.status == 500) {
                console.error('Internal Server Error [500].');
            } else if (exception === 'parsererror') {
                console.error('Requested JSON parse failed.');
            } else if (exception === 'timeout') {
                console.error('Time out error.');
            } else if (exception === 'abort') {
                console.error('Ajax request aborted.');
            } else {
                console.error('Uncaught Error.\n' + jqXHR.responseText);
            }
        },
        contentType:"application/json",
        dataType: "json"
    });
}

function deleteItemsSuccess(data) {
    data.forEach(function (entry) {
        var pp = $('[gridkey-id=' + entry + ']');
        pp.parent().parent().remove();
    });
}

function bindCarouselImage() {
    $('[data-carusel-file-id]').each(function () {
        $(this).off("click");
        $(this).on("click", handleCarouselImage);
    });
}

function handleCarouselImage(e) {
    var caller = e.target;
    var fileId = $(caller).attr('data-carusel-file-id');
    var isCarousel = $(caller).attr('data-carusel-file-isCarousel');
    if (isCarousel === "true") {
        isCarousel = "false";
    } else if (isCarousel === "false") {
        isCarousel = "true";
    }

    var postData = JSON.stringify({ "fileId": fileId, "isCarousel": isCarousel });
    ajaxMethodCall(postData, "/Ajax/ChangeIsCarouselState", changeCarouselStateSuccess);

}
function changeCarouselStateSuccess(data) {

    var mmm = $('a[data-carusel-file-id=' + data.fileId + ']');
    mmm.attr('data-carusel-file-isCarousel', data.isCarousel);
    mmm.removeClass("btn-success");
    mmm.removeClass("btn-danger");
    if (data.isCarousel) {
        mmm.addClass("btn-success");
    } else if (!data.isCarousel) {
        mmm.addClass("btn-danger");
    }
   

}