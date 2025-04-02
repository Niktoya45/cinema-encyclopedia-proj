using MediatR;
using UserDataService.Infrastructure.Models.UserDTO;

namespace UserDataService.Api.Commands.UserCommands
{
    public class DeleteUserCommand : IRequest<Unit>
    {

        public DeleteUserCommand(string id, string auserId)
        {
            Id = id;
        }

        public string Id { get; }
    }
}