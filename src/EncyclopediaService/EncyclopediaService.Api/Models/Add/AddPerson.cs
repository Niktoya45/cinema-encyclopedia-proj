using EncyclopediaService.Api.Models.Edit;
using Shared.CinemaDataService.Models.Flags;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EncyclopediaService.Api.Models.Add
{
    public class AddPerson
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; } = default(string);
        public DateOnly BirthDate { get; set; }
        public Job Jobs { get; set; }

        [DisplayName("Jobs")]
        [Required(ErrorMessage = "Choose at least one occupation")]
        public List<Job> JobsBind { get; set; } = default!;
        public Country? Country { get; set; }
        public IList<EditFilm>? Filmography { get; set; }
        public string? Description { get; set; }
    }
}
