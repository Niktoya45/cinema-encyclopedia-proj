
namespace EncyclopediaService.Api.Models
{
    public record CinemaRecord{
        public string Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string? Picture { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
