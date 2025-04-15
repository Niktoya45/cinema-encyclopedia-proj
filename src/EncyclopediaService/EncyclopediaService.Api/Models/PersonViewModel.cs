namespace EncyclopediaService.Api.Models
{
    /*
     * vvv src/Shared/.. vvv
     */
    /**/ public enum Jobs;
    /**/ public enum Country;
    /**/ public record CinemaRecord { }

    public class PersonViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateOnly BirthDate { get; set; }
        public int Country { get; set; }
        public int Jobs { get; set; }
        public string? Picture { get; set; }
        public CinemaRecord[]? Filmography { get; set; }
        public string? Description { get; set; }
    }
}
