using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using EncyclopediaService.Api.Models.Utils;
using EncyclopediaService.Api.Models.Sort;
using EncyclopediaService.Api.Models.Filter;
using EncyclopediaService.Api.Models.Display;

namespace EncyclopediaService.Api.Views.Encyclopedia.Persons.All
{
    public class IndexModel : PageModel
    {
        public const byte MaxPerPage = 28;

        [BindProperty(SupportsGet = true, Name = "pageNum")]
        public short PageNum { get; set; }
        public bool IsEnd { get; set; }
        private UISettings _settings { get; init; }

        [BindProperty(SupportsGet = true)]
        public FilterPersons _filterOptions { get; set; }
        public SortPersons _sortOptions { get; init; }

        [BindProperty(SupportsGet = true, Name = "order")]
        public bool? Order { get; set; }

        [BindProperty(SupportsGet = true, Name = "by")]
        public string? By { get; set; }
        public IList<PersonRecord> List { get; set; }

        public IndexModel(UISettings settings, SortPersons sortOptions)
        {
            _settings = settings;
            _sortOptions = sortOptions;

            _filterOptions = new FilterPersons();

                   }
        public async Task<IActionResult> OnGet([FromQuery] short pageNum = 1)
        {
            if (PageNum < 1) PageNum = pageNum;
            // request data instead

            List = Enumerable.Range(1, 25).Select(x => new PersonRecord { Id = "" + x, Name = "Person Name" + x, Jobs = Job.Actor, Picture = _settings.DefaultSmallPersonPicture }).ToList();

            IsEnd = List.Count < MaxPerPage;

            return Page();
        }

        public async Task<IActionResult> OnGetJob([FromQuery] short pageNum = 1)
        {
            if (PageNum < 1) PageNum = pageNum;
            // handle data transfer to Country endpoint instead

            List = Enumerable.Range(1, 25).Select(x => new PersonRecord { Id = "" + x, Name = "Person Name" + x, Jobs = Job.Actor, Picture = _settings.DefaultSmallPersonPicture }).ToList();

            IsEnd = List.Count < MaxPerPage;

            return Page();
        }

        public async Task<IActionResult> OnGetCountry([FromQuery] short pageNum = 1)
        {
            if (PageNum < 1) PageNum = pageNum;
            // handle data transfer to Country endpoint instead

            List = Enumerable.Range(1, 25).Select(x => new PersonRecord { Id = "" + x, Name = "Person Name" + x, Jobs = Job.Actor, Picture = _settings.DefaultSmallPersonPicture }).ToList();

            IsEnd = List.Count < MaxPerPage;

            return Page();
        }
    }
}
