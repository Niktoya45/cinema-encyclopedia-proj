using ProfileService.Api.Models.Edit;
using ProfileService.Api.Models.Utils;
using ProfileService.Api.Models;
using ProfileService.Infrastructure.Services.ImageService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProfileService.Api.Extensions;


namespace ProfileService.Api.Views.Profiles
{
    public class ProfileModel : PageModel
    {
        private IImageService _imageService { get; init; }
        private UISettings _settings { get; init; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public Profile? Profile { get; set; }

        [BindProperty]
        public EditMain? EditMain { get; set; }

        [BindProperty]
        public EditImage? EditProfilePicture { get; set; }

        public ProfileModel(IImageService imageService, UISettings settings)
        {
            _imageService = imageService;
            _settings = settings;
        }

        public async Task<IActionResult> OnGet([FromRoute] string id)
        {

            Profile = new Profile
            {
                Id = id,
                Username = "Long Long Profile Name",
                Birthdate = new DateOnly(1938, 10, 7),
                Picture = "/img/logo_placeholder.png",
                Description = "Generally interested in alternative horrors and.."
            };

            if (Profile.Picture is null)
            {
                Profile.Picture = _settings.DefaultProfilePicture;
            }

            if (Profile.Description is null)
            {
                Profile.Description = "";
            }

            EditMain = new EditMain { Id=Profile.Id, Username=Profile.Username, Birthdate = Profile.Birthdate, Description=Profile.Description };

            EditProfilePicture = new EditImage { ImageId = Profile.Picture, ImageUri = Profile.PictureUrl };

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

            return await OnGet(id);
        }

        public async Task<IActionResult> OnPostEditProfilePicture([FromRoute] string id)
        {
            // Implement: after receiving image name send put request to mediatre proxy

            if (Profile != null)
            {
                Profile.Id = id;
            }

            if (EditProfilePicture.Image is null)
            {
                // handle error 
                return await OnGet(id);
            }

            if (EditProfilePicture.Image.Length >= _settings.MaxFileLength)
            {
                // handle error?
                return await OnGet(id);
            }

            string imageName = EditProfilePicture.Image.FileName;
            string imageExt = Path.GetExtension(imageName);

            string HashName = imageName.SHA_1() + imageExt;

            if (EditProfilePicture.ImageId is null || EditProfilePicture.ImageId == String.Empty)
            {
                // if profile yet has no image

                await _imageService.AddImage(HashName, EditProfilePicture.Image.OpenReadStream().ToBase64(), _settings.SizesToInclude);
            }
            else if (EditProfilePicture.ImageId != HashName)
            {
                // if profile already has an image

                await _imageService.ReplaceImage(EditProfilePicture!.ImageId, HashName, EditProfilePicture.Image.OpenReadStream().ToBase64(), _settings.SizesToInclude);
            }

            return await OnGet(id);
        }
    }
}
