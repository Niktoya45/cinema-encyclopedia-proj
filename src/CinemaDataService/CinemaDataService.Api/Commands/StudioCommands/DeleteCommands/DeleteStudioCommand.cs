using MediatR;
using CinemaDataService.Infrastructure.Models.StudioDTO;

namespace CinemaDataService.Api.Commands.StudioCommands.DeleteCommands
{
    public class DeleteStudioCommand : IRequest<Unit>
    {
        public DeleteStudioCommand(string id)
        {
            Id = id;
        }
        public DeleteStudioCommand(string id, string auserId)
        {
            Id = id;
        }

        public string Id { get; }
    }
}