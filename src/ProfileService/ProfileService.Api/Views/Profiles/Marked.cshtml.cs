using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProfileService.Api.Models;
using ProfileService.Api.Models.Display;
using ProfileService.Api.Models.TestData;
using ProfileService.Infrastructure.Services.GatewayService;
using Shared.UserDataService.Models.Flags;
using System.Security.Claims;

namespace ProfileService.Api.Views.Profiles
{
    public class MarkedModel : PageModel
    {
        IGatewayService _gatewayService;

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        public IList<Marked> List { get; set; }

        public MarkedModel(IGatewayService gatewayService)
        { 
            _gatewayService = gatewayService;
        }

        public async Task<IActionResult> OnGet([FromRoute] string id, CancellationToken ct)
        {
            if (TestRecords.Used)
                List = TestRecords.Markeds;

            else {
                var response = await _gatewayService.GetUserLabeled(id, Label.None, ct);

                List = response is null || response.LabeledCinemas is null ? new List<Marked>()
                : response.LabeledCinemas.Select(l => new Marked 
                { 
                    ParentId = id,
                    Id = l.Cinema.Id,
                    Name = l.Cinema.Name,
                    Label = l.Label,
                    AddedAt = l.AddedAt,
                    Picture = l.Cinema.PictureUri,
                    
                }).ToArray();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDelete([FromRoute] string id, [FromQuery] string cinemaId, [FromQuery] Label label, CancellationToken ct)
        {
            if (TestRecords.Used)
                await _gatewayService.DeleteFromLabeledList(id, cinemaId, label, ct);

            return new OkObjectResult("log delete");
        }
    }
}
