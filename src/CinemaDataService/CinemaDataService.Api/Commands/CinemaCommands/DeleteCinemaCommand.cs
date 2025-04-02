using MediatR;
using CinemaDataService.Infrastructure.Models.CinemaDTO;

namespace CinemaDataService.Api.Commands.CinemaCommands
{
    public class DeleteCinemaCommand : IRequest<Unit>
    {
        public DeleteCinemaCommand(string id)
        {
            Id = id;
        }
        public DeleteCinemaCommand(string id, string auserId)
        {
            Id = id;
        }

        public string Id { get; }
    }
}