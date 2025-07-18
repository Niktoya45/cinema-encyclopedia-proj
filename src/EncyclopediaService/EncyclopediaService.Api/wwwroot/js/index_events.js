
// provide references to corresponding pagination elements preserving other query parameters

const pageNumParam = 'pagen';
const pageNumsSelector = '#pages .page-item';
const sortOrdParam = 'order';
const sortByParam = 'by';

var pageNums = document.querySelectorAll(pageNumsSelector);

var locationSearch = window.location.search;
var locationPlain = window.location.pathname;

const currentSearch = new URLSearchParams(locationSearch);

if (currentSearch.has(pageNumParam)) {
    currentSearch.delete(pageNumParam);
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

    let sortSearch = '';

    if (currentSearch.has(sortOrdParam)) {
        sortSearch = `${sortOrdParam}=${currentSearch.get(sortOrdParam)}&${sortByParam}=${currentSearch.get(sortByParam)}`;
        currentSearch.delete(sortOrdParam);
        currentSearch.delete(sortByParam);
    }

    let filterSearch = !currentSearch.size ? '' : currentSearch.toString();

    if (input.classList.contains("form-select")) {

        input.addEventListener('change', function (e) {
            let targetForm = e.target.closest("form");

            let action = targetForm.action;

            let formData = new FormData(targetForm);
            let targetData = new URLSearchParams();

            for (const [k, v] of formData) {
                targetData.set(k, v);
            }

            action += '?' + targetData.toString();

            if (sortSearch) {
                action += '&' + sortSearch;
            }

            window.location = action;
        });

    } else {

        input.addEventListener('click', function (e) {
            let targetForm = e.target.closest("form");

            let action = targetForm.action;

            if (targetForm.id == "sort-options") {

                let checkAlsoClass = e.target.classList.contains('sort-options-order') ? '.sort-options-by' : '.sort-options-order';

                let checkAlso = targetForm.querySelectorAll(checkAlsoClass);

                if (!Array.from(checkAlso).some(r => r.checked))
                    checkAlso[0].checked = true;

                let formData = new FormData(targetForm);
                let targetData = new URLSearchParams();

                for (const [k, v] of formData) {
                    targetData.set(k, v);
                }

                action = window.location.pathname + '?' + targetData.toString();

                if (filterSearch) {
                   action += '&' + filterSearch;
                }
            }
            else {

                let formData = new FormData(targetForm);
                let targetData = new URLSearchParams();

                for (const [k, v] of formData) {
                    targetData.set(k, v);
                }

                action += '?' + targetData.toString();

                if (sortSearch) {
                    action += `&` + sortSearch;
                }
            }

            window.location = action;

        });
    }
});