
using Shared.CinemaDataService.Models.RecordDTO;
using Shared.CinemaDataService.Models.Flags;

namespace Shared.CinemaDataService.Models.RecordDTO
{
    public record Starring:Value
    {
        public Job Jobs { get; set; } 
        public ActorRole? ActorRole { get; set; }
    }
}
