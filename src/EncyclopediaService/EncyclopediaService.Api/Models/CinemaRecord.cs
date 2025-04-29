
namespace EncyclopediaService.Api.Models
{
    public record CinemaRecord{
        public string ParentId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string? Picture { get; set; } = default(string);
    }
}
