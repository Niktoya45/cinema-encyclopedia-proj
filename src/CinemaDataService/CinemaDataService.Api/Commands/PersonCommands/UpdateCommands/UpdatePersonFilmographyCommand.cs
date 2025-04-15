using CinemaDataService.Api.Commands.SharedCommands.UpdateCommands;

namespace CinemaDataService.Api.Commands.PersonCommands.UpdateCommands
{
    public class UpdatePersonFilmographyCommand:UpdateFilmographyCommand
    {
        public UpdatePersonFilmographyCommand(
            string id,
            string name,
            int year,
            string? picture
        ) : base(id, name, year, picture) { }
    }
}
