namespace CinemaDataService.Api.Commands.SharedCommands.DeleteCommands
{
    public class DeleteProductionStudioCommand : DeleteRecordCommand
    {
        public DeleteProductionStudioCommand(string? parentId, string studioId) : base(parentId, studioId) { }
    }
}
