using Shared.ImageService.Models.Flags;

namespace Shared.ImageService.Models.ImageDTO
{
    public class AddImageRequest
    {
        public string Id { get; set; }
        public ImageSize Size { get; set; }
        public string FileBase64 {get; set;}
    }
}
