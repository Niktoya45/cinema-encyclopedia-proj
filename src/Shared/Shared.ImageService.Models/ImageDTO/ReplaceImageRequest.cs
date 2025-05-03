using Shared.ImageService.Models.Flags;

namespace Shared.ImageService.Models.ImageDTO
{
    public class ReplaceImageRequest
    {
        public string Id { get; set; }
        public string NewId { get; set; }
        public ImageSize Size { get; set; }
        public string FileBase64 { get; set; }
    }
}
