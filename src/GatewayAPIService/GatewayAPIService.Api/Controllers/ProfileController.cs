using Microsoft.AspNetCore.Mvc;

namespace GatewayAPIService.Api.Controllers
{
    [Route("profile")]
    public class ProfileController : Controller
    {
        public string BaseUrl { get; init; } = "https://localhost:7136";

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Profile(string id) 
        {
            var result = Redirect(BaseUrl + $"/profile/{id}");

            return result;
        }

        [HttpGet]
        [Route("{id}/marked")]
        public async Task<IActionResult> Marked(string id)
        {
            var result = Redirect(BaseUrl + $"/profile/{id}/marked");

            return result;
        }
    }
}
