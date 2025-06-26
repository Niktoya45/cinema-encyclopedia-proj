using EncyclopediaService.Api.Extensions;
using EncyclopediaService.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Net;
using System.Security.Claims;

namespace EncyclopediaService.Api.Views
{
    public class IndexModel : PageModel
    {
        const string _baseGateway = "https://localhost:7176";

        HttpClient _gatewayService;

        public IndexModel()
        {
            _gatewayService = new HttpClient();
            _gatewayService.BaseAddress = new Uri(_baseGateway);
        }

        public async Task<IActionResult> OnGet()
        {
            return Redirect("encyclopedia/cinemas/all/index");
        }

        public async Task<IActionResult> OnGetAuthorize([FromQuery] string redirect, [FromQuery] string key)
        {
            if (!User.IsLoggedIn())
            {
                var response = await _gatewayService.GetAsync("/encyclopedia/authorize" + $"?code={key}");

                if (response.IsSuccessStatusCode)
                {
                    string? atoken = await response.Content.ReadAsStringAsync();

                    if (atoken != null)
                    {
                        JsonWebTokenHandler tokenHandler = new JsonWebTokenHandler();

                        JsonWebToken token = tokenHandler.ReadJsonWebToken(atoken);

                        var claims = token.Claims;

                        ClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "cookie"));

                        HttpContext.User = principal;

                        await principal.SignInByCookies(HttpContext, true);
                    }
                }
            }

            return redirect is null ? new OkObjectResult(User.IsLoggedIn()) : User.IsLoggedIn() ? Redirect(redirect) : await OnGet();
        }

        public async Task<IActionResult> OnGetProfile([FromQuery] string id, [FromQuery] string? path = null)
        {
            if (!User.IsLoggedIn())
            {
                return await OnGetLogin(Request.GetDisplayUrl());
            }

            if (id == String.Empty)
            {
                return Redirect(_baseGateway + $"/profile/{User.FindFirst(JwtRegisteredClaimNames.Sub)}{path ?? ""}");
            }

            return Redirect(_baseGateway + $"/profile/{id}{path ?? ""}");
        }

        public async Task<IActionResult> OnGetLogin([FromQuery] string redirect)
        {
            if (!User.IsLoggedIn())
            {
                string query = Url.ActionLink(values: new { callback = Url.PageLink("/Index", "Authorize", new {redirect = redirect}) }).Split('?')[1];

                return Redirect(_baseGateway + "/encyclopedia/login" + "?" + query );
            }

            return Redirect(redirect);
        }
    }
}
