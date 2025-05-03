using ImageService.Infrastructure.Models.Flags;
using ImageService.Infrastructure.Models.ImageDTO;
using MediatR;

namespace ImageService.Api.Requests
{
    public class DeleteImage : IRequest<ImageResponse>
    {
        public string Id { get; set; }
        public ImageSize Size { get; set; }
    }
}
