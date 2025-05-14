using CinemaDataService.Infrastructure.Models.RecordDTO;
using MediatR;

namespace CinemaDataService.Api.Commands.SharedCommands.UpdateCommands
{
    public class UpdateFilmographyCommand:IRequest<FilmographyResponse>
    {
        public UpdateFilmographyCommand(
            string id,
            string name,
            int year,
            string? picture
        )
        {
            Id = id;
            Name = name;
            Picture = picture;
            Year = year;
        }
        public string Id { get; }
        public string Name { get; }
        public int Year { get; }
        public string? Picture { get; }
    }
}
