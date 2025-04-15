using CinemaDataService.Api.Commands.SharedCommands.DeleteCommands;

namespace CinemaDataService.Api.Commands.StudioCommands.DeleteCommands
{
    public class DeleteStudioFilmographyCommand : DeleteFilmographyCommand
    {
        public DeleteStudioFilmographyCommand(string? studioId, string cinemaId) : base(studioId, cinemaId) { }
    }
}
