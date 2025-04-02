
namespace UserDataService.Domain.Aggregates.UserAggregate
{
    [Flags]
    public enum Label
    {
           None = 0b_0000_0000,
        Favored = 0b_0000_0001,
        Planned = 0b_0000_0010,
       Watching = 0b_0000_0100,
           Seen = 0b_0000_1000        
    }
}
