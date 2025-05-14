namespace Shared.CinemaDataService.Models.RecordDTO
{
    public class CreateProductionStudioRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; } = default;
    }
}
