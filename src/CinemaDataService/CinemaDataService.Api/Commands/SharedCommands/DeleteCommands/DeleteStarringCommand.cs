namespace CinemaDataService.Api.Commands.SharedCommands.DeleteCommands
{
    public class DeleteStarringCommand:DeleteRecordCommand
    {
        public DeleteStarringCommand(string? parentId, string starringId) : base(parentId, starringId) { }
    }
}
