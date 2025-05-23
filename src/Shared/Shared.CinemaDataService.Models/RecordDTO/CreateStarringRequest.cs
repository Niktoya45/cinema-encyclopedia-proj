
using Shared.CinemaDataService.Models.Flags;

namespace Shared.CinemaDataService.Models.RecordDTO
{
    public class CreateStarringRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Job Jobs { get; set; }
        public string? RoleName { get; set; }
        public RolePriority RolePriority { get; set; }
        public string? Picture { get; set; } = default;
    }
}
