using Microsoft.AspNetCore.Mvc;

namespace EncyclopediaService.Api.Models.Filter
{
    public class FilterPersons
    {
        // load information from db

        public Job JobsChoice { get; set; }

        [BindProperty]
        public List<Job> JobsBind { get; set; }

        [BindProperty]
        public Country CountryChoice { get; set; }
    }
}
