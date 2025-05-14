using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Domain.Aggregates.Shared;

namespace CinemaDataService.Infrastructure.Models.RecordDTO
{
    public class StarringResponse
    {
        public string? ParentId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public Job Jobs { get; set; }
        public string? RoleName { get; set; }
        public RolePriority? RolePriority { get; set; }
        public string? Picture { get; set; } = default;
        public string? PictureUri { get; set; } = default;
    }
}
