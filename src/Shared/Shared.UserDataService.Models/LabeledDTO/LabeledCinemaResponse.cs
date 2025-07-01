using Shared.UserDataService.Models.Flags;

namespace Shared.UserDataService.Models.LabeledDTO
{
    public class LabeledCinemaResponse<TCinema> where TCinema : class
    {
        public TCinema Cinema { get; set; }
        public Label Label { get; set; }
	public DateTime AddedAt { get; set; }
    }
}
