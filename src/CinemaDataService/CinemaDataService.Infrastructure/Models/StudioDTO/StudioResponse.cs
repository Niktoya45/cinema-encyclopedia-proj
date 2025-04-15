
using CinemaDataService.Domain.Aggregates.Shared;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CinemaDataService.Infrastructure.Models.StudioDTO
{
    public class StudioResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateOnly FoundYear { get; set; }
        public Country Country { get; set; }
        public string? Picture { get; set; }
        public CinemaRecord[]? Filmography { get; set; }
        public string? PresidentName { get; set; }
        public string? Description { get; set; }
    }
}