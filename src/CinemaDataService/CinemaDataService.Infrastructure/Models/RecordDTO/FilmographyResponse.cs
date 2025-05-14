namespace CinemaDataService.Infrastructure.Models.RecordDTO
{
    public class FilmographyResponse
    {
        public string? ParentId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string? Picture { get; set; } = default;
        public string? PictureUri { get; set; } = default;
    }
}
