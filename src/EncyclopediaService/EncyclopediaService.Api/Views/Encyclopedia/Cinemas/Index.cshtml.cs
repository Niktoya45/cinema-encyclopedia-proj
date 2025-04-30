using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using EncyclopediaService.Api.Models;
using EncyclopediaService.Api.Models.Utils;

namespace EncyclopediaService.Api.Views.Encyclopedia.Cinemas
{
    public class IndexModel : PageModel
    {
        public int obj = 0;
        public int ColumnsCount = 4;
        UISettings _settings { get; init; }
        public IList<CinemaRecord> List { get; set; } 

        public IndexModel(UISettings settings)
        {
            _settings = settings;

            List = Enumerable.Range(1, 25).Select(x => new CinemaRecord { Id = x.ToString(), Year = x + 1994, Picture=_settings.DefaultSmallPosterPicture }).ToList();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }
    }
}
