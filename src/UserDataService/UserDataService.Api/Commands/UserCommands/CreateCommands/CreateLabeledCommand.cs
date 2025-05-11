using MediatR;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Models.LabeledDTO;

namespace UserDataService.Api.Commands.UserCommands.CreateCommands
{
    public class CreateLabeledCommand : IRequest<LabeledResponse>
    {
        public CreateLabeledCommand(
            string userId,
            string cinemaId,
            Label label
            )
        {
            CinemaId = cinemaId;
            UserId = userId;
            Label = label;
        }
        public string CinemaId { get; set; }
        public string UserId { get; set; }
        public Label Label { get; set; }
    }
}
