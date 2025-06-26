import { addFormAddSubmitStarring } from './starring_events.js';
import { addFormAddSubmitStudio } from './production_studio_events.js';

//** ELEMENTS USED **/


const blockStarringsId = "starrings";
const blockStudiosId   = "studios";

const formCinemaId = 'form-add-cinema';
const formCinemaSubmitId = 'submit-' + formCinemaId;

var blockStarrings = document.getElementById(blockStarringsId);
var blockStudios = document.getElementById(blockStudiosId);

var formCinema = document.getElementById(formCinemaId);
var formCinemaSubmit = document.getElementById(formCinemaSubmitId);

var formStudioPopup = document.getElementById('modal-add-studio');
var formStarringPopup = document.getElementById('modal-add-starring');

var inputPicture = formCinema.querySelector("input[type='file']");
var imgPoster = document.querySelector(".cinema-poster img");
var imgUriInput = document.querySelector(".cinema-poster input[id='ImageUri']");


//** ONLOAD ACTIONS **//


// add submit starring events

addFormAddSubmitStarring(function (divStarring) {
    divStarring.classList.add('add-starring');
    divStarring.classList.add('col');
    blockStarrings.append(divStarring);
}, false);


// add submit studio events

addFormAddSubmitStudio(function (divStudio) {
    divStudio.classList.add('add-studio');
    divStudio.classList.add('col');
    blockStudios.append(divStudio);
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





