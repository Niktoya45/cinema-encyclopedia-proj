namespace Shared.CinemaDataService.Models.RecordDTO
{
    public class CreateFilmographyRequest
    {
        public string Id { get; set; }
        public string Name {  get; set; }
        public int Year { get; set; }
        public string? Picture {  get; set; }

    }
}
