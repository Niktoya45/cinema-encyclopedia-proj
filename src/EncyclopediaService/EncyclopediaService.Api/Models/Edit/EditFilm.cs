namespace EncyclopediaService.Api.Models.Edit
{
    public class EditFilm
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string? Picture { get; set; } = default;
    }
}
