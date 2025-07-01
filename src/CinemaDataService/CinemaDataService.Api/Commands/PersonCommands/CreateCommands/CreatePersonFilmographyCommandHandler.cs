using AutoMapper;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Models.RecordDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Commands.PersonCommands.CreateCommands
{
    public class CreatePersonFilmographyCommandHandler : IRequestHandler<CreatePersonFilmographyCommand, FilmographyResponse>
    {
        IPersonRepository _repository;
        IMapper _mapper;

        public CreatePersonFilmographyCommandHandler(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<FilmographyResponse> Handle(CreatePersonFilmographyCommand request, CancellationToken cancellationToken)
        {
            //handle data validation
            CinemaRecord filmographyRecord = _mapper.Map<CreatePersonFilmographyCommand, CinemaRecord>(request);

            CinemaRecord? added = await _repository.AddToFilmography(request.PersonId, filmographyRecord, cancellationToken);

            if (added == null)
            {
                // handle 
                throw new NotFoundException(request.Id, "Person");
            }

            FilmographyResponse response = _mapper.Map<CinemaRecord, FilmographyResponse>(added);
            response.ParentId = request.PersonId;

            return response;
        }
    }
}
