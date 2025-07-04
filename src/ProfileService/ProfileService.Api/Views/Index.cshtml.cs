using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.JsonWebTokens;
using ProfileService.Api.Extensions;
using System.IO;
using System.Security.Claims;
using System.Web;

namespace ProfileService.Api.Views
{
    public class IndexModel : PageModel
    {
        const string _baseGateway = "https://localhost:7176";

        HttpClient _gatewayService;

        IMemoryCache _cache;
        public string Domain(HttpRequest request) => $"{request.Scheme}://{request.Host}";

        public IndexModel(IMemoryCache cache)
        {
            _gatewayService = new HttpClient();
            _gatewayService.BaseAddress = new Uri(_baseGateway);

            _cache = cache;
        }

        public async Task<IActionResult> OnGet()
        {
            if (!User.IsLoggedIn())
            {
                string id = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

                Redirect($"/profiles/{id}");
            }

            return await OnGetProfile("");
        }

        public async Task<IActionResult> OnGetEncyclopedia([FromQuery] string? path = null)
        {
            string? keyAF = SetAF();

            return RedirectOuter(_baseGateway + $"/encyclopedia{path??"/index"}", keyAF);
        }

        public async Task<IActionResult> OnGetProfile([FromQuery] string? id = null, [FromQuery] string? path = null)
        {
            if (!User.IsLoggedIn())
            {
                return await OnGetLogin(Request.GetDisplayUrl());
            }

            if (id == null || id == "")
            {
                return Redirect($"/profiles/{User.FindFirstValue(JwtRegisteredClaimNames.Sub)}{path ?? ""}");
            }

            return Redirect($"/profiles/{id}{path ?? ""}");
        }

        public async Task<IActionResult> OnGetLogin([FromQuery] string redirect)
        {
            if (!User.IsLoggedIn())
            {
                string query = $"callback={HttpUtility.UrlEncode(Url.PageLink("/Index", "Authorize", new { redirect = redirect }))}";

                return Redirect(_baseGateway + "/gateway/login" + "?" + query);
            }

            return Redirect(redirect);
        }

        public async Task<IActionResult> OnGetLogout() 
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect(_baseGateway + "/gateway/logout");
        }

        public async Task<IActionResult> OnGetAuthorize([FromQuery] string redirect, [FromQuery] string key)
        {
            if (!User.IsLoggedIn())
            {
                var response = await _gatewayService.GetAsync($"/gateway/authorize?code={key}");

                if (response.IsSuccessStatusCode)
                {
                    string? atoken = await response.Content.ReadAsStringAsync();

                    await TryAuthenticateByCookiesFromJwt(atoken);
                }
            }

            return redirect is null ? RedirectToPage() : User.IsLoggedIn() ? Redirect(redirect) : await OnGet();
        }

        public async Task<IActionResult> OnPostAuthorize([FromQuery] string? key)
        {
            if (key is null)
                return Unauthorized();

            string? token = GetAF(key);

            return new OkObjectResult(token);
        }

        public IActionResult RedirectOuter(string targetRedirect, string? key)
        {
            string redirect = $"{HttpUtility.UrlEncode(targetRedirect)}";

            string currentHost = Domain(Request);

            return Redirect(_baseGateway + $"/gateway/?redirect={redirect}&from={currentHost}&code={key}");
        }

        protected async Task<bool> TryAuthenticateByCookiesFromJwt(string? jwtToken)
        {
            if (jwtToken != null)
            {
                ClaimsPrincipal principal = new ClaimsPrincipal();

                bool added = principal.AddJwtTokenIdentity(jwtToken);

                if (added)
                {
                    HttpContext.User = principal;

                    await principal.SignInByCookies(HttpContext, true);

                    return true;
                }
            }

            return false;
        }

        protected string SetAF()
        {
            string keyAF = Guid.NewGuid().ToString();

            _cache.Set<string>(keyAF, User.FindFirstValue("token"), DateTime.UtcNow.AddMinutes(5));

            return keyAF;
        }

        protected string? GetAF(string keyAF)
        {
            return _cache.Get<string>(keyAF);
        }
    }

}
