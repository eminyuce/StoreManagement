function isEmpty(str) {
    return (!str || 0 === str.length);
}

$(document).ready(function () {

    searchAutoComplete();
    
    $('.tooltip').tooltip();
    $('.tooltip-left').tooltip({ placement: 'left' });
    $('.tooltip-right').tooltip({ placement: 'right' });
    $('.tooltip-top').tooltip({ placement: 'top' });
    $('.tooltip-bottom').tooltip({ placement: 'bottom' });

    $('.popover-left').popover({ placement: 'left', trigger: 'hover' });
    $('.popover-right').popover({ placement: 'right', trigger: 'hover' });
    $('.popover-top').popover({ placement: 'top', trigger: 'hover' });
    $('.popover-bottom').popover({ placement: 'bottom', trigger: 'hover' });

    $('.notification').click(function () {
        var $id = $(this).attr('id');
        switch ($id) {
            case 'notification-sticky':
                $.jGrowl("Stick this!", { sticky: true });
                break;

            case 'notification-header':
                $.jGrowl("A message with a header", { header: 'Important' });
                break;

            default:
                $.jGrowl("Hello world!");
                break;
        }
    });


    $('#MyDeleteSubmit').click(function () {
        $('#DeleteItemButton').click();
    });

    bindCarouselImage();
    var originalURL = window.location.href;
    var q = getQueryStringParameter(originalURL, "GridPageSize");
    if (!isEmpty(q)) {
        $("#GridListItemSize").val(q);
    }

    $("input[type='checkbox'][name='checkboxGrid']").click(function () {
        console.log("1212");
        var m = $(this).is(':checked');
        if (m) {
            $(this).parent().parent().addClass('gridChecked');
        } else {
            $(this).parent().parent().removeClass('gridChecked');
        }
    });

    $("#DeselectAll").click(function () {
        console.log("DeselectAll is clicked.");
        var i = 0;
        $("input[name=checkboxGrid]").each(function () {
            $(this).parent().parent().removeClass('gridChecked');
            var m = $(this).prop('checked', false);
        });
    });
    $("#SelectAll").click(function () {
        console.log("SelectAll is clicked.");
        var i = 0;
        $("input[name=checkboxGrid]").each(function () {
            $(this).parent().parent().addClass('gridChecked');
            var m = $(this).prop('checked', true);
        });
    });

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
            item.Ordering = $(this).val();
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
            console.log("Delete" + tableName + "Item");
            ajaxMethodCall(postData, "/Ajax/Delete" + tableName + "Item", deleteItemsSuccess);
        }
    });
    $("#OrderingAll").click(function () {
        console.log("OrderingAll is clicked.");
        var postData = GetSelectedOrderingValues();
        console.log(postData);
        var tableName = $("[data-gridname]").attr("data-gridname");
        ajaxMethodCall(postData, "/Ajax/Change" + tableName + "OrderingOrState", changeOrderingSuccess);
    });

    function GetSelectedStateValues(checkboxName, state) {
        var itemArray = new Array();
        var i = 0;
        $('span[name=' + checkboxName + ']').each(function () {
            var id = $(this).attr("gridkey-id");
            var m = $('input[name="checkboxGrid"]').filter('[gridkey-id="' + id + '"]').is(':checked');
            if (m) {
                var item = new OrderingItem();
                item.Id = id;
                item.Ordering = 0;
                item.State = state;
                itemArray[i++] = item;
            }
        });

        return itemArray;
    }
    $("#SetStateOffAll").click(function () {
        console.log("SetStateOffAll is clicked.");
        changeState(false);
    });
    $("#SetStateOnAll").click(function () {
        console.log("SetStateOnAll is clicked.");
        changeState(true);
    });
    function changeState(state) {
        var ppp = $("#ItemStateSelection").val();
        var selectedValues = GetSelectedStateValues("span" + ppp, state);
        if (selectedValues.length > 0) {
            var postData = JSON.stringify({ "values": selectedValues, "checkbox": ppp });
            console.log(postData);
            var tableName = $("[data-gridname]").attr("data-gridname");
            ajaxMethodCall(postData, "/Ajax/Change" + tableName + "OrderingOrState", changeStateSuccess);
            displayMessage("hide", "");

        } else {
            displayMessage("error", "Checkboxes on the grid does not selected");
        }

    }
    $("#GridListItemSize").change(function (e) {
        var originalURL = window.location.href;
        var q = getQueryStringParameter(originalURL, "GridPageSize");
        if (!isEmpty(q)) {
            window.location.href = updateUrlParameter(originalURL, 'GridPageSize', $('#GridListItemSize option:selected').val())
        } else {
            if (hasQueryStringParameter(originalURL)) {
                window.location.href = window.location.href + "&GridPageSize=" + $('#GridListItemSize option:selected').val();
            } else {
                window.location.href = window.location.href + "?GridPageSize=" + $('#GridListItemSize option:selected').val();
            }

        }
    });
    function displayMessage(messageType, message) {
        var messagePanel = $("#ErrorMessagePanel");
        var errorMessage = $("#ErrorMessage");
        messagePanel.show();
        if (messageType == "info") {
            messagePanel.attr("class", "alert alert-info");
            errorMessage.text(message);
        } else if (messageType == "error") {
            messagePanel.attr("class", "alert alert-danger");
            errorMessage.text(message);
        } else if (messageType == "hide") {
            messagePanel.hide();
        }
    }

});
function GetSelectedCheckBoxValues() {
    var stringArray = GetSelectedCheckBoxValuesArray();
    var jsonRequest = JSON.stringify({ "values": stringArray });
    return jsonRequest;
}
function GetSelectedCheckBoxValuesArray() {
    var stringArray = new Array();
    var i = 0;
    $("input[name=checkboxGrid]").each(function () {
        var m = $(this).is(':checked');
        if (m) {
            stringArray[i++] = $(this).attr("gridkey-id");
        }
    });
    return stringArray;
}
function updateUrlParameter(originalURL, param, value) {
    console.log(value);
    var windowUrl = originalURL.split('?')[0];
    var qs = originalURL.split('?')[1];
    //3- get list of query strings
    var qsArray = qs.split('&');
    var flag = false;
    //4- try to find query string key
    for (var i = 0; i < qsArray.length; i++) {
        if (qsArray[i].split('=').length > 0) {
            if (param == qsArray[i].split('=')[0]) {
                //exists key
                qsArray[i] = param + '=' + value;
            }
        }
    }

    var finalQs = qsArray.join('&');
    return windowUrl + '?' + finalQs;
    //6- prepare final url
    // window.location = windowUrl + '?' + finalQs;
}
function hasQueryStringParameter(originalURL) {

    if (originalURL.split('?').length > 1) {
        var qs = originalURL.split('?')[1];
        var qsArray = qs.split('&');
        return qsArray.length > 0;
    } else {
        return false;
    }
}
function getQueryStringParameter(originalURL, param) {

    if (originalURL.split('?').length > 1) {
        var qs = originalURL.split('?')[1];
        //3- get list of query strings
        var qsArray = qs.split('&');
        var flag = false;
        //4- try to find query string key
        for (var i = 0; i < qsArray.length; i++) {
            if (qsArray[i].split('=').length > 0) {
                if (param == qsArray[i].split('=')[0]) {
                    //exists key
                    return qsArray[i].split('=')[1];
                }
            }
        }

    }
    return "";
}

function ajaxMethodCall(postData, ajaxUrl, successFunction) {

    $.ajax({
        type: "POST",
        url: ajaxUrl,
        data: postData,
        success: successFunction,
        contentType: "application/json",
        dataType: "json",
        traditional: true
    });
}
function deleteItemsSuccess(data) {

    data.forEach(function (entry) {
        var pp = $('[gridkey-id=' + entry + ']');
        pp.parent().parent().remove();
    });
}
function changeStateSuccess(data) {
    //var parsedPostData = jQuery.parseJSON(data);
    console.log(data);
    data.values.forEach(function (entry) {
        if (entry.State) {
            $('span[name=span' + data.checkbox + ']').filter('[gridkey-id="' + entry.Id + '"]').attr('style', 'color:green;  font-size:2em;').attr('class', 'glyphicon  glyphicon-ok-circle');
        } else {
            $('span[name=span' + data.checkbox + ']').filter('[gridkey-id="' + entry.Id + '"]').attr('style', 'color:red;  font-size:2em;').attr('class', 'glyphicon  glyphicon-remove-circle');
        }


    });
}
function changeOrderingSuccess(data) {
    console.log(data);
}

function bindCarouselImage() {
    $('[data-carusel-file-id]').each(function () {
        $(this).off("click");
        $(this).on("click", handleCarouselImage);
    });
}

function handleCarouselImage(e) {
    var caller = e.target;
    var fileId = $(caller).attr('data-carusel-file-id');
    var isCarousel = $(caller).attr('data-carusel-file-isCarousel');
    if (isCarousel === "true") {
        isCarousel = "false";
    } else if (isCarousel === "false") {
        isCarousel = "true";
    }

    var postData = JSON.stringify({ "fileId": fileId, "isCarousel": isCarousel });
    ajaxMethodCall(postData, "/Ajax/ChangeIsCarouselState", changeCarouselStateSuccess);

}
function changeCarouselStateSuccess(data) {
    console.log(data);
    var mmm = $('a[data-carusel-file-id=' + data.fileId + ']');
    mmm.attr('data-carusel-file-isCarousel', data.isCarousel);
    mmm.removeClass("btn-success");
    mmm.removeClass("btn-danger");
    if (data.isCarousel) {
        mmm.addClass("btn-success");
    } else if (!data.isCarousel) {
        mmm.addClass("btn-danger");
    }


}


function populateStoreLabelsDropDown(storeId) {

    var jsonRequest = JSON.stringify({ "storeId": storeId });
    jQuery.ajax({
        url: "/Ajax/GetStoreLabels",
        type: 'POST',
        data: jsonRequest,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            console.log(data);
            // Parse the returned json data
            //   var opts = $.parseJSON(data);
            // Use jQuery's each to iterate over the opts value
            $('#selectedLabels').empty();
            $.each(data, function (i, d) {
                // You will need to alter the below to get the right values from your json object.  Guessing that d.id / d.modelName are columns in your carModels data
                $('#selectedLabels').append('<option value="' + d.Id + '">' + d.Name + '</option>');

            });

            $("#selectedLabels").select2({ tags: true });

        },
        error: function (request, status, error) {
            console.error('Error ' + status + ' ' + request.responseText);
        },
        beforeSend: function () {

        }
    });

}
function searchAutoComplete() {


    $("#search").autocomplete({
        source: function (request, response) {
            console.log("auto complate");
            var items = new Array();
            var jsonRequest = { "term": request.term, "action": $("#action").val(), "controller": $("#controller").val(), "id": $("#storeId").val() };
            console.log(jsonRequest);
            if (request.term.length > 2) {
                $.ajax({
                    type: "POST",
                    url: "/Ajax/SearchAutoComplete",
                    data: jsonRequest,
                    success: function (data) {
                        for (var i = 0; i < data.length ; i++) {
                            items[i] = { label: data[i], Id: data[i] };
                        }
                        console.log(items);
                        response(items);
                    }
                });
            }
          
        },
        select: function (event, ui) {
            //fill selected customer details on form
            //$.ajax({
            //    type: "POST",
            //    url: "/Ajax/SearchAutoCompleteDetail",
            //    data: { "id": ui.item.Id },

            //    success: function (data) {
                    
            //    },
            //    error: function (xhr, ajaxOptions, thrownError) {
            //        alert('Failed to retrieve states.');
            //    }
            //});
        }
    });

}