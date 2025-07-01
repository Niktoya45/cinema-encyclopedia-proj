using AutoMapper;
using CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.RecordDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Commands.CinemaCommands.UpdateCommands
{
    public class UpdateCinemaProductionStudioCommandHandler : IRequestHandler<UpdateCinemaProductionStudioCommand, ProductionStudioResponse>
    {
        ICinemaRepository _repository;
        IMapper _mapper;

        public UpdateCinemaProductionStudioCommandHandler(ICinemaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ProductionStudioResponse> Handle(UpdateCinemaProductionStudioCommand request, CancellationToken cancellationToken)
        {
            //handle data validation
            StudioRecord productionStudio = _mapper.Map<UpdateCinemaProductionStudioCommand, StudioRecord>(request);

            StudioRecord? updated = await _repository.UpdateProductionStudio(request.CinemaId, productionStudio, cancellationToken);

            if (updated == null)
            {
                throw new NotFoundException(request.Id, "Cinemas");
            }

            ProductionStudioResponse response = _mapper.Map<StudioRecord, ProductionStudioResponse>(updated);
            response.ParentId = request.CinemaId;

            return response;
        }
    }
}