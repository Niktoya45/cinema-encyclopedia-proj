using Shared.CinemaDataService.Models.Flags;

namespace EncyclopediaService.Api.Models.Display
{
    /**/
    public record Starring
    {
        public bool NewRecord { get; set; }
        public string? ParentId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; }
        public string? PictureUri { get; set; }

        public Job Jobs;
        public string? RoleName { get; set; }
        public RolePriority? RolePriority { get; set; }
    };
}
