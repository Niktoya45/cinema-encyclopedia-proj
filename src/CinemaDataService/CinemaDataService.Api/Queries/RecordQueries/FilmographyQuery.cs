using CinemaDataService.Infrastructure.Models.RecordDTO;
using MediatR;

namespace CinemaDataService.Api.Queries.RecordQueries
{
    public class FilmographyQuery : IRequest<FilmographyResponse>
    {
        public FilmographyQuery(string? parentId, string id)
        {
            ParentId = parentId;
            Id = id;
        }

        public string? ParentId { get; set; }
        public string Id { get; set; }
    }
}
