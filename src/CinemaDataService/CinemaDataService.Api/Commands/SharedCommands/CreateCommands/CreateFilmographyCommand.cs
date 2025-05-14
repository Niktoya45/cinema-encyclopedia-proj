using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Models.RecordDTO;
using MediatR;

namespace CinemaDataService.Api.Commands.SharedCommands.CreateCommands
{
    public class CreateFilmographyCommand:IRequest<FilmographyResponse>
    {
        public CreateFilmographyCommand(
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
