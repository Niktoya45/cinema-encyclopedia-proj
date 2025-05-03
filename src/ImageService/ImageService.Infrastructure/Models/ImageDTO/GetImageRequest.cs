using ImageService.Infrastructure.Models.Flags;

namespace ImageService.Infrastructure.Models.ImageDTO
{
    public class GetImageRequest
    {
        public string Id { get; set; }
        public ImageSize Size { get; set; }
    }
}
