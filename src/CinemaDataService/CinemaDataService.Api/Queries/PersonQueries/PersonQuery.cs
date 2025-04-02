using MediatR;
using CinemaDataService.Infrastructure.Models.PersonDTO;


namespace CinemaDataService.Api.Queries.PersonQueries
{
    public class PersonQuery : IRequest<PersonResponse>
    {
        public PersonQuery(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
