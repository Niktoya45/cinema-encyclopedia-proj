using AutoMapper;
using MediatR;
using CinemaDataService.Domain.Aggregates.StudioAggregate;
using CinemaDataService.Infrastructure.Models.StudioDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;

namespace CinemaDataService.Api.Commands.StudioCommands.CreateCommands
{
    public class CreateStudioCommandHandler : IRequestHandler<CreateStudioCommand, StudioResponse>
    {
        IStudioRepository _repository;
        IMapper _mapper;

        public CreateStudioCommandHandler(IStudioRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<StudioResponse> Handle(CreateStudioCommand request, CancellationToken cancellationToken)
        {
            //handle data validation

            Studio studio = _mapper.Map<CreateStudioCommand, Studio>(request);
            Studio added = _repository.Add(studio);
            
            return _mapper.Map<Studio, StudioResponse>(added);
        }
    }
}