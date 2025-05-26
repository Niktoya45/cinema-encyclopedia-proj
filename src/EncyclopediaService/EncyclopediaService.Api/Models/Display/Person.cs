using System.ComponentModel.DataAnnotations;

namespace EncyclopediaService.Api.Models.Display
{
    /*
     * vvv src/Shared/.. vvv
     */
    /**/ public enum Job 
    {
        [Display(Name=". . .")]
        None = 0b_0000_0000,
        Director = 0b_0000_0001,
        Producer = 0b_0000_0010,
        Scenarist = 0b_0000_0100,
        Actor = 0b_0000_1000
    }
    /**/ public enum Country;

    public class Person
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateOnly BirthDate { get; set; }
        public Country Country { get; set; }
        public Job Jobs { get; set; }
        public string? Picture { get; set; }
        public CinemaRecord[]? Filmography { get; set; }
        public string? Description { get; set; }
    }
}
