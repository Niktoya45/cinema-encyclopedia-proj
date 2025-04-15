using CinemaDataService.Api.Commands.SharedCommands.CreateCommands;

namespace CinemaDataService.Api.Commands.PersonCommands.CreateCommands
{
    public class CreatePersonFilmographyCommand:CreateFilmographyCommand
    {
        public CreatePersonFilmographyCommand(
            string? personId,
            string id,
            string name,
            int year,
            string? picture
        ) : base(id, name, year, picture)
        {
            PersonId = personId;
        }

        public string? PersonId { get; }
    }
}
