
namespace Shared.CinemaDataService.Models.Flags
{
    [Flags]
    public enum Genre
    {
               None = 0b_0000_0000_0000_0000,
             Action = 0b_0000_0000_0000_0001,
             Comedy = 0b_0000_0000_0000_0010,
        Documentary = 0b_0000_0000_0000_0100,
              Drama = 0b_0000_0000_0000_1000,
            Fantasy = 0b_0000_0000_0001_0000,
             Horror = 0b_0000_0000_0010_0000,
            Musical = 0b_0000_0000_0100_0000,
            Mystery = 0b_0000_0000_1000_0000,
            Romance = 0b_0000_0001_0000_0000,
              SciFi = 0b_0000_0010_0000_0000,
           Thriller = 0b_0000_0100_0000_0000,
            Western = 0b_0000_1000_0000_0000
    }
}
