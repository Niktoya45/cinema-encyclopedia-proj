import {
    fetchPostMVC,
    addShowCloseEvents
} from "./utils.js";


//** ONLOAD ACTIONS **/

// hide and show close buttons
addShowCloseEvents();

// hide deleted record and callback

var records = document.querySelectorAll("tbody .record");

records.forEach(function (record) {

    let delButton = record.querySelector('.del-record');

    delButton.addEventListener('click', function (e) {
        e.preventDefault();

        let action = delButton.formAction;

        fetchPostMVC(null, action, "TEXT")
            .then((result) => {
                record.classList.add('no-display');
            });
    })
});

