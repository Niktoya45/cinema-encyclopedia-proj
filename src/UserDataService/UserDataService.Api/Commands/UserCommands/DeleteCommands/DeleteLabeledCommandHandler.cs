using AutoMapper;
using MediatR;
using UserDataService.Api.Exceptions.InfrastructureExceptions;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Models.LabeledDTO;
using UserDataService.Infrastructure.Models.RatingDTO;
using UserDataService.Infrastructure.Repositories.Abstractions;

namespace UserDataService.Api.Commands.UserCommands.DeleteCommands
{
    public class DeleteLabeledCommandHandler : IRequestHandler<DeleteLabeledCommand, LabeledResponse>
    {
        IUserRepository _repository;
        IMapper _mapper;

        public DeleteLabeledCommandHandler(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<LabeledResponse> Handle(DeleteLabeledCommand request, CancellationToken cancellationToken)
        {
            LabeledRecord rating = _mapper.Map<DeleteLabeledCommand, LabeledRecord>(request);
            LabeledRecord? added = await _repository.AddToLabeledList(rating, cancellationToken);

            if (added is null)
            {
                throw new RecordNotFoundException(request.UserId, request.CinemaId, "labeled_records");
            }

            return _mapper.Map<LabeledRecord, LabeledResponse>(added);
        }
    }
}