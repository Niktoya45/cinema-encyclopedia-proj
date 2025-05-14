using Shared.CinemaDataService.Models.Flags;

namespace Shared.CinemaDataService.Models.RecordDTO
{
    public record ActorRole:Value
    {

        public RolePriority Priority { get; set; }

    }
}
