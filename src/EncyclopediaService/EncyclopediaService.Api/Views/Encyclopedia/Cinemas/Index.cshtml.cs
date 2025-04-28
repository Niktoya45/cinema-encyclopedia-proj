using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using EncyclopediaService.Api.Models;

namespace EncyclopediaService.Api.Views.Encyclopedia.Cinemas
{
    public class IndexModel : PageModel
    {
        public int obj = 0;
        public int ColumnsCount = 4;
        public IList<CinemaRecord> List { get; set; } = Enumerable.Range(1, 25).Select(x => new CinemaRecord { Id = x.ToString(), Year = x + 1994 }).ToList();
        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }
    }
}
