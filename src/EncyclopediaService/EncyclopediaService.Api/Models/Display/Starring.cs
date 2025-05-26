namespace EncyclopediaService.Api.Models.Display
{
    /**/
    public record Starring
    {

        public string? ParentId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; }

        public Job Jobs;
        public string? RoleName { get; set; }
        public RolePriority? RolePriority { get; set; }
    };
}
