
using Shared.CinemaDataService.Models.Flags;
using Shared.CinemaDataService.Models.RecordDTO;

namespace Shared.CinemaDataService.Models.CinemaDTO
{
    public class UpdateCinemaRequest
    {
        public string Name { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public Genre Genres { get; set; }
        public Language Language { get; set; }
        public string? Picture { get; set; }
        public CreateProductionStudioRequest[]? ProductionStudios { get; set; }
        public CreateStarringRequest[]? Starrings { get; set; }
        public string? Description { get; set; }
    }
}