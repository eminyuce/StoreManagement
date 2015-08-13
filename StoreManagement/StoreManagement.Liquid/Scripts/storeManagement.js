


$(document).ready(function () {
    //var mm = $("[data-view-contact-resume-contact=" + encodedResumeId + "]");
    //mm.fadeOut('slow', function () {
    //    mm.html(data);
    //    mm.fadeIn('slow');
    //});

    callAjaxMethod();
});
function callAjaxMethod() {

    if ($('[data-related-contents]').length > 0) {
        GetRelatedContents();
    }
 
    if ($('[data-related-products]').length > 0) {
        GetRelatedProducts();
    }
    if ($('[data-product-categories]').length > 0) {
        GetProductCategories();
    }

}
function GetProductCategories() {

    $('[data-product-categories]').each(function () {
        var truethis = this;
        var designName = $(this).attr('data-template-design-name');
        var postData = JSON.stringify({ "designName": designName });
        ajaxMethodCall(postData, "/Ajax/GetProductCategories", function (data) {
            $(truethis).empty();
            $(truethis).html(data).animate({ 'height': '150px' }, 'slow');
        });
    });
}
function GetRelatedContents() {
    $('[data-related-contents]').each(function () {
        var truethis = this;
        var designName = $(this).attr('data-template-design-name');
        var contentType = $(this).attr('data-related-contents-type');
        var categoryId = $(this).attr('data-related-contents');
        var postData = JSON.stringify({ "categoryId": categoryId, "contentType": contentType, "designName": designName });
        ajaxMethodCall(postData, "/Ajax/GetRelatedContents", function (data) {
            $(truethis).empty();
            $(truethis).html(data).animate({ 'height': '150px' }, 'slow');
        });
    });
}

function GetRelatedProducts() {
    $('[data-related-products]').each(function () {
        var truethis = this;
        var designName = $(this).attr('data-template-design-name');
        var productCategoryId = $(this).attr('data-related-products');
        var postData = JSON.stringify({ "categoryId": productCategoryId, "designName": designName });
        ajaxMethodCall(postData, "/Ajax/GetRelatedProducts", function (data) {
            $(truethis).empty();
            $(truethis).html(data).animate({ 'height': '150px' }, 'slow');
        });
    });
}
 

function ajaxMethodCall(postData, ajaxUrl, successFunction) {

    $.ajax({
        type: "POST",
        url: ajaxUrl,
        data: postData,
        success: successFunction,
        error: function (jqXHR, exception) {
            console.error("parameters :" + postData);
            console.error("ajaxUrl :" + ajaxUrl);
            console.error("responseText :" + jqXHR.responseText);
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
        contentType: "application/json",
        dataType: "json"
    });
}
