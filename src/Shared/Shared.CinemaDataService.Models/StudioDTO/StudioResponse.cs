
using Shared.CinemaDataService.Models.Flags;
using Shared.CinemaDataService.Models.RecordDTO;


namespace Shared.CinemaDataService.Models.StudioDTO
{
    public class StudioResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateOnly FoundDate { get; set; }
        public Country Country { get; set; }
        public string? Picture { get; set; }
        public string? PictureUri { get; set; }
        public FilmographyResponse[]? Filmography { get; set; }
        public string? PresidentName { get; set; }
        public string? Description { get; set; }
    }
}