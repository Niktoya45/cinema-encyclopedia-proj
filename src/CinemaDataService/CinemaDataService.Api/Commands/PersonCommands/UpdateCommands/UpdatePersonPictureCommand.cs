using CinemaDataService.Api.Commands.SharedCommands.UpdateCommands;

namespace CinemaDataService.Api.Commands.PersonCommands.UpdateCommands
{
    public class UpdatePersonPictureCommand:UpdatePictureCommand
    {
        public UpdatePersonPictureCommand(string id, string? picture) : base(id, picture)
        {
        }
    }
}
