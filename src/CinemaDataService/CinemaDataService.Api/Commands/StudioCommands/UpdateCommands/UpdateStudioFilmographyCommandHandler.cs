using AutoMapper;
using CinemaDataService.Api.Commands.StudioCommands.UpdateCommands;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Commands.StudioCommands.UpdateCommands
{
    public class UpdateStudioFilmographyCommandHandler : IRequestHandler<UpdateStudioFilmographyCommand, FilmographyResponse>
    {
        IStudioRepository _repository;
        IMapper _mapper;

        public UpdateStudioFilmographyCommandHandler(IStudioRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<FilmographyResponse> Handle(UpdateStudioFilmographyCommand request, CancellationToken cancellationToken)
        {
            //handle data validation
            CinemaRecord filmographyRecord = _mapper.Map<UpdateStudioFilmographyCommand, CinemaRecord>(request);

            CinemaRecord? updated = await _repository.UpdateFilmography(filmographyRecord, cancellationToken);

            if (updated == null)
            {
                // handle 
                throw new NotFoundException(request.Id, "Studio");
            }

            FilmographyResponse response = _mapper.Map<CinemaRecord, FilmographyResponse>(updated);
            response.ParentId = null;

            return response;

        }
    }
}
