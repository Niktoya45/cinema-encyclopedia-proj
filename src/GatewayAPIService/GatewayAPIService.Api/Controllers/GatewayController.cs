using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace GatewayAPIService.Api.Controllers
{

    [Route("gateway")]
    public class GatewayController : Controller
    {
        public HttpClient _client;
        public IMemoryCache _cache;
        public GatewayController(IMemoryCache cache)
        {
            _client = new HttpClient();

            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] string redirect, [FromQuery] string? from = null, [FromQuery] string? code = null) 
        {
            if (from != null && code != null)
            {
                var response = await _client.PostAsync(from + $"/authorize?key={code}", null);

                if (response.IsSuccessStatusCode)
                { 
                    string ctoken = await response.Content.ReadAsStringAsync();

                    _cache.Set<string>(code, ctoken, DateTime.UtcNow.AddMinutes(5));
                }
                
                char c = redirect.Contains('?') ? '&' : '?';

                redirect += $"{c}code={code}";
            }

            return Redirect(redirect);
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

            return Redirect(callback + $"&key={key}");
        }
    }
}
