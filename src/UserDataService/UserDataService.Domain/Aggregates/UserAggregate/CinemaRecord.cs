

namespace UserDataService.Domain.Aggregates.UserAggregate
{
    public record CinemaRecord
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public Label Label { get; set; } = Label.None;
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
