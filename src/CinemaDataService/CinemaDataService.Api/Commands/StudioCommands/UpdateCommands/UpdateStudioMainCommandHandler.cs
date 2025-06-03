using AutoMapper;
using MediatR;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.StudioAggregate;
using CinemaDataService.Infrastructure.Models.StudioDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;

namespace CinemaDataService.Api.Commands.StudioCommands.UpdateCommands
{
    public class UpdateStudioMainCommandHandler : IRequestHandler<UpdateStudioMainCommand, StudioResponse>
    {
        IStudioRepository _repository;
        IMapper _mapper;

        public UpdateStudioMainCommandHandler(IStudioRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<StudioResponse> Handle(UpdateStudioMainCommand request, CancellationToken cancellationToken)
        {
            Studio studio = _mapper.Map<UpdateStudioMainCommand, Studio>(request);

            Studio? updated = await _repository.UpdateMain(studio);

            if (updated == null)
            {
                throw new NotFoundException(studio.Id, "Studios");
            }

            return _mapper.Map<Studio, StudioResponse>(updated);
        }
    }
}