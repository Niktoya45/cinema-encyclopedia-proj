using CinemaDataService.Api.Commands.SharedCommands.UpdateCommands;

namespace CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands
{
    public class UpdateCinemaPictureCommand:UpdatePictureCommand
    {
        public UpdateCinemaPictureCommand(string id, string? picture):base(id, picture)
        {
        }
    }
}
