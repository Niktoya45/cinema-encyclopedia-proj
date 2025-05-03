using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ImageService.Infrastructure.Models.ImageDTO;
using MediatR;
using ImageService.Api.Requests;

namespace ImageService.Api.Controllers
{
    [Route("api/images")]
    public class ImagesController : Controller
    {
        private readonly IMediator _mediator;
        public ImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> OnGet([FromQuery] GetImageRequest request)
        {
            ImageResponse response = await _mediator.Send(
                new GetImage
                {
                    Id = request.Id,
                    Size = request.Size
                }
            );

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> OnPost([FromBody] AddImageRequest request)
        {
            ImageResponse response = await _mediator.Send(
                new AddImage { 
                    Id = request.Id,
                    Size = request.Size,
                    FileBase64 = request.FileBase64
                }
            );

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> OnPut([FromBody] ReplaceImageRequest request)
        {
            ImageResponse response = await _mediator.Send(
                new ReplaceImage
                {
                    Id = request.Id,
                    NewId = request.NewId,
                    Size = request.Size,
                    FileBase64 = request.FileBase64
                }
            );

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> OnDelete([FromQuery] DeleteImageRequest request)
        {
            ImageResponse response = await _mediator.Send(
                new DeleteImage
                {
                    Id = request.Id,
                    Size = request.Size
                }
            );

            return Ok(response);
        }
    }
}
