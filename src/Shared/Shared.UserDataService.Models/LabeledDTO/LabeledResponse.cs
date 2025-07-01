using Shared.UserDataService.Models.Flags;

namespace Shared.UserDataService.Models.LabeledDTO
{
    public class LabeledResponse
    {
        public string CinemaId { get; set; }
        public string UserId { get; set; }
        public Label Label { get; set; }
	public DateTime AddedAt {get; set;}
    }
}
