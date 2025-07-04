using Shared.UserDataService.Models.Flags;

namespace ProfileService.Api.Models.Display
{
    public record Marked
    {
        public string? ParentId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public Label Label { get; set; }
        public DateTime AddedAt { get; set; }
        public string? Picture { get; set; } = default(string);
    }
}
