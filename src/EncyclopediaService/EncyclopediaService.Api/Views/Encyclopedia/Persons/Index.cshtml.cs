using EncyclopediaService.Api.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using EncyclopediaService.Api.Models.Utils;

namespace EncyclopediaService.Api.Views.Encyclopedia.Persons
{
    public class IndexModel : PageModel
    {
        public int obj = 0;
        public int ColumnsCount = 4;
        
        private UISettings _settings { get; init; }
        public IList<PersonRecord> List { get; set; }

        public IndexModel(UISettings settings)
        {
            _settings = settings;

            List = Enumerable.Range(1, 25).Select(x => new PersonRecord { Id = x.ToString(), Jobs = Job.Actor, Picture = _settings.DefaultSmallPersonPicture }).ToList();
        }
        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }
    }
}
