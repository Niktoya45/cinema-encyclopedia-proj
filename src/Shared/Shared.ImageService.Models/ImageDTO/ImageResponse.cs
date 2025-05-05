
using Shared.ImageService.Models.Flags;

namespace Shared.ImageService.Models.ImageDTO
{
    public class ImageResponse
    {
        public string Id { get; set; }
        public Dictionary<ImageSize, string> Uris { get; set; }
        public ImageSize Size { get; set; }
    }
}
