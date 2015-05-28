
var linkUrl = "{id}";
$(document).ready(function () {
    console.log("image gallery script is working");
    
    $("#ImageDialog").click(function () {
        $("#dialog-message").dialog({
            modal: true,
            height: 600,
            width: 800,
            draggable: true,
            position: 'center',
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
    });


});

function createImage(thumnailLink, fileName, photoId, eventLink) {
    var img = $("<img/>").attr("src",   thumnailLink)
        .attr("title", fileName)
        .attr("id", photoId)
        .attr("style", "width:100%;max-width:70px;");
    var link = $("<a></a>").attr("href", "#").append(img);
    var caption = $("<div/>").text(fileName).addClass("caption");
    var div = $("<li/>").attr("class", "col-md-4").attr("data-file-image", photoId).append(link).append(eventLink).append(caption);
    return div;
}

function LoadImages() {
    $("#flickr-photos").empty();
    var storeId = $("#StoreId").val();
    $.getJSON("/Ajax/GetImages?storeId=" + storeId, function (data) {
        var photos = data;
        $("#image-count").text(photos.length);
        $("#filter-count").text(photos.length);


        var list = $("<ul></ul");
        $.each(photos, function (i, photo) {
            var thumnailLink = photo.ThumbnailLink;
            var fileName = photo.FileName;
            var photoId = photo.Id;
            var addLink = $("<div/>")
                .attr("data-image-add-link", photoId)
                .attr("data-image-file-name", fileName)
                .attr("data-image-file-thumnailLink", thumnailLink)
                .text("Add").addClass("addLink");
            var div = createImage(thumnailLink,fileName, photoId, addLink);
            $(list).append($("<div/>").append(div));

        });
        $("#flickr-photos .loading").remove();
        $("#flickr-photos").append(list);
        bindAddImage();
    })

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
    var thumnailLink = $(caller).attr('data-image-file-thumnailLink');
    $("#SelectedImageGallery").find('[data-file-image=' + imageId + ']').remove();
    $("#SelectedImageGallery").find('[data-selected-file=' + imageId + ']').remove();
    
    $("#existingContentImages").find('[data-file-image=' + imageId + ']').remove();
    $("#existingContentImages").find('[data-selected-file=' + imageId + ']').remove();
    $("#existingContentImages").find('[data_selected_file=' + imageId + ']').remove();
    

    var addLink = $("<div/>")
        .attr("data-image-add-link", imageId)
        .attr("data-image-file-name", fileName)
        .text("Add").addClass("addLink");
    var div = createImage(thumnailLink,fileName, imageId, addLink);
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
    var thumnailLink = $(caller).attr('data-image-file-thumnailLink');
    
    $(caller).addClass("addedImage");
    var removeLink = $("<div/>")
        .attr("data-image-remove-link", imageId)
        .attr("data-image-file-name", fileName)
        .text("Remove").addClass("addLink");
    var div = createImage(thumnailLink,fileName, imageId, removeLink);
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