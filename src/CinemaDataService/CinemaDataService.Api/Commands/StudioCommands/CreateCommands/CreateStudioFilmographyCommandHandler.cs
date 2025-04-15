using AutoMapper;
using CinemaDataService.Api.Commands.StudioCommands.CreateCommands;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Commands.StudioCommands.CreateCommands
{
    public class CreateStudioFilmographyCommandHandler : IRequestHandler<CreateStudioFilmographyCommand, FilmographyResponse>
    {
        IStudioRepository _repository;
        IMapper _mapper;

        public CreateStudioFilmographyCommandHandler(IStudioRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<FilmographyResponse> Handle(CreateStudioFilmographyCommand request, CancellationToken cancellationToken)
        {
            //handle data validation
            CinemaRecord filmographyRecord = _mapper.Map<CreateStudioFilmographyCommand, CinemaRecord>(request);

            CinemaRecord? added = await _repository.AddToFilmography(request.StudioId, filmographyRecord, cancellationToken);

            if (added == null)
            {
                // handle 
                throw new NotFoundException(request.Id, "Studio");
            }

            return _mapper.Map<CinemaRecord, FilmographyResponse>(added);
        }
    }
}
