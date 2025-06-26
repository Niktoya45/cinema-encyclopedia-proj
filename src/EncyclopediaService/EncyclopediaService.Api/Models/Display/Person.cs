using Shared.CinemaDataService.Models.Flags;

namespace EncyclopediaService.Api.Models.Display
{

    public class Person
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateOnly BirthDate { get; set; }
        public Country Country { get; set; }
        public Job Jobs { get; set; }
        public string? Picture { get; set; }
        public string? PictureUri { get; set; }
        public FilmographyRecord[]? Filmography { get; set; }
        public string? Description { get; set; }
    }
}
