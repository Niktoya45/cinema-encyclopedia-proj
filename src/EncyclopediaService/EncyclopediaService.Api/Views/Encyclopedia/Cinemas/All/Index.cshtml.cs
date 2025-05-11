using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using EncyclopediaService.Api.Models;
using EncyclopediaService.Api.Models.Utils;
using EncyclopediaService.Api.Models.Sort;
using EncyclopediaService.Api.Models.Filter;
using System.ComponentModel;

namespace EncyclopediaService.Api.Views.Encyclopedia.Cinemas.All
{
    public class IndexModel : PageModel
    {
        public const byte MaxPerPage = 28;

        [BindProperty(SupportsGet = true, Name ="pageNum")]
        public short PageNum { get; set; }
        public bool IsEnd { get; set; }

        UISettings _settings { get; init; }

        [BindProperty(SupportsGet=true)]
        public FilterCinemas _filterOptions {  get; set; }
        public SortCinemas _sortOptions { get; init; }
        public IList<CinemaRecord> List { get; set; }

        [BindProperty(SupportsGet=true, Name ="order")]
        public bool? Order { get; set; } = null;

        [BindProperty(SupportsGet=true, Name ="by")]
        public string? By { get; set; }

        public IndexModel(UISettings settings, SortCinemas sortOptions)
        {
            _settings = settings;
            _sortOptions = sortOptions;

            _filterOptions = new FilterCinemas();
        }

        public async Task<IActionResult> OnGet([FromQuery] short pageNum = 1)
        {
            if (PageNum < 1) PageNum = pageNum;
            // handle data transfer instead of below

            List = Enumerable.Range(1, 25).Select(x => new CinemaRecord { Id = ""+x, Name="Cinema "+ x, Year = x + 1994, Rating=((x%10)+0.5), Picture = _settings.DefaultSmallPosterPicture }).ToList();

            IsEnd = List.Count < MaxPerPage;

            return Page();
        }

        public async Task<IActionResult> OnGetYears([FromQuery] short pageNum = 1)
        {   
            // handle data transfer to Year endpoint instead
            List = Enumerable.Range(1, 25).Select(x => new CinemaRecord { Id = ""+x, Name = "Cinema " + x, Year = x + 1994, Rating = ((x % 10) + 0.5), Picture = _settings.DefaultSmallPosterPicture }).ToList();

            IsEnd = List.Count < MaxPerPage;

            return Page();
        }

        public async Task<IActionResult> OnGetGenres([FromQuery] short pageNum = 1)
        {
            // handle data transfer to Genre endpoint instead
            List = Enumerable.Range(1, 25).Select(x => new CinemaRecord { Id = ""+x, Name = "Cinema " + x, Year = x + 1994, Rating = ((x % 10) + 0.5), Picture = _settings.DefaultSmallPosterPicture }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnGetLanguage([FromQuery] short pageNum = 1)
        {
            // handle data transfer to Language endpoint instead
            List = Enumerable.Range(1, 25).Select(x => new CinemaRecord { Id = ""+x, Name = "Cinema " + x, Year = x + 1994, Rating = ((x % 10) + 0.5), Picture = _settings.DefaultSmallPosterPicture }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnGetStudio([FromQuery] short pageNum = 1)
        {
            // handle data transfer to Studios endpoint instead
            List = Enumerable.Range(1, 25).Select(x => new CinemaRecord { Id = ""+x, Name = "Cinema " + x, Year = x + 1994, Rating = ((x % 10) + 0.5), Picture = _settings.DefaultSmallPosterPicture }).ToList();

            return Page();
        }
    }
}
