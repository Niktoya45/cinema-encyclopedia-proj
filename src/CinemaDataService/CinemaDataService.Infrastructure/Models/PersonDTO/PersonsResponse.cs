
namespace CinemaDataService.Infrastructure.Models.PersonDTO
{
    public class PersonsResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; }
        public int Jobs { get; set; }
        public bool IsDeleted { get; set; }
    }
}
