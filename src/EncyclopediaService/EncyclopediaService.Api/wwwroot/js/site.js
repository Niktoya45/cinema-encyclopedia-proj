// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var routes = {
    0: "cinemas",
    1: "persons",
    2: "studios"
}

// define search elements

var searchForm = document.getElementById("layout-search");
var searchSelect = document.getElementById("layout-search-select");
var searchButton = document.getElementById("layout-search-submit");


// form search query

if (searchSelect && searchForm) {
    searchSelect.selectedIndex = Array.from(searchSelect.options).findIndex(o => window.location.pathname.includes(o.value));
    searchForm.action = searchSelect.selectedOptions[0].firstElementChild.href;

    searchSelect.addEventListener('change', function (e) {

        let selected = searchSelect.selectedOptions[0];

        searchForm.action = selected.firstElementChild.href;
    }); 
}


// make brand name interactive

anchorBrandLayout = document.getElementById("brand");

if (window.location.pathname.toLowerCase().includes(anchorBrandLayout.href)) {
    anchorBrandLayout.href = "#";
}


// trim form inputs

var forms = document.querySelectorAll("form.trim-inputs")

forms.forEach(function (form) {

    let trimmables = form.querySelectorAll("input[type='text'], textarea");

    trimmables.forEach(function (trimmable) {
        trimmable.addEventListener('focusout', function (e) {
            e.target.value = e.target.value.trim();
        });

    });
});

