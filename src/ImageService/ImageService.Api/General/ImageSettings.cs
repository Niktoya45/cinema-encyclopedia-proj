using ImageService.Infrastructure.Models.Flags;

namespace ImageService.Api.General
{
    public record ImageSettings
    {
        public string RootDirectory { get; set; } = default!;
        public long MaxFileLength { get; set; } = default!;
        public Dictionary<ImageSize, string> Directory { get; set; } = default!;
        public Dictionary<ImageSize, uint> Normalize { get; set; } = default!;
        public Dictionary<ImageSize, uint> Breakpoints { get; set; } = default!;

    }
}
