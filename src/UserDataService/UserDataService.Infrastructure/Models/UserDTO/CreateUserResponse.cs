namespace UserDataService.Infrastructure.Models.UserDTO
{
    public class CreateUserResponse
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public DateOnly Birthdate { get; set; }
        public string Picture { get; set; }
    }
}