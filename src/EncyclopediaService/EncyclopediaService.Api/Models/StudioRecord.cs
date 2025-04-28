namespace EncyclopediaService.Api.Models
{
    public class StudioRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string? Picture { get; set; }
        public bool IsDeleted { get; set; }
    }
}
