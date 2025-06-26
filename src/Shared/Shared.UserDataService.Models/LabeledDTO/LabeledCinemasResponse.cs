
namespace Shared.UserDataService.Models.LabeledDTO
{
    public class LabeledCinemasResponse<TCinema> where TCinema : class
    {
        public IList<LabeledCinemaResponse<TCinema>> LabeledCinemas { get; set; }
        public string UserId { get; set; }
    }
}
