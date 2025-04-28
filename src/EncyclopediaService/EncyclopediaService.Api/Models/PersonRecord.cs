namespace EncyclopediaService.Api.Models
{
    public class PersonRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; }
        public Job Jobs { get; set; }
        public bool IsDeleted { get; set; }
    }
}
