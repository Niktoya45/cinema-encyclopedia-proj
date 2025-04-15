using CinemaDataService.Api.Commands.CinemaCommands.DeleteCommands;
using CinemaDataService.Api.Commands.PersonCommands.CreateCommands;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.CinemaAggregate;
using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Commands.PersonCommands.DeleteCommands
{
    public class DeletePersonFilmographyCommandHandler: IRequestHandler<DeletePersonFilmographyCommand, Unit>
    {
        IPersonRepository _repository;

        public DeletePersonFilmographyCommandHandler(IPersonRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(DeletePersonFilmographyCommand request, CancellationToken cancellationToken)
        {
            CinemaRecord? deleted = await _repository.DeleteFromFilmography(request.ParentId, new CinemaRecord { Id = request.Id }, cancellationToken);

            if (deleted == null)
            {
                throw new NotFoundException(request.Id, "Person");
            }

            return Unit.Value;
        }
    }
}
