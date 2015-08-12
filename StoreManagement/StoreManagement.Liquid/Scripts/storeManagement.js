


$(document).ready(function () {
    //var mm = $("[data-view-contact-resume-contact=" + encodedResumeId + "]");
    //mm.fadeOut('slow', function () {
    //    mm.html(data);
    //    mm.fadeIn('slow');
    //});

    callAjaxMethod();
});
function callAjaxMethod() {
    
    if ($("#RelatedBlogs").length > 0) {
        GetRelatedBlogs($("#CategoryId").val());
    }
    if ($("#RelatedNews").length > 0) {
        GetRelatedBlogs($("#CategoryId").val());
    }
    if ($("#RelatedProducts").length > 0) {
        GetRelatedProducts($("#ProductCategoryId").val());
    }
    if ($("#ProductCategories").length > 0) {
        GetProductCategories();
    }

}
function GetProductCategories() {
    var postData = JSON.stringify({  });
    ajaxMethodCall(postData, "/Ajax/GetProductCategories", function (data) {
        $("#ProductCategories").empty();
        $("#ProductCategories").html(data).animate({ 'height': '150px' }, 'slow');
    });
}
function GetRelatedBlogs(categoryId) {
    var postData = JSON.stringify({ "categoryId": categoryId, "contentType": "blog" });
    ajaxMethodCall(postData, "/Ajax/GetRelatedContents", function (data) {
        $("#RelatedBlogs").empty();
        $("#RelatedBlogs").html(data).animate({ 'height': '150px' }, 'slow');
    });
}
function GetRelatedNews(categoryId) {
    var postData = JSON.stringify({ "categoryId": categoryId, "contentType": "news" });
    ajaxMethodCall(postData, "/Ajax/GetRelatedContents", function(data) {
        $("#RelatedNews").empty();
        $("#RelatedNews").html(data).animate({ 'height': '150px' }, 'slow');
    });
}

function GetRelatedProducts(productCategoryId) {
    var postData = JSON.stringify({ "categoryId": productCategoryId });
    ajaxMethodCall(postData, "/Ajax/GetRelatedProducts", function (data) {
        $("#RelatedProducts").empty();
        $("#RelatedProducts").html(data).animate({ 'height': '150px' }, 'slow');
    });
}

function GetProductCategories() {
    var postData = JSON.stringify({});
    ajaxMethodCall(postData, "/Ajax/GetProductCategories", function (data) {
        $("#ProductCategories").empty();
        $("#ProductCategories").html(data).animate({ 'height': '150px' }, 'slow');
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
