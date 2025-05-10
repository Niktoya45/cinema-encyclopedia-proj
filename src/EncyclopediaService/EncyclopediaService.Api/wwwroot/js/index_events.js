
// provide references to corresponding pagination elements preserving other query parameters

var pageNums = document.querySelectorAll("#pages .page-item");

var locationSearch = window.location.search;
var locationPlain = window.location.pathname;
const currentSearch = new URLSearchParams(locationSearch);

if (currentSearch.has("pageNum")) {
    currentSearch.delete("pageNum");
}
if (currentSearch.size) {

    pageNums.forEach(function (pageNum) {

        var ref = pageNum.firstElementChild;

        if (ref) { 
            ref.href = ref.href + "&" + currentSearch.toString();
        }
    });
}


// submit filter and sort options forms simply by clicking on a form element

var placeFilterAndSort = document.getElementById("filter-and-sort");

var formsInputs = placeFilterAndSort.querySelectorAll(".form-submit");

formsInputs.forEach(function (input) {

    if (input.classList.contains("form-select")) {

        input.addEventListener('change', function (e) {
            let targetForm = e.target.closest("form");
            targetForm.submit();
        });

    } else {

        input.addEventListener('click', function (e) {
            let targetForm = e.target.closest("form");

            if (targetForm.id == "sort-options") {

                let checkAlsoClass = e.target.classList.contains('sort-options-order') ? '.sort-options-by' : '.sort-options-order';

                let checkAlso = targetForm.querySelectorAll(checkAlsoClass);

                if (!Array.from(checkAlso).some(r => r.checked))
                    checkAlso[0].checked = true;
            }

            targetForm.submit();

        });
    }
});