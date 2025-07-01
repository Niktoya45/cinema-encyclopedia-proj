using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace GatewayAPIService.Api.Controllers
{
    [Route("encyclopedia")]
    public class EncyclopediaController : Controller
    {
        public const string _baseUrl = "https://localhost:7116";
        protected const string _callback = "/authorize";

        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index(string? code = null) 
        {
            return HandleRedirect("/index", code);
        }

        [HttpGet]
        [Route("cinemas")]
        public async Task<IActionResult> Cinemas(string? code = null)
        {
            return HandleRedirect("/encyclopedia/cinemas/all/", code);
        }

        [HttpGet]
        [Route("cinemas/{id}")]
        public async Task<IActionResult> Cinema(string id, string? code = null)
        {
            return HandleRedirect($"/encyclopedia/cinemas/{id}", code);
        }

        [HttpGet]
        [Route("create/cinema")]
        public async Task<IActionResult> CreateCinema(string? code = null)
        {
            return HandleRedirect($"/encyclopedia/create/cinema", code);
        }

        [HttpGet]
        [Route("persons")]
        public async Task<IActionResult> Persons(string? code = null)
        {
            return HandleRedirect("/encyclopedia/persons/all/", code);
        }

        [HttpGet]
        [Route("persons/{id}")]
        public async Task<IActionResult> Person(string id, string? code = null)
        {
            return HandleRedirect($"/encyclopedia/persons/{id}", code);
        }

        [HttpGet]
        [Route("create/person")]
        public async Task<IActionResult> CreatePerson(string? code = null)
        {
            return HandleRedirect($"/encyclopedia/create/person", code);
        }

        [HttpGet]
        [Route("studios")]
        public async Task<IActionResult> Studios(string? code = null)
        {
            return HandleRedirect("/encyclopedia/studios/all/", code);
        }

        [HttpGet]
        [Route("studios/{id}")]
        public async Task<IActionResult> Studio(string id, string? code = null)
        {
            return HandleRedirect($"/encyclopedia/studios/{id}", code);
        }

        [HttpGet]
        [Route("create/studio")]
        public async Task<IActionResult> CreateStudio(string? code = null)
        {
            return HandleRedirect($"/encyclopedia/create/studio", code);
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
