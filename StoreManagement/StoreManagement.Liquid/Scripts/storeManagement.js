

 
$(document).ready(function () {
    //var mm = $("[data-view-contact-resume-contact=" + encodedResumeId + "]");
    //mm.fadeOut('slow', function () {
    //    mm.html(data);
    //    mm.fadeIn('slow');
    //});

    callAjaxMethod();
    GetAttributeBaseAjax();
    
    
    $('[data-toggle="tooltip"]').tooltip();


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
function showLoadingImage() {
    
}
function hideLoadingImage() {
    
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

    try {
        $('[data-contents-next-page-button]').each(function () {
            $(this).off("click");
            $(this).on("click", function (e) {
                GetContentsByContentType();
            });
        });
    }
    catch (err) {
        console.error(err.message);
    }
    try {
        $('[data-products-next-page-button]').each(function () {
            $(this).off("click");
            $(this).on("click", function (e) {
                GetProductsByProductType();
            });
        });
    }
    catch (err) {
        console.error(err.message);
    }
    try {
        $('[data-auto-complete]').each(function () {
            var truethis = this;
            var autoComplete = $(truethis).attr('data-auto-complete');
            var take = parseInt($(truethis).attr('data-take'));
            $(truethis).autocomplete({
                source: function (request, response) {

                    console.log("auto complate " + autoComplete);
                    var items = new Array();
                    var jsonRequest = { "term": request.term, "type": autoComplete, "take": take };
                    console.log(jsonRequest);
                    if (request.term.length > 2) {
                        $.ajax({
                            type: "POST",
                            url: "/AjaxGenerics/SearchAutoComplete",
                            data: jsonRequest,
                            success: function (data) {
                                for (var i = 0; i < data.length ; i++) {
                                    items[i] = { text: data[i], value: data[i] };
                                }
                                console.log(items);
                                response(sortInputFirst(request.term, items));
                            }
                        });
                    }

                },
                select: function (event, ui) {
                    $("#SearchButton").click();
                }


            });
        });





    }
    catch (err) {
        console.error(err.message);
    }
    

    try {
        if ($('[data-contact-form]').length > 0) {
            GetContactForm();
        }
    }
    catch (err) {
        console.error(err.message);
    }
}

function GetContactForm() {

    $('[data-contact-form]').each(function () {
        var truethis = this;
        var designName = $(this).attr('data-template-design-name');
        var successMessage = $(this).attr('data-contact-success-message');
        var enabledLoaderImage = $(this).attr('data-enabled-loader-image');
        var postData = JSON.stringify({
            "designName": designName
        });
        ajaxMethodCall(postData, "/AjaxGenerics/GetContactForm", function (data) {
            AddDataToDiv(truethis, data, 0);
            
            if ($('#btnSaveContactForm').length > 0) {

                $('#btnSaveContactForm').off("click");
                $('#btnSaveContactForm').on("click", function (e) {
                    var request = new ContactFormModel();
                    var postData = JSON.stringify(request);
                    ajaxMethodCall(postData, "/AjaxGenerics/SaveContactForm", function (result) {
                        console.log(successMessage);
                        console.log(result);
                        SetTextMessage(successMessage);
                        EmptyContactFormModel();
                    });
                });
            }
        }, enabledLoaderImage);
    });
}
function SetTextMessage(message) {
    $("#contactMessage").animate({ opacity: 0 }, function () {
        $(this).text($("#contactMessage").val() + message).animate({ opacity: 1 });
    });
}
function GetContentsByContentType() {

    $('[data-contents-by-content-type]').each(function () {
        var truethis = this;
        var page = parseInt($(this).attr('data-page'));
        var designName = $(this).attr('data-template-design-name');
        var categoryId = GetValueInt($(this).attr('data-content-category-id'));
        var imageWidth = GetValueInt($(this).attr('data-image-width'));
        var imageHeight = GetValueInt($(this).attr('data-image-height'));
        var pageSize = GetValueInt($(this).attr('data-page-size'));
        var contentType = $(this).attr('data-content-type');
        var type = $(this).attr('data-contents-by-content-type');
        var excludedContentId = GetValueInt($(this).attr('data-excluded-content-id'));
        var enabledLoaderImage = $(this).attr('data-enabled-loader-image');
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
        }, enabledLoaderImage);
    });
}
function GetProductsByProductType() {

    $('[data-products-by-product-type]').each(function () {
        var truethis = this;
        var page = parseInt($(this).attr('data-page'));
        var designName = $(this).attr('data-template-design-name');
        var categoryId = GetValueInt($(this).attr('data-product-category-id'));
        var brandId = GetValueInt($(this).attr('data-brand-id'));
        var retailerId = GetValueInt($(this).attr('data-retailer-id'));
        var imageWidth = GetValueInt($(this).attr('data-image-width'));
        var imageHeight = GetValueInt($(this).attr('data-image-height'));
        var pageSize = GetValueInt($(this).attr('data-page-size'));
        var productType = $(this).attr('data-product-type');
        var excludedProductId = GetValueInt($(this).attr('data-excluded-product-id'));
        var enabledLoaderImage = $(this).attr('data-enabled-loader-image');
        var postData = JSON.stringify({
            "page": page,
            "designName": designName,
            "categoryId": categoryId,
            "brandId": brandId,
            "retailerId": retailerId,
            "imageWidth": imageWidth,
            "imageHeight": imageHeight,
            "pageSize": pageSize,
            "productType": productType,
            "excludedProductId": excludedProductId
        });

        ajaxMethodCall(postData, "/AjaxProducts/GetProductsByProductType", function (data) {
            AddDataToDiv(truethis, data, page);

        }, enabledLoaderImage);
    });
}

function GetProductLabels() {
    $('[data-product-labels]').each(function () {
        var truethis = this;
        var designName = $(this).attr('data-template-design-name');
        var itemid = $(this).attr('data-product-itemid');
        var enabledLoaderImage = $(this).attr('data-enabled-loader-image');
        var itemtype = 'product';
        var postData = JSON.stringify({ id: itemid, "designName": designName });
        ajaxMethodCall(postData, "/AjaxProducts/GetProductLabels", function (data) {
            AddDataToDiv(truethis, data);
        }, enabledLoaderImage);
    });
}
function GetFooter() {
    $('[data-main-footer]').each(function () {
        var truethis = this;
        var designName = $(this).attr('data-template-design-name');
        var postData = JSON.stringify({ "designName": designName });
        var enabledLoaderImage = $(this).attr('data-enabled-loader-image');
        ajaxMethodCall(postData, "/AjaxGenerics/Footer", function (data) {
            AddDataToDiv(truethis, data, 0);

        }, enabledLoaderImage);
    });
}
function GetMainNavigation() {
    $('[data-main-navigation]').each(function () {
        var truethis = this;
        var innerHtmlLength = truethis.innerHTML.length;
        if (innerHtmlLength == 0) {
            var designName = $(this).attr('data-template-design-name');
            var postData = JSON.stringify({ "designName": designName });
            var enabledLoaderImage = $(this).attr('data-enabled-loader-image');
            ajaxMethodCall(postData, "/AjaxGenerics/MainNavigation", function (data) {
                AddDataToDiv(truethis, data, 0);

            }, enabledLoaderImage);
        }
    });
}
function GetBrands() {
    $('[data-brands]').each(function () {
        var truethis = this;
        var designName = $(this).attr('data-template-design-name');
        var imageWidth = GetValueInt($(this).attr('data-image-width'));
        var imageHeight = GetValueInt($(this).attr('data-image-height'));
        var enabledLoaderImage = $(this).attr('data-enabled-loader-image');
        var postData = JSON.stringify({
            "designName": designName,
            "imageWidth": imageWidth,
            "imageHeight": imageHeight
        });
        ajaxMethodCall(postData, "/AjaxProducts/GetBrands", function (data) {
            AddDataToDiv(truethis, data, 0);
        }, enabledLoaderImage);
    });
}
function GetProductCategories() {

    $('[data-product-categories]').each(function () {
        var truethis = this;
        var designName = $(this).attr('data-template-design-name');
        var enabledLoaderImage = $(this).attr('data-enabled-loader-image');
        var imageWidth = GetValueInt($(this).attr('data-image-width'));
        var imageHeight = GetValueInt($(this).attr('data-image-height'));
        var postData = JSON.stringify({
            "designName": designName,
            "imageWidth": imageWidth,
            "imageHeight": imageHeight
        });
        ajaxMethodCall(postData, "/AjaxProducts/GetProductCategories", function (data) {
            AddDataToDiv(truethis, data, 0);
        }, enabledLoaderImage);
    });
}
function GetRelatedContents() {
    $('[data-related-contents]').each(function () {
        var truethis = this;
        var designName = $(this).attr('data-template-design-name');
        var enabledLoaderImage = $(this).attr('data-enabled-loader-image');
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
        }, enabledLoaderImage);
    });
}
function AddDataToDiv(truethis, data, page) {
    //$(truethis).empty();
    $(truethis).append(data).animate({}, 'slow');
    var dataPageAttr = $(truethis).attr('data-page');
    if (typeof dataPageAttr !== typeof undefined && dataPageAttr !== false) {
        $(truethis).attr('data-page', page + 1);
    }
}

function GetValueInt(val) {
    val = val === undefined ? 0 : val;
    return val;
}

function ajaxMethodCall(postData, ajaxUrl, successFunction, enabledLoaderImage) {


    if (enabledLoaderImage) {
        showLoadingImage();
    }

    ajaxUrl = $("#RootUrl").val() + ajaxUrl;
    console.log("ajaxUrl:"+ajaxUrl);
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
        dataType: "json",
        complete: function() {
                // no matter the result, complete will fire, so it's a good place
                // to do the non-conditional stuff, like hiding a loading image.
            if (enabledLoaderImage) {
                hideLoadingImage();
            }
              
       }
    });
}
function ContactFormModel() {
    var self = this;
    if ($('#Fullname').length > 0) {
        self.Name = $("#Fullname").val();
    }
    if ($('#Email').length > 0) {
        self.Email = $("#Email").val();
    }
    if ($('#Telefon').length > 0) {
        self.Telefon = $("#Telefon").val();
    }
    if ($('#Company').length > 0) {
        self.Company = $("#Company").val();
    }
    if ($('#Address').length > 0) {
        self.Address = $("#Address").val();
    }
    if ($('#UserMessage').length > 0) {
            self.UserMessage = $("#UserMessage").val();
    }
    if ($('#FormType').length > 0) {
        self.Type = $("#FormType").val();
    }
}
function EmptyContactFormModel() {
     
    if ($('#Fullname').length > 0) {
       $("#Fullname").val("");
    }
    if ($('#Email').length > 0) {
        $("#Email").val("");
    }
    if ($('#Telefon').length > 0) {
         $("#Telefon").val("");
    }
    if ($('#Company').length > 0) {
        $("#Company").val("");
    }
    if ($('#Address').length > 0) {
         $("#Address").val("");
    }
    if ($('#UserMessage').length > 0) {
         $("#UserMessage").val("");
    }
    if ($('#FormType').length > 0) {
        $("#FormType").val("");
    }
}