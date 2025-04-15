using Humanizer.Localisation;

namespace EncyclopediaService.Api.Models
{
    /*
     * vvv src/Shared/.. vvv
     */
    /**/ public enum Genre;
    /**/ public enum Language;
    /**/ public record StudioRecord { }
    /**/ public record Starring { }

    public class CinemaViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; } = default(string);
        public DateOnly ReleaseDate { get; set; }
        public string Genres { get; set; }
        public string Language { get; set; }
        public double RatingScore { get; set; } = 0.0;
        public StudioRecord[]? ProductionStudios { get; set; }
        public Starring[]? Starrings { get; set; }
        public string? Description { get; set; }
    }
}
