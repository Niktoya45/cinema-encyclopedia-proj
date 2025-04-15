

namespace CinemaDataService.Infrastructure.Models.StudioDTO
{
    public class StudiosResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string? Picture { get; set; }
        public bool IsDeleted { get; set; }
    }
}
