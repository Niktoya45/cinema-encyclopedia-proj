import { addFormAddSubmitFilmography } from './filmography_events.js';

//** ELEMENTS USED **/


const blockFilmographyId = "filmography";

const formStudioId = 'form-add-studio';
const formStudioSubmitId = 'submit-' + formStudioId;

var blockFilmography = document.getElementById(blockFilmographyId);

var formStudio = document.getElementById(formStudioId);
var formStudioSubmit = document.getElementById(formStudioSubmitId);

var formFilmographyPopup = document.getElementById('modal-add-filmography');

var inputPicture = formStudio.querySelector("input[type='file']");
var imgPoster = document.querySelector(".studio-logo img");
var imgUriInput = document.querySelector(".studio-logo input[id='ImageUri']");


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





