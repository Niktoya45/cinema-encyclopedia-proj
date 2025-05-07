using Microsoft.AspNetCore.Mvc;

namespace EncyclopediaService.Api.Models.Filter
{
    public class FilterStudios
    {
        // load information from db

        public List<int> YearsChoice = Enumerable.Range(1, 12).Select(i => DateTime.Now.Year - i * 10).ToList();

        [BindProperty]
        public List<int> YearsBind { get; set; }

        [BindProperty]
        public Country CountryChoice { get; set; }
    }
}
