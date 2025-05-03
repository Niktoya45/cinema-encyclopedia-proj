using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ImageService.Infrastructure.Models.ImageDTO;

namespace ImageService.Api.Controllers
{
    [Route("api/images")]
    public class ImagesController : Controller
    {

        [HttpGet]
        public async Task<IActionResult> OnGet([FromQuery] GetImageRequest request)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> OnPost([FromBody] AddImageRequest request)
        {
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> OnPut([FromBody] ReplaceImageRequest request)
        {
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> OnDelete([FromQuery] DeleteImageRequest request)
        {
            return Ok();
        }
    }
}
