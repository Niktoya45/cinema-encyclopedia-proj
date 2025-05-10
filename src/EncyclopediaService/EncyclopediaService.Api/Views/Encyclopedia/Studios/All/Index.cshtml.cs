using EncyclopediaService.Api.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using EncyclopediaService.Api.Models.Utils;
using EncyclopediaService.Api.Models.Sort;
using EncyclopediaService.Api.Models.Filter;

namespace EncyclopediaService.Api.Views.Encyclopedia.Studios.All
{
    public class IndexModel : PageModel
    {
        public const byte MaxPerPage = 28;

        [BindProperty(SupportsGet = true, Name = "pageNum")]
        [FromQuery(Name = "pageNum")]
        public short PageNum { get; set; }
        public bool IsEnd { get; set; }
        private UISettings _settings { get; init; }

        [BindProperty(SupportsGet=true)]
        public FilterStudios _filterOptions { get; set; }

        public SortStudios _sortOptions { get; init; }

        [BindProperty(SupportsGet = true, Name = "order")]
        public bool? Order { get; set; }

        [BindProperty(SupportsGet = true, Name = "by")]
        public string? By { get; set; }
        public IList<StudioRecord> List { get; set; }

        public IndexModel(UISettings settings, SortStudios sortOptions)
        {
            _settings = settings;
            _sortOptions = sortOptions;

            _filterOptions = new FilterStudios();
        }
        public async Task<IActionResult> OnGet([FromQuery] short pageNum = 1)
        {
            if (PageNum < 1) PageNum = pageNum;
            // handle data transfer instead of below

            List = Enumerable.Range(1, 25).Select(x => new StudioRecord { Id = "" + x, Name = $"Studio {x}", Picture = _settings.DefaultSmallPersonPicture }).ToList();

            IsEnd = List.Count < MaxPerPage;

            return Page();
        }

        public async Task<IActionResult> OnGetYears([FromQuery] short pageNum = 1)
        {
            if (PageNum < 1) PageNum = pageNum;
            // handle data transfer to Year endpoint instead
            List = Enumerable.Range(1, 25).Select(x => new StudioRecord { Id = "" + x, Name = $"Studio {x}", Picture = _settings.DefaultSmallPersonPicture }).ToList();

            IsEnd = List.Count < MaxPerPage;

            return Page();
        }

        public async Task<IActionResult> OnGetCountry([FromQuery] short pageNum=1) {
            if (PageNum < 1) PageNum = pageNum;

            // handle data transfer to Country endpoint instead
            List = Enumerable.Range(1, 25).Select(x => new StudioRecord { Id = "" + x, Name = $"Studio {x}", Picture = _settings.DefaultSmallPersonPicture }).ToList();

            IsEnd = List.Count < MaxPerPage;

            return Page();
        }
    }
}
