namespace ImageService.Api.General
{
    public record ImageSettings
    {
        public string RootDirectory { get; set; } = default!;
        public long MaxFileLength { get; set; } = default!;
        public string DirectoryTiny { get; set; } = default!;
        public string DirectorySmall { get; set; } = default!;
        public string DirectoryMedium { get; set; } = default!;
        public string DirectoryBig { get; set; } = default!;
        public string DirectoryLarge { get; set; } = default!;

    }
}
