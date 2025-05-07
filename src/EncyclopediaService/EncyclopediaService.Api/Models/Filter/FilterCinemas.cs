using Microsoft.AspNetCore.Mvc;

namespace EncyclopediaService.Api.Models.Filter
{
    public class FilterCinemas
    {
        // load information from db

        public static int YearsOffset = 10;

        public List<int> YearsChoice = Enumerable.Range(0, 13).Select(i => (DateTime.Now.Year/10)*10 - i * YearsOffset).ToList();

        [BindProperty(SupportsGet=true)]
        public List<int> YearsBind { get; set; } = new List<int>();

        public Genre GenresChoice { get; set; } = Enum.GetValues<Genre>().Aggregate((acc, g) => acc | g);

        [BindProperty(SupportsGet = true)]
        public List<Genre> GenresBind { get; set; } = new List<Genre>();

        [BindProperty(SupportsGet = true)]
        public Language LanguageBind { get; set; }

        public List<StudioRecord> StudiosChoice { get; set; } = Enumerable.Range(1, 10).Select(i => new StudioRecord { Id = "" + i, Name = "Studio Name" + i }).ToList();

        [BindProperty(SupportsGet = true)]
        public List<string> StudiosBind { get; set; } = new List<string>();
    }
}
