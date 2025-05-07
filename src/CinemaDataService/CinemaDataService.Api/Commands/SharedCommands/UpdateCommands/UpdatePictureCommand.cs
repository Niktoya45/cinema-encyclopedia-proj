using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using MediatR;

namespace CinemaDataService.Api.Commands.SharedCommands.UpdateCommands
{
    public class UpdatePictureCommand : IRequest<UpdatePictureResponse>
    {
        public string Id { get; set; }
        public string? Picture { get; set; }

        public UpdatePictureCommand(string id, string? picture)
        {
            Id = id;
            Picture = picture;
        }
    }
}
