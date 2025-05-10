

namespace CinemaDataService.Domain.Aggregates.CinemaAggregate
{
    public record RatingScore
    {
        public double Score { get; set; } = 0.0;
        public uint N { get; set; } = 0;
    }
}
