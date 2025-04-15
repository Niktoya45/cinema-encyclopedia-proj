using AutoMapper;
using MediatR;
using CinemaDataService.Infrastructure.Models.PersonDTO;
using CinemaDataService.Domain.Aggregates.PersonAggregate;
//using CinemaDataService.Api.Exceptions.PersonExceptions;
using CinemaDataService.Infrastructure.Repositories.Abstractions;

namespace CinemaDataService.Api.Queries.PersonQueries
{

    public class PersonQueryHandler : IRequestHandler<PersonQuery, PersonResponse>
    {
        IPersonRepository _repository;
        IMapper _mapper;
        public PersonQueryHandler(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<PersonResponse> Handle(PersonQuery request, CancellationToken cancellationToken)
        {
            Person? Person = await _repository.FindById(request.Id, cancellationToken);

            //if (Person == null)
                // handle


            return _mapper.Map<PersonResponse>(Person);
        }
    }
}
