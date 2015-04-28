function isEmpty(str) {
    return (!str || 0 === str.length);
}
function getLinkText(html) {
    var div = document.createElement("div");
    div.innerHTML = html;
    var anchors = div.getElementsByTagName("a");
    var result = '';
    if (anchors.length > 0) {
        result = anchors[0].textContent;
    }
    return result;
}

$(document).ready(function () {
    
    bindStockTicker();
    

    $(".deleteUnit").bind("click", function () {
        var crid = $(this).attr('id').replace("delete_", "");
        if (confirm('Are you sure you wish to delete?')) {
            DeleteContact(crid);
        }
    });
});

function DeleteContact(id) {

    var jsonRequest = JSON.stringify({ "id": id });
    var unitName = 'unitName_' + id;
    jQuery.ajax({
        url: "/Ajax/DeleteContact",
        type: 'POST',
        data: jsonRequest,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var m = $('#' + unitName);      
            var filterType = $('#filterType');
            console.log(filterType.val());
            if (filterType.val() == "hidedeleted") {
                m.hide();
            }
        },
        error: function (request, status, error) {
            console.error('Error ' + status + ' ' + request.responseText);
        },
        beforeSend: function () {

        }
    });

}


function UpdateAutoGenerate(id, successFunction) {

    var jsonRequest = JSON.stringify({ "unitId": id });
    console.log(jsonRequest);
    
    jQuery.ajax({
        url: "/Ajax/UpdateAutoGenerate",
        type: 'POST',
        data: jsonRequest,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: successFunction,
        error: function (request, status, error) {
            console.error('Error ' + status + ' ' + request.responseText);
        },
        beforeSend: function () {

        }
    });

}

function bindStockTicker() {
    var tickers = new Array();
    $('[stock-ticker]').each(function () {
        var v = $(this).attr('stock-ticker');
        if (!isEmpty(v)) {
            tickers.push(v);
        }
    });
    handleStockTicker(tickers);
}

function handleStockTicker(tickers) {
  // console.log(tickers);
   if (tickers.length > 0) {
       
   
    $.ajax({
        url: '/Ajax/RetrieveCompanyQuote',
        type: 'POST',
        data: { stockTickers: tickers },
        //contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (data) {
            //for (var dex = 0; dex < tickers.length; dex++) {
            //    if (data.length > dex) {
            //        var d = data[dex].trim();
            //        if (!isEmpty(d)) {
            //            $("[stock-ticker='" + tickers[dex] + "']").html(d);
            //        } else {
            //            $("[company-item='" + tickers[dex] + "']").hide();
            //        }
            //    } else {
            //        $("[company-item='" + tickers[dex] + "']").hide();
            //    }
            //}
            if (isEmpty(data)) {
                return;
            }

            for (var dex = 0; dex < tickers.length; dex++) {
                if (data[tickers[dex]] != undefined) {
                    var d = data[tickers[dex]].trim();
                    if (!isEmpty(d)) {
                        $("[stock-ticker='" + tickers[dex] + "']").empty();
                        $("[stock-ticker='" + tickers[dex] + "']").html(data[tickers[dex]]);
                    } else {
                        $("[company-item='" + tickers[dex] + "']").hide();
                    }
                } else {
                    $("[company-item='" + tickers[dex] + "']").hide();
                }
            }
        },
        error: function (requestE, status, error) {
            console.log(status + " " + error.message);
        },
    });
       
   }

}

function GetStockIndexChanges(tickers, averageStockPriceSpan) {
    console.log(tickers);
    if (tickers.length > 0) {
        $.ajax({
            url: '/Ajax/GetStockIndexChanges',
            type: 'POST',
            data: { stockTickers: tickers },
            //contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (data) {
           
                
                    averageStockPriceSpan.html(data);
                 
            },
            error: function (requestE, status, error) {
                console.log(status + " " + error.message);
            },
        });

    }

}