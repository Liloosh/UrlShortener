using Server.Enums;

namespace Server.Dtos
{
    public class LoginResponseWithoutRefreshTokenDto
    {
        public LoginResponseEnum Response { get; set; }
        public string? Token { get; set; }
    }
}
