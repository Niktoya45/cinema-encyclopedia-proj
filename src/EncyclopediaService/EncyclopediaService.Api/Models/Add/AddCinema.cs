using EncyclopediaService.Api.Models.Display;
using EncyclopediaService.Api.Models.Edit;

namespace EncyclopediaService.Api.Models.Add
{
    public class AddCinema
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; } = default(string);
        public DateOnly ReleaseDate { get; set; }
        public Genre Genres { get; set; }
        public List<Genre> GenresBind { get; set; } = default!;
        public Language? Language { get; set; }
        public IList<EditStudio>? ProductionStudios { get; set; }
        public IList<EditStarring>? Starrings { get; set; }
        public string? Description { get; set; }
    }
}
