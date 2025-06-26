using EncyclopediaService.Api.Models.Display;
using Shared.CinemaDataService.Models.Flags;

namespace EncyclopediaService.Api.Models
{

    public class Cinema
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; } = default(string);
        public string? PictureUri { get; set; } = default(string);
        public DateOnly ReleaseDate { get; set; }
        public Genre Genres { get; set; }
        public Language? Language { get; set; }
        public double RatingScore { get; set; } = 0.0;
        public ProductionStudio[]? ProductionStudios { get; set; }
        public Starring[]? Starrings { get; set; }
        public string? Description { get; set; }
    }
}
