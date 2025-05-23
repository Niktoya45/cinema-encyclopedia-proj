using MediatR;
using CinemaDataService.Infrastructure.Models.RecordDTO;

namespace CinemaDataService.Api.Queries.RecordQueries
{
    public class ProductionStudioQuery : IRequest<ProductionStudioResponse>
    {
        public ProductionStudioQuery(string? parentId, string id) 
        { 
            ParentId = parentId;
            Id = id;
        }

        public string? ParentId { get; set; }
        public string Id { get; set; }
    }
}
