import {
    fetchPostMVC, fetchForm, addSearchEvents, addSearchChoice, addSearchClose,
    getHidden, approveDelete, deleteRecord
} from './utils.js';


// ** DEFINED ELEMENTS ** //

const modalFormAddId = 'modal-add-studio';
const modalFormEditId = 'modal-edit-studio';
const formAddId = 'form-add-studio';
const formEditId = 'form-edit-studio';
const formDeleteId = 'form-delete-element';
const blockSelector = '#studios'
const optionEditSelector = ".dropdown-menu .dropdown-edit-studio";
const optionDeleteSelector = ".dropdown-menu .dropdown-delete-studio";

const classDelete = 'label-studio';

const formAddSubmitId = 'submit-' + formAddId;
const formEditSubmitId = 'submit-' + formEditId;

var modalFormAdd = document.getElementById(modalFormAddId);
var modalFormEdit = document.getElementById(modalFormEditId);

var formAdd = document.getElementById(formAddId);
var formAddSubmit = document.getElementById(formAddSubmitId);
var formEdit = document.getElementById(formEditId);
var formEditSubmit = document.getElementById(formEditSubmitId);
var formDelete = document.getElementById(formDeleteId);

const reuseAddAction = window.location.pathname + "?handler=reuseaddproductionstudio";
const searchAddAction = window.location.pathname + "?handler=searchproductionstudio";
const reuseEditAction = window.location.pathname + "?handler=reuseeditproductionstudio";
const deleteAction = window.location.pathname + "?handler=deleteproductionstudio";


//** ONLOAD ACTIONS ** //

if (formAddSubmit) {

    formAddSubmit.disabled = true;

    // add option event to display edit form

    var refsEditStudio = document.querySelectorAll(optionEditSelector);

    if (refsEditStudio)
        refsEditStudio.forEach(function (ref) {

            addFormEditStudioOption(ref);
        });

    var refsDeleteStudio = document.querySelectorAll(optionDeleteSelector);

    refsDeleteStudio.forEach(function (ref) {

        addFormDeleteStudioOption(ref);
    });

    // add events to starring add form
    refreshFormAdd(formAdd);
}


// ** METHODS USED ** //


// add onclick event to add submit
export function addFormAddSubmitStudio(actionAddStudio, doFormWrap) {

    formAddSubmit.addEventListener('click', function (e) {

        e.preventDefault();
        formAddSubmit.disabled = true;
        let form = formAdd;

        fetchFormAddStudio(form, doFormWrap, actionAddStudio);

    });
}


// add onclick event to edit submit
export function addFormEditSubmitStudio(actionEditStudio) {

    formEditSubmit.addEventListener('click', function (e) {

        e.preventDefault();

        formEditSubmit.disabled = true;
        let form = formEdit;

        fetchFormEditStudio(form, actionEditStudio);

    });
}


// send add form data
function fetchFormAddStudio(form, doFormWrap, actionAddStudio) {

    fetchForm(
        form,
        function (placeholder) {
            if (!placeholder) return;

            let div = document.createElement("div");
            let Id = '';

            if (doFormWrap) {
                let formWrap = document.createElement("form");

                formWrap.innerHTML = placeholder.innerHTML;

                Id = getHidden(formWrap, "Id").value;

                formWrap.action = reuseEditAction;
                formWrap.method = "post";
                formWrap.id = "update-edit-studio-" + Id;

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

            addFormEditStudioOption(div.querySelector(optionEditSelector));
            addFormDeleteStudioOption(div.querySelector(optionDeleteSelector));

            // callback after studio addition
            if (actionAddStudio) {
                actionAddStudio(div);
            }

        },
        refreshFormAdd,
        modalFormAdd
    )
}


// send edit form data
function fetchFormEditStudio(form, actionEditStudio) {

    fetchForm(
        form,
        function (placeholder) {
            let Id = getHidden(placeholder, "Id").value;

            let div = document.querySelector("div[id='" + Id + "']");
            let formElement = div.firstElementChild;

            formElement.innerHTML = placeholder.innerHTML;
            placeholder.remove();

            addFormEditStudioOption(div.querySelector(optionEditSelector));
            addFormDeleteStudioOption(div.querySelector(optionDeleteSelector));

            // callback after studio edit
            if (actionEditStudio) {
                actionEditStudio(div);
            }
        },
        refreshFormEdit,
        modalFormEdit
    )
}


// add onclick event to edit option
function addFormEditStudioOption(ref) {

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
function addFormDeleteStudioOption(ref) {

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

    var searchStudioDropdown = form.querySelector(".search-studio");
    addSearchEvents(searchStudioDropdown, searchAddAction, searchStudioChoice, searchStudioClose)
}

// refresh events for add form
function refreshFormEdit(form) {
    updateFormStudioDependency(form);
}


// event on clicking the hinted studio option
function searchStudioChoice(searchInput, listItem, choice) {

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
function searchStudioClose(searchInput) {

    addSearchClose(
        formAdd,
        searchInput,
        function () {
            formAddSubmit.disabled = true;
        }
    );

}
