using Server.Enums;

namespace Server.Dtos
{
    public class RegisterResponse
    {
        public RegisterResponseEnum Response { get; set; }
        public List<string>? Message { get; set; }
    }
}
