using EncyclopediaService.Api.Models.Edit;
using EncyclopediaService.Api.Models.Utils;
using EncyclopediaService.Infrastructure.Services.ImageService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EncyclopediaService.Api.Extensions;
using Shared.ImageService.Models.Flags;
using EncyclopediaService.Api.Models.Display;

namespace EncyclopediaService.Api.Views.Encyclopedia.Studios
{
    public class StudioModel : PageModel
    {
        private IImageService _imageService { get; init; }
        private UISettings _settings { get; init; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public Studio? Studio { get; set; }

        [BindProperty]
        public CinemaRecord NewFilmography { get; set; } = default!;

        [BindProperty]
        public EditMainStudio? EditMain { get; set; }

        [BindProperty]
        public EditImage? EditLogo { get; set; }

        public StudioModel(IImageService imageService, UISettings settings)
        {
            _imageService = imageService;
            _settings = settings;
        }

        public async Task<IActionResult> OnGet([FromRoute] string id)
        {

            Studio = new Studio
            {
                Id = id,
                Name = "Long Long Name Long Long Long Surname",
                FoundDate = new DateOnly(1938, 10, 7),
                Country = 0,
                PresidentName = "Name Name Surname",
                Picture = "/img/logo_placeholder.png",
                Filmography = new CinemaRecord[] {
                    new CinemaRecord { Id = "1", Name = "Cinema Title Long Long", Year=2000, Picture=null},
                    new CinemaRecord { Id = "2", Name = "Cinema Title Long", Year=2000, Picture=null},
                    new CinemaRecord { Id = "3", Name = "Cinema Title", Year=2000, Picture=null},
                    new CinemaRecord { Id = "4", Name = "Cinema Title", Year=2000, Picture=null}
                },
                Description = "Studio description goes here. A studio with a long history.."
            };

            if (Studio.Description is null)
            {
                Studio.Description = "";
            }

            EditMain = new EditMainStudio { Id = Studio.Id, Name = Studio.Name, FoundDate = Studio.FoundDate, Country = Studio.Country, PresidentName = Studio.PresidentName, Description = Studio.Description };

            EditLogo = new EditImage { ImageId = null, ImageUri = Studio.Picture };

            NewFilmography = new CinemaRecord { ParentId = Studio.Id, Id = "", Name = "", Picture = _settings.DefaultSmallPosterPicture };


            return Page();
        }

        public async Task<IActionResult> OnPostEditStudio([FromRoute] string id)
        {
            // Implement: convert EditCinema to Cinema and send put request to mediatre proxy

            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            if (EditMain != null)
            {
                Studio.Id = id;
                Studio.Name = EditMain.Name.Trim();
                Studio.PresidentName = EditMain.PresidentName == null? String.Empty : Studio.PresidentName!.Trim();
                Studio.Description = EditMain.Description == null ? null : EditMain.Description.Trim();
            }

            return await OnGet(id);
        }

        public async Task<IActionResult> OnPostAddFilmography([FromRoute] string id)
        {
            // Implement: set ParentId and send post NewFilmography to mediatre proxy 

            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            return await OnGet(id);
        }

        public async Task<IActionResult> OnPostDeleteFilmography([FromRoute] string id)
        {
            // Implement: send delete request specifying ParentId and Id to mediatre proxy

            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            return await OnGet(id);
        }

        public async Task<IActionResult> OnPostEditLogo([FromRoute] string id)
        {
            // Implement: after receiving image name send put request to mediatre proxy

            if (Studio != null)
            {
                Studio.Id = id;
            }

            if (EditLogo.Image is null)
            {
                // handle error 
                return await OnGet(id);
            }

            if (EditLogo.Image.Length >= _settings.MaxFileLength)
            {
                // handle error?
                return await OnGet(id);
            }

            string imageName = EditLogo.Image.FileName;
            string imageExt = Path.GetExtension(imageName);

            string HashName = imageName.SHA_1() + imageExt;

            if (EditLogo.ImageId is null || EditLogo.ImageId == String.Empty)
            {
                // if cinema yet has no image

                await _imageService.AddImage(HashName, EditLogo.Image.OpenReadStream().ToBase64(), _settings.SizesToInclude);
            }
            else if (EditLogo.ImageId != HashName)
            {
                // if cinema already has an image

                await _imageService.ReplaceImage(EditLogo!.ImageId, HashName, EditLogo.Image.OpenReadStream().ToBase64(), _settings.SizesToInclude);
            }

            return await OnGet(id);
        }
    }
}
