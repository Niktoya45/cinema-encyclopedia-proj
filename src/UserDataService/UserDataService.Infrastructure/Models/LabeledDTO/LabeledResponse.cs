using UserDataService.Domain.Aggregates.UserAggregate;

namespace UserDataService.Infrastructure.Models.LabeledDTO
{
    public class LabeledResponse
    {
        public string CinemaId { get; set; }
        public string UserId { get; set; }
        public Label Label { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
