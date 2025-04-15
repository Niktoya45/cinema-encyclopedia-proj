using CinemaDataService.Api.Commands.StudioCommands.DeleteCommands;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.Shared;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Commands.StudioCommands.DeleteCommands
{
    public class DeleteStudioFilmographyCommandHandler:IRequestHandler<DeleteStudioFilmographyCommand, Unit>
    {
        IStudioRepository _repository;

        public DeleteStudioFilmographyCommandHandler(IStudioRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(DeleteStudioFilmographyCommand request, CancellationToken cancellationToken)
        {
            CinemaRecord? deleted = await _repository.DeleteFromFilmography(request.ParentId, new CinemaRecord { Id = request.Id }, cancellationToken);

            if (deleted == null)
            {
                throw new NotFoundException(request.Id, "Studio");
            }

            return Unit.Value;
        }
    }
}
