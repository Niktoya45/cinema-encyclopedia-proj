using MediatR;
using UserDataService.Infrastructure.Models.UserDTO;

namespace UserDataService.Api.Commands.UserCommands.DeleteCommands
{
    public class DeleteUserCommand : IRequest<Unit>
    {

        public DeleteUserCommand(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}