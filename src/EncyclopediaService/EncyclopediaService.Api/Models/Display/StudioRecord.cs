namespace EncyclopediaService.Api.Models.Display
{
    public record StudioRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; } = default;
        public string? PictureUri { get; set; } = default;
    }
}
