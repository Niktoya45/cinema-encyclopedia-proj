using Shared.CinemaDataService.Models.Flags;
using Microsoft.AspNetCore.Mvc;
using EncyclopediaService.Api.Models.Display;

namespace EncyclopediaService.Api.Models.Filter
{
    public class FilterCinemas
    {
        // load information from db

        public const int YearSpan = 10;

        public List<int> YearsChoice = Enumerable.Range(0, 13).Select(i => (DateTime.Now.Year/10)*10 - i * YearSpan).ToList();

        [BindProperty(SupportsGet = true, Name = "yearsbind")]
        public List<int> YearsBind { get; set; } = new List<int>();

        public Genre GenresChoice { get; set; } = Enum.GetValues<Genre>().Aggregate((acc, g) => acc | g);

        [BindProperty(SupportsGet = true, Name = "genresbind")]
        public List<Genre> GenresBind { get; set; } = new List<Genre>();

        [BindProperty(SupportsGet = true, Name = "languagebind")]
        public Language LanguageBind { get; set; }

        public List<StudioRecord> StudiosChoice { get; set; }

        [BindProperty(SupportsGet = true, Name = "studiosbind")]
        public List<string> StudiosBind { get; set; } = new List<string>();


        [BindProperty(SupportsGet = true, Name = "q")]
        public string? Search { get; set; } = default;
    }
}
