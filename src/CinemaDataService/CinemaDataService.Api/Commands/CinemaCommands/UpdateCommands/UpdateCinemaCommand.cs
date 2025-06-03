using MediatR;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.RecordDTO;

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
            CreateProductionStudioRequest[]? productionStudios,
            CreateStarringRequest[]? starrings,
            string? description
            )
        {
            Id = id;
            Name = name;
            ReleaseDate = releaseDate;
            Genres = genres;
            Language = language;
            Picture = picture;
            ProductionStudios = productionStudios is null ? null
                : productionStudios.Select(psr =>
                        new StudioRecord
                        {
                            Id = psr.Id,
                            Name = psr.Name,
                            Picture = psr.Picture
                        }
                        ).ToArray();
            Starrings = starrings is null ? null
                : starrings.Select(sr =>
                new Starring
                {
                    Id = sr.Id,
                    Name = sr.Name,
                    Jobs = sr.Jobs,
                    ActorRole = sr.RoleName is null ? null : new ActorRole { Name = sr.RoleName, Priority = sr.RolePriority },
                    Picture = sr.Picture
                }
                ).ToArray();
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