using MediatR;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;

namespace CinemaDataService.Api.Commands.CinemaCommands.CreateCommands
{
	public class CreateCinemaCommand : IRequest<CinemaResponse>
	{
		public CreateCinemaCommand(
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
			Name = name;
			ReleaseDate = releaseDate;
			Genres = genres;
			Language = language;
			Picture = picture;
			ProductionStudios = productionStudios;
			Starrings = starrings;
			Description = description;
		}
        public string Name { get; }
        public DateOnly ReleaseDate { get; }
        public Genre Genres { get; }
        public Language Language { get; }
        public string? Picture { get; }
        public StudioRecord[]? ProductionStudios { get; }
        public Starring[]? Starrings { get;}
        public string? Description { get; }
    }
}