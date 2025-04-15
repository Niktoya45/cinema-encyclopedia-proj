using CinemaDataService.Api.Commands.SharedCommands.DeleteCommands;

namespace CinemaDataService.Api.Commands.PersonCommands.DeleteCommands
{
    public class DeletePersonFilmographyCommand:DeleteFilmographyCommand
    {
        public DeletePersonFilmographyCommand(string? personId, string cinemaId) : base(personId, cinemaId) { }
    }
}
