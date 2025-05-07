using AutoMapper;
using CinemaDataService.Api.Commands.PersonCommands.UpdateCommands;
using CinemaDataService.Api.Exceptions.InfrastructureExceptions;
using CinemaDataService.Domain.Aggregates.PersonAggregate;
using CinemaDataService.Infrastructure.Models.SharedDTO;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace CinemaDataService.Api.Commands.PersonCommands.UpdateCommands
{
    public class UpdatePersonPictureCommandHandler : IRequestHandler<UpdatePersonPictureCommand, UpdatePictureResponse>
    {
        IPersonRepository _repository;
        public UpdatePersonPictureCommandHandler(IPersonRepository repository)
        {
            _repository = repository;
        }
        public async Task<UpdatePictureResponse> Handle(UpdatePersonPictureCommand request, CancellationToken cancellationToken)
        {
            //handle data validation
            Person? updated = await _repository.UpdatePicture(request.Id, request.Picture, cancellationToken);

            if (updated == null)
            {
                throw new NotFoundException(request.Id, "Persons");
            }

            return new UpdatePictureResponse { Id = request.Id, Picture = request.Picture };
        }
    }
}
