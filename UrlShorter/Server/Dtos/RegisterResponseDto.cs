using Microsoft.AspNetCore.Identity;
using Server.Enums;

namespace Server.Dtos
{
    public class RegisterResponseDto
    {
        public RegisterResponseEnum Response { get; set; }
        public IdentityResult? Result { get; set; }
    }
}
