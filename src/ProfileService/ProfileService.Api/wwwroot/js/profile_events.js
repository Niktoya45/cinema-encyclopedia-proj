import {
    fetchPostMVC, fetchForm,
    addShowEditorEvents, addMainPictureEditEvents,
    addConfirmPopupEvents
} from './utils.js';


//** ELEMENTS USED **//

const formPictureId = 'form-edit-picture';
const formPictureSubmitId = 'submit-' + formPictureId;
const formEditMainId = 'form-edit-main';
const formEditMainSubmitId = 'submit-' + formEditMainId;
const formManageUserId = 'form-manage-profile';

const buttonDeleteProfileId = 'manage-delete-profile';
const buttonManageRoleId = 'manage-profile-role';

const deleteProfileAction = window.location.pathname + "?handler=deleteprofile";
const grantRoleAction  = window.location.pathname + "?handler=addAdminRole";
const revokeRoleAction = window.location.pathname + "?handler=revokeAdminRole";

var formPicture = document.getElementById(formPictureId);
var formPictureSubmit = document.getElementById(formPictureSubmitId);
var formEditMain = document.getElementById(formEditMainId);
var formEditMainSubmit = document.getElementById(formEditMainSubmitId);
var formManageUser = document.getElementById(formManageUserId);

var buttonDeleteProfile = document.getElementById(buttonDeleteProfileId);
var buttonManageRole = document.getElementById(buttonManageRoleId);

var imgPicture = document.querySelector('.profile-picture img');

const IsUser = !!formEditMain;
const IsSuper = !formEditMain && !!formManageUser;


//** ONLOAD ACTIONS **//

if (IsUser || IsSuper)
{
    // show confirm popup before calling an action
    addConfirmPopupEvents(buttonDeleteProfile, deleteProfileAction, null, (t, r) => { alert('deleted ' + r); });
}

if (IsUser)
{
    // show and hide editor options
    addShowEditorEvents();

    // add picture change events
    addMainPictureEditEvents(formPicture, formPictureSubmit, imgPicture);

    // add profile edit form submission
    formEditMainSubmit.addEventListener('click', function (e) {
        e.preventDefault();

        this.disabled = true;

        let form = formEditMain;

        fetchForm(
            form,
            (p) => { window.location.reload(); },
            null,
            null
        );

    });
}

if (IsSuper && !IsUser)
{
    addConfirmPopupEvents(
        buttonManageRole,
        null,
        (t, f) => {
            if (t.getAttribute('manage-role') == 'grant') f.action = grantRoleAction;
            else if (t.getAttribute('manage-role') == 'revoke') f.action = revokeRoleAction;
        },
        (t, r) => {
            alert('Role was changed for this profile. ' + r);

            if (t.getAttribute('manage-role') == 'grant') {
                t.setAttribute('manage-role', 'revoke');
                t.setAttribute('target-label', '#label-revoke-role');
                t.innerText = 'Revoke Admin'
            }
            else if (t.getAttribute('manage-role') == 'revoke') {
                t.setAttribute('manage-role', 'grant');
                t.setAttribute('target-label', '#label-grant-role');
                t.innerText = 'Grant Admin'
            }
        }
    );
}
