
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using AccessService.Domain.Profiles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace AccessService.Api.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AccessProfileUser> _signInManager;
        private readonly UserManager<AccessProfileUser> _userManager;
        private readonly IUserStore<AccessProfileUser> _userStore;
        private readonly IUserClaimStore<AccessProfileUser> _userClaimStore;
        private readonly IUserRoleStore<AccessProfileUser> _userRoleStore;
        private readonly IUserEmailStore<AccessProfileUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<AccessProfileUser> userManager,
            IUserStore<AccessProfileUser> userStore,
            IUserClaimStore<AccessProfileUser> userClaimStore,
            SignInManager<AccessProfileUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userManager.Options.SignIn.RequireConfirmedAccount = false;
            _userStore = userStore;
            _userClaimStore = userClaimStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }


        [BindProperty]
        public InputModel Input { get; set; }


        public string ReturnUrl { get; set; }


        public IList<AuthenticationScheme> ExternalLogins { get; set; }


        public class InputModel
        {

            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }


            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Username, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);

                    //await _userManager.AddClaimAsync(user, new Claim("sub", ""));
                    await _userManager.AddClaimAsync(user, new Claim("role", AccessUserRoles.User.ToString()));

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public async Task<IActionResult> OnPostAddRole([FromForm] string userSubjectId, [FromForm] string roleName, CancellationToken ct)
        {
            Claim? roleClaim = User.Claims.FirstOrDefault(c => c.Type == "role");

            if (roleClaim is null 
                || !(roleClaim.Value.Contains(AccessUserRoles.Administrator.ToString()) || roleClaim.Value.Contains(AccessUserRoles.Superadministrator.ToString()) ))
            {
                return new OkObjectResult(null);
            }

            AccessProfileUser? user = (await _userClaimStore.GetUsersForClaimAsync(new Claim("sub", userSubjectId), ct)).FirstOrDefault();

            if (user is null)
                return new OkObjectResult(null);

            Claim? userRoles = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "role");
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

            return new OkObjectResult(roleName);
        }

        public async Task<IActionResult> OnPostRemoveRole([FromForm] string userSubjectId, [FromForm] string roleName, CancellationToken ct)
        {
            Claim? roleClaim = User.Claims.FirstOrDefault(c => c.Type == "role");

            if (roleClaim is null
                || !(roleClaim.Value.Contains(AccessUserRoles.Administrator.ToString()) || roleClaim.Value.Contains(AccessUserRoles.Superadministrator.ToString())))
            {
                return new OkObjectResult(null);
            }

            AccessProfileUser? user = (await _userClaimStore.GetUsersForClaimAsync(new Claim("sub", userSubjectId), ct)).FirstOrDefault();

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

            return new OkObjectResult(roleName);
        }

        private AccessProfileUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AccessProfileUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(AccessProfileUser)}'. " +
                    $"Ensure that '{nameof(AccessProfileUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<AccessProfileUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<AccessProfileUser>)_userStore;
        }
    }
}
