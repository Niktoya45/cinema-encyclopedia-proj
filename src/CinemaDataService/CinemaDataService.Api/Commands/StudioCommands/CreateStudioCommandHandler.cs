using AutoMapper;
using MediatR;
using CinemaDataService.Domain.Aggregates.StudioAggregate;
using CinemaDataService.Infrastructure.Models.StudioDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;

namespace CinemaDataService.Api.Commands.StudioCommands
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
            Studio Studio = _mapper.Map<CreateStudioCommand, Studio>(request);
            Studio added = _repository.Add(Studio);

            return _mapper.Map<Studio, StudioResponse>(added);
        }
    }
}