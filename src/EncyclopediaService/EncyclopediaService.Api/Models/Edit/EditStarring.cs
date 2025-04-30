namespace EncyclopediaService.Api.Models.Edit
{
    public class EditStarring
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; }

        public Job Jobs { get; set; }
        public List<Job> JobsBind { get; set; } = default!;
        public string? RoleName { get; set; }
        public RolePriority? RolePriority { get; set; }
    }
}
