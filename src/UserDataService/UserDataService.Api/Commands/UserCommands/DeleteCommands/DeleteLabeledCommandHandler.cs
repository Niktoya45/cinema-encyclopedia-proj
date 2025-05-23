using AutoMapper;
using MediatR;
using UserDataService.Api.Exceptions.InfrastructureExceptions;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Infrastructure.Models.LabeledDTO;
using UserDataService.Infrastructure.Repositories.Abstractions;

namespace UserDataService.Api.Commands.UserCommands.DeleteCommands
{
    public class DeleteLabeledCommandHandler : IRequestHandler<DeleteLabeledCommand, IEnumerable<LabeledResponse>>
    {
        IUserRepository _repository;
        IMapper _mapper;

        public DeleteLabeledCommandHandler(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<LabeledResponse>> Handle(DeleteLabeledCommand request, CancellationToken cancellationToken)
        {
            LabeledRecord labeled = _mapper.Map<DeleteLabeledCommand, LabeledRecord>(request);
            IEnumerable<LabeledRecord>? deleted = await _repository.DeleteFromCinemaList(labeled, cancellationToken);

            if (deleted is null)
            {
                throw new RecordNotFoundException(request.UserId, request.CinemaId, "labeled_records");
            }

            return _mapper.Map<IEnumerable<LabeledRecord>, IEnumerable<LabeledResponse>>(deleted);
        }
    }
}