// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/** AJAX TEMPLATE CALL **/

/*

    // vv form submit button vv

    formSubmit.addEventListener('click', function (e) {

        // vv prevent default redirect action vv
        e.preventDefault();

        // vv you'd probably want to use submit button parent here vv

        var formElement = formSubmit.parentElement; 
        const formData = new URLSearchParams();

        for (const kv of new FormData(formElement)) {
            formData.append(kv[0], kv[1]);
        }

        fetch(formElement.action,
            {
                method: "POST",
                body: formData
            })
            .then((response) => {

                return response.text();
                // or : 
                // return response.json();
            })
            .then((result) => {

               // then do something in a callback

            });

});


*/

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


var forms = document.querySelectorAll("form.trim-inputs")

forms.forEach(function (form) {

    let trimmables = form.querySelectorAll("input[type='text'], textarea");

    trimmables.forEach(function (trimmable) {
        trimmable.addEventListener('focusout', function (e) {
            e.target.value = e.target.value.trim();
        });

    });
});