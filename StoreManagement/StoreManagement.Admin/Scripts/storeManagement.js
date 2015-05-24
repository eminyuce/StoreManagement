$(document).ready(function () {
    $("#DeselectAll").click(function () {
        console.log("DeselectAll is clicked.");
        var i = 0;
        $("input[name=checkboxGrid]").each(function () {
            var m = $(this).prop('checked', false);
        });
    });
    $("#SelectAll").click(function () {
        console.log("SelectAll is clicked.");
        var i = 0;
        $("input[name=checkboxGrid]").each(function () {
            var m = $(this).prop('checked', true);
        });
    });
    function GetSelectedCheckBoxValues() {
        var stringArray = new Array();
        var i = 0;
        $("input[name=checkboxGrid]").each(function () {
            var m = $(this).is(':checked');
            if (m) {
                stringArray[i++] = $(this).attr("gridkey-id");
            }
        });
        var jsonRequest = JSON.stringify({ "values": stringArray });
        return jsonRequest;
    }
    function OrderingItem() {
        var item = this;
        item.Id = "";
        item.Ordering = "";
        return item;
    }
    function GetSelectedOrderingValues() {
        var itemArray = new Array();
        var i = 0;
        $("input[name=gridOrdering]").each(function () {
            var id = $(this).attr("gridkey-id");
            //var m = $("input[name=checkboxGrid]").find('[gridkey-id='+id+']').is(':checked');
            //if (m) {
                var item = new OrderingItem();
                item.Id = id;
                item.Ordering=$(this).val();
                itemArray[i++] = item;
            //}
        });
     
        var jsonRequest = JSON.stringify({ "values": itemArray });
        return jsonRequest;
    }
    $("#DeleteAll").click(function () {
        console.log("DeleteAll is clicked.");
        var postData = GetSelectedCheckBoxValues();
        var parsedPostData = jQuery.parseJSON(postData);
        if (parsedPostData.values.length > 0) {
            var tableName = $("[data-gridname]").attr("data-gridname");
            if (tableName == "imagesGrid") {
                ajaxMethodCall(postData, "/FileManager/DeleteAll", deleteItemsSuccess);
            } else if (tableName == "contentGrid") {
                ajaxMethodCall(postData, "/Ajax/DeleteContentItem", deleteItemsSuccess);
            }
        }
        
    });
    $("#OrderingAll").click(function () {
        console.log("OrderingAll is clicked.");
        var postData = GetSelectedOrderingValues();
        console.log(postData);
        var tableName = $("[data-gridname]").attr("data-gridname");
        if (tableName == "imagesGrid") {
            ajaxMethodCall(postData, "/Ajax/ChangeImagesGridOrdering", changeOrderingSucessSuccess);
        } else if (tableName == "contentGrid") {
            ajaxMethodCall(postData, "/Ajax/ChangeContentGridOrdering", changeOrderingSucessSuccess);
        }
    });
});

function ajaxMethodCall(postData,ajaxUrl, successFunction) {

    $.ajax({
        type: "POST",
        url: ajaxUrl,
        data: postData,
        success: successFunction,
        contentType:"application/json",
        dataType: "json",
        traditional: true
    });
}
function deleteItemsSuccess(data) {

    data.forEach(function (entry) {
        console.log(entry);
        var pp = $('[gridkey-id=' + entry + ']');
        pp.parent().parent().remove();
        console.log(pp);
    });
}

function changeOrderingSucessSuccess(data) {
    console.log(data);
}