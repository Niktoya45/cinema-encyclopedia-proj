
namespace UserDataService.Infrastructure.Models.UserDTO
{
    public class CreateUserRequest {

        public string Username { get; set; }
        public DateOnly? Birthdate { get; set; }
        public string? Picture { get; set; }
    }
}