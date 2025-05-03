using ImageService.Infrastructure.Models.Flags;

namespace ImageService.Infrastructure.Models.ImageDTO
{
    public class ReplaceImageRequest
    {
        public string Id { get; set; }
        public string NewId { get; set; }
        public ImageSize Size { get; set; }
        public string FileBase64 { get; set; }
    }
}
