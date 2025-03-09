using CinemaDataService.Domain.Aggregates.Base;

namespace CinemaDataService.Domain.Aggregates.Shared
{
    public record CinemaRecord:Value
    {
        int Year { get; set; }
    }
}
