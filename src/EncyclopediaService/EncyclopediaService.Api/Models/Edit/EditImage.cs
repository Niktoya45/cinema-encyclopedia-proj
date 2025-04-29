namespace EncyclopediaService.Api.Models.Edit
{
    public class EditImage
    {
        public string? ImageCurrent { get; set; }
        public IFormFile? Image { get; set; }
    }
}
