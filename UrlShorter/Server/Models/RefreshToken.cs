using Microsoft.AspNetCore.Identity;

namespace Server.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public required string Token { get; set; }
        public DateTime ExpiredDate { get; set; }
        public required string UserId { get; set; }

        public IdentityUser User { get; set; }
    }
}
