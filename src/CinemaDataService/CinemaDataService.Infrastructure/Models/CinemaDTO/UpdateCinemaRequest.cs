
using CinemaDataService.Domain.Aggregates.CinemaAggregate;

namespace CinemaDataService.Infrastructure.Models.CinemaDTO
{
    public class UpdateCinemaRequest
    {
        public string Name { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public int Genres { get; set; }
        public int Language { get; set; }
        public string? Picture { get; set; }
        public StudioRecord[]? ProductionStudios { get; set; }
        public Starring[]? Starrings { get; set; }
        public string? Description { get; set; }
    }
}