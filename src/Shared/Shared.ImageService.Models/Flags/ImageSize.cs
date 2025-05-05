
namespace Shared.ImageService.Models.Flags
{
    [Flags]
    public enum ImageSize
    {
        None   = 0b_0000_0000,
        Tiny   = 0b_0000_0001,
        Small  = 0b_0000_0010,
        Medium = 0b_0000_0100,
        Big    = 0b_0000_1000,
        Large  = 0b_0001_0000
    }
}
