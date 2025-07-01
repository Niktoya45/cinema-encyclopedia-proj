import {
    addDeleteEvents, approveDelete,
    appendToCarousel, removeFromCarousel,
    addShowEditorEvents, addMainPictureEditEvents
} from './utils.js';
import { addFormAddSubmitFilmography } from './filmography_events.js';

//** ELEMENTS USED **/

const kpage = 5;
const nrows = 1;
const carouselFilmographyId = "carousel-filmography";

const classDeletePerson = 'label-person';
const classDeleteFilmography = 'label-filmography';

const formPictureId = 'form-edit-picture';
const formPictureSubmitId = 'submit-' + formPictureId;
const formDeleteId = 'form-delete-element';
const formEditMainId = 'form-edit-main';
const formEditMainSubmitId = 'submit-' + formEditMainId;

const deletePersonAction = window.location.pathname + "?handler=deleteperson";
const deleteFilmographyAction = window.location.pathname + "?handler=deletefilmography";

var formPicture = document.getElementById(formPictureId);
var formPictureSubmit = document.getElementById(formPictureSubmitId);
var formDelete = document.getElementById(formDeleteId);
var formEditMain = document.getElementById(formEditMainId);
var formEditMainSubmit = document.getElementById(formEditMainSubmitId);

var carouselFilmography = document.getElementById(carouselFilmographyId);
var imgPicture = document.querySelector(".person-picture img");

const UserAdmin = formEditMain ? true : false;


//** ONLOAD ACTIONS **//

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

    // make person page deletion confirmed 
    approveDelete("", formDelete.closest('.container-fluid'), deletePersonAction, classDeletePerson);


    // add onlick events for delete popup after delete button is clicked 
    addDeleteEvents(formDelete, function (result, deleteTargetClass) {

        if (deleteTargetClass == classDeletePerson) {
            window.location = '/';
        }
        else if (deleteTargetClass == classDeleteFilmography) {
            removeFromCarousel(carouselFilmography, result, nrows, kpage);
        }
    });
}


//** ONLOAD RECORD ACTIONS **//

if (UserAdmin) {

    // add submit picture events
    addMainPictureEditEvents(formPicture, formPictureSubmit, imgPicture);

    // add submit filmography events
    addFormAddSubmitFilmography(function (divFilmography) {
        appendToCarousel(carouselFilmography, divFilmography, nrows, kpage);
    }, true);

}
