$(document).ready(function () {

    $("#SelectAll").click(function () {
        console.log("SelectAll is clicked.");
        var i = 0;
        $("input[name=checkboxGrid]").each(function () {
            var m = $(this).attr('checked', 'checked');
        });
    });

    $("#DeleteAll").click(function () {
        console.log("DeleteAll is clicked.");
        var stringArray = new Array();
        var i = 0;
        $("input[name=checkboxGrid]").each(function () {
            var m = $(this).is(':checked');
            if (m) {
                stringArray[i++] = $(this).attr("gridkey-id");
            }
        });
        var postData = { values: stringArray };
        var tableName = $("[data-gridname]").attr("data-gridname");
        if (tableName == "imagesGrid") {
            deleteItems(postData, "/FileManager/DeleteAll");
        } else if (tableName == "contentGrid") {
            deleteItems(postData, "/Ajax/DeleteContentItem");
        }

    });
});
function deleteItems(postData,ajaxUrl) {

    $.ajax({
        type: "POST",
        url: ajaxUrl,
        data: postData,
        success: function (data) {

            data.forEach(function (entry) {
                console.log(entry);
                var pp = $('[gridkey-id=' + entry + ']');
                pp.parent().parent().remove();
                console.log(pp);
            });
        },
        dataType: "json",
        traditional: true
    });
}