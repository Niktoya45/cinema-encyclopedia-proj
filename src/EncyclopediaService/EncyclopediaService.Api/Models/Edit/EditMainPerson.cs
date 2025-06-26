using Shared.CinemaDataService.Models.Flags;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EncyclopediaService.Api.Models.Edit
{
    public class EditMainPerson
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateOnly BirthDate { get; set; }
        public Country? Country { get; set; }
        public Job Jobs { get; set; }

        [DisplayName("Jobs")]
        [Required(ErrorMessage = "Choose at least one occupation")]
        public List<Job> JobsBind { get; set; } = default!;
        public string? Description { get; set; }
    }
}
