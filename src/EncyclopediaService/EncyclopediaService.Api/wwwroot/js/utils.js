

const deleteAttribute = 'target-delete';
const clickEvent = new Event("click");

var controllerCancel = new AbortController();
var signalCancel = controllerCancel.signal;

export function refreshRequestCancel() {
    controllerCancel = new AbortController();
    signalCancel = controllerCancel.signal;
}

export function fetchForm(form, actionAdd, actionRefreshForm, modalForm) {

    fetchPostMVC(form, form.action, "TEXT")
        .then((result) => {
            if (!result) return null;

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

        })
        .catch(error => {
            if (error.name === 'AbortError') {
                console.log('request cancelled');
            }
            return null;
        });
}

export function fetchPostMVC(form, action, responseType) {

    var formElement = form;
    const formData = formElement instanceof HTMLFormElement ? new FormData(formElement) : formElement;

    return fetch(action,
        {
            method: "POST",
            body: formData,
            signal: signalCancel
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
        .catch(error => {
            if (error.name === 'AbortError') {
                console.log('request cancelled');
                return null;
            }
        });
}

export function fetchGetMVC(action, responseType) {

    return fetch(action,
        {
            method: "GET",
            signal: signalCancel
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
        .catch(error => {
            if (error.name === 'AbortError') {
                console.log('request cancelled');
                return null;
            }
        });
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

export function addShowCloseEvents() {

    var classCloseToggler = "show-close-buttons";
    var classCloseTogglerArea = "hide-delete";
    var classCloseToggledElement = "del-record";
    var classCloseToggledHide = "hide-close-buttons";

    addShowToggle(
        classCloseToggler,
        classCloseTogglerArea,
        classCloseToggledElement,
        classCloseToggledHide
    );
    
}

export function addShowEditorEvents() {

    var classEditorToggler = "show-editor-options";
    var classEditorTogglerArea = null;
    var classEditorToggledElement = "editor-toggle";
    var classEditorToggledHide = "hide-editor-options";

    addShowToggle(
        classEditorToggler,
        classEditorTogglerArea,
        classEditorToggledElement,
        classEditorToggledHide
    );
    
}

export function addShowToggle(classToggler, classToggledArea, classToggledElement, classToggledHide) {

    var refTogglers = document.querySelectorAll('.' + classToggler);

    let showTogglerAttribute = "show-toggled-by";
    let hideTogglerAttribute = "hide-show-toggled-by";
    
    refTogglers.forEach(ref => {

        let showTogglerSelector = ref.id ? `[${showTogglerAttribute}='${ref.id}']` : "";
        let hideTogglerSelector = ref.id ? `[${hideTogglerAttribute}='${ref.id}']` : "";

        var closeButtons = document.querySelectorAll(
            classToggledArea ?
            '.' + classToggledArea + showTogglerSelector + " ."  + classToggledElement
            : '.' + classToggledElement + showTogglerSelector

        );
        var cancels = document.querySelectorAll(
            classToggledHide ?
            '.' + classToggledHide + hideTogglerSelector
            : hideTogglerSelector
        );

        ref.addEventListener('click', (e) => {
            
            closeButtons.forEach((b) => {
                b.style.visibility = 'visible';
            });
            cancels.forEach((c) => {
                c.style.visibility = 'visible';
            })
        });

        cancels.forEach((c) => {

            c.addEventListener('click', (e) => {

                closeButtons.forEach((b) => {
                    b.style.visibility = 'hidden';
                });
                cancels.forEach((c) => {
                    c.style.visibility = 'hidden';
                })
            });
        })
    });
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
                    if (!result) return null;

                    resultAction(result, button.getAttribute(deleteAttribute));
                })
                .catch(error => {
                    if (error.name === 'AbortError') {
                        console.log('request cancelled');
                        return null;
                    }
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

export function addSearchChoice(formAdd, actionSearch, searchInput, listItem, choice, afterSetAction) {

    fetchPostMVC(new URLSearchParams({ recordId : choice.id }), actionSearch, "JSON")
    .then((result) => {
        if (!result) return;

        let choiceRes = result;

        getHidden(formAdd, "Id").value = choiceRes.id;
        getHidden(formAdd, "Name").value = choiceRes.name;
        getHidden(formAdd, "Picture").value = choiceRes.picture;
        getHidden(formAdd, "PictureUri").value = choiceRes.pictureUri;
        let img = formAdd.querySelector("img");

        if (choiceRes.pictureUri) {
            img.classList.remove('img-placeholder');
            img.src = choiceRes.pictureUri;
        }

        if (afterSetAction) {
            afterSetAction();
        }
    });

}

export function addSearchClose(formAdd, searchInput, afterCloseAction) {
    getHidden(formAdd, "Id").value = null;
    getHidden(formAdd, "Name").value = null;
    getHidden(formAdd, "Picture").value = null;
    getHidden(formAdd, "PictureUri").value = null;
    let img = formAdd.querySelector("img");
    img.src = null;
    img.classList.add('img-placeholder');

    if (afterCloseAction) {
        afterCloseAction();
    }
}

export function addSearchEvents(searchDropdown, action, chooseAction, closeAction) {

    let searchInput = searchDropdown.querySelector(".search-input");

    searchInput.addEventListener("input", function (e) {
        let inputValue = this.value.toLowerCase();
        let suggestionsList = searchDropdown.querySelector(".search-suggestions");

        console.log(inputValue);
        controllerCancel.abort();
        refreshRequestCancel();

        if (inputValue.length < 3) {
            if (closeAction) {
                closeAction(searchInput);
            }
            suggestionsList.style.visibility = "hidden";
            return;
        }

        if (inputValue) {

            fetchPostMVC(new URLSearchParams({ search: inputValue }), action, "JSON")
                .then((result) => {
                    console.log(result);

                    let filteredChoices = result;

                    suggestionsList.innerHTML = "";
                    if (!filteredChoices || filteredChoices.length == 0) {
                        suggestionsList.style.visibility = "hidden";
                        return;
                    }
                    suggestionsList.style.visibility = "visible";

                    filteredChoices.forEach(choice => {
                        console.log(choice);

                        let listItem = document.createElement("li");
                        let listItemImg = document.createElement("img");
                        let listItemSpan = document.createElement("span");

                        listItemImg.src = choice.pictureUri;
                        if (!choice.pictureUri) {
                            listItemImg.classList.add("img-placeholder");
                        }
                        listItemSpan.textContent = choice.name;

                        listItem.append(listItemImg);
                        listItem.append(listItemSpan);
                        listItem.classList.add("list-group-item");
                        listItem.addEventListener("click", function (e) {

                            searchInput.value = choice.name;
                            suggestionsList.innerHTML = "";
                            suggestionsList.style.visibility = "hidden";

                            if (chooseAction) {
                                console.log("choice action");
                                chooseAction(searchInput, listItem, choice);
                            }

                        });
                        suggestionsList.appendChild(listItem);
                    });

                })
                .catch(error => {
                    if (error.name === 'AbortError') {
                        console.log('request cancelled');
                    }
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