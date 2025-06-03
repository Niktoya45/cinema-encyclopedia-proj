using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using MediatR;

namespace CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands
{
    public class UpdateCinemaMainCommand : IRequest<CinemaResponse>
    {

        public UpdateCinemaMainCommand(
            string id,
            string name,
            DateOnly releaseDate,
            Genre genres,
            Language language,
            string? description
            )
        {
            Id = id;
            Name = name;
            ReleaseDate = releaseDate;
            Genres = genres;
            Language = language;
            Description = description;
        }
        public string Id { get; }
        public string Name { get; }
        public DateOnly ReleaseDate { get; }
        public Genre Genres { get; }
        public Language Language { get; }
        public string? Description { get; }
    }
}
