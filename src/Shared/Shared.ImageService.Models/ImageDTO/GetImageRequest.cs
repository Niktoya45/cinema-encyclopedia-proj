using Shared.ImageService.Models.Flags;

namespace Shared.ImageService.Models.ImageDTO
{
    public class GetImageRequest
    {
        public string Id { get; set; }
        public ImageSize Size { get; set; }
    }
}
