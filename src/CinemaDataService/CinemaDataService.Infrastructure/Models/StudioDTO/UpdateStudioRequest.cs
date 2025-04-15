
using CinemaDataService.Domain.Aggregates.Shared;

namespace CinemaDataService.Infrastructure.Models.StudioDTO
{
    public class UpdateStudioRequest
    {
        public string Name { get; set; }
        public DateOnly FoundDate { get; set; }
        public Country Country { get; set; }
        public string? Picture { get; set; }
        public CinemaRecord[]? Filmography { get; set; }
        public string? PresidentName { get; set; }
        public string? Description { get; set; }
    }
}