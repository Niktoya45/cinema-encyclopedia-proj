using CinemaDataService.Api.Commands.SharedCommands.CreateCommands;

namespace CinemaDataService.Api.Commands.StudioCommands.CreateCommands
{
    public class CreateStudioFilmographyCommand : CreateFilmographyCommand
    {
        public CreateStudioFilmographyCommand(
            string? studioId,
            string id,
            string name,
            int year,
            string? picture
        ) : base(id, name, year, picture)
        {
            StudioId = studioId;
        }

        public string? StudioId { get; }
    }
}
