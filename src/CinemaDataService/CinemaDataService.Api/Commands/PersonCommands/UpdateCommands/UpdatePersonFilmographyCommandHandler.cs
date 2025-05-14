using AutoMapper;
using CinemaDataService.Api.Commands.PersonCommands.UpdateCommands;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Models.RecordDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Commands.PersonCommands.UpdateCommands
{
    public class UpdatePersonFilmographyCommandHandler : IRequestHandler<UpdatePersonFilmographyCommand, FilmographyResponse>
    {
        IPersonRepository _repository;
        IMapper _mapper;

        public UpdatePersonFilmographyCommandHandler(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<FilmographyResponse> Handle(UpdatePersonFilmographyCommand request, CancellationToken cancellationToken)
        {
            //handle data validation
            CinemaRecord filmographyRecord = _mapper.Map<UpdatePersonFilmographyCommand, CinemaRecord>(request);

            CinemaRecord? updated = await _repository.UpdateFilmography(filmographyRecord, cancellationToken);

            if (updated == null)
            {
                // handle 
                throw new NotFoundException(request.Id, "Person");
            }

            return _mapper.Map<CinemaRecord, FilmographyResponse>(updated);
        }
    }
}
