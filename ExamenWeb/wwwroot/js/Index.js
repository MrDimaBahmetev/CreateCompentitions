$(".ripple").on("click", function (event) {
    $(this).append("<span class='ripple-effect'>");
    $(this).find(".ripple-effect").css({
        left: event.pageX - $(this).position().left,
        top: event.pageY - $(this).position().top
    }).animate({
        opacity: 0,
    }, 1500, function () {
        $(this).remove();
    });
});