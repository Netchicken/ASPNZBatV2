// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//https://www.w3schools.com/howto/howto_js_navbar_shrink_scroll.asp
window.onscroll = function () { scrollFunction(); };

//make the navbar smaller - overwritten by the hide function below
function scrollFunction() {
    if (document.body.scrollTop > 50 || document.documentElement.scrollTop > 50) {
        document.getElementById("menu").style.padding = "2px 2px";
        //document.getElementById("navbar-brand").style.fontSize = "10px";
    } else {
        document.getElementById("menu").style.padding = "10px 10px";
        //document.getElementById("navbar-brand").style.fontSize = "35px";
    }
}

/* When the user scrolls down, hide the navbar. When the user scrolls up, show the navbar */
var prevScrollpos = window.pageYOffset;
window.onscroll = function () {
    var currentScrollPos = window.pageYOffset;
    if (prevScrollpos > currentScrollPos) {
        document.getElementById("menu").style.top = "0";
    } else {
        document.getElementById("menu").style.top = "-100px";

    }
    prevScrollpos = currentScrollPos;
};

//function resize() {
//    if ($(window).width() < 400) {
//        $(".intro").prop('background', 'url(/image/logo-mobile.svg) no-repeat center center;');
//    } else {
//        $(".intro").prop('background', 'url(/image/campus.jpg) no-repeat center center;');
//    }
//}
//resize();
//$(window).on('resize', resize);