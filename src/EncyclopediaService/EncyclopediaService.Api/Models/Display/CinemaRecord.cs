namespace EncyclopediaService.Api.Models.Display
{
    public record CinemaRecord{
        public string Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public double Rating { get; set; }
        public string? Picture { get; set; } = default;
        public string? PictureUri { get; set; } = default;
    }
}
