namespace Shared.CinemaDataService.Models.RecordDTO
{
    public class UpdateFilmographyRequest
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public string? Picture { get; set; }
    }
}
