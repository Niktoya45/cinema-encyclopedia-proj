using AutoMapper;
using MediatR;
using CinemaDataService.Api.Exceptions.StudioExceptions;
using CinemaDataService.Domain.Aggregates.StudioAggregate;
using CinemaDataService.Infrastructure.Models.StudioDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;

namespace CinemaDataService.Api.Commands.StudioCommands
{
    public class UpdateStudioCommandHandler : IRequestHandler<UpdateStudioCommand, StudioResponse>
    {
        IRepository _repository;
        IMapper _mapper;

        public UpdateStudioCommandHandler(IStudioRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<StudioResponse> Handle(UpdateStudioCommand request, CancellationToken cancellationToken)
        {
            Studio Studio = _mapper.Map<UpdateStudioCommand, Studio>(request);

            Studio? updated = await _repository.Update(Studio);

            if (updated == null)
            {
                // handle;
            }

            return _mapper.Map<Studio, StudioResponse>(updated);
        }
    }
}