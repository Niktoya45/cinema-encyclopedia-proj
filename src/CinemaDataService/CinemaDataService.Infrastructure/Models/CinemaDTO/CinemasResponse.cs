namespace CinemaDataService.Infrastructure.Models.CinemaDTO
{
    public class CinemasResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public double Rating { get; set; }
        public string? Picture { get; set; }
        public string? PictureUri { get; set; }
        public bool IsDeleted { get; set; }
    }
}
