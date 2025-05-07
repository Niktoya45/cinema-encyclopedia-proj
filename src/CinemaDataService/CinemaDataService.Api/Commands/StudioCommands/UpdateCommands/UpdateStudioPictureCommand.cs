using CinemaDataService.Api.Commands.SharedCommands.UpdateCommands;

namespace CinemaDataService.Api.Commands.StudioCommands.UpdateCommands
{
    public class UpdateStudioPictureCommand:UpdatePictureCommand
    {
        public UpdateStudioPictureCommand(string id, string? picture) : base(id, picture)
        {
        }
    }
}
