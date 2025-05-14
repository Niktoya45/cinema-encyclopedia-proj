using AutoMapper;
using CinemaDataService.Api.Commands.SharedCommands;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.CinemaDTO;
using CinemaDataService.Infrastructure.Models.RecordDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Commands.CinemaCommands.CreateCommands
{
    public class CreateCinemaProductionStudioCommandHandler : IRequestHandler<CreateCinemaProductionStudioCommand, ProductionStudioResponse>
    {
        ICinemaRepository _repository;
        IMapper _mapper;

        public CreateCinemaProductionStudioCommandHandler(ICinemaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ProductionStudioResponse> Handle(CreateCinemaProductionStudioCommand request, CancellationToken cancellationToken)
        {
            //handle data validation
            StudioRecord productionStudio = _mapper.Map<CreateCinemaProductionStudioCommand, StudioRecord>(request);

            StudioRecord? added = await _repository.AddProductionStudio(request.CinemaId, productionStudio, cancellationToken);

            if (added == null)
            {
                throw new NotFoundException(request.Id, "Cinemas");
            }

            ProductionStudioResponse response = _mapper.Map<StudioRecord, ProductionStudioResponse>(added);
            response.ParentId = request.CinemaId;

            return response;
        }
    }
}
