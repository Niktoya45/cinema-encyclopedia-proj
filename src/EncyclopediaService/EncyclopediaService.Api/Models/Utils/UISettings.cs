namespace EncyclopediaService.Api.Models.Utils
{
    public record UISettings
    {
        public long MaxFileLength { get; set; } = default!;
        public string DefaultPosterPicture { get; set; } = default!;
        public string DefaultSmallPosterPicture { get; set; } = default!;

        public string DefaultPersonPicture { get; set; } = default!;
        public string DefaultSmallPersonPicture { get; set; } = default!;

        public string DefaultLogoPicture { get; set; } = default!;
        public string DefaultSmallLogoPicture { get; set; } = default!;

    }
}
