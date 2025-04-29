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