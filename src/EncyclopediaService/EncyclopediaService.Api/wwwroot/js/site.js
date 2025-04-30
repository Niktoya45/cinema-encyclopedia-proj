// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var routes = {
    0: "cinemas",
    1: "persons",
    2: "studios"
}

var selectLayout = document.getElementById("layout-select");

for (let i = 0; i < selectLayout.options.length; i++) {
    if (window.location.href.toLowerCase().includes(routes[i])) {
        selectLayout.selectedIndex = i;
        selectLayout.options[i].firstChild.href = "#";
        break;
    }
}