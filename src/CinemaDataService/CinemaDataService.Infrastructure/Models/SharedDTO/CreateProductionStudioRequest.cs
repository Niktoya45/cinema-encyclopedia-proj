
namespace CinemaDataService.Infrastructure.Models.SharedDTO
{
    public class CreateProductionStudioRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; } = default(string);
    }
}
