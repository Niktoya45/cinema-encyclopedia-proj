using UserDataService.Domain.Aggregates.UserAggregate;

namespace UserDataService.Infrastructure.Models.LabeledDTO
{
    public class LabeledCinemaResponse<TCinema> where TCinema : class
    {
        public TCinema Cinema { get; set; }
        public Label Label { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
