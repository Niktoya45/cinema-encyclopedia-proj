

namespace UserDataService.Domain.Aggregates.UserAggregate
{
    public record CinemaRecord
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public Label Label { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
