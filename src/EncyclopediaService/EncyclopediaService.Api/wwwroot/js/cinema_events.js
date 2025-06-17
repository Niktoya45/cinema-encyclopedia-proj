
import { fetchPostMVC, addDeleteEvents, approveDelete, appendToCarousel, removeFromCarousel } from './utils.js';
import { addFormAddSubmitStarring, addFormEditSubmitStarring } from './starring_events.js';

//** ELEMENTS USED **/

const kpage = 5;
const nrows = 1;
const carouselStarringId = "carousel-starrings";

const classDeleteCinema = 'label-cinema';
const classDeleteStarring = 'label-starring';
const classDeleteStudio = 'label-studio';

const formDeleteId = 'form-delete-element';
const deleteStudioAction = window.location.pathname + "?handler=deleteproductionstudio";

var formDelete = document.getElementById(formDeleteId);

var carouselStarring = document.getElementById(carouselStarringId);

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
        
    }
    else if (deleteTargetClass == classDeleteStarring) {
        removeFromCarousel(carouselStarring, result, nrows, kpage);
    }
    else if (deleteTargetClass == classDeleteStudio) {
        document.querySelector(".studios-block div[id='" + result + "']").remove();
    }
});


//** ONLOAD RECORD ACTIONS **//

addFormAddSubmitStarring(function (divStarring) {
    appendToCarousel(carouselStarring, divStarring, nrows, kpage);
});

addFormEditSubmitStarring(null);

// show close buttons for studio

var refsShowClose = document.querySelectorAll(".dropdown-menu .show-close-buttons");

refsShowClose.forEach(ref => {
    var closeButtons = document.querySelectorAll(".hide-delete[toggled-by='" + ref.id + "'] .del-record");
    var cancel = document.getElementById(ref.id + "-cancel");

    ref.addEventListener('click', (e) => {

        closeButtons.forEach((b) => {
            b.style.visibility = 'visible';
        });
        cancel.style.visibility = 'visible';
    });

    cancel.addEventListener('click', (e) => {

        closeButtons.forEach((b) => {
            b.style.visibility = 'hidden';
        });

        e.currentTarget.style.visibility = 'hidden';
    });
});


// add action after studio delete

var refsDeleteStudio = document.querySelectorAll(".hide-delete-studios .del-record");

refsDeleteStudio.forEach(ref => {
    ref.addEventListener('click', (e) => {

        approveDelete(ref.value, formDelete.closest('.container-fluid'), deleteStudioAction, classDeleteStudio);
    });

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