using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.JsonWebTokens;
using ProfileService.Api.Extensions;
using ProfileService.Api.Models.Display;
using ProfileService.Api.Models.Edit;
using ProfileService.Api.Models.TestData;
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

        public async Task<IActionResult> OnGet([FromRoute] string id, CancellationToken ct)
        {

            if (TestEntities.Used)
            { 
                Profile = TestEntities.Profile;

                Role = "User,Administrator";
            }
            else {
                var response = await _gatewayService.GetUser(id, ct);

                if (response is null)
                {
                    return RedirectToPage("/Index", "Encyclopedia");
                }

                Profile = new Profile
                {
                    Id = response.Id,
                    Username = response.Username,
                    Picture = response.Picture,
                    PictureUri = response.PictureUri,
                    Birthdate = response.Birthdate,
                    Description = response.Description
                };

                if (User.IsSuperAdmin())
                {
                    var responseRole = await _gatewayService.GetUserRole(id, ct);

                    Role = responseRole;
                }

            }
            EditMain = new EditMain { Id=Profile.Id, Username=Profile.Username, Birthdate = Profile.Birthdate, Description=Profile.Description };

            EditProfilePicture = new EditImage { ImageId = Profile.Picture, ImageUri = Profile.PictureUri };

            return Page();
        }

        public async Task<IActionResult> OnPostEditMain([FromRoute] string id, CancellationToken ct)
        {
            // Implement: convert EditCinema to Cinema and send put request to mediatre proxy

            if (!ModelState.IsValid)
            {
                return OnPostReuseEditMain(true);
            }

            if (EditMain != null)
            {
                EditMain.Id = id;

                if (!TestEntities.Used)
                {
                    await _gatewayService.UpdateUser(id, new Shared.UserDataService.Models.UserDTO.UpdateUserRequest 
                    {
                        Username = EditMain.Username,
                        Birthdate = EditMain.Birthdate,
                        Description = EditMain.Description
                    }, ct);
                }
            }

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditPicture([FromRoute] string id, CancellationToken ct)
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
                Id = EditProfilePicture.ImageId??"0",
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
            var response = await _gatewayService.AddUserRole(id, "Administrator", ct);

            return new OkObjectResult("ok");
            return new OkObjectResult(response != null ? "ok" : "error");
        }

        public async Task<IActionResult> OnPostRevokeAdminRole([FromRoute] string id, CancellationToken ct)
        {
            var response = await _gatewayService.RemoveUserRole(id, "Administrator", ct);

            return new OkObjectResult("ok");
            return new OkObjectResult(response != null ? "ok" : "error");
        }

        public async Task<IActionResult> OnPostDeleteProfile([FromRoute] string id, CancellationToken ct)
        {
            var response = await _gatewayService.Delete(id, ct);

            return new OkObjectResult(response ? "ok" : "error");
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
