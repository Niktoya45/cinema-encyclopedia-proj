using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProfileService.Api.Models;

namespace ProfileService.Api.Views.Profiles
{
    public class MarkedModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        public IList<CinemaRecord> List { get; set; }
        public async Task<IActionResult> OnGet([FromRoute] string id)
        {
            Label[] labels = Enum.GetValues<Label>();

            List = Enumerable.Range(1, 55).Select(x => new CinemaRecord { 
                            ParentId = id,
                            Id = "" + x, 
                            Name = "Cinema " + x, 
                            Label = labels[x%4+1], 
                            AddedAt = new DateTime(2020+x/10, x%12+1, x%30+1, x%12+1, x%60+1, x%60+1) , 
                            Picture = null }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostDelete([FromQuery] string id, [FromQuery] string cinemaId)
        {
            return new OkObjectResult("log delete");
        }
    }
}
