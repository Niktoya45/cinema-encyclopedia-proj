using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.JsonWebTokens;
using ProfileService.Api.Extensions;
using ProfileService.Api.Models;
using ProfileService.Api.Models.Edit;
using ProfileService.Api.Models.Utils;
using ProfileService.Infrastructure.Services.GatewayService;
using Shared.ImageService.Models.Flags;
using Shared.ImageService.Models.ImageDTO;
using System.Security.Claims;


namespace ProfileService.Api.Views.Profiles
{
    public class ProfileModel : PageModel
    {
        private IGatewayService _gatewayService { get; init; }
        private UISettings _settings { get; init; }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        public string Role { get; set; }

        [BindProperty]
        public Profile? Profile { get; set; }

        [BindProperty]
        public EditMain? EditMain { get; set; }

        [BindProperty]
        public EditImage EditProfilePicture { get; set; }

        public ProfileModel(IGatewayService gatewayService, UISettings settings)
        {
            _gatewayService = gatewayService;
            _settings = settings;
        }

        public async Task<IActionResult> OnGet([FromRoute] string id)
        {
            Profile = new Profile
            {
                Id = id,
                Username = "Long Long Profile Name",
                Birthdate = new DateOnly(2000, 1, 1),
                //Picture = "/img/logo_placeholder.png",
                Description = "Generally interested in alternative horrors and.."
            };

            Role = "User,Administrator";

            EditMain = new EditMain { Id=Profile.Id, Username=Profile.Username, Birthdate = Profile.Birthdate, Description=Profile.Description };

            EditProfilePicture = new EditImage { ImageId = Profile.Picture, ImageUri = Profile.PictureUri };

            return Page();
        }

        public async Task<IActionResult> OnPostEditProfile([FromRoute] string id)
        {
            // Implement: convert EditCinema to Cinema and send put request to mediatre proxy

            if (!ModelState.IsValid)
            {
                return Redirect("/Error");
            }

            if (EditMain != null)
            {
                Profile.Id = id;
                Profile.Username = EditMain.Username.Trim();
                Profile.Description = EditMain.Description == null ? null : EditMain.Description.Trim();
            }

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditProfilePicture([FromRoute] string id, CancellationToken ct)
        {
            // Implement: after receiving image name send put request to mediatre proxy

            if (EditProfilePicture.Image is null)
            {
                return new OkObjectResult(new { PictureUri = EditProfilePicture.ImageUri });
            }

            if (EditProfilePicture.Image.Length >= _settings.MaxFileLength)
            {
                return new OkObjectResult(new { PictureUri = EditProfilePicture.ImageUri });
            }

            string imageName = EditProfilePicture.Image.FileName;
            string imageExt = Path.GetExtension(imageName);

            string HashName = imageName.SHA_1() + imageExt;
            string HashImage = EditProfilePicture.Image.OpenReadStream().ToBase64();

            var response = await _gatewayService.UpdateUserPhoto(id, new ReplaceImageRequest
            {
                Id = EditProfilePicture.ImageId,
                NewId = HashName,
                Size = (ImageSize)31,
                FileBase64 = HashImage
            },
                ct);

            if (response is null)
            {
                return new OkObjectResult(new { PictureUri = EditProfilePicture.ImageUri });
            }

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostAddAdminRole([FromRoute] string id, CancellationToken ct)
        {
            return new OkObjectResult("ok");
        }

        public async Task<IActionResult> OnPostRevokeAdminRole([FromRoute] string id, CancellationToken ct)
        {
            return new OkObjectResult("ok");
        }

        public async Task<IActionResult> OnPostDeleteProfile()
        {
            return new OkObjectResult("ok");
        }

        public IActionResult OnPostReuseEditMain(bool error = false)
        {
            if (!error)
            {
                ModelState.Clear();
            }
            return Partial("_EditMain", EditMain);

        }
    }
}
