

namespace CinemaDataService.Infrastructure.Models.SharedDTO
{
    public class UpdateFilmographyRequest
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public string? Picture { get; set; }
    }
}
