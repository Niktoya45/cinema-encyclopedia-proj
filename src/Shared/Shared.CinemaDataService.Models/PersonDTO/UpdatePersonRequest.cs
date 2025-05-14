
using Shared.CinemaDataService.Models.Flags;
using Shared.CinemaDataService.Models.RecordDTO;

namespace Shared.CinemaDataService.Models.PersonDTO
{
    public class UpdatePersonRequest
    {
        public string Name { get; set; }
        public DateOnly BirthDate { get; set; }
        public Country Country { get; set; }
        public Job Jobs { get; set; }
        public string? Picture {  get; set; }
        public CinemaRecord[]? Filmography { get; set; }
        public string? Description { get; set; }
    }
}