using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using EncyclopediaService.Api.Models;
using EncyclopediaService.Api.Models.Utils;
using EncyclopediaService.Api.Models.Sort;
using EncyclopediaService.Api.Models.Filter;

namespace EncyclopediaService.Api.Views.Encyclopedia.Cinemas
{
    public class AllModel : PageModel
    {
        public const byte MaxPerPage = 28;

        [BindProperty(SupportsGet = true)]
        public short PageNum { get; set; }
        public bool IsEnd { get; set; }

        UISettings _settings { get; init; }
        public FilterCinemas _filterOptions {  get; set; }

        public SortCinemas _sortOptions { get; init; }
        public IList<CinemaRecord> List { get; set; }

        [BindProperty]
        public bool? Order { get; set; } = null;

        [BindProperty]
        public string? By { get; set; }

        public AllModel(UISettings settings, SortCinemas sortOptions)
        {
            _settings = settings;
            _sortOptions = sortOptions;

            _filterOptions = new FilterCinemas();
        }

        public async Task<IActionResult> OnGet([FromQuery] short pageNum=1)
        { 
            // handle data transfer instead of below
            PageNum = pageNum;

            List = Enumerable.Range(1, 25).Select(x => new CinemaRecord { Id = x.ToString(), Year = x + 1994, Picture = _settings.DefaultSmallPosterPicture }).ToList();

            IsEnd = List.Count < MaxPerPage;

            return Page();
        }

        public async Task<IActionResult> OnGetYears([FromQuery] List<int> YearsBind)
        {
            // handle data transfer to Year endpoint instead
            List = Enumerable.Range(1, 25).Select(x => new CinemaRecord { Id = x.ToString(), Year = x + 1994, Picture = _settings.DefaultSmallPosterPicture }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnGetGenres([FromQuery] List<Genre> GenresBind)
        {
            // handle data transfer to Genre endpoint instead
            List = Enumerable.Range(1, 25).Select(x => new CinemaRecord { Id = x.ToString(), Year = x + 1994, Picture = _settings.DefaultSmallPosterPicture }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnGetLanguage([FromQuery] List<int> GenresBind)
        {
            // handle data transfer to Language endpoint instead
            List = Enumerable.Range(1, 25).Select(x => new CinemaRecord { Id = x.ToString(), Year = x + 1994, Picture = _settings.DefaultSmallPosterPicture }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnGetStudio([FromQuery] List<int> StudiosBind)
        {
            // handle data transfer to Studios endpoint instead
            List = Enumerable.Range(1, 25).Select(x => new CinemaRecord { Id = x.ToString(), Year = x + 1994, Picture = _settings.DefaultSmallPosterPicture }).ToList();

            return Page();
        }
    }
}
