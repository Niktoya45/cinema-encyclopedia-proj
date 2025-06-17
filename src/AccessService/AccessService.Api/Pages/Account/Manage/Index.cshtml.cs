// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using AccessService.Domain.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AccessService.Api.Pages.Account.Manage
{
    [Authorize(Policy = "Authenticated")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<AccessProfileUser> _userManager;
        private readonly SignInManager<AccessProfileUser> _signInManager;

        public IndexModel(
            UserManager<AccessProfileUser> userManager,
            SignInManager<AccessProfileUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        private async Task LoadAsync(AccessProfileUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (_userManager.GetUserId(User) == null)
            {
                return RedirectToPage("../Login", new { ReturnUrl = HttpContext.Request.Path } );
            }

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);


            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddRole([FromBody] string userSubjectId, [FromBody] string roleName, CancellationToken ct)
        {
            Claim roleClaim = User.Claims.FirstOrDefault(c => c.Type == "role");

            if (roleClaim is null
                || !(roleClaim.Value.Contains(AccessUserRoles.Administrator.ToString()) || roleClaim.Value.Contains(AccessUserRoles.Superadministrator.ToString())))
            {
                return new OkObjectResult(null);
            }

            AccessProfileUser? user = (_userManager.Users.FirstOrDefault(u => u.Sub == userSubjectId));

            if (user is null)
                return new OkObjectResult(null);

            Claim userRoles = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "role");
            if (userRoles is null)
            {
                userRoles = new Claim("role", "," + roleName);
                await _userManager.AddClaimAsync(user, userRoles);
            }
            else
            {
                Claim newUserRoles = new Claim("role", userRoles.Value + "," + roleName);
                await _userManager.ReplaceClaimAsync(user, userRoles, newUserRoles);
            }

            await _signInManager.RefreshSignInAsync(user);

            return new OkObjectResult(roleName);
        }

        public async Task<IActionResult> OnPostRemoveRole([FromBody] string userSubjectId, [FromBody] string roleName, CancellationToken ct)
        {
            Claim roleClaim = User.Claims.FirstOrDefault(c => c.Type == "role");

            if (roleClaim is null
                || !(roleClaim.Value.Contains(AccessUserRoles.Administrator.ToString()) || roleClaim.Value.Contains(AccessUserRoles.Superadministrator.ToString())))
            {
                return new OkObjectResult(null);
            }

            AccessProfileUser user = (_userManager.Users.FirstOrDefault(u => u.Sub == userSubjectId));

            if (user is null)
                return new OkObjectResult(null);

            Claim? userRoles = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "role");
            if (userRoles != null)
            {
                if (userRoles.Value.Contains(roleName))
                {
                    Claim newUserRoles = new Claim("role", userRoles.Value.Replace("," + roleName, ""));
                    await _userManager.ReplaceClaimAsync(user, userRoles, newUserRoles);
                }

            }

            await _signInManager.RefreshSignInAsync(user);

            return new OkObjectResult(roleName);
        }

        public async Task<IActionResult> OnPostDeleteAccount([FromBody] string userSubjectId) 
        {
            AccessProfileUser user = _userManager.Users.FirstOrDefault(u => u.Sub == userSubjectId);

            if (user is null)
            {
                return new NotFoundResult();
            }

            await _userManager.DeleteAsync(user);

            return new OkObjectResult(userSubjectId);
        }
    }
}
