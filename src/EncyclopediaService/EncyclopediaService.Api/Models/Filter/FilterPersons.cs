using Shared.CinemaDataService.Models.Flags;
using Microsoft.AspNetCore.Mvc;

namespace EncyclopediaService.Api.Models.Filter
{
    public class FilterPersons
    {
        // load information from db

        public Job JobsChoice { get; set; }

        [BindProperty(SupportsGet=true, Name ="jobsbind")]
        public List<Job> JobsBind { get; set; } = new List<Job>();

        [BindProperty(SupportsGet=true, Name="countrybind")]
        public Country CountryBind { get; set; }


        [BindProperty(SupportsGet = true, Name = "search")]
        public string? Search { get; set; } = default;
    }
}
