using Server.Enums;

namespace Server.Dtos
{
    public class LoginResponseDto
    {
        public LoginResponseEnum Response { get; set; }
        public string? Token { get; set; }
    }
}
