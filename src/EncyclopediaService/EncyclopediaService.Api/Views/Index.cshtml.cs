using EncyclopediaService.Api.Extensions;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Web;

namespace EncyclopediaService.Api.Views
{
    public class IndexModel : PageModel
    {
        const string _baseGateway = "https://localhost:7176";

        const string _defaultRedirect = "encyclopedia/cinemas/all/index";
        public string Domain(HttpRequest request) => $"{request.Scheme}://{request.Host}";

        HttpClient _gatewayService;
        public IMemoryCache _cache { get; init; }

        public IndexModel(IMemoryCache cache)
        {
            _gatewayService = new HttpClient();
            _gatewayService.BaseAddress = new Uri(_baseGateway);

            _cache = cache;
        }

        public async Task<IActionResult> OnGet([FromQuery] string? redirect = null, [FromQuery] string? key = null)
        {
            if (!User.IsLoggedIn() && key != null)
            {
                string? token = GetAF(key);

                await TryAuthenticateByCookiesFromJwt(token);
            }
            return Redirect(redirect??_defaultRedirect);
        }

        public async Task<IActionResult> OnGetProfile([FromQuery] string? id = null, [FromQuery] string? path = null)
        {
            if (!User.IsLoggedIn())
            {
                return await OnGetLogin(Request.GetDisplayUrl());
            }

            string? keyAF = SetAF();

            if (id == null || id == "")
            {
                return RedirectOuter(_baseGateway + $"/profile/{User.FindFirstValue(JwtRegisteredClaimNames.Sub)}{path ?? ""}", keyAF);
            }

            return RedirectOuter(_baseGateway + $"/profile/{id}{path ?? ""}", keyAF);
        }

        public async Task<IActionResult> OnGetLogin([FromQuery] string redirect)
        {
            if (!User.IsLoggedIn())
            {
                string query = $"callback={HttpUtility.UrlEncode(Url.PageLink("/Index", "Authorize", new { redirect = redirect }))}";

                return Redirect(_baseGateway + "/gateway/login" + "?" + query );
            }

            return Redirect(redirect);
        }


        public async Task<IActionResult> OnGetLogout()
        {
            await User.SignOutByCookies(HttpContext);

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

        protected string? SetAF() 
        {
            string keyAF = Guid.NewGuid().ToString();

            if (!User.IsLoggedIn())
                return null;

            _cache.Set<string>(keyAF, User.FindFirstValue("token"), DateTime.UtcNow.AddMinutes(5));

            return keyAF;
        }

        protected string? GetAF(string keyAF) 
        {
            return _cache.Get<string>(keyAF);
        }
    }
}
