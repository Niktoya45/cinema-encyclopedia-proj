using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccessService.Api.Pages.Account
{
    public class IndexModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            return RedirectToPage("./Login");
        }
    }
}
