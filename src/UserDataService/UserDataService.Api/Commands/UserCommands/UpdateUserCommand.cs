using MediatR;
using UserDataService.Infrastructure.Models.UserDTO;

namespace UserDataService.Api.Commands.UserCommands
{

    public class UpdateUserCommand : IRequest<UpdateUserResponse>
    {

        public UpdateUserCommand(
            string id,
            string username,
            DateOnly? birthdate,
            string? picture
            )
        {
            Id = id;
            Username = username;
            Birthdate = birthdate;
            Picture = picture;
        }
        public string Id { get; }
        public string Username { get; }
        public DateOnly? Birthdate { get; }
        public string? Picture { get; }
    }
}