using MediatR;
using UserDataService.Infrastructure.Models.UserDTO;

namespace UserDataService.Api.Commands.UserCommands
{
    public class CreateUserCommand : IRequest<CreateUserResponse>
    {

        public CreateUserCommand(
            string username, 
            DateOnly? birthdate, 
            string? picture
            )
        {
            Username = username;
            Birthdate = birthdate;
            Picture = picture;
        }

        public string Username { get; }
        public DateOnly? Birthdate { get; }
        public string? Picture { get; }
    }
}