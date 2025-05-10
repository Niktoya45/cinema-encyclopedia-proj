namespace ProfileService.Api.Models.Edit
{
    public class EditMain
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public DateOnly Birthdate { get; set; }
        public string? Description { get; set; }
    }
}
