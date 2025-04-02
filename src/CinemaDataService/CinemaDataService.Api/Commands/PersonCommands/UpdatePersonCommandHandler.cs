using AutoMapper;
using MediatR;
using CinemaDataService.Api.Exceptions.PersonExceptions;
using CinemaDataService.Domain.Aggregates.PersonAggregate;
using CinemaDataService.Infrastructure.Models.PersonDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;

namespace CinemaDataService.Api.Commands.PersonCommands
{
    public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, PersonResponse>
    {
        IPersonRepository _repository;
        IMapper _mapper;

        public UpdatePersonCommandHandler(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<PersonResponse> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            Person Person = _mapper.Map<UpdatePersonCommand, Person>(request);

            Person? updated = await _repository.Update(Person);

            if (updated == null)
            {
                // handle 
            }

            return _mapper.Map<Person, PersonResponse>(updated);
        }
    }
}