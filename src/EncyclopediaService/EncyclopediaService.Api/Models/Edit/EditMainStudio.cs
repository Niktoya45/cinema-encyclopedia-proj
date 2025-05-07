namespace EncyclopediaService.Api.Models.Edit
{
    public class EditMainStudio
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateOnly FoundDate { get; set; }
        public Country Country { get; set; }
        public string? PresidentName { get; set; }
        public string? Description { get; set; }
    }
}
