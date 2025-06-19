
import {
    fetchPostMVC, fetchForm, addSearchEvents, addSearchChoice, addSearchClose,
    getHidden, approveDelete
} from './utils.js';


// ** DEFINED ELEMENTS ** //

const modalFormAddId = 'modal-add-starring';
const modalFormEditId = 'modal-edit-starring';
const formAddId = 'form-add-starring';
const formEditId = 'form-edit-starring';
const formDeleteId = 'form-delete-element'
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

formAddSubmit.disabled = true;

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
export function addFormAddSubmitStarring(actionAddStarring) {

    formAddSubmit.addEventListener('click', function (e) {

        e.preventDefault();
        formAddSubmit.disabled = true;
        let form = formAdd;

        fetchFormAddStarring(form, actionAddStarring);

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
function fetchFormAddStarring(form, actionAddStarring) {

    fetchForm(
        form,
        function (placeholder) {

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
                input.checked = false;
                input.disabled = true;
            });
        }
    );

}
