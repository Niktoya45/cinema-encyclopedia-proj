
using CinemaDataService.Domain.Aggregates.Shared;

namespace CinemaDataService.Infrastructure.Models.PersonDTO
{
    public class PersonResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateOnly BirthDate { get; set; }
        public Country Country { get; set; }
        public Job Jobs { get; set; }
        public string? Picture { get; set; }
        public CinemaRecord[]? Filmography { get; set; }
        public string? Description { get; set; }
    }
}