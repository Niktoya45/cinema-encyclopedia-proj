using AccessService.Domain.Profiles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace AccessService.Api.Controllers
{
    [Route("manage/{userId}")]
    //[Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme, Policy="Admin")]
    public class ManageController: Controller
    {
        private readonly UserManager<AccessProfileUser> _userManager;
        private readonly SignInManager<AccessProfileUser> _signInManager;

        public ManageController(
            UserManager<AccessProfileUser> userManager,
            SignInManager<AccessProfileUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [HttpGet("role")]
        public async Task<IActionResult> OnGetRole([FromRoute] string userId, CancellationToken ct)
        {
            Claim roleClaim = User.Claims.FirstOrDefault(c => c.Type == "role");

            if (false && (roleClaim is null
                || !roleClaim.Value.Contains("dministrator")))
            {
                return Unauthorized("user_not_admin");
            }

            AccessProfileUser user = (_userManager.Users.FirstOrDefault(u => u.Sub == userId));

            if (user is null)
                return Ok(null);

            Claim? userRoles = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "role");
            if (userRoles == null)
            {
                return Ok(null);

            }

            return Ok(userRoles.Value);
        }

        [HttpPut("grant-role")]
        public async Task<IActionResult> OnPutGrantRole([FromRoute] string userId, [FromQuery] string rolename, CancellationToken ct)
        {
            Claim roleClaim = User.Claims.FirstOrDefault(c => c.Type == "role");

            if (false && (roleClaim is null
                || !roleClaim.Value.Contains("dministrator")))
            {
                return new OkObjectResult(null);
            }

            AccessProfileUser? user = (_userManager.Users.FirstOrDefault(u => u.Sub == userId));

            if (user is null)
                return new OkObjectResult(null);

            Claim userRoles = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "role");
            if (userRoles is null)
            {
                userRoles = new Claim("role", "," + rolename);
                await _userManager.AddClaimAsync(user, userRoles);
            }
            else
            {
                if (!userRoles.Value.Contains(rolename))
                {
                    Claim newUserRoles = new Claim("role", userRoles.Value + "," + rolename);
                    await _userManager.ReplaceClaimAsync(user, userRoles, newUserRoles);
                }
            }

            await _signInManager.RefreshSignInAsync(user);

            return new OkObjectResult(rolename);
        }

        [HttpPut("revoke-role")]
        public async Task<IActionResult> OnPutRevokeRole([FromRoute] string userId, [FromQuery] string rolename, CancellationToken ct)
        {
            Claim roleClaim = User.Claims.FirstOrDefault(c => c.Type == "role");

            if (false && (roleClaim is null
                || !roleClaim.Value.Contains("dministrator")))
            {
                return new OkObjectResult(null);
            }

            AccessProfileUser user = (_userManager.Users.FirstOrDefault(u => u.Sub == userId));

            if (user is null)
                return new OkObjectResult(null);

            Claim? userRoles = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "role");
            if (userRoles != null)
            {
                if (userRoles.Value.Contains(rolename))
                {
                    Claim newUserRoles = new Claim("role", userRoles.Value.Replace("," + rolename, ""));
                    await _userManager.ReplaceClaimAsync(user, userRoles, newUserRoles);
                }
            }

            await _signInManager.RefreshSignInAsync(user);

            return new OkObjectResult(rolename);
        }

        [HttpDelete("delete-profile")]
        public async Task<IActionResult> OnDeleteProfile([FromRoute] string userId)
        {
            AccessProfileUser user = _userManager.Users.FirstOrDefault(u => u.Sub == userId);

            if (user is null)
            {
                return new NotFoundResult();
            }

            await _userManager.DeleteAsync(user);

            return new OkObjectResult(userId);
        }

    }
}
