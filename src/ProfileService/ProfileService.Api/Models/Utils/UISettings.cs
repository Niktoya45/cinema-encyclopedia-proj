using Shared.ImageService.Models.Flags;

namespace ProfileService.Api.Models.Utils
{
    public record UISettings
    {
        public long MaxFileLength { get; set; } = default!;
        public ImageSize SizesToInclude { get; set; } = default!;
        public string DefaultProfilePicture { get; set; } = default!;
        public string DefaultSmallProfilePicture { get; set; } = default!;

    }
}
