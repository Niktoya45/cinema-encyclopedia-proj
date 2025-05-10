using AutoMapper;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.PersonAggregate;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Queries.PersonQueries
{
    public class PersonsSearchQueryHandler : IRequestHandler<PersonsSearchQuery, IEnumerable<SearchResponse>>
    {
        IPersonRepository _repository;
        IMapper _mapper;
        public PersonsSearchQueryHandler(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SearchResponse>> Handle(PersonsSearchQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Person>? persons = await _repository.FindByName(tokens: request.Search, pg: request.Pg, ct: cancellationToken);


            if (persons == null)
            {
                // handle
                throw new NotFoundException("Persons");
            }

            return _mapper.Map<IEnumerable<Person>, IEnumerable<SearchResponse>>(persons);
        }
    }
}
