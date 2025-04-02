using MediatR;
using CinemaDataService.Infrastructure.Models.PersonDTO;

namespace CinemaDataService.Api.Commands.PersonCommands
{
    public class DeletePersonCommand : IRequest<Unit>
    {
        public DeletePersonCommand(string id)
        {
            Id = id;
        }

        public DeletePersonCommand(string id, string auserId)
        {
            Id = id;
        }

        public string Id { get; }
    }
}