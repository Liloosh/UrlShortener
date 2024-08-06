using Microsoft.AspNetCore.Identity;

namespace Server.Models
{
    public class Url
    {
        public int Id { get; set; }
        public required string FullUrl { get; set; }
        public required string ShortUrl { get; set; }
        
        //CreatedBy
        public required string UserId { get; set; }
        public required DateTime CreatedDate { get; set; }

        public IdentityUser User { get; set; }
    }
}
