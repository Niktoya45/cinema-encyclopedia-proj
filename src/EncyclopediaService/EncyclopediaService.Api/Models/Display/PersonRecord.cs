namespace EncyclopediaService.Api.Models.Display
{
    public class PersonRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; }
        public string? PictureYri { get; set; }
        public Job Jobs { get; set; }
    }
}
