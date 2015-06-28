﻿
var linkUrl = "{id}";
$(document).ready(function () {
    console.log("image gallery script is working");
    
    $("#ImageDialog").click(function () {
        createAndOpenDialog();
    });
    RetrieveContentImages();
    bindRemoveImage();
});
function createAndOpenDialog() {
    $("#dialog-message").dialog({
        modal: true,
        height: 520,
        width: 720,
        show: {
            effect: "fade",
            duration: 1000
        },
        hide: {
            effect: "fade",
            duration: 500
        },
        buttons: {
            Ok: function () {

                $("#contentImages").empty();
                $("#contentImages").html($("#SelectedImageGallery").clone().html());
                $(this).dialog("close");


            },
            Close: function () {
                $(this).dialog("close");
            }
        },
        open: function (event, ui) {
            setTimeout(function () {
                LoadImages();
            }, 2);
        }
    });

    $("#dialog-message").position({
        my: "center",
        at: "center",
        of: window
    });
}
function RetrieveContentImages() {
    $('[data-file-item-id]').each(function () {
        $(this).off("click");
        $(this).on("click", handleRetrieveContentImages);
    });
}

function handleRetrieveContentImages(e) {
    e.preventDefault();
    var caller = e.target;
    var itemId = $(caller).attr('data-file-item-id');
    var itemType = $(caller).attr('data-file-item-type');
    var list = $("<ul></ul");
    $("#ImagesPanel").empty();
    var jsonRequest = JSON.stringify({ "itemId": itemId, "itemType": itemType });
    jQuery.ajax({
        url: "/Ajax/GetImagesByItemTypeAndId",
        type: 'POST',
        data: jsonRequest,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var photos = data;
            $.each(photos, function (i, photo) {
                console.log(photo);
                var img = $("<img/>").attr("src",  'https://docs.google.com/uc?id=' +  photo.GoogleImageId)
                                    .attr("title", photo.FileName).attr("id", photo.Id);
                var div = $("<li/>").attr("class", "col-md-4").attr("data-file-image", photo.Id).append(img);
                $(list).append($("<div/>").append(div));
            });
            $("#ImagesPanel").append(list);
            createImageDialog();
        },
        error: function (request, status, error) {
            console.error('Error ' + status + ' ' + request.responseText);
        },
        beforeSend: function () {

        }
    });
}
function createImageDialog() {
    
   
    $("#ImagesPanel").dialog({
        modal: true,
        height: 520,
        width: 720,
        show: {
            effect: "fade",
            duration: 1000
        },
        hide: {
            effect: "fade",
            duration: 500
        },
        buttons: {
            Ok: function () {
                $(this).dialog("close");
            },
            Close: function () {
                $("#ImagesPanel").empty();
                $(this).dialog("close");
            }
        },
        open: function (event, ui) {
         
        }
    });

    $("#ImagesPanel").position({
        my: "center",
        at: "center",
        of: window
    });
}

function createImage(googleImageId, fileName, photoId, eventLink) {
    var img = $("<img/>").attr("src", 'https://docs.google.com/uc?id=' + googleImageId)
        .attr("title", fileName)
        .attr("id", photoId)
        .attr("class", "fileManagerImg");
    var link = $("<a></a>").attr("href", "#").append(img);
    var caption = $("<div/>").text(fileName).addClass("caption");
    var div = $("<li/>").attr("class", "col-md-4 thumbnail").attr("data-file-image", photoId).append(link).append(eventLink).append(caption);
    return div;
}

function LoadImages() {
    $("#flickr-photos").empty();
    var storeId = $("#StoreId").val();
    $.getJSON("/Ajax/GetImages?storeId=" + storeId, function(data) {
        var photos = data;
        $("#image-count").text(photos.length);
        $("#filter-count").text(photos.length);


        var list = $("<ul></ul");
        $.each(photos, function(i, photo) {
            var googleImageId = photo.GoogleImageId;
            var fileName = photo.Title;
            var photoId = photo.Id;
            var addLink = $("<div/>")
                .attr("data-image-add-link", photoId)
                .attr("data-image-file-name", fileName)
                .attr("data-image-file-googleImageId", googleImageId)
                .text("Add").addClass("addLink btn btn-default btn-block");
            var div = createImage(googleImageId, fileName, photoId, addLink);
            $(list).append($("<div/>").append(div));

        });
        

        $("#flickr-photos .loading").remove();
        $("#flickr-photos").append(list);
        bindAddImage();
    });

    $("#filter").keyup(function () {
        var filter = $(this).val(), count = 0;
        $(".filtered:first li").each(function () {
            if ($(this).text().search(new RegExp(filter, "i")) < 0) {
                $(this).addClass("hidden");
            } else {
                $(this).removeClass("hidden");
                count++;
            }
        });
        $("#filter-count").text(count);
    });

}

function bindRemoveImage() {
    $('[data-image-remove-link]').each(function () {
        $(this).off("click");
        $(this).on("click", handleRemoveImage);
    });
}

function handleRemoveImage(e) {
    var caller = e.target;
    var imageId = $(caller).attr('data-image-remove-link');
    var fileName = $(caller).attr('data-image-file-name');
    var googleImageId = $(caller).attr('data-image-file-googleImageId');
    $("#SelectedImageGallery").find('[data-file-image=' + imageId + ']').remove();
    $("#SelectedImageGallery").find('[data-selected-file=' + imageId + ']').remove();
    
    $("#existingContentImages").find('[data-file-image=' + imageId + ']').remove();
    $("#existingContentImages").find('[data-selected-file=' + imageId + ']').remove();
    $("#existingContentImages").find('[data_selected_file=' + imageId + ']').remove();
    

    var addLink = $("<div/>")
        .attr("data-image-add-link", imageId)
        .attr("data-image-file-name", fileName)
        .text("Add").addClass("addLink btn btn-default  btn-block");
    var div = createImage(googleImageId, fileName, imageId, addLink);
    $("#flickr-photos").append(div);
    bindAddImage();
}

function bindAddImage() {
    $('[data-image-add-link]').each(function () {
        $(this).off("click");
        $(this).on("click", handleAddImage);
    });
}

function handleAddImage(e) {
    var caller = e.target;
    var imageId = $(caller).attr('data-image-add-link');
    var fileName = $(caller).attr('data-image-file-name');
    var googleImageId = $(caller).attr('data-image-file-googleImageId');
    
    $(caller).addClass("addedImage");
    var removeLink = $("<div/>")
        .attr("data-image-remove-link", imageId)
        .attr("data-image-file-name", fileName)
        .text("Remove").addClass("addLink btn btn-danger  btn-block");
    var div = createImage(googleImageId, fileName, imageId, removeLink);
    var file = $('<input>').attr({
        type: 'hidden',
        id: 'fileId_' + imageId,
        value:imageId,
        name: 'selectedFileId',
        data_selected_file : imageId
    });
    $("#SelectedImageGallery").append(div);
    $("#SelectedImageGallery").append(file);
    $("#flickr-photos").find('[data-file-image=' + imageId + ']').remove();
    bindRemoveImage();

}