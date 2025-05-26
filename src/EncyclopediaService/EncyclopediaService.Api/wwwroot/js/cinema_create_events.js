// used elements

formCinema   = document.getElementById("form-add-cinema");
formStudio   = document.getElementById("form-add-studio");
formStarring = document.getElementById("form-add-starring");

formStudioName = formStudio.querySelector("#search-studio");
formStarringName = formStarring.querySelector("#search-starring");

formStudioPopup = document.getElementById('modal-add-studio');
formStarringPopup = document.getElementById('modal-add-starring');

var studios = document.getElementById("studios");
const studio_class = "add-studio";
const studio_prefix = "studio-";

var starrings = document.getElementById("starrings");
const starring_class = "add-starring";
const starring_prefix = "starring-";

const col_class = "col";


getFormSubmitButton = function (form) {
    return document.getElementById("submit-" + form.id);
}

deleteRecord = function (deleteButton, prefix) {
    let record = document.getElementById(prefix + deleteButton.value);

    record.remove();
}

addStarringDropdownEvents = function (span) {

    let deleteButton = span.querySelector(".dropdown-delete-starring");

    deleteButton.addEventListener('click', function (e) {
        deleteRecord(deleteButton, starring_prefix);
    });

}

addStudioDropdownEvents = function (span) {

    let deleteButton = span.querySelector(".dropdown-delete-studio");

    deleteButton.addEventListener('click', function (e) {
        deleteRecord(deleteButton, studio_prefix);
    });

}

updateFormStarringDependency = function (form) {
    var jobsCheckbox = form.querySelector("#starring-jobs_8");

    const isActor = jobsCheckbox.checked;
    var inputsJobsDependant = form.querySelectorAll(".jobs-dependant");

    if (!isActor) {
        inputsJobsDependant.forEach(function (input) {
            input.disabled = true;
        });
    }

    jobsCheckbox.addEventListener('click', function (e) {
        inputsJobsDependant.forEach(function (input) {
            input.disabled = !jobsCheckbox.checked;
        });
    });
}


formStarringName.addEventListener('change', function (e) {

    let checkBoxes = formStarring.querySelectorAll('.check.name-dependant');

    checkBoxes.forEach(function (checkbox) {
        checkbox.disabled = (formStarringName.value ? false : true);
    });

    updateFormStarringDependency(formStarring);

});

// change photo

var pictureInput = formCinema.querySelector("input[type='file']");
var picture = formCinema.querySelector("img");

pictureInput.addEventListener('change', function (e) {

    const [file] = e.target.files;
    if (file) {
        picture.src = URL.createObjectURL(file);
    }

});


// handle new starring form redraw

formStarringPopup.addEventListener('show.bs.modal', function (e) {

    // button which has activated the event
    // var button = e.relatedTarget;

});

// add new starring

submitStarring = getFormSubmitButton(formStarring);

submitStarring.addEventListener('click', function (e) {

    if (!formStarringName.value){
        return;
    }

    e.preventDefault();

    var formElement = formStarring;
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

            let spanStarring = document.createElement("div");
            spanStarring.classList.add(starring_class);
            spanStarring.classList.add(col_class);

            spanStarring.innerHTML = result;
            spanStarring.id = starring_prefix + spanStarring.querySelector("input[name$='Index']").value;

            addStarringDropdownEvents(spanStarring);

            starrings.append(spanStarring);
        });
});


// handle new studio form redraw

formStudioPopup.addEventListener('show.bs.modal', function (e) {

    // button which has activated the event
    // var button = e.relatedTarget;

});


// add new studio

submitStudio = getFormSubmitButton(formStudio);

submitStudio.addEventListener('click', function (e) {

    if (!formStudioName.value) {
        return;
    }

    e.preventDefault();

    var formElement = formStudio;
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

            let spanStudio = document.createElement("div");
            spanStudio.classList.add(studio_class);
            spanStudio.classList.add(col_class);

            spanStudio.innerHTML = result;
            spanStudio.id = studio_prefix + spanStudio.querySelector("input[name$='Index']").value;

            addStudioDropdownEvents(spanStudio);

            studios.append(spanStudio);
        });
});
