


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
    if ($('[data-related-products-by-brand]').length > 0) {
        GetRelatedProductsPartialByBrand();
    }
    if ($('[data-related-products-by-category]').length > 0) {
        GetRelatedProductsPartialByCategory();
    }
    if ($('[data-product-categories]').length > 0) {
        GetProductCategories();
    }
    if ($('[data-brands]').length > 0) {
        GetBrands();
    }
}
function GetBrands() {

    $('[data-brands]').each(function () {
        var truethis = this;
        var designName = $(this).attr('data-template-design-name');
        var postData = JSON.stringify({ "designName": designName });
        ajaxMethodCall(postData, "/AjaxProducts/GetBrands", function (data) {
            $(truethis).empty();
            $(truethis).html(data).animate({ 'height': '150px' }, 'slow');
        });
    });
}
function GetProductCategories() {

    $('[data-product-categories]').each(function () {
        var truethis = this;
        var designName = $(this).attr('data-template-design-name');
        var postData = JSON.stringify({ "designName": designName });
        ajaxMethodCall(postData, "/AjaxProducts/GetProductCategories", function (data) {
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
        ajaxMethodCall(postData, "/AjaxContents/GetRelatedContents", function (data) {
            $(truethis).empty();
            $(truethis).html(data).animate({ 'height': '150px' }, 'slow');
        });
    });
}

function GetRelatedProductsPartialByCategory() {
    $('[data-related-products-by-category]').each(function () {
        var truethis = this;
        var designName = $(this).attr('data-template-design-name');
        var productCategoryId = $(this).attr('data-related-products-by-category');
        var excludedProductId = $(this).attr('data-excluded-product-id');
        var postData = JSON.stringify({ "categoryId": productCategoryId, "excludedProductId" : excludedProductId, "designName": designName });
        ajaxMethodCall(postData, "/AjaxProducts/GetRelatedProductsPartialByCategory", function (data) {
            $(truethis).empty();
            $(truethis).html(data).animate({ 'height': '150px' }, 'slow');
        });
    });
}
function GetRelatedProductsPartialByBrand() {
    $('[data-related-products-by-brand]').each(function () {
        var truethis = this;
        var designName = $(this).attr('data-template-design-name');
        var brandId = $(this).attr('data-related-products-by-brand');
        var excludedProductId = $(this).attr('data-excluded-product-id');
        var postData = JSON.stringify({ "brandId": brandId, "excludedProductId": excludedProductId, "designName": designName });
        ajaxMethodCall(postData, "/AjaxProducts/GetRelatedProductsPartialByBrand", function (data) {
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
