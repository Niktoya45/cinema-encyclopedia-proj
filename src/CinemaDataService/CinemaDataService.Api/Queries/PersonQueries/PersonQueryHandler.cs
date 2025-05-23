using AutoMapper;
using MediatR;
using CinemaDataService.Infrastructure.Models.PersonDTO;
using CinemaDataService.Domain.Aggregates.PersonAggregate;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;

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
            Person? person = await _repository.FindById(request.Id, cancellationToken);

            if (person == null)
            {
                // handle
                throw new NotFoundException(request.Id, "Persons");
            }

            return _mapper.Map<PersonResponse>(person);
        }
    }
}
