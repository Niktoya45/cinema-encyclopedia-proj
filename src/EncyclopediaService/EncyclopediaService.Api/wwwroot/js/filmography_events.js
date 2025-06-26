import {
    fetchPostMVC, fetchForm, addSearchEvents, addSearchChoice, addSearchClose,
    getHidden, approveDelete, deleteRecord
} from './utils.js';


// ** DEFINED ELEMENTS ** //

const modalFormAddId = 'modal-add-filmography';
const modalFormEditId = 'modal-edit-filmography';
const formAddId = 'form-add-filmography';
const formEditId = 'form-edit-filmography';
const formDeleteId = 'form-delete-element';
const blockSelector = '#filmography'
const optionEditSelector = ".dropdown-menu .dropdown-edit-filmography";
const optionDeleteSelector = ".dropdown-menu .dropdown-delete-filmography";

const classDelete = 'label-filmography';

const formAddSubmitId = 'submit-' + formAddId;
const formEditSubmitId = 'submit-' + formEditId;

var modalFormAdd = document.getElementById(modalFormAddId);
var modalFormEdit = document.getElementById(modalFormEditId);

var formAdd = document.getElementById(formAddId);
var formAddSubmit = document.getElementById(formAddSubmitId);
var formEdit = document.getElementById(formEditId);
var formEditSubmit = document.getElementById(formEditSubmitId);
var formDelete = document.getElementById(formDeleteId);

const reuseAddAction = window.location.pathname + "?handler=reuseaddfilmography";
const searchAddAction = window.location.pathname + "?handler=searchfilmography";
const reuseEditAction = window.location.pathname + "?handler=reuseeditfilmography";
const deleteAction = window.location.pathname + "?handler=deletefilmography";


//** ONLOAD ACTIONS ** //

if (formAddSubmit) {

    formAddSubmit.disabled = true;

    // add option event to display edit form

    var refsEditFilmography = document.querySelectorAll(optionEditSelector);

    if (refsEditFilmography)
        refsEditFilmography.forEach(function (ref) {

            addFormEditFilmographyOption(ref);
        });

    var refsDeleteFilmography = document.querySelectorAll(optionDeleteSelector);

    refsDeleteFilmography.forEach(function (ref) {

        addFormDeleteFilmographyOption(ref);
    });

    // add events to starring add form
    refreshFormAdd(formAdd);
}


// ** METHODS USED ** //


// add onclick event to add submit
export function addFormAddSubmitFilmography(actionAddFilmography, doFormWrap) {

    formAddSubmit.addEventListener('click', function (e) {

        e.preventDefault();
        formAddSubmit.disabled = true;
        let form = formAdd;

        fetchFormAddFilmography(form, doFormWrap, actionAddFilmography);

    });
}


// add onclick event to edit submit
export function addFormEditSubmitFilmography(actionEditFilmography) {

    formEditSubmit.addEventListener('click', function (e) {

        e.preventDefault();

        formEditSubmit.disabled = true;
        let form = formEdit;

        fetchFormEditFilmography(form, actionEditFilmography);

    });
}


// send add form data
function fetchFormAddFilmography(form, doFormWrap, actionAddFilmography) {

    fetchForm(
        form,
        function (placeholder) {
            let div = document.createElement("div");
            let Id = '';

            if (doFormWrap) {
                let formWrap = document.createElement("form");

                formWrap.innerHTML = placeholder.innerHTML;

                Id = getHidden(formWrap, "Id").value;

                formWrap.action = reuseEditAction;
                formWrap.method = "post";
                formWrap.id = "update-edit-filmography-" + Id;

                div.append(formWrap);
            } else {
                div.innerHTML = placeholder.innerHTML;
                let inpId = getHidden(div, "Id");
                if (!inpId) {
                    inpId = div.querySelector("[name$='Index']");
                }
                if (inpId)
                    Id = inpId.value;
            }
            div.id = Id;
            placeholder.remove();

            addFormEditFilmographyOption(div.querySelector(optionEditSelector));
            addFormDeleteFilmographyOption(div.querySelector(optionDeleteSelector));

            // callback after filmography addition
            if (actionAddFilmography) {
                actionAddFilmography(div);
            }

        },
        refreshFormAdd,
        modalFormAdd
    )
}


// send edit form data
function fetchFormEditFilmography(form, actionEditFilmography) {

    fetchForm(
        form,
        function (placeholder) {
            let Id = getHidden(placeholder, "Id").value;

            let div = document.querySelector("div[id='" + Id + "']");
            let formElement = div.firstElementChild;

            formElement.innerHTML = placeholder.innerHTML;
            placeholder.remove();

            addFormEditFilmographyOption(div.querySelector(optionEditSelector));
            addFormDeleteFilmographyOption(div.querySelector(optionDeleteSelector));

            // callback after filmography edit
            if (actionEditFilmography) {
                actionEditFilmography(div);
            }
        },
        refreshFormEdit,
        modalFormEdit
    )
}


// add onclick event to edit option
function addFormEditFilmographyOption(ref) {

    if (!ref) return;

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
function addFormDeleteFilmographyOption(ref) {

    if (!ref) return;

    ref.addEventListener('click', function (e) {

        e.preventDefault();

        if (formDelete)
            approveDelete(ref.value, formDelete.closest('.container-fluid'), deleteAction, classDelete);
        else {
            deleteRecord(ref.value, blockSelector);
        }
    });

}

// refresh events for add form
function refreshFormAdd(form) {

    // additional action dependant on form
    formAddSubmit.disabled = true;

    var searchFilmographyDropdown = form.querySelector(".search-filmography");
    addSearchEvents(searchFilmographyDropdown, searchAddAction, searchFilmographyChoice, searchFilmographyClose)
}

// refresh events for add form
function refreshFormEdit(form) {
    updateFormFilmographyDependency(form);
}


// event on clicking the hinted filmography option
function searchFilmographyChoice(searchInput, listItem, choice) {

    addSearchChoice(
        formAdd,
        searchAddAction,
        searchInput,
        listItem,
        choice,
        function () {
            formAddSubmit.disabled = false;
        }
    );

}

// event on closing starring options
function searchFilmographyClose(searchInput) {

    addSearchClose(
        formAdd,
        searchInput,
        function () {
            formAddSubmit.disabled = true;
        }
    );

}
