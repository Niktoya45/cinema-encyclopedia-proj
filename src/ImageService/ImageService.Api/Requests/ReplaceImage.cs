using ImageService.Infrastructure.Models.Flags;
using ImageService.Infrastructure.Models.ImageDTO;
using MediatR;

namespace ImageService.Api.Requests
{
    public class ReplaceImage : IRequest<ImageResponse>
    {
        public string Id { get; set; }
        public string NewId { get; set; }
        public ImageSize Size { get; set; }
        public string FileBase64 { get; set; }
    }
}
