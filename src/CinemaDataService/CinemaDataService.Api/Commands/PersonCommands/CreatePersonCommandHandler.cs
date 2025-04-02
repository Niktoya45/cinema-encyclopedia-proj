using AutoMapper;
using MediatR;
using CinemaDataService.Domain.Aggregates.PersonAggregate;
using CinemaDataService.Infrastructure.Models.PersonDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;

namespace CinemaDataService.Api.Commands.PersonCommands
{
    public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, PersonResponse>
    {
        IPersonRepository _repository;
        IMapper _mapper;

        public CreatePersonCommandHandler(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<PersonResponse> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            Person Person = _mapper.Map<CreatePersonCommand, Person>(request);
            Person added = _repository.Add(Person);

            return _mapper.Map<Person, PersonResponse>(added);
        }
    }
}