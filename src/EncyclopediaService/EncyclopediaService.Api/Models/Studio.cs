namespace EncyclopediaService.Api.Models
{
    /*
     * vvv src/Shared/.. vvv
     */
    /* public enum Jobs;*/
    /* public enum Country;*/
    /* public record CinemaRecord { }*/
    public class Studio
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateOnly FoundDate { get; set; }
        public Country Country { get; set; }
        public string? Picture { get; set; }
        public CinemaRecord[]? Filmography { get; set; }
        public string? PresidentName { get; set; }
        public string? Description { get; set; }
    }
}
