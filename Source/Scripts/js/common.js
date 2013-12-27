$(function () {
    // Login form
    $('.dropdown-menu').find('form').click(function (e) {
        e.stopPropagation();
    });
    
    // strar rating
    $(".rating > span").hover(function () {
        $(this).removeClass();
        $(this).addClass('glyphicon glyphicon-star');
    });
});
