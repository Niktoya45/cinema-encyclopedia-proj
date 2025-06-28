using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace GatewayAPIService.Api.Controllers
{
    [Route("profile")]
    public class ProfileController : Controller
    {
        public const string _baseUrl = "https://localhost:7136";
        public const string _callback= "/authorize";

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Profile(string id, string? code = null) 
        {
            return HandleRedirect($"/profiles/{id}", code);
        }

        [HttpGet]
        [Route("{id}/marked")]
        public async Task<IActionResult> Marked(string id, string? code = null)
        {
            return HandleRedirect($"/profiles/{id}/marked", code);
        }

        protected IActionResult HandleRedirect(string url, string? code)
        {
            if (code != null && code != "")
                return RedirectAuthorize(url, code);

            return Redirect(_baseUrl + url);
        }

        protected IActionResult RedirectAuthorize(string targetRedirect, string code)
        {
            string redirect = HttpUtility.UrlEncode(targetRedirect);

            return Redirect(_baseUrl + $"{_callback}?redirect={redirect}&key={code}");
        }
    }
}
