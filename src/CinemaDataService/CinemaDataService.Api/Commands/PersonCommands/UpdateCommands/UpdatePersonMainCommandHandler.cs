using AutoMapper;
using MediatR;
using CinemaDataService.Domain.Aggregates.PersonAggregate;
using CinemaDataService.Infrastructure.Models.PersonDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using System.Security.Cryptography.Xml;

namespace CinemaDataService.Api.Commands.PersonCommands.UpdateCommands
{
    public class UpdatePersonMainCommandHandler : IRequestHandler<UpdatePersonMainCommand, PersonResponse>
    {
        IPersonRepository _repository;
        IMapper _mapper;

        public UpdatePersonMainCommandHandler(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<PersonResponse> Handle(UpdatePersonMainCommand request, CancellationToken cancellationToken)
        {
            Person person = _mapper.Map<UpdatePersonMainCommand, Person>(request);

            Person? updated = await _repository.UpdateMain(person);

            if (updated == null)
            {
                throw new NotFoundException(person.Id, "Persons");
            }

            return _mapper.Map<Person, PersonResponse>(updated);
        }
    }
}