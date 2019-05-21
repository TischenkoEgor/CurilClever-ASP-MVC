// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function SwitchToPrintView() {
    //запомним этот режим в куки
    $.cookie('print_view', '1');
    $('header').css('display', 'none');
    $('.jumbotron').css('display', 'none')
    
    $('#print_view_btn').css('display', 'none');
    $('#default_view_btn').css('display', 'block');
}
function SwitchToDefaultView() {
    $.cookie('print_view', 'null');
    $('header').css('display', 'block');
    $('.jumbotron').css('display', 'block')
    $('.container').css('max-width', '')

    $('#print_view_btn').css('display', 'block');
    $('#default_view_btn').css('display', 'none');
}