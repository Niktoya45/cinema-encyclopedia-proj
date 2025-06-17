

const deleteAttribute = 'target-delete';
const clickEvent = new Event("click");

export function fetchForm(form, actionAdd, actionRefreshForm, modalForm) {

    fetchPostMVC(form, form.action, "TEXT")
        .then((result) => {

            let placeholder = document.createElement("div");

            placeholder.innerHTML = result;

            if (getPartialType(placeholder) == "form") {

                form.innerHTML = result;
                placeholder.remove();

                actionRefreshForm(form);

                return;
            }

            actionAdd(placeholder);

            if (modalForm) {
                let modal = modalForm;
                bsElementClose(modal);
            }

        });
}

export function fetchPostMVC(form, action, responseType) {

    var formElement = form;
    const formData = new URLSearchParams();

    for (const kv of new FormData(formElement)) {
        formData.append(kv[0], kv[1]);
    }

    return fetch(action,
        {
            method: "POST",
            body: formData
        })
        .then((response) => {

            if (responseType == "TEXT") {
                return response.text();
            }

            else if (responseType == "JSON") {
                return response.json();
            }

            return null;
        })
}

export function fetchGetMVC(action, responseType) {

    return fetch(action,
        {
            method: "GET"
        })
        .then((response) => {

            if (responseType == "TEXT") {
                return response.text();
            }

            else if (responseType == "JSON") {
                return response.json();
            }

            return null;
        })
}


export function bsElementClose(element) {

    element.querySelector("[data-bs-dismiss]").dispatchEvent(clickEvent);
}

export function getHidden(element, selectorId) {
    return element.querySelector("input[type='hidden']#" + selectorId);
}

export function getPartialType(partialResultPlaceholder) {
    return partialResultPlaceholder.querySelector("input[type='hidden']#partial").value;
}

export function addDeleteEvents(formDelete, resultAction) {
    let formDeleteSubmits = formDelete.querySelectorAll("button");
    formDeleteSubmits.forEach(function (button) {

        button.addEventListener('click', function (e) {

            e.preventDefault();

            if (button.value == "cancel") {
                return;
            }

            let form = formDelete;

            fetchPostMVC(form, form.action, "TEXT")
                .then((result) => {
                    resultAction(result, button.getAttribute(deleteAttribute));
                });

        });

    });
}

export function approveDelete(targetId, modalContainer, deleteActionPath, deleteClass) {
    let form = modalContainer.querySelector("form");

    let labels = modalContainer.querySelectorAll(".label-delete");

    labels.forEach(function (label) {

        if (label.classList.contains(deleteClass)) {

            label.classList.remove('no-display');
        } else {
            label.classList.add('no-display');
        }

    });

    form.action = deleteActionPath;
    form.querySelectorAll('button').forEach(function (button) {
        button.setAttribute(deleteAttribute, deleteClass);
    });
    getHidden(form, "RecordId").value = targetId;
}

export function addSearchEvents(searchDropdown, action, chooseAction, closeAction) {

    let searchInput = searchDropdown.querySelector(".search-input");

    searchInput.addEventListener("input", function (e) {
        let inputValue = this.value.toLowerCase();
        let suggestionsList = searchDropdown.querySelector(".search-suggestions");

        console.log(inputValue);

        if (inputValue.length < 3) {
            if (closeAction) {
                closeAction(searchInput);
            }

            suggestionsList.style.visibility = "hidden";

            return;
        }

        suggestionsList.innerHTML = "";
        suggestionsList.style.visibility = "visible";

        if (inputValue) {

            // handle data transfer using 'action' parameter instead of below

            const choices = ["Actor 1", "Actor 2", "Director 3", "Director 4", "Scenarist 5", "Scenarist 6", "Producer 7", "Producer 8"];

            let filteredChoices = choices.filter(choice => choice.toLowerCase().includes(inputValue));

            filteredChoices.forEach(choice => {
                let listItem = document.createElement("li");
                listItem.textContent = choice;
                listItem.classList.add("list-group-item");
                listItem.addEventListener("click", function (e) {

                    if (chooseAction) {
                        chooseAction(searchInput, listItem);
                    }

                    searchInput.value = listItem.textContent;
                    suggestionsList.innerHTML = "";
                    suggestionsList.style.visibility = "hidden";

                });
                suggestionsList.appendChild(listItem);
            });
        } else {
            inputsNameDependant.forEach(function (input) {
                input.disabled = true;
            });
        }
    });

}

export function appendToCarousel(carousel, card, mr, mc) {
    let max = mr * mc;
    let lastPageItem = carousel.querySelector(".carousel-item:last-child");

    carousel.querySelector(".carousel-item.active").classList.remove("active");

    let lastCount = lastPageItem.querySelectorAll(".carousel-card").length;
    let page;

    if (lastCount == 0 || lastCount == max) {

        let inner = carousel.querySelector(".carousel-inner");

        let newPageItem = document.createElement("div");
        newPageItem.classList.add("carousel-item");
        newPageItem.classList.add("active");

        page = document.createElement("div");
        page.classList.add("row");
        page.classList.add("row-cols-" + mc);
        page.classList.add("carousel-page");

        newPageItem.append(page);
        inner.append(newPageItem);

        if (lastCount != 0) {
            carousel.querySelectorAll(".carousel-button").forEach(function (button) {
                button.classList.remove("no-display");
            });
        }

    } else {
        lastPageItem.classList.add("active");
        page = lastPageItem.querySelector(".carousel-page");
    }

    card.classList.add("col");
    card.classList.add("carousel-card");

    page.append(card);
}

export function removeFromCarousel(carousel, cardId, mr, mc) {
    let selector = "div[id='" + cardId + "']";

    let nextCard = carousel.querySelector(selector);

    let pages = carousel.querySelectorAll(".carousel-page:has(" + selector + "), .carousel-item:has(" + selector + ") ~ .carousel-item > .carousel-page");

    nextCard.remove();

    for (let i = 0; i < pages.length - 1; i++) {
        nextCard = pages[i + 1].querySelector(".carousel-card:first-child");
        pages[i + 1].removeChild(nextCard);
        pages[i].append(nextCard);
    }

    let lastCount = pages[pages.length - 1].querySelectorAll(".carousel-card").length;

    if (lastCount == 0) {

        if (pages.length == 1) {
            carousel.querySelector(".carousel-item:has(+ .carousel-item:last-child)").classList.add("active");
        }

        carousel.querySelector(".carousel-item:last-child").remove();

        carousel.querySelectorAll(".carousel-button").forEach(function (button) {
            button.classList.add("no-display");
        });
    }

}