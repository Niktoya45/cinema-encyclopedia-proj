using MediatR;
using UserDataService.Infrastructure.Models.UserDTO;

namespace UserDataService.Api.Commands.UserCommands.CreateCommands
{
    public class CreateUserCommand : IRequest<CreateUserResponse>
    {

        public CreateUserCommand(
            string username, 
            DateOnly? birthdate, 
            string? picture,
            string? description
            )
        {
            Username = username;
            Birthdate = birthdate;
            Picture = picture;
            Description = description;
        }

        public string Username { get; }
        public DateOnly? Birthdate { get; }
        public string? Picture { get; }
        public string? Description { get; }
    }
}