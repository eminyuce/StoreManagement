


$(document).ready(function () {
    //var mm = $("[data-view-contact-resume-contact=" + encodedResumeId + "]");
    //mm.fadeOut('slow', function () {
    //    mm.html(data);
    //    mm.fadeIn('slow');
    //});

    callAjaxMethod();
    GetAttributeBaseAjax();
});
function callAjaxMethod() {

    try {
        GetMainNavigation();
    }
    catch (err) {
        console.error(err.message);
    }

    try {
        GetFooter();
    }
    catch (err) {
        console.error(err.message);
    }



}
function GetAttributeBaseAjax() {
    try {
        if ($('[data-related-contents]').length > 0) {
            GetRelatedContents();
        }
    }
    catch (err) {
        console.error(err.message);
    }

    try {
        if ($('[data-product-labels]').length > 0) {
            GetProductLabels();
        }
    }
    catch (err) {
        console.error(err.message);
    }



    try {
        if ($('[data-product-categories]').length > 0) {
            GetProductCategories();
        }
    }
    catch (err) {
        console.error(err.message);
    }

    try {
        if ($('[data-brands]').length > 0) {
            GetBrands();
        }

    }
    catch (err) {
        console.error(err.message);
    }

    try {
        if ($('[data-products-by-product-type]').length > 0) {
            GetProductsByProductType();
        }
    }
    catch (err) {
        console.error(err.message);
    }


    try {
        if ($('[data-contents-by-content-type]').length > 0) {
            GetContentsByContentType();
        }
    }
    catch (err) {
        console.error(err.message);
    }
}

function GetContentsByContentType() {

    $('[data-contents-by-content-type]').each(function () {
        var truethis = this;
        var innerHtmlLength = truethis.innerHTML.length;
        if (innerHtmlLength == 0) {
            var page = parseInt($(this).attr('data-page'));
            var designName = $(this).attr('data-template-design-name');
            var categoryId = GetValueInt($(this).attr('data-content-category-id'));
            var imageWidth = GetValueInt($(this).attr('data-image-width'));
            var imageHeight = GetValueInt($(this).attr('data-image-height'));
            var pageSize = GetValueInt($(this).attr('data-page-size'));
            var contentType = $(this).attr('data-content-type');
            var type = $(this).attr('data-contents-by-content-type');
            var excludedContentId = GetValueInt($(this).attr('data-excluded-content-id'));
            var postData = JSON.stringify({
                "page": page,
                "designName": designName,
                "categoryId": categoryId,
                "type": type,
                "imageWidth": imageWidth,
                "imageHeight": imageHeight,
                "pageSize": pageSize,
                "contentType": contentType,
                "excludedContentId": excludedContentId
            });

            ajaxMethodCall(postData, "/AjaxContents/GetContentsByContentType", function (data) {
                AddDataToDiv(truethis, data, page);
            });
        }
    });
}
function GetProductsByProductType() {

    $('[data-products-by-product-type]').each(function () {
        var truethis = this;
        var innerHtmlLength = truethis.innerHTML.length;
        if (innerHtmlLength == 0) {
            var page = parseInt($(this).attr('data-page'));
            var designName = $(this).attr('data-template-design-name');
            var categoryId = GetValueInt($(this).attr('data-product-category-id'));
            var brandId = GetValueInt($(this).attr('data-brand-id'));
            var imageWidth = GetValueInt($(this).attr('data-image-width'));
            var imageHeight = GetValueInt($(this).attr('data-image-height'));
            var pageSize = GetValueInt($(this).attr('data-page-size'));
            var productType = $(this).attr('data-product-type');
            var excludedProductId = GetValueInt($(this).attr('data-excluded-product-id'));
            var postData = JSON.stringify({
                "page": page,
                "designName": designName,
                "categoryId": categoryId,
                "brandId": brandId,
                "imageWidth": imageWidth,
                "imageHeight": imageHeight,
                "pageSize": pageSize,
                "productType": productType,
                "excludedProductId": excludedProductId
            });

            ajaxMethodCall(postData, "/AjaxProducts/GetProductsByProductType", function (data) {
                AddDataToDiv(truethis, data, page);

            });
        }
    });
}

function GetProductLabels() {
    $('[data-product-labels]').each(function () {
        var truethis = this;
        var innerHtmlLength = truethis.innerHTML.length;
        if (innerHtmlLength == 0) {
            var designName = $(this).attr('data-template-design-name');
            var itemid = $(this).attr('data-product-itemid');
            var itemtype = 'product';
            var postData = JSON.stringify({ id: itemid, "designName": designName });
            ajaxMethodCall(postData, "/AjaxProducts/GetProductLabels", function (data) {
                AddDataToDiv(truethis, data);
            });
        }
    });
}
function GetFooter() {
    $('[data-main-footer]').each(function () {
        var truethis = this;
        var innerHtmlLength = truethis.innerHTML.length;
        if (innerHtmlLength == 0) {
            var designName = $(this).attr('data-template-design-name');
            var postData = JSON.stringify({ "designName": designName });
            ajaxMethodCall(postData, "/AjaxGenerics/Footer", function(data) {
                AddDataToDiv(truethis, data, 0);
 
            });
        }
    });
}
function GetMainNavigation() {
    $('[data-main-navigation]').each(function () {
        var truethis = this;
        var innerHtmlLength = truethis.innerHTML.length;
        if (innerHtmlLength == 0) {
            var designName = $(this).attr('data-template-design-name');
            var postData = JSON.stringify({ "designName": designName });
            ajaxMethodCall(postData, "/AjaxGenerics/MainNavigation", function (data) {
                AddDataToDiv(truethis, data, 0);
                 
            });
        }
    });
}
function GetBrands() {
    $('[data-brands]').each(function () {
        var truethis = this;
        var innerHtmlLength = truethis.innerHTML.length;
        if (innerHtmlLength == 0) {
            var designName = $(this).attr('data-template-design-name');
            var imageWidth = GetValueInt($(this).attr('data-image-width'));
            var imageHeight = GetValueInt($(this).attr('data-image-height'));

            var postData = JSON.stringify({
                "designName": designName,
                "imageWidth": imageWidth,
                "imageHeight": imageHeight
            });
            ajaxMethodCall(postData, "/AjaxProducts/GetBrands", function (data) {
                AddDataToDiv(truethis, data, 0);
            });
        }
    });
}
function GetProductCategories() {

    $('[data-product-categories]').each(function () {
        var truethis = this;
        var innerHtmlLength = truethis.innerHTML.length;
        if (innerHtmlLength == 0) {
            var designName = $(this).attr('data-template-design-name');
            var imageWidth = GetValueInt($(this).attr('data-image-width'));
            var imageHeight = GetValueInt($(this).attr('data-image-height'));
            var postData = JSON.stringify({
                "designName": designName,
                "imageWidth": imageWidth,
                "imageHeight": imageHeight
            });
            ajaxMethodCall(postData, "/AjaxProducts/GetProductCategories", function (data) {
                AddDataToDiv(truethis, data, 0);
            });
        }
    });
}
function GetRelatedContents() {
    $('[data-related-contents]').each(function () {
        var truethis = this;
        var innerHtmlLength = truethis.innerHTML.length;
        if (innerHtmlLength == 0) {
            var designName = $(this).attr('data-template-design-name');
            var contentType = $(this).attr('data-related-contents-type');
            var categoryId = $(this).attr('data-related-contents');
            var imageWidth = GetValueInt($(this).attr('data-image-width'));
            var imageHeight = GetValueInt($(this).attr('data-image-height'));
            var take = GetValueInt($(this).attr('data-number-of-items'));
            var postData = JSON.stringify({
                "categoryId": categoryId,
                "contentType": contentType,
                "designName": designName,
                "take": take,
                "imageWidth": imageWidth,
                "imageHeight": imageHeight
            });
            ajaxMethodCall(postData, "/AjaxContents/GetRelatedContents", function (data) {
                AddDataToDiv(truethis, data, 0);
            });
        }
    });
}
function AddDataToDiv(truethis, data, page) {
    $(truethis).empty();
    $(truethis).html(data).animate({}, 'slow');
    var dataPageAttr = $(truethis).attr('data-page');
    if (typeof dataPageAttr !== typeof undefined && dataPageAttr !== false) {
        $(truethis).attr('data-page', page + 1);
    }
}

function GetValueInt(val) {
    val = val === undefined ? 0 : val;
    return val;
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
