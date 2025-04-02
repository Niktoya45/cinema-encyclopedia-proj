using MediatR;
using UserDataService.Infrastructure.Models.UserDTO;

namespace UserDataService.Api.Commands.UserCommands
{
    public class CreateUserCommand : IRequest<CreateUserResponse>
    {

        public CreateUserCommand(string name, string auserId)
        {
            Name = name;
        }

        public string Name { get; }
    }
}