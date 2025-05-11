using MediatR;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Models.LabeledDTO;

namespace UserDataService.Api.Commands.UserCommands.DeleteCommands
{
    public class DeleteLabeledCommand : IRequest<LabeledResponse>
    {
        public DeleteLabeledCommand(
            string userId,
            string cinemaId
            )
        {
            CinemaId = cinemaId;
            UserId = userId;
        }
        public string CinemaId { get; set; }
        public string UserId { get; set; }
    }
}
