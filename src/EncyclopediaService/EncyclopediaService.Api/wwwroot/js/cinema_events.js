﻿
updateFormEditStarring = function (ref) {

    ref.addEventListener('click', function (e) {

        e.preventDefault();

        var formElement = document.getElementById("update-" + e.currentTarget.id);
        const formData = new URLSearchParams();

        for (const kv of new FormData(formElement)) {
            formData.append(kv[0], kv[1]);
        }

        fetch(formElement.action,
            {
                method: "POST",
                body: formData
            })
            .then((response) => {

                return response.text();
            })
            .then((result) => {

                var content = document.getElementById('form-edit-starring');

                content.innerHTML = result;

                updateFormStarringDependency(content);
            });

    });

}

updateFormStarringDependency = function (form) {
    var editJobsCheckbox = form.querySelector("#starring-jobs_8");

    const isActor = editJobsCheckbox.checked;
    var inputsJobsDependant = form.querySelectorAll(".jobs-dependant");

    if (!isActor) {
        inputsJobsDependant.forEach(function (input) {
            input.disabled = true;
        });
    }

    editJobsCheckbox.addEventListener('click', function (e) {
        inputsJobsDependant.forEach(function (input) {
            input.disabled = !jobsCheckbox.checked;
        });
    });
}


// show close buttons

var refs = document.querySelectorAll(".dropdown-menu .show-close-buttons");

refs.forEach(ref => {
    var closeButtons = document.querySelectorAll("div.position-relative button[id^='" + ref.id + "']");
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

// disable | enable fields which depend on jobs field

var addJobsCheckbox = document.querySelector("#form-add-starring #starring-jobs_8");

addJobsCheckbox.addEventListener('click', function (e) {
    var inputsJobsDependant = document.querySelectorAll("#form-add-starring .jobs-dependant");

    inputsJobsDependant.forEach(function (input) {
        input.disabled = !e.currentTarget.checked;
    });
});


// update content of starring edit partial

var refsEditStarring = document.querySelectorAll(".dropdown-menu .dropdown-edit-starring");

refsEditStarring.forEach(function (ref) {

    updateFormEditStarring(ref);
});

// display hint choices forcstarring search field

const choices = ["Actor 1", "Actor 2", "Director 3", "Director 4", "Scenarist 5", "Scenarist 6", "Producer 7", "Producer 8"];

var searchStarring = document.getElementById("search-starring");

searchStarring.addEventListener("input", function (e) {
    let inputValue = this.value.toLowerCase();
    let suggestionsList = document.getElementById("suggestions-starring");
    var inputsNameDependant = document.querySelectorAll("#form-add-starring .name-dependant")

    suggestionsList.innerHTML = "";
    suggestionsList.style.visibility = "visible";

    if (inputValue) {

        // handle data transfer instead of below
        let filteredChoices = choices.filter(choice => choice.toLowerCase().includes(inputValue));

        filteredChoices.forEach(choice => {
            let listItem = document.createElement("li");
            listItem.textContent = choice;
            listItem.classList.add("list-group-item");
            listItem.addEventListener("click", function (e) {
                searchStarring.value = this.textContent;
                suggestionsList.innerHTML = "";
                suggestionsList.style.visibility = "hidden";

                inputsNameDependant.forEach(function (input) {
                    input.disabled = input.classList.contains("jobs-dependant");
                });
            });
            suggestionsList.appendChild(listItem);
        });
    } else {
        inputsNameDependant.forEach(function (input) {
            input.disabled = true;
        });
    }
});



// make user rating change

var starsCollapse = document.getElementById("collapse-stars");
var starsClick = starsCollapse.querySelectorAll("button.rail");
var stars = Array.from(starsClick).map(starClick => starClick.querySelector(".star"));


for (let i = 0; i < stars.length; i++) {

    starsClick.item(i).addEventListener('click', function (e) {
 
        for (let j = 0; j < stars.length; j++) {
            if (j < i) {
                stars[j].classList.remove('rail');
            }
            else if (j > i) {
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

    });
}
