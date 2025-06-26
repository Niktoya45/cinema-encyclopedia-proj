using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GatewayAPIService.Api.Controllers
{
    [Route("test")]
    public class TestController : Controller
    {
        [HttpGet("test-protected")]
        [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme, Policy = "Authenticated")]
        public async Task<IActionResult> TestProtected()
        {

            var claims = HttpContext.User.Claims;
            var client = new HttpClient();

            var token = await HttpContext.GetTokenAsync("access_token");

            client.BaseAddress = new Uri("https://localhost:7176");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            var response = await client.GetStringAsync("/test/test-protected-data");

            return Ok(response);
        }

        [HttpGet("test-protected-data")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ProtectedData()
        {
            return Ok(new {
                Sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub),
                Sid = User.FindFirstValue(JwtRegisteredClaimNames.Sid),
                Role = User.FindFirstValue("role"),
                Message = "Api_Authorization_Succeeded" });
        }
    }
}
