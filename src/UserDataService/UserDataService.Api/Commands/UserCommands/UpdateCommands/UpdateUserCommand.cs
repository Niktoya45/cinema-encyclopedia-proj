using MediatR;
using UserDataService.Infrastructure.Models.UserDTO;

namespace UserDataService.Api.Commands.UserCommands.UpdateCommands
{

    public class UpdateUserCommand : IRequest<UpdateUserResponse>
    {

        public UpdateUserCommand(
            string id,
            string username,
            DateOnly? birthdate,
            string? picture,
            string? description
            )
        {
            Id = id;
            Username = username;
            Birthdate = birthdate;
            Picture = picture;
            Description = description;
        }
        public string Id { get; }
        public string Username { get; }
        public DateOnly? Birthdate { get; }
        public string? Picture { get; }
        public string? Description { get; }
    }
}