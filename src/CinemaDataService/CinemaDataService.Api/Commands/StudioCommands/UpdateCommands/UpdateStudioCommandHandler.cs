using AutoMapper;
using MediatR;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.StudioAggregate;
using CinemaDataService.Infrastructure.Models.StudioDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;

namespace CinemaDataService.Api.Commands.StudioCommands.UpdateCommands
{
    public class UpdateStudioCommandHandler : IRequestHandler<UpdateStudioCommand, StudioResponse>
    {
        IStudioRepository _repository;
        IMapper _mapper;

        public UpdateStudioCommandHandler(IStudioRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<StudioResponse> Handle(UpdateStudioCommand request, CancellationToken cancellationToken)
        {
            Studio studio = _mapper.Map<UpdateStudioCommand, Studio>(request);

            Studio? updated = await _repository.Update(studio);

            if (updated == null)
            {
                throw new NotFoundException(studio.Id, "Studios");
            }

            return _mapper.Map<Studio, StudioResponse>(updated);
        }
    }
}