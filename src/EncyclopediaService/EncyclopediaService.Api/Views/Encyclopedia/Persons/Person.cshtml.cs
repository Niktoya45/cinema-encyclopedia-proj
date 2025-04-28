using EncyclopediaService.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EncyclopediaService.Api.Views.Encyclopedia.Persons
{
    public class PersonModel:PageModel
    {
        [BindProperty(SupportsGet=true)]
        public int Id { get; set; }
        public Person Person { get; set; }
        public async Task<IActionResult> OnGet(string id) 
        {
            // send data request instead of block below

            Person = new Person
            {
                Id = id,
                Name = "Long Long Name Long Long Long Surname",
                Country = 0,
                Jobs = Job.Actor,
                Picture = "/img/poster_placeholder.png",
                Filmography = new CinemaRecord[] { 
                    new CinemaRecord { Name = "Cinema Title", Year=2000, Picture="/img/grid_poster_placeholder.png"}, 
                    new CinemaRecord { Name = "Cinema Title", Year=2000, Picture="/img/grid_poster_placeholder.png"}, 
                    new CinemaRecord { Name = "Cinema Title", Year=2000, Picture="/img/grid_poster_placeholder.png"}, 
                    new CinemaRecord { Name = "Cinema Title", Year=2000, Picture="/img/grid_poster_placeholder.png"} 
                },
                Description = "Person description goes here. Person was born.."
            };

            if (Person.Picture is null)
            {
                Person.Picture = "/img/grid_person_placeholder.png";
            }

            if (Person.Description is null)
            {
                Person.Description = "";
            }

            return Page();
        }
    }
}
