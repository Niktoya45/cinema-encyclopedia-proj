
// Write your JavaScript code.

/********* 
*
* AJAX CALLS TEMPLATE

// vv ref == submit form button 

    ref.addEventListener('click', function (e) {

        e.preventDefault();

// vv getting form node

        var formElement = document.getElementById("form-" + e.currentTarget.id);
        const formData = new URLSearchParams();

        for (const kv of new FormData(formElement)) {
            formData.append(kv[0], kv[1]);
        }

        fetch(window.location.href + '?handler=handlerName',
            {
                method: "POST",
                body: formData
            })
            .then((response) => {

                return response.text();
            })
            .then((text) => {
                // do something on page if needed
            });
        });

*
**********/




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