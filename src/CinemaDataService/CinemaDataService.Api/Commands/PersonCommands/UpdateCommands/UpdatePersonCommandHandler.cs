using AutoMapper;
using MediatR;
using CinemaDataService.Domain.Aggregates.PersonAggregate;
using CinemaDataService.Infrastructure.Models.PersonDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using System.Security.Cryptography.Xml;

namespace CinemaDataService.Api.Commands.PersonCommands.UpdateCommands
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
            Person person = _mapper.Map<UpdatePersonCommand, Person>(request);

            Person? updated = await _repository.Update(person);

            if (updated == null)
            {
                throw new NotFoundException(person.Id, "Persons");
            }

            return _mapper.Map<Person, PersonResponse>(updated);
        }
    }
}