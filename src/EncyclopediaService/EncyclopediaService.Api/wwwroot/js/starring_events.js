
import { fetchPostMVC, addSearchEvents, getPartialType, getHidden, bsElementClose, appendToCarousel, approveDelete } from './utils.js';


// ** DEFINED ELEMENTS ** //
const kpage = 5;
const nrows = 1;
const modalFormAddId = 'modal-add-starring';
const modalFormEditId = 'modal-edit-starring';
const formAddId = 'form-add-starring';
const formEditId = 'form-edit-starring';
const formDeleteId = 'form-delete-element'
const optionEditSelector = ".dropdown-menu .dropdown-edit-starring";
const optionDeleteSelector = ".dropdown-menu .dropdown-delete-starring";
const carouselId = "carousel-starrings";

const classDelete = 'label-starring';

const formAddSubmitId = 'submit-' + formAddId;
const formEditSubmitId = 'submit-' + formEditId;

var modalFormAdd = document.getElementById(modalFormAddId);
var modalFormEdit = document.getElementById(modalFormEditId);

var formAdd = document.getElementById(formAddId);
var formAddSubmit = document.getElementById(formAddSubmitId);
var formEdit = document.getElementById(formEditId);
var formEditSubmit = document.getElementById(formEditSubmitId);
var formDelete = document.getElementById(formDeleteId);

var carousel = document.getElementById(carouselId);

const reuseAddAction = window.location.pathname + "?handler=reuseaddstarring";
const reuseEditAction = window.location.pathname + "?handler=reuseeditstarring";
const deleteAction = window.location.pathname + "?handler=deletestarring";


//** ONLOAD ACTIONS ** //

// add option event to display edit form

var refsEditStarring = document.querySelectorAll(optionEditSelector);

refsEditStarring.forEach(function (ref) {

    addFormEditStarringOption(ref);
});

var refsDeleteStarring = document.querySelectorAll(optionDeleteSelector);

refsDeleteStarring.forEach(function (ref) {

    addFormDeleteStarringOption(ref);
});

// add events to starring add form

refreshFormAdd(formAdd);

// ** METHODS USED ** //


// add onclick event to add submit

formAddSubmit.addEventListener('click', function (e) {

    e.preventDefault();

    let form = formAdd;

    fetchFormAdd(form);

});

// add onclick event to edit submit

formEditSubmit.addEventListener('click', function (e) {

    e.preventDefault();

    let form = formEdit;

    fetchFormEdit(form);

});


// send add form data
function fetchFormAdd(form) {

    fetchPostMVC(form, form.action, "TEXT")
        .then((result) => {

            let placeholder = document.createElement("div");

            placeholder.innerHTML = result;

            if (getPartialType(placeholder) == "form") {

                let form = formAdd;

                form.innerHTML = result;
                placeholder.remove();

                refreshFormAdd(form);

                return;
            }

            let formWrap = document.createElement("form");

            formWrap.innerHTML = placeholder.innerHTML;
            placeholder.remove();

            let Id = getHidden(formWrap, "Id").value;

            let div = document.createElement("div");
            div.id = Id;

            formWrap.action = reuseEditAction;
            formWrap.method = "post";
            formWrap.id = "update-edit-starring-" + Id;

            div.append(formWrap);
            addFormEditStarringOption(div.querySelector(optionEditSelector));
            addFormDeleteStarringOption(div.querySelector(optionDeleteSelector));

            // arrange elements in carousel
            appendToCarousel(carousel, div, nrows, kpage);

            let modal = modalFormAdd;
            bsElementClose(modal);
        });    
}

// send edit form data
function fetchFormEdit(form) {

    fetchPostMVC(form, form.action, "TEXT")
        .then((result) => {

            let placeholder = document.createElement("div");

            placeholder.innerHTML = result;

            if (getPartialType(placeholder) == "form") {

                let form = formEdit;

                form.innerHTML = placeholder.innerHTML;
                placeholder.remove();

                refreshFormEdit(form);

                return;
            }
            let Id = getHidden(placeholder, "Id").value;

            let div = document.querySelector("div[id='"+Id+"']");
            let form = div.firstElementChild;

            form.innerHTML = placeholder.innerHTML;
            placeholder.remove();

            addFormEditStarringOption(div.querySelector(optionEditSelector));
            addFormDeleteStarringOption(div.querySelector(optionDeleteSelector))

            let modal = modalFormEdit;
            bsElementClose(modal);
        });    
}


// add onclick event to edit option

function addFormEditStarringOption(ref) {

    ref.addEventListener('click', function (e) {

        e.preventDefault();

        let formFetch = document.getElementById("update-" + ref.id);

        fetchPostMVC(formFetch, formFetch.action, "TEXT")
            .then((result) => {

                let form = formEdit;

                form.innerHTML = result;

                refreshFormEdit(form);
            });

    });

}

// add onclick event to delete option

function addFormDeleteStarringOption(ref) {

    ref.addEventListener('click', function (e) {

        e.preventDefault();

        approveDelete(ref.value, formDelete.closest('.container-fluid'), deleteAction, classDelete)
    });

}


// refresh events for add form
function refreshFormAdd(form) {

    updateFormStarringDependency(form);

    var searchStarringDropdown = form.querySelector(".search-starring");
    addSearchEvents(searchStarringDropdown, null, searchStarringChoice, searchStarringClose)
}

// refresh events for add form
function refreshFormEdit(form) {
    updateFormStarringDependency(form);
}

// add form events to manage input cross-dependencies
function updateFormStarringDependency(form) {
    var actorCheckbox = form.querySelector("#starring-jobs_8");

    const isActor = actorCheckbox.checked;
    var inputsJobsDependant = form.querySelectorAll(".jobs-dependant");

    if (!isActor) {
        inputsJobsDependant.forEach(function (input) {
            input.disabled = true;
        });
    }

    actorCheckbox.addEventListener('click', function (e) {
        inputsJobsDependant.forEach(function (input) {
            input.disabled = !actorCheckbox.checked;
        });
    });
}


// event on clicking the hinted starring option
function searchStarringChoice(searchInput, listItem) {
    var inputsNameDependant = formAdd.querySelectorAll(".name-dependant");
    getHidden(formAdd, "Id").value = Date.now();
    getHidden(formAdd, "Name").value = listItem.textContent;

    inputsNameDependant.forEach(function (input) {
        input.disabled = input.classList.contains("jobs-dependant");
    });
}

// event on closing starring options
function searchStarringClose(searchInput) {
    var inputsNameDependant = formAdd.querySelectorAll(".name-dependant");
    getHidden(formAdd, "Id").value = null;
    getHidden(formAdd, "Name").value = null;

    inputsNameDependant.forEach(function (input) {
        input.checked = false;
        input.disabled = true;
    });
}
