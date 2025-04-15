using MediatR;

namespace CinemaDataService.Api.Commands.SharedCommands.DeleteCommands
{
    public class DeleteRecordCommand:IRequest<Unit>
    {
        public DeleteRecordCommand(string? parentId, string id)
        {
            ParentId = parentId;
            Id = id;
        }
        public string? ParentId { get; set; }
        public string Id { get; }
    }
}
