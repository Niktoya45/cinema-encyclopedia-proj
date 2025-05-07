using CinemaDataService.Domain.Aggregates.Shared;

namespace CinemaDataService.Infrastructure.Models.PersonDTO
{
    public class PersonsResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; }
        public string? PictureUri { get; set; }
        public Job Jobs { get; set; }
        public bool IsDeleted { get; set; }
    }
}
