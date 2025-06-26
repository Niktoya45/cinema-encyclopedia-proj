using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using EncyclopediaService.Api.Models.Utils;
using EncyclopediaService.Api.Models.Sort;
using EncyclopediaService.Api.Models.Filter;
using EncyclopediaService.Api.Models.Display;
using Shared.CinemaDataService.Models.Flags;
using EncyclopediaService.Api.Models.TestData;

namespace EncyclopediaService.Api.Views.Encyclopedia.Cinemas.All
{
    public class IndexModel : PageModel
    {
        public const byte MaxPerPage = 28;

        [BindProperty(SupportsGet = true, Name ="pageNum")]
        public short PageNum { get; set; }
        public bool IsEnd { get; set; }


        [BindProperty(SupportsGet=true)]
        public FilterCinemas _filterOptions {  get; set; }
        public SortCinemas _sortOptions { get; init; }
        public IList<CinemaRecord> List { get; set; }

        [BindProperty(SupportsGet=true, Name ="order")]
        public bool? Order { get; set; } = null;

        [BindProperty(SupportsGet=true, Name ="by")]
        public string? By { get; set; }

        public IndexModel(SortCinemas sortOptions)
        {
            _sortOptions = sortOptions;

            _filterOptions = new FilterCinemas();
        }

        public async Task<IActionResult> OnGet()
        {
            if (PageNum < 1) PageNum = 1;

            // handle data transfer instead of below
            List = TestRecords.CinemasList;

            IsEnd = List.Count < MaxPerPage;

            return Page();
        }

        public async Task<IActionResult> OnGetYears()
        {
            if (PageNum < 1) PageNum = 1;

            // handle data transfer to Year endpoint instead
            List = TestRecords.CinemasList;

            IsEnd = List.Count < MaxPerPage;

            return Page();
        }

        public async Task<IActionResult> OnGetGenres()
        {
            if (PageNum < 1) PageNum = 1;

            // handle data transfer to Genre endpoint instead
            List = TestRecords.CinemasList;

            return Page();
        }

        public async Task<IActionResult> OnGetLanguage()
        {
            if (PageNum < 1) PageNum = 1;

            // handle data transfer to Language endpoint instead
            List = TestRecords.CinemasList; 

            return Page();
        }

        public async Task<IActionResult> OnGetStudio()
        {
            if (PageNum < 1) PageNum = 1;

            // handle data transfer to Studios endpoint instead
            List = TestRecords.CinemasList;

            return Page();
        }

        public async Task<IActionResult> OnGetSearch()
        {
            if (PageNum < 1) PageNum = 1;

            // handle data transfer instead of below
            List = TestRecords.CinemasList;

            IsEnd = List.Count < MaxPerPage;

            return Page();
        }
    }
}
