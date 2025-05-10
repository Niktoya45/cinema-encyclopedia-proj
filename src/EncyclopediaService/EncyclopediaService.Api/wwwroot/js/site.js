// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var routes = {
    0: "cinemas",
    1: "persons",
    2: "studios"
}

var selectLayout = document.getElementById("layout-select");


anchorBrandLayout = document.getElementById("brand");

if (window.location.pathname.toLowerCase().includes(anchorBrandLayout.href)) {
    anchorBrandLayout.href = "#";
}