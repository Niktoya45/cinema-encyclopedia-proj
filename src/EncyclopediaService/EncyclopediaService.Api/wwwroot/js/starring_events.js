
import {
    fetchPostMVC, fetchForm,
    addSearchEvents, addSearchChoice, addSearchClose,
    getHidden, approveDelete, deleteRecord
} from './utils.js';


// ** DEFINED ELEMENTS ** //

const modalFormAddId = 'modal-add-starring';
const modalFormEditId = 'modal-edit-starring';
const formAddId = 'form-add-starring';
const formEditId = 'form-edit-starring';
const formDeleteId = 'form-delete-element';
const blockSelector = '#starrings'
const optionEditSelector = ".dropdown-menu .dropdown-edit-starring";
const optionDeleteSelector = ".dropdown-menu .dropdown-delete-starring";

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

const reuseAddAction = window.location.pathname + "?handler=reuseaddstarring";
const searchAddAction = window.location.pathname + "?handler=searchstarring";
const reuseEditAction = window.location.pathname + "?handler=reuseeditstarring";
const deleteAction = window.location.pathname + "?handler=deletestarring";

//** ONLOAD ACTIONS ** //

if (formAddSubmit) {

    formAddSubmit.disabled = true;

    // add option event to display edit form

    var refsEditStarring = document.querySelectorAll(optionEditSelector);

    if (refsEditStarring)
        refsEditStarring.forEach(function (ref) {

            addFormEditStarringOption(ref);
        });

    var refsDeleteStarring = document.querySelectorAll(optionDeleteSelector);

    refsDeleteStarring.forEach(function (ref) {

        addFormDeleteStarringOption(ref);
    });

    // add events to starring add form
    refreshFormAdd(formAdd);
}


// ** METHODS USED ** //


// add onclick event to add submit
export function addFormAddSubmitStarring(actionAddStarring, doFormWrap) {

    formAddSubmit.addEventListener('click', function (e) {

        e.preventDefault();
        formAddSubmit.disabled = true;
        let form = formAdd;

        fetchFormAddStarring(form, doFormWrap, actionAddStarring);

    });
}


// add onclick event to edit submit
export function addFormEditSubmitStarring(actionEditStarring) {

    formEditSubmit.addEventListener('click', function (e) {

        e.preventDefault();

        formEditSubmit.disabled = true;
        let form = formEdit;

        fetchFormEditStarring(form, actionEditStarring);

    });
}


// send add form data
function fetchFormAddStarring(form, doFormWrap, actionAddStarring) {

    fetchForm(
        form,
        function (placeholder) {
            let div = document.createElement("div");
            let Id = "";

            if (doFormWrap) {
                let formWrap = document.createElement("form");

                formWrap.innerHTML = placeholder.innerHTML;
                
                Id = getHidden(formWrap, "Id").value;

                formWrap.action = reuseEditAction;
                formWrap.method = "post";
                formWrap.id = "update-edit-starring-" + Id;

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

            addFormEditStarringOption(div.querySelector(optionEditSelector));
            addFormDeleteStarringOption(div.querySelector(optionDeleteSelector));

            // callback after starring addition
            if (actionAddStarring) {
                actionAddStarring(div);
            }

        },
        refreshFormAdd,
        modalFormAdd
    )    
}


// send edit form data
function fetchFormEditStarring(form, actionEditStarring) {

    fetchForm(
        form,
        function (placeholder) {
            let Id = getHidden(placeholder, "Id").value;

            let div = document.querySelector("div[id='" + Id + "']");
            let formElement = div.firstElementChild;

            formElement.innerHTML = placeholder.innerHTML;
            placeholder.remove();

            addFormEditStarringOption(div.querySelector(optionEditSelector));
            addFormDeleteStarringOption(div.querySelector(optionDeleteSelector));

            // callback after starring edit
            if (actionEditStarring) {
                actionEditStarring(div);
            }
        },
        refreshFormEdit,
        modalFormEdit
    )
}


// add onclick event to edit option

function addFormEditStarringOption(ref) {

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
function addFormDeleteStarringOption(ref) {

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

    updateFormStarringDependency(form);
    formAddSubmit.disabled = true;

    var searchStarringDropdown = form.querySelector(".search-starring");
    addSearchEvents(searchStarringDropdown, searchAddAction, searchStarringChoice, searchStarringClose)
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
function searchStarringChoice(searchInput, listItem, choice) {

    addSearchChoice(
        formAdd,
        searchAddAction,
        searchInput,
        listItem,
        choice,
        function () {
            formAddSubmit.disabled = false;

            var inputsNameDependant = formAdd.querySelectorAll(".name-dependant");

            inputsNameDependant.forEach(function (input) {
                input.disabled = input.classList.contains("jobs-dependant");
            });
        }
    );


}

// event on closing starring options
function searchStarringClose(searchInput) {

    addSearchClose(
        formAdd,
        searchInput,
        function () {
            formAddSubmit.disabled = true;
            var inputsNameDependant = formAdd.querySelectorAll(".name-dependant");


            inputsNameDependant.forEach(function (input) {
                if (input.type == 'checkbox' || input.type == 'radio')
                    input.checked = false;
                else if (input.firstElementChild)
                    input.selected = '0';
                else input.value = '';
                input.disabled = true;
            });
        }
    );

}
