using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Shared.UserDataService.Models.UserDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace GatewayAPIService.Api.Controllers
{
    [Route("encyclopedia")]
    public class EncyclopediaController : Controller
    {
        public string BaseUrl { get; init; } = "https://localhost:7116";
        public string CurrentUrl { get; init; } = "https://localhost:7176";
        public HttpClient _client { get; set; }
        public IMemoryCache _cache { get; set; }
        public EncyclopediaController(IMemoryCache cache) 
        {
            _client = new HttpClient();
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> Index() 
        {
            var result = Redirect(BaseUrl + "/index");

            return result;
        }

        [HttpGet]
        [Route("authorize")]
        public async Task<IActionResult> Authorize([FromQuery] string code)
        {
            string? token = null;  

            if (!_cache.TryGetValue(code, out token))
            {
                return Ok(null);
            }

            return Ok(token);
        }

        [HttpGet]
        [Route("login")]
        [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme, Policy = "Authenticated")]
        public async Task<IActionResult> Login([FromQuery] string callback)
        {
            var ctoken = await HttpContext.GetTokenAsync("access_token");

            var key = Guid.NewGuid().ToString();

            _cache.Set<string>(key, ctoken, DateTime.UtcNow.AddMinutes(5));
            _cache.Set<string>(User.FindFirstValue(JwtRegisteredClaimNames.Name)??User.Identity.Name, ctoken);

            return Redirect(callback + $"&key={key}");
        }

        [HttpGet]
        [Route("cinemas")]
        public async Task<IActionResult> Cinemas()
        {
            var result = Redirect(BaseUrl+"/encyclopedia/cinemas/all/");

            return result;
        }

        [HttpGet]
        [Route("cinemas/{id}")]
        public async Task<IActionResult> Cinema(string id)
        {
            var result = Redirect(BaseUrl + $"/encyclopedia/cinemas/{id}");

            return result;
        }

        [HttpGet]
        [Route("persons")]
        public async Task<IActionResult> Persons()
        {
            var result = Redirect(BaseUrl + "/encyclopedia/persons/all/");

            return result;
        }

        [HttpGet]
        [Route("persons/{id}")]
        public async Task<IActionResult> Person(string id)
        {
            var result = Redirect(BaseUrl + $"/encyclopedia/persons/{id}");

            return result;
        }

        [HttpGet]
        [Route("studios")]
        public async Task<IActionResult> Studios()
        {
            var result = Redirect(BaseUrl + "/encyclopedia/studios/all/");

            return result;
        }

        [HttpGet]
        [Route("studios/{id}")]
        public async Task<IActionResult> Studio(string id)
        {
            var result = Redirect(BaseUrl + $"/encyclopedia/studios/{id}");

            return result;
        }
    }
}
