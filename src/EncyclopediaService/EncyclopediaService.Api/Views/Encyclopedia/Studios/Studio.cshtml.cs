using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EncyclopediaService.Api.Views.Encyclopedia.Studios
{
    public class StudioModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }
    }
}
