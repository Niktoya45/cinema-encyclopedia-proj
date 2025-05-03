
using ImageService.Infrastructure.Models.Flags;

namespace ImageService.Infrastructure.Models.ImageDTO
{
    public class DeleteImageRequest
    {
        public string Id { get; set; }
        public ImageSize Size { get; set; }
    }
}
