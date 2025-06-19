
import { fetchForm, addDeleteEvents, approveDelete, appendToCarousel, removeFromCarousel, addShowCloseEvents, addSearchChoice } from './utils.js';
import { addFormAddSubmitStarring, addFormEditSubmitStarring } from './starring_events.js';

//** ELEMENTS USED **/

const kpage = 5;
const nrows = 1;
const carouselStarringId = "carousel-starrings";

const modalFormStudioAddId = 'modal-add-starring';

const classDeleteCinema = 'label-cinema';
const classDeleteStarring = 'label-starring';
const classDeleteStudio = 'label-studio';

const formDeleteId = 'form-delete-element';
const formAddStudioId = 'form-add-studio';
const formAddStudioSubmitId = 'submit-' + formAddStudioId;

const deleteStudioAction = window.location.pathname + "?handler=deleteproductionstudio";
const searchStudioAddAction = window.location.pathname + "?handler=searchproductionstudio";
const reuseStudioAddAction = window.location.pathname + "?handler=reuseaddproductionstudio";

var formDelete = document.getElementById(formDeleteId);
var formAddStudio = document.getElementById(formAddStudioId);
var formAddStudioSubmit = document.getElementById(formAddStudioSubmitId);

var modalFormStudioAdd = document.getElementById(modalFormStudioAddId);

var carouselStarring = document.getElementById(carouselStarringId);
var blockStudio = document.querySelector(".studios-block");

var starsCollapse = document.getElementById("collapse-stars");
var starsRate = starsCollapse.querySelector("#rate-cinema");
var starsClick = starsCollapse.querySelectorAll("button.rail");
var stars = Array.from(starsClick).map(starClick => starClick.querySelector(".star"));


//** ONLOAD ACTIONS **//

// make user rating change

for (let i = 0; i < stars.length; i++) {

    starsClick.item(i).addEventListener('click', function (e) {

        e.preventDefault();

        let rating = 0;
        if (stars[i].classList.contains('rail')) {
            rating = this.value;
        } else {
            if(i != 0)
                rating = starsClick.item(i-1).value
        }

        fillStars(i);
    });
}


// add onlick event for delete popup 

addDeleteEvents(formDelete, function (result, deleteTargetClass) {

    if (deleteTargetClass == classDeleteCinema) {
        window.location = '/';
    }
    else if (deleteTargetClass == classDeleteStarring) {
        removeFromCarousel(carouselStarring, result, nrows, kpage);
    }
    else if (deleteTargetClass == classDeleteStudio) {
        document.querySelector(".studios-block div[id='" + result + "']").remove();
    }
});


//** ONLOAD RECORD ACTIONS **//

// add submit starring events
addFormAddSubmitStarring(function (divStarring) {
    appendToCarousel(carouselStarring, divStarring, nrows, kpage);
});

addFormEditSubmitStarring(null);

// add submit studio events 
formAddStudioSubmit.disabled = true;
refreshFormAdd(formStudioAdd);
addFormAddSubmitStudio(function (div) { blockStudio.append(div.firstElementChild); });

// show close buttons for studio
addShowCloseEvents();


// add action after studio delete
var refsDeleteStudio = document.querySelectorAll(".hide-delete-studios .del-record");

refsDeleteStudio.forEach(ref => {
    addApproveStudioDelete(ref);
});


//** METHODS USED **//

function fillStars(starIndex) {

    for (let j = 0; j < stars.length; j++) {
        if (j < starIndex) {
            stars[j].classList.remove('rail');
        }
        else if (j > starIndex) {
            if (stars[j].classList.contains('rail')) {
                break;
            }
            stars[j].classList.add('rail');
        }
        else {
            if (stars[j].classList.contains('rail')) {
                stars[j].classList.remove('rail');
            } else {
                stars[j].classList.add('rail');
            }
        }
    }
}


//** Studio Logo **//



// add onclick event to add studio submit
function addFormAddSubmitStudio(actionAddStudioLogo) {

    console.log("added");
    formAddStudioSubmit.addEventListener('click', function (e) {

        e.preventDefault();
        formAddStudioSubmit.disabled = true;

        let form = formAddStudio;

        fetchFormAddStudioLogo(form, actionAddStudioLogo);

    });
}

// send add form studio data
function fetchFormAddStudioLogo(form, actionAddStudioLogo) {

    fetchForm(
        form,
        function (placeholder) {
            let div = document.createElement("div");
            div.innerHTML = placeholder.innerHTML;
            placeholder.remove();

            addApproveStudioDelete(div.querySelector(".del-record"));

            // callback after studio addition
            if (actionAddStudioLogo) {
                actionAddStudioLogo(div);
            }

        },
        refreshFormStudioAdd,
        modalFormStudioAdd
    )
}

// refresh events for studio add form
function refreshFormStudioAdd(form) {

    formStudioAddSubmit.disabled = true;
    var searchStudioDropdown = form.querySelector(".search-studio");
    addSearchEvents(searchStudioDropdown, null, searchStudioChoice, searchStudioClose)
}

// event on clicking the hinted studio option
function searchStudioChoice(searchInput, listItem, choice) {

    addSearchChoice(
        formStudioAdd,
        searchStudioAddAction,
        searchInput,
        listItem,
        choice,
        function () {
            formStudioAddSubmit.disabled = false;
        }
    );
}

// event on closing studio options
function searchStudioClose(searchInput) {

    addSearchClose(
        formAdd,
        searchInput,
        function () {
            formStudioAddSubmit.disabled = true;
        }
    );
}


// approve studio deletion popup 
function addApproveStudioDelete(ref) {
    ref.addEventListener('click', (e) => {
        approveDelete(ref.value, formDelete.closest('.container-fluid'), deleteStudioAction, classDeleteStudio);
    });
}