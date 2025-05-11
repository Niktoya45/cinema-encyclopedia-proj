using UserDataService.Domain.Aggregates.UserAggregate;

namespace UserDataService.Infrastructure.Models.LabeledDTO
{
    public class CreateLabeledRequest
    {
        public string CinemaId { get; set; }
        public Label Label { get; set; }
    }
}
