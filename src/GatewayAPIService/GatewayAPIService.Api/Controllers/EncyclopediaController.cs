using Microsoft.AspNetCore.Mvc;

namespace GatewayAPIService.Api.Controllers
{
    [Route("encyclopedia")]
    public class EncyclopediaController : Controller
    {
        public string BaseUrl { get; init; } = "https://localhost:7116";

        [HttpGet]
        public async Task<IActionResult> Index() 
        {
            var result = Redirect(BaseUrl + "/encyclopedia/index");

            return result;
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
