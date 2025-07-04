using CinemaDataService.Infrastructure.Models.PersonDTO;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using MediatR;

namespace CinemaDataService.Api.Queries.PersonQueries
{
    public class PersonsQueryCommonWrapper : IRequest<Page<PersonsResponse>>
    {
        public PersonsQueryCommonWrapper(PersonsQuery query) 
        {
            Query = query;
        }

        public PersonsQuery Query { get; }
    }
}
