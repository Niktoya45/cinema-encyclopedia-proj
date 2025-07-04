
import {
    fetchPostMVC, fetchForm,
    addDeleteEvents, approveDelete,
    appendToCarousel, removeFromCarousel,
    bsElementShow, 
    addShowEditorEvents, addMainPictureEditEvents,
    addShowCloseEvents,
    getHidden,
} from './utils.js';
import { addFormAddSubmitStarring, addFormEditSubmitStarring } from './starring_events.js';
import { addFormAddSubmitStudio } from './production_studio_events.js';

//** ELEMENTS USED **/

const kpage = 5;
const nrows = 1;
const carouselStarringId = "carousel-starrings";

const classDeleteCinema = 'label-cinema';
const classDeleteStarring = 'label-starring';
const classDeleteStudio = 'label-studio';

const formPictureId = 'form-edit-poster';
const formPictureSubmitId = 'submit-' + formPictureId;
const formDeleteId = 'form-delete-element';
const formEditMainId = 'form-edit-main';
const formEditMainSubmitId = 'submit-' + formEditMainId;
const formAddStudioId = 'form-add-studio';
const formAddStudioSubmitId = 'submit-' + formAddStudioId;
const formLoginId = 'form-login';

const deleteCinemaAction = window.location.pathname + "?handler=deletecinema";
const deleteStudioAction = window.location.pathname + "?handler=deleteproductionstudio";
const rateAction = window.location.pathname + "?handler=rate";
const labelAction = window.location.pathname + "?handler=label";

var formPicture = document.getElementById(formPictureId);
var formPictureSubmit = document.getElementById(formPictureSubmitId);
var formDelete = document.getElementById(formDeleteId);
var formEditMain = document.getElementById(formEditMainId);
var formEditMainSubmit = document.getElementById(formEditMainSubmitId);
var formLogin = document.getElementById(formLoginId);

var carouselStarring = document.getElementById(carouselStarringId);
var blockStudios = document.querySelector(".studios-block");
var blockLabels = document.querySelector(".labels-block");
var imgPicture = document.querySelector(".cinema-poster img");

var starsCollapse = document.getElementById("collapse-stars");
var starsRate = starsCollapse.querySelector("#rate-cinema");
var starsClick = starsCollapse.querySelectorAll("button.rail");
var stars = Array.from(starsClick).map(starClick => starClick.querySelector(".star"));

var labelsClick = blockLabels.querySelectorAll("button");

const UserLoggedIn = !formLogin;
const UserAdmin = !!formEditMain;


//** ONLOAD ACTIONS **//

// make user rating change

    for (let i = 0; i < stars.length; i++) {

        starsClick.item(i).addEventListener('click', function (e) {

            e.preventDefault();

            if (!UserLoggedIn || starsRate.classList.contains('login-proceed')) {
                return;
            }

            let rating = 0;
            if (stars[i].classList.contains('rail')) {
                rating = this.value;
            } else {
                if (i != 0)
                    rating = starsClick.item(i - 1).value
            }

            let uScore = getHidden(starsRate, 'UserScore').value;

            fetchPostMVC(new URLSearchParams({ score: rating, userScore: uScore }), starsRate.action, "JSON")
                .then((result) => {

                    console.log(result);

                    fillStars(i);
                });
        });
    }


    // make user label change
for (let i = 0; i < labelsClick.length; i++) {

    labelsClick.item(i).addEventListener('click', function (e) {

        e.preventDefault();

        let labelValue = 0;

        if (this.classList.contains('active')) {
            labelValue = -parseInt(this.value);
        }
        else {
            labelValue = parseInt(this.value);
        }

        if (labelValue == 0) {
            return;
        }
        console.log(labelValue);

        if (!UserLoggedIn || blockLabels.classList.contains('login-proceed')) {
            return;
        }

        fetchPostMVC(new URLSearchParams({ label: labelValue }), labelAction, "JSON")
            .then((result) => {

                labelsClick.forEach(function (click) {

                    if (parseInt(click.value) & result.label) {
                        click.classList.add('active');
                    }
                    else {
                        click.classList.remove('active');
                    }
                });
            });

    });
}


if (UserAdmin) {

    // add onclick event to show hidden editor options
    addShowEditorEvents();

    // add main object edit form submission
    formEditMainSubmit.addEventListener('click', function (e) {
        e.preventDefault();

        this.disabled = true;

        let form = formEditMain;

        fetchForm(
            form,
            (p) => { window.location.reload(); },
            null,
            null
        );

    });

    // make cinema page deletion confirmed 
    approveDelete("", formDelete.closest('.container-fluid'), deleteCinemaAction, classDeleteCinema);


    // add onlick events for delete popup after delete button is clicked 
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
}


//** ONLOAD RECORD ACTIONS **//

if (UserAdmin) {

    // add submit picture events
    addMainPictureEditEvents(formPicture, formPictureSubmit, imgPicture);

    // add submit starring events
    addFormAddSubmitStarring(function (divStarring) {
        appendToCarousel(carouselStarring, divStarring, nrows, kpage);
    }, true);

    addFormEditSubmitStarring(null);

    // add submit studio events 
    addFormAddSubmitStudio(function (div) {
        addApproveStudioDelete(div.querySelector(".del-record"));
        blockStudios.append(div.firstElementChild);
    }, false);

    // show close buttons for studio
    addShowCloseEvents();

    // add action after studio delete
    var refsDeleteStudio = document.querySelectorAll(".hide-delete-studios .del-record");

    refsDeleteStudio.forEach(ref => {
        addApproveStudioDelete(ref);
    });

}


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


// approve studio deletion popup 
function addApproveStudioDelete(ref) {
    ref.addEventListener('click', function(e) {
        approveDelete(ref.value, formDelete.closest('.container-fluid'), deleteStudioAction, classDeleteStudio);
    });
}
