
using ImageService.Infrastructure.Models.Flags;

namespace ImageService.Infrastructure.Models.ImageDTO
{
    public class ImageResponse
    {
        public string Id { get; set; }
        public string Uri { get; set; }
        public ImageSize Size { get; set; }
    }
}
