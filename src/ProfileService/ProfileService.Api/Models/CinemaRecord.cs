
using System.ComponentModel.DataAnnotations;

namespace ProfileService.Api.Models
{
    public record CinemaRecord{
        public string? ParentId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public Label Label { get; set; }
        public DateTime AddedAt { get; set; }
        public string? Picture { get; set; } = default(string);
    }
}
