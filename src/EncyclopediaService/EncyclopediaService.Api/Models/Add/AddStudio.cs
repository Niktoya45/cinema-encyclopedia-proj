using EncyclopediaService.Api.Models.Edit;
using Shared.CinemaDataService.Models.Flags;

namespace EncyclopediaService.Api.Models.Add
{
    public class AddStudio
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; } = default(string);
        public DateOnly FoundDate { get; set; }
        public string? PresidentName { get; set; }
        public Country? Country { get; set; }
        public IList<EditFilm>? Filmography { get; set; }
        public string? Description { get; set; }
    }
}
