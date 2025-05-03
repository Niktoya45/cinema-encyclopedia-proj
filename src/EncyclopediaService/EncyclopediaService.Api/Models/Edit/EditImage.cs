namespace EncyclopediaService.Api.Models.Edit
{
    public class EditImage
    {
        public string? ImageUri { get; set; }
        public string? ImageId { get; set; }
        public IFormFile? Image { get; set; }
    }
}
