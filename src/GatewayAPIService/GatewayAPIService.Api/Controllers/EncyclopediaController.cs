using Microsoft.AspNetCore.Mvc;

namespace GatewayAPIService.Api.Controllers
{
    [Route("encyclopedia")]
    public class EncyclopediaController : Controller
    {
        public string BaseUrl { get; init; } = "https://localhost:7116";

        public async Task<IActionResult> Index() 
        {
            var result = Redirect(BaseUrl + "/encyclopedia/index");

            return result;
        }

        [Route("cinemas")]
        public async Task<IActionResult> Cinemas()
        {
            var result = Redirect(BaseUrl+"/encyclopedia/cinemas/all/");

            return result;
        }

        [Route("cinemas/{id}")]
        public async Task<IActionResult> Cinema(string id)
        {
            var result = Redirect(BaseUrl + $"/encyclopedia/cinemas/{id}");

            return result;
        }

        [Route("persons")]
        public async Task<IActionResult> Persons()
        {
            var result = Redirect(BaseUrl + "/encyclopedia/persons/all/");

            return result;
        }

        [Route("persons/{id}")]
        public async Task<IActionResult> Person(string id)
        {
            var result = Redirect(BaseUrl + $"/encyclopedia/persons/{id}");

            return result;
        }

        [Route("studios")]
        public async Task<IActionResult> Studios()
        {
            var result = Redirect(BaseUrl + "/encyclopedia/studios/all/");

            return result;
        }

        [Route("studios/{id}")]
        public async Task<IActionResult> Studio(string id)
        {
            var result = Redirect(BaseUrl + $"/encyclopedia/studios/{id}");

            return result;
        }
    }
}
