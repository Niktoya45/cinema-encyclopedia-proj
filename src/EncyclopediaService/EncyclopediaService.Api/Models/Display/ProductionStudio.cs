namespace EncyclopediaService.Api.Models.Display
{
    public class ProductionStudio
    {
        public bool NewRecord { get; set; }
        public string? ParentId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; } = default;
        public string? PictureUri { get; set; } = default;
    }
}
