using EncyclopediaService.Api.Models.Edit;
using Shared.CinemaDataService.Models.Flags;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EncyclopediaService.Api.Models.Add
{
    public class AddCinema
    {
        public string? Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string? Picture { get; set; } = default(string);
        public DateOnly ReleaseDate { get; set; }
        public Genre Genres { get; set; }

        [DisplayName("Genres")]
        [Required(ErrorMessage = "Choose at least one genre")]
        public List<Genre> GenresBind { get; set; } = default!;
        public Language? Language { get; set; }
        public IList<EditStudio>? ProductionStudios { get; set; }
        public IList<EditStarring>? Starrings { get; set; }
        public string? Description { get; set; }
    }
}
