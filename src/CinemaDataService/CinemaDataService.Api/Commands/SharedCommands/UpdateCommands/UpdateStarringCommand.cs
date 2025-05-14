using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Models.RecordDTO;
using MediatR;

namespace CinemaDataService.Api.Commands.SharedCommands.UpdateCommands
{
    public class UpdateStarringCommand:IRequest<StarringResponse>
    {
        public UpdateStarringCommand(
                string id,
                string name,
                Job jobs,
                string roleName,
                RolePriority rolePriority,
                string? picture
        )
        {
            Id = id;
            Name = name;
            Jobs = jobs;
            RoleName = roleName;
            RolePriority = rolePriority;
            Picture = picture;
        }
        public string Id { get; }
        public string Name { get; }
        public Job Jobs { get; set; }
        public string RoleName { get; set; }
        public RolePriority RolePriority { get; set; }
        public string? Picture { get; }
    }
}
