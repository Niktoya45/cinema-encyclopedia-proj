
using MediatR;
using UserDataService.Infrastructure.Models.SharedDTO;

namespace UserDataService.Api.Commands.UserCommands.UpdateCommands
{
    public class UpdateUserPictureCommand : IRequest<UpdatePictureResponse>
    {
        public string Id { get; set; }
        public string? Picture { get; set; }

        public UpdateUserPictureCommand(string id, string? picture)
        {
            Id = id;
            Picture = picture;
        }
    }
}
