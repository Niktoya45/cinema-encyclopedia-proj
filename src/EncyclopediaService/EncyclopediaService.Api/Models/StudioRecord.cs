namespace EncyclopediaService.Api.Models
{
    public record StudioRecord
    {
        public string? ParentId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; } = default(string);
    }
}
