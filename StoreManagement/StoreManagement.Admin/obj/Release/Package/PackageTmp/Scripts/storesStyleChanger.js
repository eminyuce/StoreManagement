$(document).ready(function () {
  
    $('[back_ground_color_code]').each(function () {
        $(this).css('background-color', $("#back_ground_color_code").val());
    });
    $('[navigation-text-color]').each(function () {
        $(this).css('color', $("#navigation_text_color").val());
    });
    $('[navigation_background_text_color]').each(function () {
        $(this).css('background-color', $("#navigation_background_text_color").val());
    });
    $('[navigation_text_size]').each(function () {
        $(this).css('font-size', $("#navigation_text_size").val());
    });


});