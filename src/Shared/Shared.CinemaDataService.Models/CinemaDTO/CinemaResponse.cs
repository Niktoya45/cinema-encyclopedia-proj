
using Shared.CinemaDataService.Models.RecordDTO;
using Shared.CinemaDataService.Models.Flags;

namespace Shared.CinemaDataService.Models.CinemaDTO
{

    public class CinemaResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public Genre Genres { get; set; }
        public Language Language { get; set; }
        public RatingScore Rating { get; set; } = new RatingScore();
        public string? Picture { get; set; }
        public string? PictureUri { get; set; }
        public ProductionStudioResponse[]? ProductionStudios { get; set; }
        public StarringResponse[]? Starrings { get; set; }
        public string? Description { get; set; }

    }
}