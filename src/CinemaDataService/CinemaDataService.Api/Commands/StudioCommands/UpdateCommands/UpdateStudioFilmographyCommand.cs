using CinemaDataService.Api.Commands.SharedCommands.UpdateCommands;

namespace CinemaDataService.Api.Commands.StudioCommands.UpdateCommands
{
    public class UpdateStudioFilmographyCommand : UpdateFilmographyCommand
    {
        public UpdateStudioFilmographyCommand(
            string id,
            string name,
            int year,
            string? picture
        ) : base(id, name, year, picture) { }
    }
}
