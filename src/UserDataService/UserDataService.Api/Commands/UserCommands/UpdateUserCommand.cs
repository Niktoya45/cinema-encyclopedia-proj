using MediatR;
using UserDataService.Infrastructure.Models.UserDTO;

namespace UserDataService.Api.Commands.UserCommands
{

    public class UpdateUserCommand : IRequest<UpdateUserResponse>
    {

        public UpdateUserCommand(string id, string name, string auserId)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }

        public string Name { get; }
    }
}