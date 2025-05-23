using CinemaDataService.Api.Commands.SharedCommands.UpdateCommands;

namespace CinemaDataService.Api.Commands.PersonCommands.UpdateCommands
{
    public class UpdatePersonFilmographyCommand:UpdateFilmographyCommand
    {
        public UpdatePersonFilmographyCommand(
            string? parentId,
            string id,
            string name,
            int year,
            string? picture
        ) : base(parentId, id, name, year, picture) { }
    }
}
