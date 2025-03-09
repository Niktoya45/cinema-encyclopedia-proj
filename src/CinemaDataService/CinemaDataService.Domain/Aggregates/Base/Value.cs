
namespace CinemaDataService.Domain.Aggregates.Base
{
    public record Value
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
    }
}
