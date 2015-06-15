


    $(document).ready(function() {
        console.log("jquery is workingg");
        



        $("#contentTreeview").bind("select_node.jstree", function(event, data) {
            var m = $("#" + data.selected[0]).find("[data-category]");
            var selectedCategoryId = m.first().attr("data-category");
            console.log(selectedCategoryId);
            $("#CategoryId").val(selectedCategoryId);

        });

        $("#productTreeview").bind("select_node.jstree", function (event, data) {
            var m = $("#" + data.selected[0]).find("[data-product-category]");
            var selectedCategoryId = m.first().attr("data-product-category");
            console.log(selectedCategoryId);
            $("#ProductCategoryId").val(selectedCategoryId);

        });

        GetCategoryTree($("#StoreId").val(), $("#categoryType").val());
        GetProductCategoryTree($("#StoreId").val(), $("#categoryType").val());
        


        $('select#StoreDropDownId').chosen({}).change(function(event) {
            GetCategoryTree($(this).val(), $("#categoryType").val());
            GetProductCategoryTree($(this).val(), $("#categoryType").val());
        });

        console.log("jquery is working");

        GetFiles($("#Id").val());
        

        $('textarea#Description').ckeditor({
            height: '150px',
            toolbar: [
                { name: 'document', items: ['Source', '-', 'NewPage', 'Preview', '-', 'Templates'] }, // Defines toolbar group with name (used to create voice label) and items in 3 subgroups.
                ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'], // Defines toolbar group without name.
                { name: 'basicstyles', items: ['Bold', 'Italic'] }
            ]
        });


    });

function GetCategoryTree(id, categoryType) {

    var jsonRequest = JSON.stringify({ "storeId": id, "categoryType": categoryType });
    console.log(jsonRequest);

    jQuery.ajax({
        url: "/Ajax/GetCategoriesTree",
        type: 'POST',
        data: jsonRequest,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function(data) {
            $("#categoryTree").html(data);
            bindcategoryTree();
            var categoryId = $("#CategoryId").val();
            var selectedCategory =  $('[data-category=' + categoryId + ']').text();
            console.log(selectedCategory);
            $("#SelectedCategory").text(selectedCategory);
        },
        error: function(request, status, error) {
            console.error('Error ' + status + ' ' + request.responseText);
        },
        beforeSend: function() {

        }
    });

}

function bindcategoryTree() {
    $('[data-category]').each(function () {
        $(this).off("click");
        $(this).on("click", handleCategoryTree);
    });
}

function handleCategoryTree(e) {
    var caller = e.target;
    var categoryId = $(caller).attr('data-category');
    var category = $(caller).text();
    $("#SelectedCategory").text(category);
    $("#CategoryId").val(categoryId);
}

//////////////////////////////////////////////////////////////////////////////
function GetProductCategoryTree(id, categoryType) {

    var jsonRequest = JSON.stringify({ "storeId": id, "categoryType": categoryType });
    console.log(jsonRequest);

    jQuery.ajax({
        url: "/Ajax/GetProductCategoriesTree",
        type: 'POST',
        data: jsonRequest,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#productCategoryTree").html(data);
            bindProductCategoryTree();
            var categoryId = $("#ProductCategoryId").val();
            var selectedCategory = $('[data-product-category=' + categoryId + ']').text();
            console.log(selectedCategory);
            $("#ProductSelectedCategory").text(selectedCategory);
        },
        error: function (request, status, error) {
            console.error('Error ' + status + ' ' + request.responseText);
        },
        beforeSend: function () {

        }
    });

}

function bindProductCategoryTree() {
    $('[data-product-category]').each(function () {
        $(this).off("click");
        $(this).on("click", handleProductCategoryTree);
    });
}

function handleProductCategoryTree(e) {
    var caller = e.target;
    var categoryId = $(caller).attr('data-product-category');
    var category = $(caller).text();
    $("#ProductSelectedCategory").text(category);
    $("#ProductCategoryId").val(categoryId);
}


function GetFiles(id) {

    var jsonRequest = JSON.stringify({ "contentId": id });
    console.log(jsonRequest);

    jQuery.ajax({
        url: "/Ajax/GetFiles",
        type: 'POST',
        data: jsonRequest,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function(data) {
            var files = data;
            $.each(files, function(i, file) {
                var imageId = file.FileManagerId;
                var fileName = file.FileManager.FileName;
                var thumbnailLink = file.FileManager.ThumbnailLink;


                var removeLink = $("<div/>")
                    .attr("data-image-remove-link", imageId)
                    .attr("data-image-file-name", fileName)
                    .attr("data-image-file-thumnailLink", thumbnailLink)
                    .text("Remove").addClass("addLink");
                var div = createImage(thumbnailLink, fileName, imageId, removeLink);
                var fileHiddenLink = $('<input>').attr({
                    type: 'hidden',
                    id: 'fileId_' + imageId,
                    value: imageId,
                    name: 'selectedFileId',
                    data_selected_file: imageId
                });

                $("#existingContentImages").append(div);
                $("#existingContentImages").append(fileHiddenLink);
                bindRemoveImage();
            });
        },
        error: function(request, status, error) {
            console.error('Error ' + status + ' ' + request.responseText);
        },
        beforeSend: function() {

        }
    });

}

