
using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Models.RecordDTO;

namespace CinemaDataService.Infrastructure.Models.PersonDTO
{
    public class UpdatePersonRequest
    {
        public string Name { get; set; }
        public DateOnly BirthDate { get; set; }
        public Country Country { get; set; }
        public Job Jobs { get; set; }
        public string? Picture {  get; set; }
        public CreateFilmographyRequest[]? Filmography { get; set; }
        public string? Description { get; set; }
    }
}