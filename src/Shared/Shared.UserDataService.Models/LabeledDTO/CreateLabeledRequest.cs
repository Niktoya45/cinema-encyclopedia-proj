using Shared.UserDataService.Models.Flags;

namespace Shared.UserDataService.Models.LabeledDTO
{
    public class CreateLabeledRequest
    {
        public string CinemaId { get; set; }
        public Label Label { get; set; }
    }
}
