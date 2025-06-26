import { addFormAddSubmitFilmography } from './filmography_events.js';

//** ELEMENTS USED **/


const blockFilmographyId = "filmography";

const formPersonId = 'form-add-person';
const formPersonSubmitId = 'submit-' + formPersonId;

var blockFilmography = document.getElementById(blockFilmographyId);

var formPerson = document.getElementById(formPersonId);
var formPersonSubmit = document.getElementById(formPersonSubmitId);

var formFilmographyPopup = document.getElementById('modal-add-filmography');

var inputPicture = formPerson.querySelector("input[type='file']");
var imgPoster = document.querySelector(".person-picture img");
var imgUriInput = document.querySelector(".person-picture input[id='ImageUri']");


//** ONLOAD ACTIONS **//


// add submit filmography events

addFormAddSubmitFilmography(function (divFilmography) {
    divFilmography.classList.add('add-filmography');
    divFilmography.classList.add('col');
    blockFilmography.append(divFilmography);
}, false);


// change photo

inputPicture.addEventListener('change', function (e) {

    const [file] = e.target.files;
    if (file) {
        imgPoster.classList.remove('img-placeholder');
        imgPoster.src = URL.createObjectURL(file);
        imgUriInput.value = imgPoster.src;
    }

});





