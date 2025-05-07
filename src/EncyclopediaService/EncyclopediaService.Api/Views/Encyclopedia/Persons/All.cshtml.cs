using EncyclopediaService.Api.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using EncyclopediaService.Api.Models.Utils;
using EncyclopediaService.Api.Models.Sort;

namespace EncyclopediaService.Api.Views.Encyclopedia.Persons
{
    public class AllModel : PageModel
    {
        public int obj = 0;
        public int ColumnsCount = 4;
        
        private UISettings _settings { get; init; }
        public SortPersons _sortOptions { get; init; }

        [BindProperty]
        public bool? Order { get; set; }

        [BindProperty]
        public string? By { get; set; }
        public IList<PersonRecord> List { get; set; }

        public AllModel(UISettings settings, SortPersons sortOptions)
        {
            _settings = settings;
            _sortOptions = sortOptions;

            List = Enumerable.Range(1, 25).Select(x => new PersonRecord { Id = x.ToString(), Jobs = Job.Actor, Picture = _settings.DefaultSmallPersonPicture }).ToList();
        }
        public async Task<IActionResult> OnGet()
        {
            return Page();
        }
    }
}
