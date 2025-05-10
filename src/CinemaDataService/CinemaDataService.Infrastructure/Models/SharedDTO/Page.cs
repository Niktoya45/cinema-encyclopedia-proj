

namespace CinemaDataService.Infrastructure.Models.SharedDTO
{
    public class Page<T>
    {
        public IEnumerable<T> Response { get; set; }
        
        public bool IsEnd { get; set; }
    }
}
