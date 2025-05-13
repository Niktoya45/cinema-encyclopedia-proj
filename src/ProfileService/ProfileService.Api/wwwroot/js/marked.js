
// hide deleted record and callback

let deleteRecords = document.querySelectorAll("tbody tr")

deleteRecords.forEach(function (record) {

    let delButton = record.querySelector('.del-record');

    delButton.addEventListener('click', function (e) {

        e.preventDefault();

        record.classList.add('hide-delete');

        let targetAction = e.target.formAction;

        fetch(targetAction,
            {
                method: "POST"
            }).then((response) => {

                return response.text();

            }).then((text) => {

                console.log(text);
            });
    })
});

