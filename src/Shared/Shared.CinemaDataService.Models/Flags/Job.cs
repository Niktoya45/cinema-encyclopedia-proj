
namespace Shared.CinemaDataService.Models.Flags
{
    [Flags]
    public enum Job
    {
            None  = 0b_0000_0000,
        Director  = 0b_0000_0001,
        Producer  = 0b_0000_0010,
        Scenarist = 0b_0000_0100,
            Actor = 0b_0000_1000
    }
}
