using Shared.CinemaDataService.Models.Flags;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EncyclopediaService.Api.Models.Edit
{
    public class EditStarring
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; }
        public string? PictureUri { get; set; }

        public Job Jobs { get; set; }

        [DisplayName("Jobs")]
        [Required(ErrorMessage="Choose at least one occupation")]
        public List<Job> JobsBind { get; set; } = default!;
        public string? RoleName { get; set; }
        public RolePriority? RolePriority { get; set; }
    }
}
