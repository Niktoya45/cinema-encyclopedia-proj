using Microsoft.AspNetCore.Mvc;

namespace EncyclopediaService.Api.Models.Edit
{
    public class EditMainCinema
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public Genre Genres { get; set; }
        public List<Genre> GenresBind { get; set; } = default!;
        public Language? Language { get; set; }
        public string? Description { get; set; }
    }
}
