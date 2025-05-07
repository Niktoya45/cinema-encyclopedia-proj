using AutoMapper;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Commands.CinemaCommands.CreateCommands
{
    public class CreateCinemaStarringCommandHandler : IRequestHandler<CreateCinemaStarringCommand, StarringResponse>
    {
        ICinemaRepository _repository;
        IMapper _mapper;

        public CreateCinemaStarringCommandHandler(ICinemaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<StarringResponse> Handle(CreateCinemaStarringCommand request, CancellationToken cancellationToken)
        {
            //handle data validation
            Starring starring = _mapper.Map<CreateCinemaStarringCommand, Starring>(request);

            Starring? added = await _repository.AddStarring(request.CinemaId, starring, cancellationToken);

            if (added == null)
            {
                throw new NotFoundException(request.Id, "Cinemas");
            }

            StarringResponse response = _mapper.Map<Starring, StarringResponse>(added);
            response.ParentId = request.CinemaId;

            return response;
        }
    }
}
