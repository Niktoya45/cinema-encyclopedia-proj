using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.CinemaDataService.Models.Flags;
using EncyclopediaService.Api.Models.Utils;
using EncyclopediaService.Api.Models.Sort;
using EncyclopediaService.Api.Models.Filter;
using EncyclopediaService.Api.Models.Display;
using EncyclopediaService.Api.Models.TestData;

namespace EncyclopediaService.Api.Views.Encyclopedia.Studios.All
{
    public class IndexModel : PageModel
    {
        public const byte MaxPerPage = 28;

        [BindProperty(SupportsGet = true, Name = "pagen")]
        public short PageNum { get; set; }
        public bool IsEnd { get; set; }

        [BindProperty(SupportsGet=true)]
        public FilterStudios _filterOptions { get; set; }

        public SortStudios _sortOptions { get; init; }

        [BindProperty(SupportsGet = true, Name = "order")]
        public bool? Order { get; set; }

        [BindProperty(SupportsGet = true, Name = "by")]
        public string? By { get; set; }
        public IList<StudioRecord> List { get; set; }

        public IndexModel(SortStudios sortOptions)
        {
            _sortOptions = sortOptions;

            _filterOptions = new FilterStudios();
        }
        public async Task<IActionResult> OnGet()
        {
            if (PageNum < 1) PageNum = 1;

            // handle data transfer instead of below
            List = TestRecords.StudiosList;

            IsEnd = List.Count < MaxPerPage;

            return Page();
        }

        public async Task<IActionResult> OnGetYears()
        {
            if (PageNum < 1) PageNum = 1;

            // handle data transfer to Year endpoint instead
            List = TestRecords.StudiosList;

            IsEnd = List.Count < MaxPerPage;

            return Page();
        }

        public async Task<IActionResult> OnGetCountry() {

            if (PageNum < 1) PageNum = 1;

            // handle data transfer to Country endpoint instead
            List = TestRecords.StudiosList;

            IsEnd = List.Count < MaxPerPage;

            return Page();
        }

        public async Task<IActionResult> OnGetSearch()
        {
            if (_filterOptions.Search is null)
            {
                RedirectToPage();
            }

            if (PageNum < 1) PageNum = 1;

            // handle data transfer instead of below
            List = TestRecords.StudiosList;

            IsEnd = List.Count < MaxPerPage;

            return Page();
        }
    }
}
