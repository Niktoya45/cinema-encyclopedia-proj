using MediatR;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;

namespace CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands
{

    public class UpdateCinemaCommand : IRequest<CinemaResponse>
    {

        public UpdateCinemaCommand(
            string id, 
            string name,
            DateOnly releaseDate,
            Genre genres,
            Language language,
            string? picture,
            StudioRecord[]? productionStudios,
            Starring[]? starrings,
            string? description
            )
        {
            Id = id;
            Name = name;
            ReleaseDate = releaseDate;
            Genres = genres;
            Language = language;
            Picture = picture;
            ProductionStudios = productionStudios;
            Starrings = starrings;
            Description = description;
        }
        public string Id { get; }
        public string Name { get; }
        public DateOnly ReleaseDate { get; }
        public Genre Genres { get; }
        public Language Language { get; }
        public string? Picture { get; }
        public StudioRecord[]? ProductionStudios { get; }
        public Starring[]? Starrings { get; }
        public string? Description { get; }
    }
}